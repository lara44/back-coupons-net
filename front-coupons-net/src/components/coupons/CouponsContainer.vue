<template>
  <v-container>
    <v-dialog v-model="dialog" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="headline">{{
            selectedCoupon ? "Editar Cupón" : "Crear Cupón"
          }}</span>
        </v-card-title>
        <v-card-text>
          <v-form @submit.prevent="submitForm">
            <v-select
              v-if="userStore.role === 'Admin'"
              v-model="newCoupon.companyId"
              :items="companies"
              item-title="name"
              item-value="id"
              label="Empresa"
              :rules="[requiredRule('Empresa')]"
              required
            ></v-select>
            <v-text-field
              v-model="newCoupon.couponCode"
              label="Código"
              :rules="[requiredRule('Código del cupón')]"
              required
            ></v-text-field>
            <v-text-field
              v-model="newCoupon.name"
              label="Nombre"
              :rules="[requiredRule('Nombre')]"
              required
            ></v-text-field>
            <v-text-field
              v-model="newCoupon.startDate"
              type="date"
              label="Fecha inicial"
              :rules="[requiredRule('Fecha de Inicio')]"
              required
            ></v-text-field>
            <v-text-field
              v-model="newCoupon.expiryDate"
              type="date"
              label="Fecha Final"
              :rules="[requiredRule('Fecha de Finalización'), expiryDateRule]"
              required
            ></v-text-field>
            <v-text-field
              v-model="newCoupon.quantityInitial"
              label="Cantidad Inicial"
              :rules="[requiredRule('Cantidad inicial'), numberRule]"
              required
            ></v-text-field>
            <v-text-field
              v-model="newCoupon.photo"
              label="Foto"
              :rules="[requiredRule('Foto')]"
              required
            ></v-text-field>
            <v-select
              v-model="selectedProducts"
              :items="products"
              item-title="name"
              item-value="id"
              label="Producto"
              multiple
              :rules="[requiredRule('País')]"
              required
            >
            </v-select>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn size="small" class="btn-general" text @click="closeModal"
            >Cancelar</v-btn
          >
          <v-btn size="small" class="mr-4 btn-general" @click="submitForm">{{
            selectedCoupon ? "Actualizar" : "Guardar"
          }}</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <!-- Tabla HTML para mostrar los usuarios -->
    <v-card>
      <v-card-title>
        <v-row>
          <v-col md="6" sm="6" cols="12">
            <span class="headline">Lista de Cupones registrados</span>
          </v-col>
          <v-col md="6" sm="6" cols="12">
            <v-text-field
              v-model="search"
              append-icon="mdi-magnify"
              label="Buscar Cupón"
              single-line
              hide-details
              variant="underlined"
            ></v-text-field>
          </v-col>
        </v-row>
      </v-card-title>
      <v-btn size="small" class="ma-2 btn-general" dark @click="openModal"
        >Nuevo</v-btn
      >
      <v-card-text>
        <v-table density="compact">
          <thead>
            <tr>
              <th>ID</th>
              <th>Código</th>
              <th>Nombre</th>
              <th>fecha inicial</th>
              <th>fecha final</th>
              <th>cantidad inicial</th>
              <th>cantidad actual</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="coupon in filteredCoupons" :key="coupon.id">
              <td>{{ coupon.id }}</td>
              <td>{{ coupon.couponCode }}</td>
              <td>{{ coupon.name }}</td>
              <td>{{ coupon.startDate }}</td>
              <td>{{ coupon.expiryDate }}</td>
              <td>{{ coupon.quantityInitial }}</td>
              <td>{{ coupon.quantityActual }}</td>
              <td>
                <v-icon @click="editCoupon(coupon)" class="btn-icon"
                  >mdi-pencil</v-icon
                >
                <v-icon @click="deleteCoupon(coupon)" class="btn-icon"
                  >mdi-delete</v-icon
                >
                <v-icon @click="QrCoupon(coupon)" class="btn-icon"
                  >mdi-qrcode</v-icon
                >
              </td>
            </tr>
            <tr v-if="!filteredCoupons.length">
              <td colspan="4">No se encontraron cupones</td>
            </tr>
          </tbody>
        </v-table>
      </v-card-text>
      <v-pagination
        v-model="currentPage"
        rounded="circle"
        :length="totalPages"
        style="box-shadow: none !important"
      ></v-pagination>
    </v-card>
  </v-container>
</template>

<script setup>
import { ref, reactive, computed, onMounted, watch } from "vue";
import QRCode from "qrcode";
import { saveAs } from "file-saver";
import { useCouponStore } from "../../stores/couponStore";
import { useProductStore } from "../../stores/productStore";
import { useCompanyStore } from "../../stores/companyStore";
import { useUserStore } from "../../stores/userStore";
import { jwtDecode } from "jwt-decode";
import { useToast } from "vue-toastification";

const toast = useToast();

const token = localStorage.getItem("spa_token");
const decodedToken = jwtDecode(token);
const role =
  decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
const company = decodedToken["CompanyId"];

const productStore = useProductStore();
const companyStore = useCompanyStore();
const products = computed(() => productStore.listProducts);
const companies = computed(() => companyStore.listCompanies);

