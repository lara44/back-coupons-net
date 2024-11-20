import { ref, onMounted } from "vue";
import axios from "axios";

export function useCarouselData() {
  const carouselItems = ref([]);
  const loading = ref(false);

  const fetchCarouselData = async () => {
    try {
      loading.value = true;
      const response = await axios.get(
        "http://localhost:5163/api/report/GetCountRedeemedCouponsByCompany"
      );
      carouselItems.value = response.data.data || [];
    } catch (error) {
      console.error("Error fetching carousel data:", error);
    } finally {
      loading.value = false;
    }
  };

  onMounted(() => {
    fetchCarouselData();
  });

  return { carouselItems, loading };
}
