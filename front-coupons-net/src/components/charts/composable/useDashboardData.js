import { ref } from "vue";
import axios from "axios";

export const useDashboardData = () => {
  const data = ref([]);
  const loading = ref(false);

  // Función para obtener los datos desde la API usando los filtros proporcionados
  const fetchData = async (startDate, endDate, state, companyId) => {
    loading.value = true;

    const url = `http://localhost:5163/api/report/GetRedeemedCouponsByClient?startDate=${startDate}&endDate=${endDate}&companyId=${companyId}&state=${state}`;

    try {
      const response = await axios.get(url);
      data.value = response.data.data;
    } catch (error) {
      console.error("Error al obtener datos:", error);
    } finally {
      loading.value = false;
    }
  };

  return { data, fetchData, loading };
};
