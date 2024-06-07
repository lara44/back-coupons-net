<template>
  <v-container>
    <h2 class="title">Gestión de Roles</h2>
    <v-form @submit.prevent="submitForm">
      <v-text-field
        v-model="newRole.name"
        label="Nombre Rol"
        :error-messages="getNameErrors"
        :rules="[requiredRule('Nombre Rol')]"
        required
      >
      </v-text-field>
      <v-btn color="primary" type="submit">{{
        selectedRole ? "Actualizar" : "Guardar"
      }}</v-btn>
    </v-form>
    <br />
    <v-data-table
      :headers="headers"
      :items="listRoles"
      :search="search"
      item-key="id"
    >
      <template #item.actions="{ item }">
        <v-btn @click="editRole(item)" color="primary">Editar</v-btn>
      </template>
    </v-data-table>
    <!-- Snackbar para mostrar el mensaje de éxito -->
    <v-snackbar v-model="successMessageVisible" timeout="3000">
      {{
        selectedRole
          ? "Rol actualizado exitosamente"
          : "Rol creado exitosamente"
      }}
    </v-snackbar>
  </v-container>
</template>

<script>
import { ref, reactive, onMounted } from "vue";
import { useRoleStore } from "../../stores/roleStore";

export default {
  name: "RoleDataTable",
  setup() {
    const roleStore = useRoleStore();
    const successMessageVisible = ref(false); // Variable de estado para controlar la visibilidad del mensaje de éxito
    const search = ref("");
    const newRole = reactive({
      name: "",
      email: "",
    });
    const selectedRole = ref(null);

    // Headers de la tabla de datos
    const headers = [
      { text: "ID", value: "id" },
      { text: "Rol", value: "name" },
      { text: "Creado", value: "created_at" },
      { text: "Acciones", value: "actions", sortable: false },
    ];

    // Regla de validación para campos requeridos
    const requiredRule = (fieldName) => (value) =>
      !!value || `El campo "${fieldName}" es obligatorio`;

    // Funciones para obtener mensajes de error específicos
    const getNameErrors = () =>
      newRole.value.name ? null : ['El campo "Rol" es obligatorio'];

    // Obtener la lista de usuarios al montar el componente
    onMounted(() => {
      roleStore.getRoles();
    });

    // Función para enviar el formulario (crea o actualiza el rol)
    const submitForm = async () => {
      if (!newRole.name) {
        return;
      }
      if (selectedRole.value) {
        // Si hay un usuario seleccionado, actualizamos
        await roleStore.updateRole({ ...selectedRole.value, ...newRole });
      } else {
        // Si no hay un usuario seleccionado, creamos uno nuevo
        await roleStore.createRole(newRole);
      }

      // Limpiar los campos del formulario después de crear o actualizar el rol
      newRole.name = "";

      // Mostrar el mensaje de éxito
      successMessageVisible.value = true;

      // Ocultar el mensaje de éxito después de 3 segundos
      setTimeout(() => {
        successMessageVisible.value = false;
      }, 3000);
    };

    // Función para editar un usuario
    const editRole = (role) => {
      selectedRole.value = { ...role };
      newRole.name = selectedRole.value.name;
    };

    return {
      headers,
      listRoles: roleStore.listRoles,
      search,
      newRole,
      successMessageVisible,
      selectedRole,
      submitForm,
      requiredRule,
      getNameErrors,
      editRole,
    };
  },
};
</script>