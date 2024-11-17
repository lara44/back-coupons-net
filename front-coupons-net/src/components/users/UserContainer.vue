<template>
  <v-container>
    <v-dialog v-model="dialog" max-width="600px">
      <v-card>
        <v-card-title>
          <span class="headline">{{
            selectedUser ? "Editar Usuario" : "Crear Usuario"
          }}</span>
        </v-card-title>
        <v-card-text>
          <v-form @submit.prevent="submitForm">
            <v-container>
              <v-row>
                <v-col cols="12" sm="6">
                  <v-select
                    v-model="newUser.companyId"
                    :items="companies"
                    item-title="name"
                    item-value="id"
                    label="Empresa"
                    :rules="[requiredRule('Empresa')]"
                    required
                  ></v-select>
                </v-col>
                <v-col cols="12" sm="6">
                  <v-select
                    v-model="newUser.userType"
                    :items="roles"
                    label="Rol"
                    :rules="[requiredRule('Rol')]"
                    outlined
                  />
                </v-col>

                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="newUser.email"
                    label="Correo electrónico"
                    :rules="[requiredRule('Correo electrónico'), emailRule]"
                    required
                  ></v-text-field>
                </v-col>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="newUser.document"
                    label="Identificación"
                    :rules="[requiredRule('Identificación')]"
                    required
                  ></v-text-field>
                </v-col>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="newUser.firstName"
                    label="Nombres"
                    :rules="[requiredRule('Nombres')]"
                    required
                  ></v-text-field>
                </v-col>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="newUser.lastName"
                    label="Apellidos"
                    :rules="[requiredRule('Apellidos')]"
                    required
                  ></v-text-field>
                </v-col>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="newUser.address"
                    label="Dirección"
                    :rules="[requiredRule('Dirección')]"
                    required
                  ></v-text-field>
                </v-col>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="newUser.phoneNumber"
                    label="Teléfono"
                    :rules="[requiredRule('Teléfono'), numberRule, phoneRule]"
                    required
                  ></v-text-field>
                </v-col>
                <v-col v-if="countries.length > 0" cols="12" sm="6">
                  <v-select
                    v-model="newUser.countryId"
                    :items="countries"
                    item-title="name"
                    item-value="id"
                    @update:modelValue="getStates"
                    label="País"
                    :rules="[requiredRule('País')]"
                    required
                  ></v-select>
                </v-col>
                <v-col v-if="states.length > 0" cols="12" sm="6">
                  <v-select
                    v-model="newUser.stateId"
                    :items="states"
                    item-title="name"
                    item-value="id"
                    @update:modelValue="getCities"
                    label="Departamento"
                    :rules="[requiredRule('Departamento')]"
                    required
                  ></v-select>
                </v-col>
                <v-col v-if="cities.length > 0" cols="12" sm="6">
                  <v-select
                    v-model="newUser.cityId"
                    :items="cities"
                    item-title="name"
                    item-value="id"
                    label="Ciudad"
                    :rules="[requiredRule('Ciudad')]"
                    required
                  ></v-select>
                </v-col>
                <v-col v-if="!editScenario" cols="12" sm="6">
                  <v-text-field
                    v-model="newUser.password"
                    label="Contraseña"
                    type="password"
                    :rules="[requiredRule('Contraseña'), passwordRule]"
                    required
                  ></v-text-field>
                </v-col>
                <v-col v-if="!editScenario" cols="12" sm="6">
                  <v-text-field
                    v-model="newUser.passwordConfirm"
                    label="Confirmar Contraseña"
                    type="password"
                    :rules="[
                      requiredRule('Confirmar Contraseña'),
                      passwordRule,
                      passwordMatchRule,
                    ]"
                    required
                  ></v-text-field>
                </v-col>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="newUser.photoFile"
                    @change="handleFileSelect"
                    label="Foto"
                    type="file"
                    :rules="[requiredRule('Foto')]"
                    required
                  ></v-text-field>
                </v-col>
                <v-col v-if="newUser.photoPreview" cols="12" sm="6">
                  <img
                    :src="`data:image/png;base64,${newUser.photoPreview}`"
                    alt="Vista previa de la imagen"
                    width="100"
                  />
                </v-col>
                <v-col v-if="showPhoto" cols="12" sm="6">
                  <img :src="newUser.photoPreviewEdit" width="100" />
                </v-col>
                <v-col cols="12">
                  <div
                    v-if="errorMessages.length > 0"
                    style="color: red; text-align: center"
                  >
                    <ul>
                      <li v-for="error in errorMessages" :key="error">
                        {{ error }}
                      </li>
                    </ul>
                  </div>
                </v-col>
              </v-row>
            </v-container>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn size="small" class="btn-general" text @click="closeModal"
            >Cancelar</v-btn
          >
          <v-btn size="small" class="mr-4 btn-general" @click="submitForm">{{
            selectedUser ? "Actualizar" : "Guardar"
          }}</v-btn>
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
            <v-text-field
              v-model="search"
              append-icon="mdi-magnify"
              label="Buscar Usuario"
              single-line
              hide-details
              variant="underlined"
            ></v-text-field>
          </v-col>
        </v-row>
      </v-card-title>
      <v-btn size="small" class="ma-2 btn-general" dark @click="openModal"
        >Nuevo</v-btn
      >
      <v-card-text>
        <v-table density="compact">
          <thead>
            <tr>
              <th>Documento</th>
              <th>Nombre</th>
              <th>Correo</th>
              <th>Empresa</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="user in filteredUsers" :key="user.id">
              <td>{{ user.document }}</td>
              <td>{{ user.fullName }}</td>
              <td>{{ user.email }}</td>
              <td>{{ user.company.name }}</td>
              <td>
                <v-icon @click="editUser(user)" color="#457b9d"
                  >mdi-pencil</v-icon
                >
                <v-icon @click="deleteUser(user)" color="#457b9d"
                  >mdi-delete</v-icon
                >
                <v-icon
                  @click="emailConfirmation(user.email)"
                  color="#457b9d"
                  title=" Enviar Correo de Confirmanción"
                  >mdi-email</v-icon
                >
              </td>
            </tr>
            <tr v-if="!filteredUsers.length">
              <td colspan="4">No se encontraron usuarios</td>
            </tr>
          </tbody>
        </v-table>
      </v-card-text>
      <v-pagination
        v-model="currentPage"
        rounded="circle"
        :length="totalPages"
        style="box-shadow: none !important"
      ></v-pagination>
    </v-card>

    <!-- Snackbar para mostrar el mensaje de éxito -->
    <v-snackbar v-model="successMessageVisible" timeout="3000">
      {{ notification }}
    </v-snackbar>
  </v-container>
