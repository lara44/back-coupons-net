<template>
  <v-container>
    <v-dialog v-model="dialog" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="headline">{{
            selectedProduct ? "Editar Producto" : "Crear Producto"
          }}</span>
        </v-card-title>
        <v-card-text>
          <v-form @submit.prevent="submitForm">
            <v-select
              v-if="userStore.role === 'Admin'"
              v-model="newProduct.companyId"
              :items="companies"
              item-title="name"
              item-value="id"
              label="Empresa"
              :rules="[requiredRule('Empresa')]"
              required
            ></v-select>

            <v-text-field
              v-model="newProduct.name"
              label="Nombre"
              :rules="[requiredRule('Nombre')]"
              required
            ></v-text-field>
            <v-text-field
              v-model="newProduct.price"
              label="Precio"
              :rules="[requiredRule('Precio'), priceRule]"
              required
            ></v-text-field>
            <v-text-field
              v-model="newProduct.barcode"
              label="Código de barras"
              :rules="[requiredRule('Código de Barras'), barcodeRule]"
              required
            ></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn size="small" class="btn-general" text @click="closeModal"
            >Cancelar</v-btn
          >
          <v-btn size="small" class="mr-2 btn-general" @click="submitForm">{{
            selectedProduct ? "Actualizar" : "Guardar"
          }}</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <!-- Tabla HTML para mostrar los usuarios -->
    <v-card>
      <v-card-title>
        <v-row>
          <v-col md="6" sm="6" cols="12">
            <span class="headline">Lista de Productos registrados</span>
          </v-col>
          <v-col md="6" sm="6" cols="12">
            <v-text-field
              v-model="search"
              append-icon="mdi-magnify"
              label="Buscar Producto"
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
              <th>Nombre</th>
              <th>Precio</th>
              <th>Código de Barras</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="product in filteredProducts" :key="product.id">
              <td>{{ product.id }}</td>
              <td>{{ product.name }}</td>
              <td>{{ product.price }}</td>
              <td>{{ product.barcode }}</td>
              <td>
                <v-icon @click="editProduct(product)" class="btn-icon"
                  >mdi-pencil</v-icon
                >
                <v-icon @click="deleteProduct(product)" class="btn-icon"
                  >mdi-delete</v-icon
                >
              </td>
            </tr>
            <tr v-if="!filteredProducts.length">
              <td colspan="4">No se encontraron Productos</td>
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
        selectedProduct
          ? "Producto actualizado exitosamente"
          : "Producto creado exitosamente"
      }}
    </v-snackbar>
  </v-container>
</template>

<script setup>
import { ref, reactive, computed, onMounted } from "vue";
import { useProductStore } from "../../stores/productStore";
import { useUserStore } from "../../stores/userStore";
import { useCompanyStore } from "../../stores/companyStore";
import { jwtDecode } from "jwt-decode";
import { useToast } from "vue-toastification";

const toast = useToast();

const currentPage = ref(1); // Página actual
const itemsPerPage = 10; // Número de usuarios por página
const productStore = useProductStore();
const companyStore = useCompanyStore();
const userStore = useUserStore();
const successMessageVisible = ref(false);
const search = ref("");
const newProduct = reactive({
  name: "",
  price: "",
  barcode: "",
  companyId: "",
  DetailCoupons: [],
});
const selectedProduct = ref(null);
const dialog = ref(false);

const companies = computed(() => companyStore.listCompanies);
const totalProducts = computed(() => productStore.listProducts.length);
const totalPages = computed(() =>
  Math.ceil(totalProducts.value / itemsPerPage)
);

const filteredProducts = computed(() => {
  const startIndex = (currentPage.value - 1) * itemsPerPage;
  const endIndex = startIndex + itemsPerPage;
  const productList = productStore.listProducts;
  return productList
    .filter((product) =>
      product.name.toLowerCase().includes(search.value.toLowerCase())
    )
    .slice(startIndex, endIndex);
});

const requiredRule = (fieldName) => (value) =>
  !!value || `El campo "${fieldName}" es obligatorio`;

const priceRule = (value) => {
  const numberRegex = /^[0-9]+$/;
  return numberRegex.test(value) || "El campo debe ser un número válido";
};

const barcodeRule = (value) => {
  return (
    value.length >= 8 || "El código de Barras debe tener al menos 8 caracteres"
  );
};

const openModal = () => {
  dialog.value = true;
  selectedProduct.value = null;
  newProduct.name = "";
  newProduct.price = "";
  userStore.role === "Admin"
    ? (newProduct.companyId = "")
    : (newProduct.companyId = userStore.companyId);
};

const submitForm = async () => {
  if (!newProduct.name || !newProduct.price || !newProduct.barcode) {
    return;
  }
  if (selectedProduct.value) {
    await productStore.updateProduct({
      ...selectedProduct.value,
      ...newProduct,
    });
    toast.success("Producto actualizado exitosamente");
  } else {
    await productStore.createProduct(newProduct);
    toast.success("Producto creado exitosamente");
  }

  userStore.role === "Admin"
    ? useProductStore().getProducts()
    : useProductStore().getProductsByCompany(userStore.companyId);

  newProduct.name = "";
  newProduct.price = "";
  newProduct.barcode = "";
  newProduct.companyId = "";

  dialog.value = false;
};

const deleteProduct = (product) => {
  productStore.deleteProduct(product);

  userStore.role === "Admin"
    ? useProductStore().getProducts()
    : useProductStore().getProductsByCompany(userStore.companyId);
};

const editProduct = (product) => {
  selectedProduct.value = { ...product };
  newProduct.name = selectedProduct.value.name;
  newProduct.price = selectedProduct.value.price;
  newProduct.barcode = selectedProduct.value.barcode;
  newProduct.companyId = selectedProduct.value.companyId;
  dialog.value = true;
};

const closeModal = () => {
  dialog.value = false;
  selectedProduct.value = null;
  newProduct.name = "";
  newProduct.price = "";
  newProduct.barcode = "";
  newProduct.companyId = "";
};

onMounted(() => {
  try {
    const token = localStorage.getItem("spa_token");
    const decodedToken = jwtDecode(token);
    const company = decodedToken["CompanyId"];

    userStore.role === "Admin"
      ? useProductStore().getProducts()
      : useProductStore().getProductsByCompany(company);

    companyStore.getCompanies();
  } catch (error) {
    console.error(error);
  }
});
</script>
