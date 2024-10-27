<template>
  <v-app>
    <Header />
    <v-main style="background: #eceff1; min-height: calc(100vh - 128px)">
      <v-container fluid>
        <v-row>
          <v-col cols="12" style="padding: 1% 5% 5% 5%">
            <v-card style="background-color: #eceff1">
              <v-card-text>
                <v-row>
                  <v-col
                    v-for="coupon in couponStore.listCoupons"
                    :key="coupon.id"
                    cols="12"
                    sm="6"
                    md="4"
                    lg="3"
                  >
                    <v-card max-width="344" class="custom-card">
                      <v-img height="200px" :src="coupon.photo" cover></v-img>
                      <v-card-title class="card-title">{{
                        coupon.name
                      }}</v-card-title>
                      <v-card-subtitle class="card-subtitle">
                        Válido hasta: {{ coupon.expiryDate.substr(0, 10) }}
                      </v-card-subtitle>
                      <v-card-actions style="padding: 0% 1% 5% 1%">
                        <v-btn
                          style="font-size: 1rem !important"
                          class="bold-text"
                          color="#E63946"
                          text
                          @click="openClientForm(coupon.couponCode)"
                        >
                          Reclamar
                        </v-btn>
                      </v-card-actions>
                    </v-card>
                  </v-col>
                </v-row>
              </v-card-text>
            </v-card>
          </v-col>
        </v-row>
      </v-container>
    </v-main>
    <Footer />
    <v-dialog v-model="isClientFormOpen" width="600" max-width="600">
      <ClientForm
        :couponCode="selectedCouponCode"
        @close="isClientFormOpen = false"
      />
    </v-dialog>
  </v-app>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useCouponStore } from "../stores/couponStore";
import { useRoute } from "vue-router";
import QRCode from "qrcode";
import { saveAs } from "file-saver";
import ClientForm from "../components/clients/ClientForm.vue";
import Header from "./Header.vue";
import Footer from "./Footer.vue";

const router = useRoute();
const couponStore = useCouponStore();
const isClientFormOpen = ref(false);
const selectedCouponCode = ref(null);

onMounted(() => {
  couponStore.getCoupons();
});

const openClientForm = (couponCode) => {
  selectedCouponCode.value = couponCode;
  isClientFormOpen.value = true;
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
</script>

<style scoped>
.custom-card {
  border-radius: 9px !important;
  background: #ffffff !important;
  box-shadow: 8px 8px 13px #d2d2d7, -8px -8px 13px #ffffff !important;
}

.card-title {
  padding: 1% 2% 0% 2% !important;
  font-size: 1.063rem !important;
}

.card-subtitle {
  padding: 0% 2% 0% 2% !important;
  font-size: 0.883rem !important;
}
</style>
