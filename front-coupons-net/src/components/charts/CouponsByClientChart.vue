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
import { Bar } from "vue-chartjs";
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale,
} from "chart.js";
import { ref, onMounted, watch } from "vue";
import { useDashboardData } from "./composable/useDashboardData";

// Registramos los componentes necesarios de Chart.js
ChartJS.register(
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale
);

// Props para recibir los filtros
const props = defineProps(["filters"]);

// Variables de estado
const { data, fetchData, loading } = useDashboardData();
const chartData = ref(null);
const chartOptions = ref({
  responsive: true,
  maintainAspectRatio: false,
});

// Función para actualizar los datos de la gráfica
const updateChart = () => {
  if (!data.value || data.value.length === 0) {
    chartData.value = { labels: [], datasets: [] };
    return;
  }

  const clientNames = data.value.map(
    (item) => `${item.client.firstName} ${item.client.lastName}`
  );

  const counts = clientNames.reduce((acc, name) => {
    acc[name] = (acc[name] || 0) + 1;
    return acc;
  }, {});

  chartData.value = {
    labels: Object.keys(counts),
    datasets: [
      {
        label: "Cupones X Cliente",
        data: Object.values(counts),
        backgroundColor: "rgba(54, 162, 235, 0.2)",
        borderColor: "rgba(54, 162, 235, 1)",
        borderWidth: 1,
      },
    ],
  };
};

// Obtenemos los datos al montar el componente
onMounted(async () => {
  await fetchData(
    props.filters.startDate,
    props.filters.endDate,
    props.filters.state,
    props.filters.companyId
  );
  updateChart();
});

// Actualizamos la gráfica cada vez que los filtros cambien
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