</template>

<script setup>
import { ref, reactive, computed, onMounted } from "vue";
import { useUserStore } from "../../stores/userStore";
import { useCountryStore } from "../../stores/countryStore";
import { useStateStore } from "../../stores/stateStore";
import { useCityStore } from "../../stores/cityStore";
import { useCompanyStore } from "../../stores/companyStore";
import { useToast } from "vue-toastification";

const toast = useToast();
const userStore = useUserStore();
const countryStore = useCountryStore();
const stateStore = useStateStore();
const cityStore = useCityStore();
const companyStore = useCompanyStore();

const countries = computed(() => countryStore.listCountries);
const states = computed(() => stateStore.listStates);
const cities = computed(() => cityStore.listCities);
const companies = computed(() => companyStore.listCompanies);
const editScenario = ref(false);
const showPhoto = ref(true);

const newUser = reactive({
  username: "",
  email: "",
  phoneNumber: "",
  document: "",
  firstName: "",
  lastName: "",
  fullName: "",
  address: "",
  photoFile: "",
  photo: "",
  countryId: "",
  stateId: "",
  cityId: "",
  companyId: "",
  password: "",
  passwordConfirm: "",
  userType: "",
});

const roles = [
  { title: "Administrador", value: 0 },
  { title: "Empresa", value: 1 },
];

const errorMessages = ref([]);
const notification = ref("");
const selectedUser = ref(null);
const currentPage = ref(1); // Página actual
const itemsPerPage = 10; // Número de usuarios por página
const successMessageVisible = ref(false);
const search = ref("");
const dialog = ref(false);
const totalUsers = computed(() => userStore.listUsers.length);
const totalPages = computed(() => Math.ceil(totalUsers.value / itemsPerPage));

const filteredUsers = computed(() => {
  const startIndex = (currentPage.value - 1) * itemsPerPage;
  const endIndex = startIndex + itemsPerPage;
  const userList = userStore.listUsers;
  return userList
    .filter((user) =>
      user.fullName.toLowerCase().includes(search.value.toLowerCase())
    )
    .slice(startIndex, endIndex);
});

