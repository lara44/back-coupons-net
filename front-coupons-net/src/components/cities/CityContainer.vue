<template>
  <v-container>
    <v-dialog v-model="dialog" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="headline">{{
            selectedCity ? "Editar Ciudad" : "Crear ciudad"
          }}</span>
        </v-card-title>
        <v-card-text>
          <v-form @submit.prevent="submitForm">
            <v-text-field
              v-model="newCity.name"
              label="Nombre"
              :rules="[requiredRule('Nombre')]"
              required
            ></v-text-field>
            <v-text-field
              v-model="newCity.stateId"
              label="Departamento"
              :rules="[requiredRule('Departamento'), stateIdRule]"
              required
            ></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn color="#457b9d" text @click="closeModal">Cancelar</v-btn>
          <v-btn color="#457b9d" @click="submitForm">{{
            selectedCity ? "Actualizar" : "Guardar"
          }}</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <!-- Tabla HTML para mostrar los usuarios -->
    <v-card>
      <v-card-title>
        <v-row>
          <v-col md="6" sm="6" cols="12">
            <span class="headline">Lista de Ciudades registradas</span>
          </v-col>
          <v-col md="6" sm="6" cols="12">
            <v-text-field
              v-model="search"
              append-icon="mdi-magnify"
              label="Buscar Ciudad"
              single-line
              hide-details
              variant="underlined"
            ></v-text-field>
          </v-col>
        </v-row>
      </v-card-title>
      <v-btn class="ma-2" color="#457b9d" dark @click="openModal">Nuevo</v-btn>
      <v-card-text>
        <v-table density="compact">
          <thead>
            <tr>
              <th>ID</th>
              <th>Nombre</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="city in filteredCities" :key="city.id">
              <td>{{ city.id }}</td>
              <td>{{ city.name }}</td>
              <td>
                <v-icon @click="editCity(city)" color="#457b9d"
                  >mdi-pencil</v-icon
                >
                <v-icon @click="deleteCity(city)" color="#457b9d"
                  >mdi-delete</v-icon
                >
              </td>
            </tr>
            <tr v-if="!filteredCities.length">
              <td colspan="4">No se encontraron Ciudades</td>
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
        selectedCity
          ? "Ciudad actualizada exitosamente"
          : "Ciudad creada exitosamente"
      }}
    </v-snackbar>
  </v-container>
</template>

<script>
import { ref, reactive, computed } from "vue";
import { useCityStore } from "../../stores/cityStore";

export default {
  name: "CityDataTable",
  setup() {
    const currentPage = ref(1); // Página actual
    const itemsPerPage = 10; // Número de usuarios por página
    const cityStore = useCityStore();
    const successMessageVisible = ref(false);
    const search = ref("");
    const newCity = reactive({
      name: "",
      stateId: "",
    });
    const selectedCity = ref(null);
    const dialog = ref(false);

    const totalCities = computed(() => cityStore.listCities.length);
    const totalPages = computed(() =>
      Math.ceil(totalCities.value / itemsPerPage)
    );

    const filteredCities = computed(() => {
      const startIndex = (currentPage.value - 1) * itemsPerPage;
      const endIndex = startIndex + itemsPerPage;
      const cityList = cityStore.listCities;
      return cityList
        .filter((city) =>
          city.name.toLowerCase().includes(search.value.toLowerCase())
        )
        .slice(startIndex, endIndex);
    });

    const requiredRule = (fieldName) => (value) =>
      !!value || `El campo "${fieldName}" es obligatorio`;

    const stateIdRule = (value) => {
      const numberRegex = /^[0-9]+$/;
      return numberRegex.test(value) || "El campo debe ser un número válido";
    };

    const openModal = () => {
      dialog.value = true;
      selectedCity.value = null;
      newCity.name = "";
      newCity.stateId = "";
    };

    const submitForm = async () => {
      if (!newCity.name || !newCity.stateId) {
        return;
      }
      if (selectedCity.value) {
        await cityStore.updateCity({ ...selectedCity.value, ...newCity });
      } else {
        await cityStore.createCity(newCity);
      }

      await cityStore.getCities();

      newCity.name = "";
      newCity.stateId = "";

      successMessageVisible.value = true;

      setTimeout(() => {
        successMessageVisible.value = false;
      }, 3000);

      dialog.value = false;
    };

    const editCity = (city) => {
      selectedCity.value = { ...city };
      newCity.name = selectedCity.value.name;
      newCity.stateId = selectedCity.value.stateId;
      dialog.value = true;
    };

    const deleteCity = (product) => {
      cityStore.deleteCity(product).then(() => {
        cityStore.getCities();
      });
    };

    const closeModal = () => {
      dialog.value = false;
      selectedCity.value = null;
      newCity.name = "";
      newCity.stateId = "";
    };

    return {
      search,
      currentPage,
      itemsPerPage,
      filteredCities,
      newCity,
      successMessageVisible,
      selectedCity,
      dialog,
      submitForm,
      requiredRule,
      stateIdRule,
      editCity,
      openModal,
      closeModal,
      deleteCity,
      totalPages,
    };
  },

  async mounted() {
    try {
      await useCityStore().getCities();
    } catch (error) {
      console.error(error);
    } finally {
      this.loading = false;
    }
  },
};
</script>