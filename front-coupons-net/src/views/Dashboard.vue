<template>
  <v-container>
    <v-card class="pa-4 mb-6" elevation="2">
      <v-card-title class="mb-4 text-h5 text-center">Dashboard</v-card-title>

      <v-row>
        <v-col cols="12" md="3" v-if="isAdmin">
          <v-select
            v-model="filters.companyId"
            :items="companies"
            item-title="name"
            item-value="id"
            label="Empresa"
            outlined
          />
        </v-col>

        <v-col cols="12" md="3">
          <v-text-field
            v-model="filters.startDate"
            label="Fecha Inicio"
            type="date"
            outlined
            required
          />
        </v-col>

        <v-col cols="12" md="3">
          <v-text-field
            v-model="filters.endDate"
            label="Fecha Fin"
            type="date"
            outlined
            required
          />
        </v-col>

        <v-col cols="12" md="3">
          <v-select
            v-model="filters.state"
            :items="states"
            label="Estado"
            outlined
          />
        </v-col>

        <v-col cols="12" md="3">
          <v-btn color="primary" block @click="applyFilters">
            Aplicar Filtros
          </v-btn>
        </v-col>
      </v-row>
    </v-card>

    <v-card class="pa-4 mb-6">
      <CouponsByDateChart :filters="appliedFilters" />
    </v-card>

    <v-card class="pa-4">
      <TablaCupones :filters="appliedFilters" />
    </v-card>
  </v-container>
</template>

<script setup>
import { reactive, ref, computed, onMounted } from "vue";
import CouponsByDateChart from "../components/charts/CouponsByDateChart.vue";
import CouponsByClientChart from "../components/charts/CouponsByClientChart.vue";
import TablaCupones from "../components/charts/CuponsTablaClientes.vue";
import { useUserStore } from "../stores/userStore";
import { useCompanyStore } from "../stores/companyStore";

// Stores
const userStore = useUserStore();
const companyStore = useCompanyStore();

// Estados disponibles para filtrar
const states = [
  { title: "Reclamado", value: 0 },
  { title: "Canjeado", value: 1 },
  { title: "Expirado", value: 2 },
];

const companyId = userStore.role === "Admin" ? 0 : userStore.companyId;

// Filtros reactivos
const currentYear = new Date().getFullYear();

const filters = reactive({
  startDate: `${currentYear}-01-01`,
  endDate: `${currentYear}-12-31`,
  state: 0,
  companyId: companyId,
});

const appliedFilters = ref({ ...filters });

const companies = computed(() => companyStore.listCompanies);

console.log(companies.value);

const isAdmin = computed(() => userStore.role === "Admin");
console.log(isAdmin.value);

onMounted(async () => {
  if (isAdmin.value) {
    await companyStore.getCompanies();
  } else {
    filters.companyId = userStore.companyId;
  }
});

// Aplicar filtros
const applyFilters = () => {
  appliedFilters.value = { ...filters };
  console.log("Filtros aplicados:", appliedFilters.value);
};
</script>