const requiredRule = (fieldName) => (value) =>
  !!value || `El campo "${fieldName}" es obligatorio`;

const emailRule = (value) => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return (
    emailRegex.test(value) ||
    'El campo "Correo electrónico" debe ser un correo electrónico válido'
  );
};

const numberRule = (value) => {
  const numberRegex = /^[0-9]+$/;
  return numberRegex.test(value) || "El campo debe ser un número válido";
};

const phoneRule = (value) => {
  return value.length >= 10 || "El teléfono debe tener al menos 10 caracteres";
};

const passwordRule = (value) => {
  return value.length >= 6 || "La contraseña debe tener al menos 6 caracteres";
};

const passwordMatchRule = (value) => {
  return value === newUser.password || "Las contraseñas no coinciden";
};

const submitForm = () => {
  if (selectedUser.value) {
    userStore
      .updateUser({ ...selectedUser.value, ...newUser })
      .then((response) => {
        if (response.status === 400) {
          userStore.errorMessages.push(response.response.data);
        } else {
          toast.success("Usuario actualizado exitosamente");
          notification.value = "Usuario actualizado exitosamente";
          onSave();
        }
      });
  } else {
    newUser.username = newUser.email;
    userStore.createUser(newUser).then((response) => {
      if (response.status === 400) {
        userStore.errorMessages.push(response.response.data);
      } else {
        toast.success("Usuario creado exitosamente");
        notification.value = "Usuario Creado exitosamente";
        onSave();
      }
    });
  }
};

const onSave = () => {
  clearForm();
  userStore.getUsers();
  successMessage();
  dialog.value = false;
};

const editUser = (user) => {
  clearForm();
  editScenario.value = true;
  selectedUser.value = { ...user };
  newUser.phoneNumber = selectedUser.value.phoneNumber;
  newUser.email = selectedUser.value.email;
  newUser.username = selectedUser.value.email;
  newUser.document = selectedUser.value.document;
  newUser.firstName = selectedUser.value.firstName;
  newUser.lastName = selectedUser.value.lastName;
  newUser.address = selectedUser.value.address;
  newUser.cityId = selectedUser.value.cityId;
  newUser.companyId = selectedUser.value.companyId;
  newUser.photoPreviewEdit = selectedUser.value.photo;
  newUser.userType = selectedUser.value.userType;
  dialog.value = true;
};

const deleteUser = (state) => {
  userStore.deleteUser(state);
  toast.success("Usuario eliminado");
  notification.value = "Usuario eliminado";
};

const openModal = () => {
  editScenario.value = false;
  dialog.value = true;
  selectedUser.value = null;
  clearForm();
};

const closeModal = () => {
  dialog.value = false;
  clearForm();
};

const handleFileSelect = (event) => {
  const file = event.target.files[0];
  if (file) {
    const reader = new FileReader();
    reader.onload = (e) => {
      if (editScenario) {
        newUser.photo = e.target.result.split(",")[1];
        newUser.photoPreview = e.target.result.split(",")[1];
        showPhoto.value = false;
      } else {
        newUser.photo = e.target.result.split(",")[1];
        newUser.photoPreview = e.target.result.split(",")[1];
      }
    };
    reader.readAsDataURL(file);
  }
};

const successMessage = () => {
  successMessageVisible.value = true;

  setTimeout(() => {
    successMessageVisible.value = false;
  }, 3000);
};

const clearForm = () => {
  newUser.phoneNumber = "";
  newUser.username = "";
  newUser.email = "";
  newUser.document = "";
  newUser.firstName = "";
  newUser.lastName = "";
  newUser.address = "";
  newUser.countryId = "";
  newUser.stateId = "";
  newUser.cityId = "";
  newUser.companyId = "";
  newUser.password = "";
  newUser.passwordConfirm = "";
  newUser.photoFile = "";
  newUser.photoPreview = "";
  newUser.photo = "";
  newUser.userType = "";
  userStore.cleanErrors();
};

const emailConfirmation = (email) => {
  const user = { email: email };
  userStore.resedToken(user);
  toast.success("Email enviado exitosamente");
  notification.value = "Email enviado exitosamente";
  successMessage();
};

const getStates = () => {
  stateStore.getStatesByCountry(newUser.countryId);
};

const getCities = () => {
  cityStore.getCitieByStates(newUser.stateId);
};

onMounted(() => {
  companyStore.getCompanies();
  countryStore.getCountries();
  userStore.getUsers();
});
</script>
