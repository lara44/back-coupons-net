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

ChartJS.register(
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale
);

const props = defineProps(["filters"]);

const { data, fetchData, loading } = useDashboardData();
const chartData = ref(null);
const chartOptions = ref({
  responsive: true,
  maintainAspectRatio: false,
});

const updateChart = () => {
  if (!data.value || data.value.length === 0) {
    chartData.value = { labels: [], datasets: [] };
    return;
  }

  const clientData = data.value.reduce((acc, item) => {
    const clientId = item.client.id;
    const clientName = `${item.client.firstName} ${item.client.lastName}`;

    if (!acc[clientId]) {
      acc[clientId] = { name: clientName, count: 0 };
    }
    acc[clientId].count += 1;
    return acc;
  }, {});

  chartData.value = {
    labels: Object.values(clientData).map((client) => client.name),
    datasets: [
      {
        label: "Cupones X Cliente",
        data: Object.values(clientData).map((client) => client.count),
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
