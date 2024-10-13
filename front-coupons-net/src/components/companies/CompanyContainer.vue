<template>
  <v-container>
    <v-dialog v-model="dialog" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="headline">{{
            selectedCompany ? "Editar Empresa" : "Crear Empresa"
          }}</span>
        </v-card-title>
        <v-card-text>
          <v-form @submit.prevent="submitForm">
            <v-text-field
              v-model="newCompany.nit"
              label="Nit"
              :rules="[requiredRule('Nit'), numberdRule]"
              required
            ></v-text-field>

            <v-text-field
              v-model="newCompany.name"
              label="Nombre"
              :rules="[requiredRule('Nombre')]"
              required
            ></v-text-field>

            <v-text-field
              v-model="newCompany.address"
              label="Dirección"
              :rules="[requiredRule('Dirección')]"
              required
            ></v-text-field>

            <v-text-field
              v-model="newCompany.email"
              label="Correo electrónico"
              :rules="[requiredRule('Correo electrónico'), emailRule]"
              required
            ></v-text-field>

            <v-text-field
              v-model="newCompany.phone"
              label="Teléfono"
              :rules="[requiredRule('Teléfono'), numberdRule]"
              required
            ></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn size="small" class="btn-general" @click="closeModal">Cancelar</v-btn>
          <v-btn size="small" class="mr-4 btn-general" @click="submitForm">{{
            selectedCompany ? "Actualizar" : "Guardar"
          }}</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="dialogContacts" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="headline">Contactos</span>
        </v-card-title>
        <v-card-text>
          <v-table density="compact">
            <thead>
              <tr>
                <th>ID</th>
                <th>Nombre</th>
                <th>Telefono</th>
                <th>Dirección</th>
                <th>Email</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="contact in contactsByCompany" :key="contact.id">
                <td>{{ contact.id }}</td>
                <td>{{ contact.name }}</td>
                <td>{{ contact.phone }}</td>
                <td>{{ contact.address }}</td>
                <td>{{ contact.email }}</td>
              </tr>
              <tr v-if="!contactsByCompany.length">
                <td colspan="4">No se encontraron Contactos</td>
              </tr>
            </tbody>
          </v-table>
        </v-card-text>
        <v-card-actions>
          <v-btn color="blue darken-1" text @click="dialogContacts = false"
            >Cerrar</v-btn
          >
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-card>
      <v-card-title>
        <v-row>
          <v-col md="6" sm="6" cols="12">
            <span class="headline">Lista de Empresas registradas</span>
          </v-col>
          <v-col md="6" sm="6" cols="12">
            <v-text-field
              v-model="search"
              append-icon="mdi-magnify"
              label="Buscar Empresa"
              single-line
              hide-details
              variant="underlined"
            ></v-text-field>
          </v-col>
        </v-row>
      </v-card-title>
      <v-btn size="small" class="ma-2 btn-general" dark @click="openModal">Nuevo</v-btn>
      <v-card-text>
        <v-table density="compact">
          <thead>
            <tr>
              <th>ID</th>
              <th>Nit</th>
              <th>Nombre</th>
              <th>Dirección</th>
              <th>Email</th>
              <th>Telefono</th>
              <th>Contactos</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="company in filteredCompanies" :key="company.id">
              <td>{{ company.id }}</td>
              <td>{{ company.nit }}</td>
              <td>{{ company.name }}</td>
              <td>{{ company.address }}</td>
              <td>{{ company.email }}</td>
              <td>{{ company.phone }}</td>
              <td>{{ company.contacts.length }}</td>
              <td>
                <v-icon
                  @click="getContactsByCompany(company.contacts)"
                  class="btn-icon"
                  >mdi-eye</v-icon
                >
                <v-icon @click="editCompany(company)" class="btn-icon"
                  >mdi-pencil</v-icon
                >
                <v-icon @click="deleteCompany(company)" class="btn-icon"
                  >mdi-delete</v-icon
                >
              </td>
            </tr>
            <tr v-if="!filteredCompanies.length">
              <td colspan="4">No se encontraron Empresas</td>
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
      {{
        selectedCompany
          ? "Empresa actualizada exitosamente"
          : "Empresa creada exitosamente"
      }}
    </v-snackbar>
  </v-container>
