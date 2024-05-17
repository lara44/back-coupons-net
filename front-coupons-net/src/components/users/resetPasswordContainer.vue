<template>
  <v-container fluid class="container-principal">
    <v-row justify="center">
      <v-col cols="12" sm="8" md="4">
        <v-card>
          <v-toolbar color="#324c6e" dark flat>
            <v-img
              style="left: 25px"
              max-height="50"
              max-width="130"
              src="../src/assets/neocode.png"
              alt="Vuetify"
            ></v-img>
          </v-toolbar>
          <v-card-title>
            <v-row>
              <v-col md="6" sm="6" cols="12">
                <span class="headline">Restablecer Contraseña</span>
              </v-col>
            </v-row>
          </v-card-title>
          <v-card-text>
            <v-form>
              <v-text-field
                label="Email"
                prepend-icon="mdi-mail"
                type="email"
                v-model="changePasword.email"
                :rules="[requiredRule('Email'), emailRule]"
              >
              </v-text-field>

              <v-text-field
                label="Contraseña Nueva"
                prepend-icon="mdi-lock"
                type="password"
                v-model="changePasword.password"
                :rules="[requiredRule('Contraseña Nueva'), passwordRule]"
              >
              </v-text-field>

              <v-text-field
                label="Confirmar Contraseña"
                prepend-icon="mdi-lock"
                type="password"
                v-model="changePasword.confirmPassword"
                :rules="[
                  requiredRule('Confirmar Contraseña'),
                  passwordRule,
                  passwordMatchRule,
                ]"
              >
              </v-text-field>

              <div
                v-if="userStore.errorMessages.length > 0"
                style="color: red; text-align: center"
              >
                <ul>
                  <li v-for="error in userStore.errorMessages" :key="error">
                    {{ error }}
                  </li>
                </ul>
              </div>
            </v-form>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn class="font-weight-bold" color="#324c6e" text to="/login">
              <v-icon>mdi-arrow-left</v-icon>Volver
            </v-btn>
            <v-btn
              class="font-weight-bold"
              color="#324c6e"
              text
              @click="submitForm"
            >
              <v-icon>mdi-lock</v-icon>Cambiar Contraseña
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
    <v-snackbar v-model="successMessageVisible" timeout="3000">
      {{ notification }}
    </v-snackbar>
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
const changePasword = reactive({
  email: "",
  password: "",
  confirmPassword: "",
  token: router.query.token,
});

const requiredRule = (fieldName) => (value) =>
  !!value || `El campo "${fieldName}" es obligatorio`;

const passwordRule = (value) =>
  value.length >= 6 || "La contraseña debe tener al menos 6 caracteres";

const passwordMatchRule = (value) =>
  value === changePasword.password || "Las contraseñas no coinciden";

const emailRule = (value) => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return (
    emailRegex.test(value) ||
    'El campo "Correo electrónico" debe ser un correo electrónico válido'
  );
};

const submitForm = () => {
  if (
    !changePasword.email ||
    !changePasword.password ||
    !changePasword.confirmPassword
  ) {
    return;
  }

  userStore.resetPassword(changePasword).then((response) => {
    if (response.status === 400) {
      if (response.data.errors) {
        if (response.data?.errors["NewPassword"])
          userStore.errorMessages.push(response.data.errors["NewPassword"]);
        if (response.data?.errors["CurrentPassword"])
          userStore.errorMessages.push(response.data.errors["CurrentPassword"]);
        if (response.data?.errors["Confirm"])
          userStore.errorMessages.push(response.data.errors["Confirm"]);
      } else {
        userStore.errorMessages.push(response.data);
      }
    } else {
      notification.value = "Contraseña Cambiada Exitosamente";
      successMessage();
      userStore.cleanErrors();
    }
  });
};

const successMessage = () => {
  successMessageVisible.value = true;
  setTimeout(() => {
    successMessageVisible.value = false;
  }, 3000);
};
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