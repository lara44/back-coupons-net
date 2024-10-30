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
  plugins: {
    legend: {
      display: true,
      position: "top",
    },
  },
});

// Función para agrupar los cupones por mes
const groupByMonth = () => {
  const monthCounts = {};

  data.value.forEach((item) => {
    const month = new Date(item.dateRedeem).toLocaleString("es-ES", {
      month: "long",
      year: "numeric",
    });

    if (!monthCounts[month]) {
      monthCounts[month] = 0;
    }
    monthCounts[month] += 1;
  });

  return monthCounts;
};

// Actualiza la gráfica con los datos agrupados por mes
const updateChart = () => {
  if (!data.value || data.value.length === 0) {
    chartData.value = { labels: [], datasets: [] };
    return;
  }

  const groupedData = groupByMonth();
  const labels = Object.keys(groupedData);
  const values = Object.values(groupedData);

  chartData.value = {
    labels,
    datasets: [
      {
        label: "Cupones por Mes",
        data: values,
        backgroundColor: "rgba(54, 162, 235, 0.2)",
        borderColor: "rgba(54, 162, 235, 1)",
        borderWidth: 1,
      },
    ],
  };
};

// Cargar los datos al montar el componente
onMounted(async () => {
  await fetchData(
    props.filters.startDate,
    props.filters.endDate,
    props.filters.state,
    props.filters.companyId
  );
  updateChart();
});

// Observar cambios en los filtros y recargar los datos
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