</template>

<script>
import { ref, reactive, computed } from "vue";
import { useCompanyStore } from "../../stores/companyStore";

export default {
  name: "CompanyDataTable",
  setup() {
    const contactsByCompany = ref([]);
    const currentPage = ref(1); // Página actual
    const itemsPerPage = 10; // Número de usuarios por página
    const companyStore = useCompanyStore();
    const successMessageVisible = ref(false);
    const search = ref("");
    const newCompany = reactive({
      nit: "",
      name: "",
      address: "",
      email: "",
      phone: "",
    });
    const selectedCompany = ref(null);
    const dialog = ref(false);
    const dialogContacts = ref(false);

    const totalCompanies = computed(() => companyStore.listCompanies.length);
    const totalPages = computed(() =>
      Math.ceil(totalCompanies.value / itemsPerPage)
    );

    const filteredCompanies = computed(() => {
      const startIndex = (currentPage.value - 1) * itemsPerPage;
      const endIndex = startIndex + itemsPerPage;
      const companyList = companyStore.listCompanies;
      return companyList
        .filter((company) =>
          company.name.toLowerCase().includes(search.value.toLowerCase())
        )
        .slice(startIndex, endIndex);
    });

    const requiredRule = (fieldName) => (value) =>
      !!value || `El campo "${fieldName}" es obligatorio`;

    const numberdRule = (value) => {
      const numberRegex = /^[0-9]+$/;
      return numberRegex.test(value) || "El campo debe ser un número válido";
    };

    const emailRule = (value) => {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      return (
        emailRegex.test(value) ||
        'El campo "Correo electrónico" debe ser un correo electrónico válido'
      );
    };

    const openModal = () => {
      dialog.value = true;
      selectedCompany.value = null;
      newCompany.nit = "";
      newCompany.name = "";
      newCompany.address = "";
      newCompany.email = "";
      newCompany.phone = "";
    };

    const submitForm = async () => {
      if (
        !newCompany.nit ||
        !newCompany.name ||
        !newCompany.phone ||
        !newCompany.address ||
        !newCompany.email
      ) {
        return;
      }
      if (selectedCompany.value) {
        await companyStore.updateCompany({
          ...selectedCompany.value,
          ...newCompany,
        });
      } else {
        await companyStore.createCompany(newCompany);
      }

      await companyStore.getCompanies();

      newCompany.nit = "";
      newCompany.name = "";
      newCompany.address = "";
      newCompany.email = "";
      newCompany.phone = "";

      successMessageVisible.value = true;

      setTimeout(() => {
        successMessageVisible.value = false;
      }, 3000);

      dialog.value = false;
    };

    const editCompany = (company) => {
      selectedCompany.value = { ...company };
      newCompany.nit = selectedCompany.value.nit;
      newCompany.name = selectedCompany.value.name;
      newCompany.address = selectedCompany.value.address;
      newCompany.email = selectedCompany.value.email;
      newCompany.phone = selectedCompany.value.phone;
      dialog.value = true;
    };

    const deleteCompany = (company) => {
      companyStore
        .deleteCompany(company)
        .then((response) => {
          companyStore.getCompanies();
        })
        .catch((e) => {
          console.log("------------------------------------------", e);
        });
    };

    const closeModal = () => {
      dialog.value = false;
      selectedCompany.value = null;
      newCompany.nit = "";
      newCompany.name = "";
      newCompany.address = "";
      newCompany.email = "";
      newCompany.phone = "";
    };

    const getContactsByCompany = (data) => {
      dialogContacts.value = true;
      contactsByCompany.value = data;
    };

    return {
      search,
      currentPage,
      emailRule,
      itemsPerPage,
      filteredCompanies,
      newCompany,
      successMessageVisible,
      selectedCompany,
      deleteCompany,
      dialog,
      submitForm,
      requiredRule,
      numberdRule,
      editCompany,
      openModal,
      closeModal,
      dialogContacts,
      getContactsByCompany,
      contactsByCompany,
      totalPages,
    };
  },

  async mounted() {
    try {
      await useCompanyStore().getCompanies();
    } catch (error) {
      console.error(error);
    } finally {
      this.loading = false;
    }
  },
};
</script>