const userStore = useUserStore();
const currentPage = ref(1); // Página actual
const itemsPerPage = 10; // Número de usuarios por página
const couponStore = useCouponStore();
const search = ref("");
const newCoupon = reactive({
  name: "",
  couponCode: "",
  startDate: "",
  expiryDate: "",
  quantityInitial: "",
  quantityActual: "",
  companyId: "",
  photo: "",
  detailCoupons: [],
});

const formatDate = (date) => {
  const d = new Date(date);
  const year = d.getFullYear();
  const month = String(d.getMonth() + 1).padStart(2, "0");
  const day = String(d.getDate()).padStart(2, "0");
  return `${year}-${month}-${day}`;
};

const selectedCoupon = ref(null);
const selectedProducts = ref([]);
const dialog = ref(false);

const totalCoupons = computed(() => couponStore.listCoupons.length);
const totalPages = computed(() => Math.ceil(totalCoupons.value / itemsPerPage));

const filteredCoupons = computed(() => {
  const startIndex = (currentPage.value - 1) * itemsPerPage;
  const endIndex = startIndex + itemsPerPage;
  const couponList = couponStore.listCoupons;
  return couponList
    .filter((coupon) =>
      coupon.name.toLowerCase().includes(search.value.toLowerCase())
    )
    .slice(startIndex, endIndex);
});

const requiredRule = (fieldName) => (value) =>
  !!value || `El campo "${fieldName}" es obligatorio`;

const numberRule = (value) =>
  !isNaN(value) || 'El campo "Cantidad inicial" debe ser un número';

const expiryDateRule = (value) => {
  if (!value) return 'El campo "Fecha de Finalización" es obligatorio';

  const currentDate = new Date();
  const startDate = new Date(newCoupon.startDate);
  const expiryDate = new Date(value);

  if (expiryDate <= currentDate) {
    return "La fecha de finalización debe ser mayor a la fecha actual";
  }

  if (expiryDate <= startDate) {
    return "La fecha de finalización debe ser mayor a la fecha de inicio";
  }

  return true; // Si todas las validaciones pasan, devuelve true
};

watch(selectedProducts, (newVal) => {
  if (newVal.length > 2) {
    selectedProducts.value = newVal.slice(0, 2);
  }
});

const openModal = () => {
  dialog.value = true;
  selectedCoupon.value = null;
  newCoupon.name = "";
  newCoupon.couponCode = "";
  newCoupon.startDate = "";
  newCoupon.expiryDate = "";
  newCoupon.quantityInitial = "";
  newCoupon.photo = "";
  selectedProducts.value = [];
};

const submitForm = async () => {
  if (!newCoupon.name) {
    return;
  }

  newCoupon.detailCoupons = selectedProducts.value.map((id) => ({
    productId: id,
  }));

  if (selectedCoupon.value) {
    await couponStore.updateCoupon({
      ...selectedCoupon.value,
      ...newCoupon,
    });
    toast.success("Cupon Actualizado exitosamente");
  } else {
    newCoupon.quantityActual = newCoupon.quantityInitial;
    newCoupon.companyId = company;
    await couponStore.createCoupon(newCoupon);
    toast.success("Cupon creado exitosamente");
  }

  await couponStore.getCoupons(role, company);

  newCoupon.name = "";
  newCoupon.couponCode = "";
  newCoupon.startDate = "";
  newCoupon.expiryDate = "";
  newCoupon.quantityInitial = "";
  newCoupon.photo = "";
  (newCoupon.companyId = ""), (selectedProducts.value = []);

  dialog.value = false;
};

const editCoupon = (coupon) => {
  selectedCoupon.value = { ...coupon };
  newCoupon.name = selectedCoupon.value.name;
  newCoupon.couponCode = selectedCoupon.value.couponCode;
  newCoupon.startDate = formatDate(selectedCoupon.value.startDate);
  newCoupon.expiryDate = formatDate(selectedCoupon.value.expiryDate);
  newCoupon.quantityInitial = selectedCoupon.value.quantityInitial;
  newCoupon.quantityActual = selectedCoupon.value.quantityActual;
  newCoupon.companyId = selectedCoupon.value.companyId;
  newCoupon.photo = selectedCoupon.value.photo;
  selectedProducts.value = selectedCoupon.value.detailCoupons.map(
    (dc) => dc.productId
  );
  dialog.value = true;
};

const deleteCoupon = async (coupon) => {
  await couponStore.deleteCoupon(coupon);
  await couponStore.getCoupons(role, company);
};

const closeModal = () => {
  dialog.value = false;
  selectedCoupon.value = null;
  newCoupon.name = "";
  newCoupon.couponCode = "";
  newCoupon.startDate = "";
  newCoupon.expiryDate = "";
  newCoupon.quantityInitial = "";
  newCoupon.photo = "";
  selectedProducts.value = [];
};

const QrCoupon = async (coupon) => {
  try {
    const url = `http://localhost:5173/coupons/redeem?code=${coupon.couponCode}`;
    const qrCodeDataUrl = await QRCode.toDataURL(url);
    const blob = await (await fetch(qrCodeDataUrl)).blob();
    saveAs(blob, `${coupon.couponCode}.png`);
  } catch (error) {
    console.error("Error generating QR code", error);
  }
};

onMounted(() => {
  useCouponStore().getCoupons(role, company);
  companyStore.getCompanies();
  userStore.role === "Admin"
    ? useProductStore().getProducts()
    : useProductStore().getProductsByCompany(company);
});
</script>
