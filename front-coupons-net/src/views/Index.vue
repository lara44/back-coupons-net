<template>
  <v-layout>

    <v-app-bar color="#1d3557">
      <v-img max-height="80" max-width="150" src="/neocode.png" alt="Vuetify" style="margin-left: 5em;"></v-img>
      <v-spacer></v-spacer>
      <v-responsive max-width="200">
        <v-text-field density="compact" label="Buscar cupones" rounded="lg" variant="solo-filled" flat hide-details
          single-line></v-text-field>
      </v-responsive>
      <template v-slot:append>
        <v-btn to="/login" style="margin-right: 6em;" class="bold-text">
          Ingresar
        </v-btn>
      </template>
    </v-app-bar>

    <v-main style="background: #eceff1; min-height: 100vh">
      <v-container fluid style="width: 100%;">
        <v-row style="width: 100%;">
          <v-col cols="12" style="padding: 1% 5% 5% 5%;">
            <v-card style="background-color: #eceff1;">
              <v-card-text>
                <v-row>
                  <v-col v-for="coupon in couponStore.listCoupons" :key="coupon.id" cols="12" sm="6" md="4" lg="3">
                    <v-card max-width="344" class="custom-card">
                      <v-img height="200px" :src="coupon.photo" cover></v-img>
                      <v-card-title class="card-title regular-text">{{ coupon.name }}</v-card-title>
                      <v-card-subtitle class="card-subtitle regular-text">Válido hasta: {{ coupon.expiryDate.substr(0,
                        10) }}</v-card-subtitle>
                      <v-card-actions style="padding: 0% 1% 5% 1%;">
                        <v-btn 
                        style="font-size: 1rem !important;"
                          class="bold-text" 
                          color="#E63946" 
                          text 
                          @click="QrCoupon(coupon)">Reclamar
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
  </v-layout>
  <!-- <v-footer dense bottom absolute width="100%"> -->
  <v-footer style="background-color: #a8dadc; padding: 2% 6% 2% 6%;">
    <span 
      class="bold-text"
      style="color: #1d3557 !important; margin: auto;"
    >Neocode — &copy; {{ new Date().getFullYear() }}</span>
  </v-footer>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useCouponStore } from "../stores/couponStore";
import { useRoute } from "vue-router";
import QRCode from "qrcode";
import { saveAs } from "file-saver";

const router = useRoute();
const couponStore = useCouponStore();
const notification = ref("");
const successMessageVisible = ref(false);

onMounted(() => {
  couponStore.getCoupons();
});

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

<style>
.neocode-logo {
  position: relative;
  left: 25px;
}

.ingresar-btn {
  top: 2px;
  /* Ajusta este valor según sea necesario */
}

.custom-card {
  -webkit-border-radius: 9px !important;
  border-radius: 9px !important;
  background: #ffffff !important;
  -webkit-box-shadow: 8px 8px 13px #d2d2d7, -8px -8px 13px #ffffff !important;
  box-shadow: 8px 8px 13px #d2d2d7, -8px -8px 13px #ffffff !important;
}

.card-title {
  padding: 1% 2% 0% 2% !important;
  font-size: 1.063rem !important;
}
.card-subtitle {
  padding: 0% 2% 0% 2% !important;
  font-size: .883rem !important;
}


</style>
