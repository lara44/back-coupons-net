<template>
  <v-container>
    <v-card class="pa-8" max-width="600">
      <v-card-title class="text-h5 text-center">Reclamar Cupon</v-card-title>
      <v-card-subtitle class="mb-4 text-h12 text-left">
        Para reclamar el cupon, ingresa tus datos
      </v-card-subtitle>

      <v-alert v-if="redeem === false" type="error" class="mb-4" dismissible>
        Ya existe el cupón asociado a tu identificación
      </v-alert>

      <v-form @submit.prevent="handleRegister" ref="formRef">
        <v-text-field
          v-model="form.identification"
          label="Identificacion"
          :error="!!errors.identification"
          :error-messages="errors.identification"
          @blur="validateField('identification')"
          outlined
          required
        />

        <v-text-field
          v-model="form.firstName"
          label="Nombre"
          :error="!!errors.firstName"
          :error-messages="errors.firstName"
          @blur="validateField('firstName')"
          outlined
          required
        />

        <v-text-field
          v-model="form.lastName"
          label="Apellido"
          :error="!!errors.lastName"
          :error-messages="errors.lastName"
          @blur="validateField('lastName')"
          outlined
          required
        />

        <v-text-field
          v-model="form.phone"
          label="Celular"
          :error="!!errors.phone"
          :error-messages="errors.phone"
          @blur="validateField('phone')"
          outlined
          required
        />

        <v-text-field
          v-model="form.email"
          label="Email"
          :error="!!errors.email"
          :error-messages="errors.email"
          @blur="validateField('email')"
          outlined
          required
        />

        <v-btn type="submit" color="primary" class="mt-4" block>
          Reclamar Cupon
        </v-btn>
      </v-form>
    </v-card>
  </v-container>
</template>

<script setup>
import { useToast } from "vue-toastification";
import { useRouter } from "vue-router";
import { useClientForm } from "./composables/useClientForm";
import { ref } from "vue";

const props = defineProps(["couponCode"]);
const { form, errors, validateField, validateForm, manageClient } =
  useClientForm();
const router = useRouter();
const toast = useToast();
const formRef = ref(null);
const redeem = ref(null);
const showAlert = ref(false); // Controlar la visibilidad de la alerta

const handleRegister = async () => {
  const isValid = await validateForm();
  if (isValid) {
    redeem.value = await manageClient(props.couponCode);
    if (redeem.value === true) {
      toast.success("Cupon reclamado exitosamente");
      router.push("/consultar-cupones");
    } else {
      showAlert.value = true;
      setTimeout(() => {
        showAlert.value = false;
      }, 3000);
    }
  }
};
</script>
