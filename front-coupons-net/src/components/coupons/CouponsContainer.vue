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
              v-model="newCoupon.name"
              label="Nombre"
              :rules="[requiredRule('Nombre')]"
              required
            ></v-text-field>
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
            <span class="headline">Lista de Cupóns registradas</span>
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
              <th>Nombre</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="coupon in filteredCoupons" :key="coupon.id">
              <td>{{ coupon.id }}</td>
              <td>{{ coupon.name }}</td>
              <td>
                <v-icon @click="editCoupon(coupon)" color="primary"
                  >mdi-pencil</v-icon
                >
                <v-icon @click="deleteCoupon(coupon)" color="primary"
                  >mdi-delete</v-icon
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
import { ref, reactive, computed, onMounted } from "vue";
import { useCouponStore } from "../../stores/couponStore";

const currentPage = ref(1); // Página actual
const itemsPerPage = 10; // Número de usuarios por página
const couponStore = useCouponStore();
const successMessageVisible = ref(false);
const search = ref("");
const newCoupon = reactive({
  name: "",
});
const selectedCoupon = ref(null);
const dialog = ref(false);

const totalCoupons = computed(() => couponStore.listCoupons.length);
const totalPages = computed(() =>
  Math.ceil(totalCoupons.value / itemsPerPage)
);

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

const openModal = () => {
  dialog.value = true;
  selectedCoupon.value = null;
  newCoupon.name = "";
};

const submitForm = async () => {
  if (!newCoupon.name) {
    return;
  }
  if (selectedCoupon.value) {
    await couponStore.updateCoupon({
      ...selectedCoupon.value,
      ...newCoupon,
    });
  } else {
    await couponStore.createCoupon(newCoupon);
  }

  await couponStore.getCoupons();

  newCoupon.name = "";

  successMessageVisible.value = true;

  setTimeout(() => {
    successMessageVisible.value = false;
  }, 3000);

  dialog.value = false;
};

const editCoupon = (coupon) => {
  selectedCoupon.value = { ...coupon };
  newCoupon.name = selectedCoupon.value.name;
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
};

onMounted(() => {
  useCouponStore().getCoupons();
});
</script>