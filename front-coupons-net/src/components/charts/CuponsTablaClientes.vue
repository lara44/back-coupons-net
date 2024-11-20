<template>
  <div>
    <div v-if="loading" class="flex justify-center items-center h-64">
      <v-progress-circular indeterminate color="primary" size="70" />
    </div>

    <div v-else-if="paginatedItems.length > 0">
      <!-- Card con márgenes adicionales -->
      <v-card class="elevation-2 card-container">
        <v-card-title class="text-h5 text-primary">
          <span>Reporte de Cupones</span>
        </v-card-title>

        <!-- Encabezado con Total a la izquierda y Botón a la derecha -->
        <div class="flex justify-between items-center mb-4">
          <span class="text-muted"
            ><strong>Total:</strong> {{ totalCupones }}</span
          >
          <v-btn color="primary" @click="exportToExcel" class="btn-export">
            <v-icon left>mdi-download</v-icon>Exportar a xls
          </v-btn>
        </div>

        <!-- Tabla -->
        <table class="styled-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Fecha</th>
              <th>Nombre del Cupón</th>
              <th>Código del Cupón</th>
              <th>Nombre del Cliente</th>
              <th>Email</th>
              <th>Teléfono</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in paginatedItems" :key="item.id">
              <td>{{ item.id }}</td>
              <td>{{ new Date(item.dateRedeem).toLocaleString() }}</td>
              <td>{{ item.coupon.name }}</td>
              <td>{{ item.coupon.couponCode }}</td>
              <td>{{ item.client.firstName }} {{ item.client.lastName }}</td>
              <td>{{ item.client.email }}</td>
              <td>{{ item.client.phone }}</td>
            </tr>
          </tbody>
        </table>

        <!-- Paginación -->
        <div class="pagination-container">
          <v-btn
            :disabled="currentPage === 1"
            @click="currentPage--"
            color="primary"
            small
            rounded
            class="pagination-btn"
          >
            <v-icon small>mdi-chevron-left</v-icon>
          </v-btn>
          <span class="pagination-text"
            >Página {{ currentPage }} de {{ totalPages }}</span
          >
          <v-btn
            :disabled="currentPage === totalPages"
            @click="currentPage++"
            color="primary"
            small
            rounded
            class="pagination-btn"
          >
            <v-icon small>mdi-chevron-right</v-icon>
          </v-btn>
        </div>
      </v-card>
    </div>

    <v-alert v-else type="info" text>
      No hay datos disponibles para los filtros seleccionados.
    </v-alert>
  </div>
</template>

<script setup>
import { ref, computed, watch, onMounted } from "vue";
import { useDashboardData } from "./composable/useDashboardData";
import ExcelJS from "exceljs";
import { saveAs } from "file-saver";

// Recibir los filtros desde el Dashboard
const props = defineProps(["filters"]);

// Estado de los datos
const { data, fetchData, loading } = useDashboardData();
const items = ref([]);
const totalCupones = ref(0);

// Paginación
const currentPage = ref(1);
const itemsPerPage = ref(10);

// Función para actualizar la tabla y el total
const updateTable = () => {
  items.value = data.value || [];
  totalCupones.value = items.value.length;
};

// Cálculo de elementos paginados
const paginatedItems = computed(() => {
  const start = (currentPage.value - 1) * itemsPerPage.value;
  const end = start + itemsPerPage.value;
  return items.value.slice(start, end);
});

// Total de páginas
const totalPages = computed(() =>
  Math.ceil(items.value.length / itemsPerPage.value)
);

// Función para exportar los datos a Excel
const exportToExcel = async () => {
  const workbook = new ExcelJS.Workbook();
  const worksheet = workbook.addWorksheet("Cupones Redimidos");

  worksheet.columns = [
    { header: "ID", key: "id", width: 10 },
    { header: "Fecha", key: "dateRedeem", width: 20 },
    { header: "Nombre del Cupón", key: "couponName", width: 30 },
    { header: "Código del Cupón", key: "couponCode", width: 20 },
    { header: "Nombre del Cliente", key: "clientName", width: 30 },
    { header: "Email", key: "clientEmail", width: 30 },
    { header: "Teléfono", key: "clientPhone", width: 15 },
  ];

  items.value.forEach((item) => {
    worksheet.addRow({
      id: item.id,
      dateRedeem: new Date(item.dateRedeem).toLocaleString(),
      couponName: item.coupon.name,
      couponCode: item.coupon.couponCode,
      clientName: `${item.client.firstName} ${item.client.lastName}`,
      clientEmail: item.client.email,
      clientPhone: item.client.phone,
    });
  });

  const buffer = await workbook.xlsx.writeBuffer();
  saveAs(new Blob([buffer]), "ReporteCupones.xlsx");
};

// Ejecutar la primera carga de datos al montar el componente
onMounted(async () => {
  await fetchData(
    props.filters.startDate,
    props.filters.endDate,
    props.filters.state,
    props.filters.companyId
  );
  updateTable();
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
    updateTable();
  },
  { immediate: true }
);
</script>

<style scoped>
.card-container {
  margin: 20px; /* Márgenes alrededor del v-card */
  padding: 15px; /* Relleno interno del card */
}

.styled-table {
  border-collapse: collapse;
  margin: 25px 0;
  font-size: 0.9em;
  font-family: Arial, sans-serif;
  width: 100%;
  box-shadow: 0 0 20px rgba(0, 0, 0, 0.15);
}

.styled-table thead tr {
  background-color: #1976d2; /* Color primario */
  color: #ffffff;
  text-align: left;
}

.styled-table th,
.styled-table td {
  padding: 12px 15px;
  border: 1px solid #dddddd;
}

.styled-table tbody tr {
  border-bottom: 1px solid #dddddd;
}

.styled-table tbody tr:nth-of-type(even) {
  background-color: #f3f3f3;
}

.styled-table tbody tr:last-of-type {
  border-bottom: 2px solid #1976d2;
}

.flex {
  display: flex;
}

.justify-between {
  justify-content: space-between;
}

.items-center {
  align-items: center;
}

.mb-4 {
  margin-bottom: 16px;
}

.text-muted {
  color: #6c757d;
}

.btn-export {
  margin-left: auto; /* Mueve el botón al extremo derecho */
}

.pagination-container {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 20px;
}

.pagination-btn {
  margin: 0 10px; /* Más margen en todos los lados */
  padding: 0 8px;
  min-width: 32px;
  height: 32px;
}

.pagination-text {
  margin: 0 15px; /* Espaciado del texto */
  font-size: 0.9rem;
  font-weight: bold;
}
</style>
