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
            <v-select
              v-model="newCoupon.companyId"
              :items="companies"
              item-title="name"
              item-value="id"
              label="Empresa"
              :rules="[requiredRule('Empresa')]"
              required
            ></v-select>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn color="blue darken-1" text @click="closeModal">Cancelar</v-btn>
          <v-btn color="primary" @click="submitForm">{{
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
      <v-btn class="ma-2" color="primary" dark @click="openModal">Nuevo</v-btn>
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
                <v-icon @click="editCoupon(coupon)" color="primary"
                  >mdi-pencil</v-icon
                >
                <v-icon @click="deleteCoupon(coupon)" color="primary"
                  >mdi-delete</v-icon
                >
                <v-icon @click="QrCoupon(coupon)" color="primary"
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

    <!-- Snackbar para mostrar el mensaje de éxito -->
    <v-snackbar v-model="successMessageVisible" timeout="3000">
      {{
        selectedCoupon
          ? "Cupón actualizado exitosamente"
          : "Cupón creado exitosamente"
      }}
    </v-snackbar>
  </v-container>
</template>

<script setup>
import { ref, reactive, computed, onMounted, watch } from "vue";
import QRCode from "qrcode";
import { saveAs } from "file-saver";
import { useCouponStore } from "../../stores/couponStore";
import { useProductStore } from "../../stores/productStore";
import { useCompanyStore} from "../../stores/companyStore";

const productStore = useProductStore();
const companyStore = useCompanyStore();
const products = computed(() => productStore.listProducts);
const companies = computed(() => companyStore.listCompanies);

const currentPage = ref(1); // Página actual
const itemsPerPage = 10; // Número de usuarios por página
const couponStore = useCouponStore();
const successMessageVisible = ref(false);
const search = ref("");
const newCoupon = reactive({
  name: "",
  couponCode: "",
  startDate: "",
  expiryDate: "",
  quantityInitial: "",
  quantityActual: "",
  companyId: "",
  detailCoupons: [],
});

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
  return (
    new Date(value) > new Date(newCoupon.startDate) ||
    "La fecha de finalización debe ser mayor a la fecha de inicio"
  );
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
  newCoupon.companyId= "",
  selectedProducts.value = [];
};

const submitForm = async () => {
  if (!newCoupon.name) {
    return;
  }

  newCoupon.detailCoupons = selectedProducts.value.map(id => ({ productId: id }));

  if (selectedCoupon.value) {
    await couponStore.updateCoupon({
      ...selectedCoupon.value,
      ...newCoupon,
    });
  } else {
    newCoupon.quantityActual = newCoupon.quantityInitial;
    await couponStore.createCoupon(newCoupon);
  }

  await couponStore.getCoupons();

  newCoupon.name = "";
  newCoupon.couponCode = "";
  newCoupon.startDate = "";
  newCoupon.expiryDate = "";
  newCoupon.quantityInitial = "";
  newCoupon.companyId= "",
  selectedProducts.value = [];

  successMessageVisible.value = true;

  setTimeout(() => {
    successMessageVisible.value = false;
  }, 3000);

  dialog.value = false;
};

const editCoupon = (coupon) => {
  selectedCoupon.value = { ...coupon };
  newCoupon.name = selectedCoupon.value.name;
  newCoupon.couponCode = selectedCoupon.value.couponCode;
  newCoupon.startDate = selectedCoupon.value.startDate;
  newCoupon.expiryDate = selectedCoupon.value.expiryDate;
  newCoupon.quantityInitial = selectedCoupon.value.quantityInitial;
  newCoupon.companyId = selectedCoupon.value.companyId;
  selectedProducts.value = selectedCoupon.value.detailCoupons.map(dc => dc.productId);
  dialog.value = true;
};

const deleteCoupon = (coupon) => {
  couponStore.deleteCoupon(coupon).then(() => {
    couponStore.getCoupons();
  });
};

const closeModal = () => {
  dialog.value = false;
  selectedCoupon.value = null;
  newCoupon.name = "";
  newCoupon.couponCode = "";
  newCoupon.startDate = "";
  newCoupon.expiryDate = "";
  newCoupon.quantityInitial = "";
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
  useCouponStore().getCoupons();
  productStore.getProducts();
  companyStore.getCompanies();
});
</script>
