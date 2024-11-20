<template>
  <div v-if="loading" class="flex justify-center items-center h-24">
    <v-progress-circular indeterminate color="primary" />
  </div>
  <div v-else class="carousel-container">
    <v-btn
      icon
      class="carousel-btn"
      :disabled="currentPage === 1"
      @click="currentPage--"
    >
      <v-icon>mdi-chevron-left</v-icon>
    </v-btn>
    <div class="cards-wrapper">
      <v-card
        v-for="item in paginatedItems"
        :key="item.companyId"
        class="carousel-card"
        @click="openModal(item)"
      >
        <v-card-title class="text-primary text-h6">{{
          item.name
        }}</v-card-title>
        <v-card-subtitle class="text-muted">
          <strong>NIT:</strong> {{ item.nit }}
        </v-card-subtitle>
        <v-card-text>
          <span class="redemption-count">
            {{ item.redemptionCount }} Cupones Gestionados
          </span>
        </v-card-text>
      </v-card>
    </div>
    <v-btn
      icon
      class="carousel-btn"
      :disabled="currentPage === totalPages"
      @click="currentPage++"
    >
      <v-icon>mdi-chevron-right</v-icon>
    </v-btn>

    <!-- Modal para Detalles -->
    <v-dialog v-model="showModal" max-width="500">
      <v-card class="modal-card">
        <v-card-title class="text-h6 modal-title">
          Detalle de Cupones
        </v-card-title>
        <v-divider></v-divider>
        <v-card-subtitle class="modal-subtitle">
          <strong>Compañía:</strong> {{ selectedCompany?.name || "" }}<br />
          <strong>NIT:</strong> {{ selectedCompany?.nit }}
        </v-card-subtitle>
        <v-divider></v-divider>
        <v-card-text class="modal-content">
          <div class="info-row">
            <v-icon class="info-icon" color="blue">mdi-check-circle</v-icon>
            <span class="info-text">
              <strong>Reclamados:</strong> {{ claimedCount }}
            </span>
          </div>
          <div class="info-row">
            <v-icon class="info-icon" color="green">mdi-check-circle</v-icon>
            <span class="info-text">
              <strong>Canjeados:</strong> {{ redeemedCount }}
            </span>
          </div>
        </v-card-text>
        <v-divider></v-divider>
        <v-card-actions>
          <v-btn text color="primary" @click="showModal = false">Cerrar</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script setup>
import { computed, ref } from "vue";
import axios from "axios";
import { useCarouselData } from "./composable/useCarouselData";

const { carouselItems, loading } = useCarouselData();

const currentPage = ref(1);
const itemsPerPage = 3;

// Modal state
const showModal = ref(false);
const selectedCompany = ref(null);
const claimedCount = ref(0);
const redeemedCount = ref(0);

// Calcular las tarjetas paginadas
const paginatedItems = computed(() => {
  const start = (currentPage.value - 1) * itemsPerPage;
  const end = start + itemsPerPage;
  return carouselItems.value.slice(start, end);
});

// Total de páginas
const totalPages = computed(() =>
  Math.ceil(carouselItems.value.length / itemsPerPage)
);

// Abrir el modal y cargar datos
const openModal = async (company) => {
  selectedCompany.value = company;
  showModal.value = true;
  await fetchDetails(company.companyId);
};

// Obtener detalles de los cupones
const fetchDetails = async (companyId) => {
  try {
    // Obtener reclamados
    const claimedResponse = await axios.get(
      "http://localhost:5163/api/report/GetCountRedeemedCouponsByCompany",
      {
        params: { companyId, state: 0 },
      }
    );
    claimedCount.value = claimedResponse.data.data[0]?.redemptionCount || 0;

    // Obtener canjeados
    const redeemedResponse = await axios.get(
      "http://localhost:5163/api/report/GetCountRedeemedCouponsByCompany",
      {
        params: { companyId, state: 1 },
      }
    );
    redeemedCount.value = redeemedResponse.data.data[0]?.redemptionCount || 0;
  } catch (error) {
    console.error("Error fetching coupon details:", error);
    claimedCount.value = 0;
    redeemedCount.value = 0;
  }
};
</script>

<style scoped>
.carousel-container {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin: 20px 0;
}

.cards-wrapper {
  display: flex;
  justify-content: space-around;
  width: 100%;
  overflow: hidden;
}

.carousel-card {
  margin: 0 10px;
  padding: 15px;
  min-width: 250px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.15);
  border-radius: 10px;
  cursor: pointer;
}

.redemption-count {
  font-size: 1.2rem;
  font-weight: bold;
  color: #1976d2;
}

.carousel-btn {
  color: #1976d2;
  margin: 0 10px;
}

/* Modal Estilizado */
.modal-card {
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
}

.modal-title {
  font-weight: bold;
  font-size: 1.2rem;
  text-align: center;
  margin-bottom: 10px;
}

.modal-subtitle {
  text-align: center;
  color: #555;
  font-size: 1rem;
  margin: 10px 0;
}

.modal-content {
  margin-top: 20px;
}

.info-row {
  display: flex;
  align-items: center;
  margin-bottom: 15px;
}

.info-icon {
  margin-right: 10px;
}

.info-text {
  font-size: 1rem;
  color: #333;
}
</style>
