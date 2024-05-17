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
                <span class="headline">Olvidé mi Contraseña</span>
              </v-col>
            </v-row>
          </v-card-title>
          <v-card-text>
            <v-form>
              <v-text-field
                label="Email"
                prepend-icon="mdi-mail"
                v-model="forgotPassword.email"
                :rules="[requiredRule('Email'), emailRule]"
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
              <v-icon>mdi-send</v-icon>Enviar
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

const userStore = useUserStore();
const notification = ref("");
const successMessageVisible = ref(false);
const forgotPassword = reactive({
  email: "",
});

const emailRule = (value) => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return (
    emailRegex.test(value) ||
    'El campo "Correo electrónico" debe ser un correo electrónico válido'
  );
};

const requiredRule = (fieldName) => (value) =>
  !!value || `El campo "${fieldName}" es obligatorio`;

const submitForm = () => {
  if (!forgotPassword.email) {
    return;
  }

  userStore.recoverPassword(forgotPassword).then((response) => {
    if (response.status === 400) {
      console.log(response);
      if (response.data.errors) {
        if (response.data?.errors["Email"])
          userStore.errorMessages.push(response.data.errors["Email"]);
      } else {
        userStore.errorMessages.push(response.data);
      }
    } else {
      notification.value = "Correo enviado Exitosamente";
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
