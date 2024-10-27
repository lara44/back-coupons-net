<template>
  <v-container>
    <v-card class="pa-8" max-width="600">
      <v-card-title class="text-h5 text-center">Reclamar Cupon</v-card-title>
      <v-card-subtitle class="mb-4 text-h12 text-left">
        Para reclamar el cupon, ingresa tus datos
      </v-card-subtitle>

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
import { useRouter } from "vue-router";
import { useClientForm } from "./composables/useClientForm";
import { ref } from "vue";

const props = defineProps(["couponCode"]);
const { form, errors, validateField, validateForm, manageClient } =
  useClientForm();
const router = useRouter();
const formRef = ref(null);

const handleRegister = async () => {
  const isValid = await validateForm();
  if (isValid) {
    await manageClient(props.couponCode);
    router.push("/consultar-cupones");
  }
};
</script>
