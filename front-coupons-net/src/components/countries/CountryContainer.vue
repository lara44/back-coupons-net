<template>
  <v-container>
    <v-dialog v-model="dialog" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="headline">{{
            selectedCountry ? "Editar País" : "Crear País"
          }}</span>
        </v-card-title>
        <v-card-text>
          <v-form @submit.prevent="submitForm">
            <v-text-field
              v-model="newCountry.name"
              label="Nombre"
              :rules="[requiredRule('Nombre')]"
              required
            ></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn size="small" class="btn-general" text @click="closeModal">Cancelar</v-btn>
          <v-btn size="small" class="mr-4 btn-general" @click="submitForm">{{
            selectedCountry ? "Actualizar" : "Guardar"
          }}</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="dialogStates" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="headline">Departamentos</span>
        </v-card-title>
        <v-card-text>
          <v-table density="compact">
            <thead>
              <tr>
                <th>ID</th>
                <th>Departamentos</th>
                <th>Número de municipios</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="state in statesByDepartment" :key="state.id">
                <td>{{ state.id }}</td>
                <td>{{ state.name }}</td>
                <td>{{ state.cities.length }}</td>
              </tr>
              <tr v-if="!statesByDepartment.length">
                <td colspan="4">No se encontraron Departamentos</td>
              </tr>
            </tbody>
          </v-table>
        </v-card-text>
        <v-card-actions>
          <v-btn color="blue darken-1" text @click="dialogStates = false"
            >Cerrar</v-btn
          >
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-card>
      <v-card-title>
        <v-row>
          <v-col md="6" sm="6" cols="12">
            <span class="headline">Lista de Países registrados</span>
          </v-col>
          <v-col md="6" sm="6" cols="12">
            <v-text-field
              v-model="search"
              append-icon="mdi-magnify"
              label="Buscar País"
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
              <th>Nombre</th>
              <th>Departamentos</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="country in filteredCountries" :key="country.id">
              <td>{{ country.id }}</td>
              <td>{{ country.name }}</td>
              <td>{{ country.states.length }}</td>
              <td>
                <v-icon
                  @click="getDeparmentsByState(country.states)"
                  class="btn-icon"
                  >mdi-eye</v-icon
                >
                <v-icon @click="editCountry(country)" class="btn-icon"
                  >mdi-pencil</v-icon
                >
                <v-icon @click="deleteCountry(country)" class="btn-icon"
                  >mdi-delete</v-icon
                >
              </td>
            </tr>
            <tr v-if="!filteredCountries.length">
              <td colspan="4">No se encontraron Paises</td>
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
        selectedCountry
          ? "País actualizado exitosamente"
          : "País creado exitosamente"
      }}
    </v-snackbar>
  </v-container>
</template>

<script>
import { ref, reactive, computed } from "vue";
import { useCountryStore } from "../../stores/countryStore";

export default {
  name: "CountryDataTable",
  setup() {
    const statesByDepartment = ref([]);
    const currentPage = ref(1); // Página actual
    const itemsPerPage = 10; // Número de usuarios por página
    const countryStore = useCountryStore();
    const successMessageVisible = ref(false);
    const search = ref("");
    const newCountry = reactive({
      name: "",
    });
    const selectedCountry = ref(null);
    const dialog = ref(false);
    const dialogStates = ref(false);

    const totalCountries = computed(() => countryStore.listCountries.length);
    const totalPages = computed(() =>
      Math.ceil(totalCountries.value / itemsPerPage)
    );

    const filteredCountries = computed(() => {
      const startIndex = (currentPage.value - 1) * itemsPerPage;
      const endIndex = startIndex + itemsPerPage;
      const countryList = countryStore.listCountries;
      return countryList
        .filter((country) =>
          country.name.toLowerCase().includes(search.value.toLowerCase())
        )
        .slice(startIndex, endIndex);
    });

    const requiredRule = (fieldName) => (value) =>
      !!value || `El campo "${fieldName}" es obligatorio`;

    const openModal = () => {
      dialog.value = true;
      selectedCountry.value = null;
      newCountry.name = "";
    };

    const submitForm = async () => {
      if (!newCountry.name) {
        return;
      }
      if (selectedCountry.value) {
        await countryStore.updateCountry({
          ...selectedCountry.value,
          ...newCountry,
        });
      } else {
        await countryStore.createCountry(newCountry);
      }

      await countryStore.getCountries();

      newCountry.name = "";

      successMessageVisible.value = true;

      setTimeout(() => {
        successMessageVisible.value = false;
      }, 3000);

      dialog.value = false;
    };

    const editCountry = (country) => {
      selectedCountry.value = { ...country };
      newCountry.name = selectedCountry.value.name;
      dialog.value = true;
    };

    const deleteCountry = (country) => {
      countryStore
        .deleteCountry(country)
        .then(() => {
          countryStore.getCountries();
        })
        .catch((e) => {
          console.log(e);
        });
    };

    const closeModal = () => {
      dialog.value = false;
      selectedCountry.value = null;
      newCountry.name = "";
    };

    const getDeparmentsByState = (data) => {
      dialogStates.value = true;
      statesByDepartment.value = data;
    };

    return {
      search,
      currentPage,
      itemsPerPage,
      filteredCountries,
      newCountry,
      successMessageVisible,
      selectedCountry,
      dialog,
      submitForm,
      deleteCountry,
      requiredRule,
      editCountry,
      openModal,
      closeModal,
      getDeparmentsByState,
      statesByDepartment,
      dialogStates,
      totalPages,
    };
  },

  async mounted() {
    try {
      await useCountryStore().getCountries();
    } catch (error) {
      console.error(error);
    } finally {
      this.loading = false;
    }
  },
};
</script>