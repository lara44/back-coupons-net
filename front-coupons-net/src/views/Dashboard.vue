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

    <v-card>
      <v-row>
        <v-col cols="12" md="6">
          <CouponsByDateChart :filters="appliedFilters" />
        </v-col>
        <v-col cols="12" md="6">
          <CouponsByClientChart :filters="appliedFilters" />
        </v-col>
      </v-row>
    </v-card>
  </v-container>
</template>

<script setup>
import { reactive, ref, computed, onMounted } from "vue";
import CouponsByDateChart from "../components/charts/CouponsByDateChart.vue";
import CouponsByClientChart from "../components/charts/CouponsByClientChart.vue";
import { useUserStore } from "../stores/userStore";
import { useCompanyStore } from "../stores/companyStore";

// Estados disponibles para filtrar
const states = [
  { title: "Todos", value: 0 },
  { title: "Activos", value: 1 },
  { title: "Inactivos", value: 2 },
];

// Filtros reactivos
const filters = reactive({
  startDate: "2024-01-01",
  endDate: "2024-12-31",
  state: 0,
  companyId: 1,
});

const appliedFilters = ref({ ...filters });

// Stores
const userStore = useUserStore();

const companyStore = useCompanyStore();
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
