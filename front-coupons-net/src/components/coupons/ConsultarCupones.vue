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
      <v-table density="compact" class="elevation-1 rounded">
        <thead>
          <tr>
            <th class="text-center text-white bg-primary pa-3 font-weight-bold">
              #
            </th>
            <th class="text-center text-white bg-primary pa-3 font-weight-bold">
              Empresa
            </th>
            <th class="text-center text-white bg-primary pa-3 font-weight-bold">
              Cupón
            </th>
            <th class="text-center text-white bg-primary pa-3 font-weight-bold">
              Código
            </th>
            <th class="text-center text-white bg-primary pa-3 font-weight-bold">
              Fecha de Vencimiento
            </th>
            <th class="text-center text-white bg-primary pa-3 font-weight-bold">
              Estado
            </th>
            <th class="text-center text-white bg-primary pa-3 font-weight-bold">
              Productos
            </th>
            <th class="text-center text-white bg-primary pa-3 font-weight-bold">
              Acciones
            </th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(coupon, index) in coupons" :key="coupon.id">
            <td>{{ index + 1 }}</td>
            <td>{{ coupon.coupon.nameCompany }}</td>
            <td>{{ coupon.coupon.name }}</td>
            <td>{{ coupon.coupon.couponCode }}</td>
            <td>{{ coupon.coupon.expiryDate.substr(0, 10) }}</td>
            <td>
              <v-chip
                :color="
                  coupon.coupon.state === 0
                    ? 'blue lighten-4'
                    : 'green lighten-4'
                "
                :text-color="
                  coupon.coupon.state === 0 ? 'blue darken-2' : 'green darken-2'
                "
                class="rounded-pill px-3"
              >
                {{ coupon.coupon.state === 0 ? "Reclamado" : "Canjeado" }}
              </v-chip>
            </td>
            <td>
              <v-chip-group column class="product-list">
                <v-chip
                  v-for="detail in coupon.coupon.detailCoupons"
                  :key="detail.id"
                  class="ma-1"
                >
                  {{ detail.product.name }}
                </v-chip>
              </v-chip-group>
            </td>
            <td>
              <v-icon
                v-if="coupon.coupon.state === 0"
                @click="QrCoupon(coupon)"
                class="btn-icon clickable-icon mr-2"
              >
                mdi-qrcode
              </v-icon>
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
