<template>
  <Header />

  <v-container class="mt-8">
    <v-card class="pa-4" elevation="2">
      <v-card-title class="text-h5 text-center">
        Consulta tus cupones
      </v-card-title>

      <v-form @submit.prevent="searchCoupons">
        <v-text-field
          v-model="identification"
          label="Identificación"
          :error="!!errors.identification"
          :error-messages="errors.identification"
          outlined
          required
        />

        <v-btn type="submit" color="primary" class="mt-4" block>
          Buscar Cupones
        </v-btn>
      </v-form>
    </v-card>

    <v-card v-if="coupons.length > 0" class="mt-6" elevation="2">
      <v-table density="compact">
        <thead>
          <tr>
            <th>ID</th>
            <th>Cupón</th>
            <th>Código</th>
            <th>Fecha de Vencimiento</th>
            <th>Productos</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="coupon in coupons" :key="coupon.id">
            <td>{{ coupon.id }}</td>
            <td>{{ coupon.coupon.name }}</td>
            <td>{{ coupon.coupon.couponCode }}</td>
            <td>{{ coupon.coupon.expiryDate.substr(0, 10) }}</td>
            <td>
              <ul class="product-list">
                <li
                  v-for="detail in coupon.coupon.detailCoupons"
                  :key="detail.id"
                >
                  {{ detail.product.name }}
                </li>
              </ul>
            </td>
            <td>
              <v-icon @click="QrCoupon(coupon)" class="btn-icon"
                >mdi-qrcode</v-icon
              >
            </td>
          </tr>
        </tbody>
      </v-table>
    </v-card>

    <v-alert
      v-else-if="searched && coupons.length === 0"
      type="info"
      class="mt-6"
    >
      No se encontraron cupones para esta identificación.
    </v-alert>
  </v-container>

  <Footer class="mt-14" />
</template>

<script setup>
import Header from "../../views/Header.vue";
import Footer from "../../views/Footer.vue";
import { useCouponsByClient } from "./composables/useCouponsByClient";
import { ref } from "vue";

const {
  identification,
  coupons,
  errors,
  searchCoupons: search,
  QrCoupon,
} = useCouponsByClient();

const searched = ref(false);

const searchCoupons = async () => {
  const response = await search();
  searched.value = response;
};
</script>
