import { ref } from "vue";
import * as yup from "yup";
import axios from "axios";

// Esquema de validación utilizando Yup
const validationSchema = yup.object().shape({
  identification: yup
    .string()
    .required("La identificación es obligatoria")
    .matches(/^[0-9]+$/, "La identificación solo debe contener números")
    .min(10, "La identificación debe tener al menos 10 caracteres")
    .max(10, "La identificación debe tener menos de 10 caracteres"),
  firstName: yup.string().required("El nombre es obligatorio"),
  lastName: yup.string().required("El apellido es obligatorio"),
  phone: yup
    .string()
    .required("El teléfono es obligatorio")
    .matches(/^[0-9]+$/, "El teléfono solo debe contener números")
    .min(10, "El teléfono debe tener al menos 10 caracteres")
    .max(15, "El teléfono debe tener menos de 15 caracteres"),
  email: yup
    .string()
    .email("Formato de email inválido")
    .required("El email es obligatorio"),
});

export const useClientForm = () => {
  const form = ref({
    identification: "",
    firstName: "",
    lastName: "",
    phone: "",
    email: "",
    id: 0,
    state: "1",
  });

  const errors = ref({});

  const fetchClientData = async () => {
    if (!form.value.identification) return;

    try {
      const response = await axios.get(
        `/api/clients/${form.value.identification}`
      );
      if (response.data) {
        const client = response.data;
        form.value = { ...client };
      }
    } catch (error) {
      console.error("Error al buscar el cliente:", error);
    }
  };

  const manageClient = async (couponCode) => {
    try {
      const method = form.value.id !== 0 ? "put" : "post";
      const url = form.value.id
        ? `/api/clients/${form.value.id}`
        : `/api/clients`;

      const { data } = await axios({
        method,
        url,
        data: {
          identification: form.value.identification,
          firstName: form.value.firstName,
          lastName: form.value.lastName,
          phone: form.value.phone,
          email: form.value.email,
          state: form.value.state,
          id: form.value.id,
        },
      });

      console.log("Cliente manejado exitosamente.");

      // Reclama el cupón después de crear o actualizar al cliente
      await claimCoupon(couponCode, data.data.id);
    } catch (error) {
      console.error("Error al manejar el cliente:", error);
    }
  };

  const claimCoupon = async (couponCode, clientId) => {
    try {
      const response = await axios.post("/api/redeem/ClaimCustomerCoupon", {
        code: couponCode,
        clientId: clientId,
      });
      console.log("Cupón reclamado exitosamente:", response.data);
    } catch (error) {
      console.error("Error al reclamar el cupón:", error);
    }
  };

  const validateField = async (field) => {
    try {
      await validationSchema.validateAt(field, form.value);
      errors.value[field] = "";

      if (field === "identification") {
        await fetchClientData();
      }
    } catch (validationError) {
      errors.value[field] = validationError.message;
    }
  };

  const validateForm = async () => {
    try {
      errors.value = {};
      await validationSchema.validate(form.value, { abortEarly: false });
      return true;
    } catch (validationError) {
      validationError.inner.forEach((error) => {
        errors.value[error.path] = error.message;
      });
      return false;
    }
  };

  return {
    form,
    errors,
    validateField,
    validateForm,
    manageClient,
  };
};
