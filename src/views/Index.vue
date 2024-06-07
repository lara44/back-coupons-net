<template>
  <v-layout>
    
    <v-app-bar color="#324c6e">
      <v-img
        max-height="50"
        max-width="90"
        src="../src/assets/neocode.png"
        alt="Vuetify"
      ></v-img>
      
      <template v-slot:append>  
        <v-btn
        to="/login"
        >
            Ingresar
          </v-btn>
      </template>
    </v-app-bar>

    <v-main style="background: #eceff1; min-height: 100vh">
      <v-container fluid>
        <v-row>
          <v-col cols="12">
            <v-card>
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
                    <v-card max-width="344">
                      <v-img height="200px" :src="coupon.photo" cover></v-img>
                      <v-card-title>{{ coupon.name }}</v-card-title>
                    <v-card-subtitle>Válido hasta - {{ coupon.expiryDate.substr(0,10) }}</v-card-subtitle>
                      <v-card-actions>
                        <v-btn
                          @click="QrCoupon(coupon)"
                          color="orange-lighten-2"
                          text
                          >Reclamar
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
  <v-footer>
    <span style="color: #324c6e !important; font-weight: 400"
      >Neocode — &copy; {{ new Date().getFullYear() }}</span
    >
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
</style>
