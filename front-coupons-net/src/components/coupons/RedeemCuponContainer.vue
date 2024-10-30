<template>
  <v-container fluid class="container-principal">
    <v-row justify="center">
      <v-col cols="12" sm="8" md="4">
        <v-card
          append-icon="mdi-open-in-new"
          to="/home"
          max-width="700"
          prepend-icon="mdi-check-decagram"
          rel="noopener"
          subtitle="Click para volver al home"
          :title="notification"
        >
          <v-toolbar color="#324c6e" dark flat>
            <v-img
              style="left: 25px"
              max-height="50"
              max-width="130"
              src="../src/assets/neocode.png"
              alt="Vuetify"
            ></v-img>
          </v-toolbar>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import { ref, reactive, computed, onMounted } from "vue";
import { useCouponStore } from "../../stores/couponStore";
import { useRoute } from "vue-router";

const router = useRoute();
const couponStore = useCouponStore();
const notification = ref("");
const successMessageVisible = ref(false);

onMounted(() => {
  const redeem = {
    couponId: router.query.couponId,
    clientId: router.query.clientId,
    signature: router.query.signature,
  };

  couponStore.redeemCoupon(redeem).then((response) => {
    if (response.status == 200) {
      notification.value = "Cupón Canjeado Exitosamente";
    } else {
      notification.value = "El Cupón ya fue canjeado";
      console.log(response);
    }
  });
});
</script>

<style>
.container-principal {
  min-height: 100vh;
  background: #eceff1;
  overflow: hidden;
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>
