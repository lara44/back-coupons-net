<template>
  <v-container>
    <v-dialog v-model="dialog" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="headline">{{ selectedUser ? 'Editar Usuario' : 'Crear Usuario' }}</span>
        </v-card-title>
        <v-card-text>
          <v-form @submit.prevent="submitForm">
            <v-text-field v-model="newUser.name" label="Nombre" 
              :rules="[requiredRule('Nombre')]" required></v-text-field>
            <v-text-field v-model="newUser.email" label="Correo electrónico"
              :rules="[requiredRule('Correo electrónico'), emailRule]" required></v-text-field>
            <v-text-field v-model="newUser.password" label="Contraseña"
              :rules="[requiredRule('Contraseña'), passwordRule]" required></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn color="blue darken-1" text @click="closeModal">Cancelar</v-btn>
          <v-btn color="primary" @click="submitForm">{{ selectedUser ? 'Actualizar' : 'Guardar' }}</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <!-- Tabla HTML para mostrar los usuarios -->
    <v-card>
      <v-card-title>
        <v-row>
          <v-col md="6" sm="6" cols="12">
            <span class="headline">Lista de usuarios registrados</span>
          </v-col>
          <v-col md="6" sm="6" cols="12">
            <v-text-field v-model="search" append-icon="mdi-magnify" label="Buscar Usuario" single-line hide-details
              variant="underlined"></v-text-field>
          </v-col>
        </v-row>
      </v-card-title>
      <v-btn class="ma-2" color="primary" dark @click="openModal" >Nuevo</v-btn>
      <v-card-text>
        <v-table density="compact">
          <thead>
            <tr>
              <th>ID</th>
              <th>Nombre</th>
              <th>Correo</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="user in filteredUsers" :key="user.id">
              <td>{{ user.id }}</td>
              <td>{{ user.name }}</td>
              <td>{{ user.email }}</td>
              <td>
                <v-icon @click="editUser(user)" color="primary">mdi-pencil</v-icon>
              </td>
            </tr>
            <tr v-if="!filteredUsers.length">
              <td colspan="4">No se encontraron usuarios</td>
            </tr>
          </tbody>
        </v-table>
      </v-card-text>
      <v-pagination v-model="currentPage" rounded="circle" :length="totalPages" style="box-shadow: none !important;"></v-pagination>
    </v-card>

    <!-- Snackbar para mostrar el mensaje de éxito -->
    <v-snackbar v-model="successMessageVisible" timeout="3000">
      {{ selectedUser ? 'Usuario actualizado exitosamente' : 'Usuario creado exitosamente' }}
    </v-snackbar>
  </v-container>
</template>

<script>
import { ref, reactive, computed } from 'vue';
import { useUserStore } from '../../stores/userStore';

export default {
  name: 'UserDataTable',
  setup() {
    const currentPage = ref(1); // Página actual
    const itemsPerPage = 10; // Número de usuarios por página
    const userStore = useUserStore();
    const successMessageVisible = ref(false);
    const search = ref('');
    const newUser = reactive({
      name: '',
      email: '',
      password: '',
    });
    const selectedUser = ref(null);
    const dialog = ref(false);

    const totalUsers = computed(() => userStore.listUsers.length);
    const totalPages = computed(() => Math.ceil(totalUsers.value / itemsPerPage));

    const filteredUsers = computed(() => {
      const startIndex = (currentPage.value - 1) * itemsPerPage;
      const endIndex = startIndex + itemsPerPage;
      const userList = userStore.listUsers;
      return userList
        .filter(user => user.name.toLowerCase().includes(search.value.toLowerCase()))
        .slice(startIndex, endIndex);
    });

    const requiredRule = (fieldName) => (value) => !!value || `El campo "${fieldName}" es obligatorio`;

    const emailRule = (value) => {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      return emailRegex.test(value) || 'El campo "Correo electrónico" debe ser un correo electrónico válido';
    };

    const passwordRule = (value) => {
      return value.length >= 8 || 'La contraseña debe tener al menos 8 caracteres';
    };

 

    const openModal = () => {
      dialog.value = true;
      selectedUser.value = null;
      newUser.name = '';
      newUser.email = '';
      newUser.password = '';
    };

    const submitForm = async () => {
      if (!newUser.name || !newUser.email) {
        return;
      }
      if (selectedUser.value) {
        await userStore.updateUser({ ...selectedUser.value, ...newUser });
      } else {
        await userStore.createUser(newUser);
      }

      await userStore.getUsers();

      newUser.name = '';
      newUser.email = '';
      newUser.password = '';

      successMessageVisible.value = true;

      setTimeout(() => {
        successMessageVisible.value = false;
      }, 3000);

      dialog.value = false;
    };

    const editUser = (user) => {
      selectedUser.value = { ...user };
      newUser.name = selectedUser.value.name;
      newUser.email = selectedUser.value.email;
      dialog.value = true;
    };

    const closeModal = () => {
      dialog.value = false;
      selectedUser.value = null;
      newUser.name = '';
      newUser.email = '';
      newUser.password = '';
    };

    return {
      search,
      currentPage,
      itemsPerPage,
      filteredUsers,
      newUser,
      successMessageVisible,
      selectedUser,
      dialog,
      submitForm,
      requiredRule,
      emailRule,
      passwordRule,
      editUser,
      openModal,
      closeModal,
      totalPages
    };
  },

  async mounted() {
    try {
      await useUserStore().getUsers();
    } catch (error) {
      console.error(error);
    } finally {
      this.loading = false;
    }
  },
};
</script>