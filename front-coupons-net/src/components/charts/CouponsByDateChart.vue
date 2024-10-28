<template>
  <div>
    <div v-if="loading">
      <v-progress-circular indeterminate color="primary" />
    </div>
    <div v-else-if="chartData && chartData.datasets.length > 0">
      <Bar :data="chartData" :options="chartOptions" />
    </div>
    <div v-else>
      <v-alert type="info" text>
        No hay datos disponibles para los filtros seleccionados.
      </v-alert>
    </div>
  </div>
</template>

<script setup>
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale,
} from "chart.js";
import { Bar } from "vue-chartjs";
import { ref, watch, onMounted } from "vue";
import { useDashboardData } from "./composable/useDashboardData";

// Registrar los componentes de Chart.js
ChartJS.register(
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale
);

// Props recibidas desde el Dashboard
const props = defineProps(["filters"]);

// Variables de estado
const { data, fetchData, loading } = useDashboardData();
const chartData = ref(null);
const chartOptions = ref({
  responsive: true,
  maintainAspectRatio: false,
});

// Actualiza la gráfica con los datos obtenidos
const updateChart = () => {
  if (!data.value || data.value.length === 0) {
    chartData.value = { labels: [], datasets: [] };
    return;
  }

  const labels = data.value.map((item) =>
    new Date(item.dateRedeem).toLocaleDateString()
  );
  const values = data.value.map(() => 1); // Cuenta cada cupón

  chartData.value = {
    labels,
    datasets: [
      {
        label: "Cupones X Fecha",
        data: values,
        backgroundColor: "rgba(54, 162, 235, 0.2)",
        borderColor: "rgba(54, 162, 235, 1)",
        borderWidth: 1,
      },
    ],
  };
};

onMounted(async () => {
  await fetchData(
    props.filters.startDate,
    props.filters.endDate,
    props.filters.state,
    props.filters.companyId
  );
  updateChart();
});

watch(
  () => props.filters,
  async (newFilters) => {
    await fetchData(
      newFilters.startDate,
      newFilters.endDate,
      newFilters.state,
      newFilters.companyId
    );
    updateChart();
  },
  { immediate: true }
);
</script>
