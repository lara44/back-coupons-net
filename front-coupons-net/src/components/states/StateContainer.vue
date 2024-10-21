<template>
  <v-container>
    <v-dialog v-model="dialog" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="headline">{{
            selectedState ? "Editar Departamento" : "Crear Departamento"
          }}</span>
        </v-card-title>
        <v-card-text>
          <v-form @submit.prevent="submitForm">
            <v-text-field
              v-model="newState.name"
              label="Nombre"
              :rules="[requiredRule('Nombre')]"
              required
            ></v-text-field>
            <v-text-field
              v-model="newState.countryId"
              label="País"
              :rules="[requiredRule('País'), countryIdRule]"
              required
            ></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn size="small" class="btn-general" text @click="closeModal">Cancelar</v-btn>
          <v-btn size="small" class="mr-4 btn-general" @click="submitForm">{{
            selectedState ? "Actualizar" : "Guardar"
          }}</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="dialogCities" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="headline">Ciudades</span>
        </v-card-title>
        <v-card-text>
          <v-table density="compact">
            <thead>
              <tr>
                <th>ID</th>
                <th>Nombre</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="city in cityByState" :key="city.id">
                <td>{{ city.id }}</td>
                <td>{{ city.name }}</td>
              </tr>
              <tr v-if="!cityByState.length">
                <td colspan="4">No se encontraron ciudades</td>
              </tr>
            </tbody>
          </v-table>
        </v-card-text>
        <v-card-actions>
          <v-btn color="blue darken-1" text @click="dialogCities = false"
            >Cerrar</v-btn
          >
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-card>
      <v-card-title>
        <v-row>
          <v-col md="6" sm="6" cols="12">
            <span class="headline">Lista de Departamentos registrados</span>
          </v-col>
          <v-col md="6" sm="6" cols="12">
            <v-text-field
              v-model="search"
              append-icon="mdi-magnify"
              label="Buscar Departamento"
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
              <th>Número de ciudades</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="state in filteredStates" :key="state.id">
              <td>{{ state.id }}</td>
              <td>{{ state.name }}</td>
              <td>{{ state.citiesNumber }}</td>
              <td>
                <v-icon @click="getCityByState(state.cities)" class="btn-icon"
                  >mdi-eye</v-icon
                >
                <v-icon @click="editState(state)" class="btn-icon"
                  >mdi-pencil</v-icon
                >
                <v-icon @click="deleteState(state)" class="btn-icon"
                  >mdi-delete</v-icon
                >
              </td>
            </tr>
            <tr v-if="!filteredStates.length">
              <td colspan="4">No se encontraron Departamentos</td>
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
        selectedState
          ? "Departamento actualizado exitosamente"
          : "Departamento creado exitosamente"
      }}
    </v-snackbar>
  </v-container>
</template>

<script>
import { ref, reactive, computed } from "vue";
import { useStateStore } from "../../stores/stateStore";

export default {
  name: "StateDataTable",
  setup() {
    const cityByState = ref([]);
    const currentPage = ref(1); // Página actual
    const itemsPerPage = 10; // Número de usuarios por página
    const stateStore = useStateStore();
    const successMessageVisible = ref(false);
    const search = ref("");
    const newState = reactive({
      name: "",
      countryId: "",
    });
    const selectedState = ref(null);
    const dialog = ref(false);
    const dialogCities = ref(false);

    const totalStates = computed(() => stateStore.listStates.length);
    const totalPages = computed(() =>
      Math.ceil(totalStates.value / itemsPerPage)
    );

    const filteredStates = computed(() => {
      const startIndex = (currentPage.value - 1) * itemsPerPage;
      const endIndex = startIndex + itemsPerPage;
      const stateList = stateStore.listStates;
      return stateList
        .filter((state) =>
          state.name.toLowerCase().includes(search.value.toLowerCase())
        )
        .slice(startIndex, endIndex);
    });

    const requiredRule = (fieldName) => (value) =>
      !!value || `El campo "${fieldName}" es obligatorio`;

    const countryIdRule = (value) => {
      const numberRegex = /^[0-9]+$/;
      return numberRegex.test(value) || "El campo debe ser un número válido";
    };

    const openModal = () => {
      dialog.value = true;
      selectedState.value = null;
      newState.name = "";
      newState.countryId = "";
    };

    const submitForm = async () => {
      if (!newState.name || !newState.countryId) {
        return;
      }
      if (selectedState.value) {
        await stateStore.updateState({ ...selectedState.value, ...newState });
      } else {
        await stateStore.createState(newState);
      }

      await stateStore.getStates();

      newState.name = "";
      newState.countryId = "";

      successMessageVisible.value = true;

      setTimeout(() => {
        successMessageVisible.value = false;
      }, 3000);

      dialog.value = false;
    };

    const editState = (state) => {
      selectedState.value = { ...state };
      newState.name = selectedState.value.name;
      newState.countryId = selectedState.value.countryId;
      dialog.value = true;
    };

    const deleteState = (state) => {
      stateStore
        .deleteState(state)
        .then(() => {
          stateStore.getStates();
        })
        .catch((e) => {
          console.log(e);
        });
    };

    const closeModal = () => {
      dialog.value = false;
      selectedState.value = null;
      newState.name = "";
      newState.countryId = "";
    };

    const getCityByState = (data) => {
      dialogCities.value = true;
      cityByState.value = data;
    };

    return {
      search,
      currentPage,
      itemsPerPage,
      filteredStates,
      newState,
      successMessageVisible,
      selectedState,
      dialog,
      submitForm,
      requiredRule,
      countryIdRule,
      editState,
      openModal,
      closeModal,
      deleteState,
      dialogCities,
      cityByState,
      getCityByState,
      totalPages,
    };
  },

  async mounted() {
    try {
      await useStateStore().getStates();
    } catch (error) {
      console.error(error);
    } finally {
      this.loading = false;
    }
  },
};
</script>
