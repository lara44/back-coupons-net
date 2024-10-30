<template>
  <div>
    <div v-if="loading">
      <v-progress-circular indeterminate color="primary" />
    </div>

    <div v-else-if="items.length > 0">
      <v-card>
        <v-card-title class="text-h5">Reporte de Cupones</v-card-title>

        <v-card-subtitle class="text-right mb-4">
          <strong>Total Cupones:</strong> {{ totalCupones }}
        </v-card-subtitle>

        <v-btn color="primary" class="mb-4" @click="exportToExcel">
          Exportar a Excel
        </v-btn>

        <table class="w-full border-collapse border">
          <thead>
            <tr>
              <th class="border px-4 py-2">ID</th>
              <th class="border px-4 py-2">Fecha</th>
              <th class="border px-4 py-2">Nombre del Cupón</th>
              <th class="border px-4 py-2">Código del Cupón</th>
              <th class="border px-4 py-2">Nombre del Cliente</th>
              <th class="border px-4 py-2">Email</th>
              <th class="border px-4 py-2">Teléfono</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in items" :key="item.id">
              <td class="border px-4 py-2">{{ item.id }}</td>
              <td class="border px-4 py-2">
                {{ new Date(item.dateRedeem).toLocaleString() }}
              </td>
              <td class="border px-4 py-2">{{ item.coupon.name }}</td>
              <td class="border px-4 py-2">{{ item.coupon.couponCode }}</td>
              <td class="border px-4 py-2">
                {{ item.client.firstName }} {{ item.client.lastName }}
              </td>
              <td class="border px-4 py-2">{{ item.client.email }}</td>
              <td class="border px-4 py-2">{{ item.client.phone }}</td>
            </tr>
          </tbody>
        </table>
      </v-card>
    </div>

    <v-alert v-else type="info" text>
      No hay datos disponibles para los filtros seleccionados.
    </v-alert>
  </div>
</template>

<script setup>
import { ref, watch, onMounted } from "vue";
import { useDashboardData } from "./composable/useDashboardData";
import ExcelJS from "exceljs";
import { saveAs } from "file-saver";

// Recibir los filtros desde el Dashboard
const props = defineProps(["filters"]);

// Estado de los datos
const { data, fetchData, loading } = useDashboardData();
const items = ref([]);
const totalCupones = ref(0); // Estado para el total de cupones

// Función para actualizar la tabla y el total
const updateTable = () => {
  items.value = data.value || [];
  totalCupones.value = items.value.length; // Actualizar el total
};

// Función para exportar los datos a Excel
const exportToExcel = async () => {
  const workbook = new ExcelJS.Workbook();
  const worksheet = workbook.addWorksheet("Cupones Redimidos");

  // Definir encabezados
  worksheet.columns = [
    { header: "ID", key: "id", width: 10 },
    { header: "Fecha", key: "fecha", width: 20 },
    { header: "Nombre del Cupón", key: "nombreCupon", width: 30 },
    { header: "Código del Cupón", key: "codigoCupon", width: 20 },
    { header: "Nombre del Cliente", key: "nombreCliente", width: 30 },
    { header: "Email", key: "email", width: 30 },
    { header: "Teléfono", key: "telefono", width: 15 },
  ];

  // Agregar filas con los datos
  items.value.forEach((item) => {
    worksheet.addRow({
      id: item.id,
      fecha: new Date(item.dateRedeem).toLocaleString(),
      nombreCupon: item.coupon.name,
      codigoCupon: item.coupon.couponCode,
      nombreCliente: `${item.client.firstName} ${item.client.lastName}`,
      email: item.client.email,
      telefono: item.client.phone,
    });
  });

  // Generar archivo y descargar
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
table {
  border-collapse: collapse;
  width: 100%;
}

th,
td {
  text-align: left;
  padding: 8px;
  border: 1px solid #ddd;
}
</style>
