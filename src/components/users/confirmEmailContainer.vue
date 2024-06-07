<template>
  <v-container fluid class="container-principal">
    <v-row justify="center">
      <v-col cols="12" sm="8" md="4">
        <v-card
          append-icon="mdi-open-in-new"
          to="/login"
          max-width="700"
          prepend-icon="mdi-check-decagram"
          rel="noopener"
          subtitle="Click para volver al login"
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
import { useUserStore } from "../../stores/userStore";
import { useRoute } from "vue-router";

const router = useRoute();
const userStore = useUserStore();
const notification = ref("");
const successMessageVisible = ref(false);

onMounted(() => {
  const changePasword = {
    userid: router.query.userid,
    token: router.query.token,
  };

  userStore.confirmEmail(changePasword).then((response) => {
    if (response.status == 200) {
      notification.value = "Email Confirmado Exitosamente";
    } else {
      notification.value =
        "Error Confirmando el Email, Contacte el adminsitrador";
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
