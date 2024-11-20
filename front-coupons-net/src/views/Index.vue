<template>
  <v-app>
    <Header />
    <v-main style="background: #eceff1; min-height: calc(100vh - 128px)">
      <v-container fluid>
        <v-row>
          <v-col cols="12" style="padding: 1% 5% 5% 5%">
            <v-card style="background-color: #eceff1">
              <v-card-text>
                <v-row>
                  <!-- Tarjetas de Cupones -->
                  <v-col
                    v-for="coupon in couponStore.listCoupons"
                    :key="coupon.id"
                    cols="12"
                    sm="6"
                    md="4"
                    lg="3"
                  >
                    <v-card
                      max-width="344"
                      class="custom-card"
                      @click="openCouponDetail(coupon)"
                    >
                      <v-img height="200px" :src="coupon.photo" cover></v-img>
                      <v-card-title class="card-title">{{
                        coupon.name
                      }}</v-card-title>
                      <v-card-subtitle class="card-subtitle">
                        Válido hasta: {{ coupon.expiryDate.substr(0, 10) }}
                      </v-card-subtitle>
                      <v-card-actions style="padding: 0% 1% 5% 1%">
                        <v-btn
                          style="font-size: 1rem !important"
                          class="bold-text"
                          color="#E63946"
                          text
                          @click.stop="openClientForm(coupon.couponCode)"
                        >
                          Reclamar
                        </v-btn>
                      </v-card-actions>
                    </v-card>
                  </v-col>
                </v-row>
              </v-card-text>
            </v-card>
          </v-col>
        </v-row>
      </v-container>
    </v-main>
    <Footer />

    <!-- Modal para Detalles del Cupón -->
    <v-dialog v-model="isCouponDetailOpen" max-width="800px">
      <v-card class="custom-modal-card">
        <!-- Encabezado del modal -->
        <v-card-title class="text-h5">
          <div class="modal-header">
            <v-avatar size="60" class="mr-3">
              <v-img :src="selectedCoupon.photo" />
            </v-avatar>
            <div>
              <span class="modal-title">{{ selectedCoupon.name }}</span>
              <p class="modal-subtitle">
                Válido desde {{ formatDate(selectedCoupon.startDate) }} hasta
                {{ formatDate(selectedCoupon.expiryDate) }}
              </p>
            </div>
          </div>
        </v-card-title>
        <v-divider></v-divider>

        <!-- Contenido del modal -->
        <v-card-text>
          <v-container>
            <v-row>
              <!-- Información principal -->
              <v-col cols="12" md="6">
                <h6><strong>Código del cupón:</strong></h6>
                <p class="info-text">{{ selectedCoupon.couponCode }}</p>

                <h6><strong>Cantidad inicial:</strong></h6>
                <p class="info-text">{{ selectedCoupon.quantityInitial }}</p>

                <h6><strong>Cantidad actual:</strong></h6>
                <p class="info-text">{{ selectedCoupon.quantityActual }}</p>
              </v-col>

              <!-- Lista de productos -->
              <v-col cols="12" md="6">
                <h6><strong>Productos incluidos:</strong></h6>
                <v-list dense class="custom-list">
                  <v-list-item
                    v-for="productDetail in selectedCoupon.detailCoupons || []"
                    :key="productDetail.id"
                  >
                    <v-list-item-title class="product-item">
                      {{
                        productDetail.product?.name || "Producto desconocido"
                      }}
                    </v-list-item-title>
                  </v-list-item>
                </v-list>
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>
        <v-divider></v-divider>

        <!-- Acciones del modal -->
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="primary" text @click="isCouponDetailOpen = false"
            >Cerrar</v-btn
          >
        </v-card-actions>
      </v-card>
    </v-dialog>
    <!-- Modal para Reclamar Cupon -->
    <v-dialog v-model="isClientFormOpen" width="600" max-width="600">
      <ClientForm
        :couponCode="selectedCouponCode"
        @close="isClientFormOpen = false"
      />
    </v-dialog>
  </v-app>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useCouponStore } from "../stores/couponStore";
import { useRoute } from "vue-router";
import QRCode from "qrcode";
import { saveAs } from "file-saver";
import ClientForm from "../components/clients/ClientForm.vue";
import Header from "./Header.vue";
import Footer from "./Footer.vue";

const router = useRoute();
const couponStore = useCouponStore();
const isClientFormOpen = ref(false);
const selectedCouponCode = ref(null);

// Nuevo estado para el detalle del cupón
const isCouponDetailOpen = ref(false);
const selectedCoupon = ref({});

onMounted(() => {
  couponStore.getCouponsFull();
});

// Función para abrir el modal de detalle del cupón
const openCouponDetail = (coupon) => {
  selectedCoupon.value = coupon;
  isCouponDetailOpen.value = true;
};

// Función para abrir el modal de reclamar
const openClientForm = (couponCode) => {
  selectedCouponCode.value = couponCode;
  isClientFormOpen.value = true;
};

// Generar QR
const QrCoupon = async (coupon) => {
  try {
    const url = `http://localhost:5173/coupons/redeem?code=${coupon.couponCode}`;
    const qrCodeDataUrl = await QRCode.toDataURL(url);
    const blob = await (await fetch(qrCodeDataUrl)).blob();
    saveAs(blob, `${coupon.couponCode}.png`);
  } catch (error) {
    console.error("Error generating QR code", error);
  }
};

// Formatear Fechas
const formatDate = (date) => {
  return new Date(date).toLocaleDateString("es-ES", {
    year: "numeric",
    month: "long",
    day: "numeric",
  });
};
</script>

<style scoped>
.custom-card {
  border-radius: 9px !important;
  background: #ffffff !important;
  box-shadow: 8px 8px 13px #d2d2d7, -8px -8px 13px #ffffff !important;
  cursor: pointer;
  transition: transform 0.2s;
}
.custom-card:hover {
  transform: scale(1.03);
}
.card-title {
  padding: 1% 2% 0% 2% !important;
  font-size: 1.063rem !important;
}
.card-subtitle {
  padding: 0% 2% 0% 2% !important;
  font-size: 0.883rem !important;
}
.custom-modal-card {
  border-radius: 12px !important;
  padding: 20px;
  background: linear-gradient(145deg, #f3f3f7, #ffffff);
  box-shadow: 8px 8px 20px #d1d1d8, -8px -8px 20px #ffffff;
}

.modal-header {
  display: flex;
  align-items: center;
}

.modal-title {
  font-size: 1.5rem;
  font-weight: 600;
  color: #1d3557;
}

.modal-subtitle {
  font-size: 0.875rem;
  color: #6b6b6b;
}

.info-text {
  font-size: 1rem;
  color: #444;
  margin-bottom: 8px;
}

.custom-list {
  background: #f9fafb;
  border-radius: 8px;
  padding: 10px;
  box-shadow: inset 2px 2px 8px rgba(0, 0, 0, 0.05);
}

.product-item {
  font-size: 0.95rem;
  color: #333;
  padding: 8px 0;
  border-bottom: 1px solid #eaeaea;
}

.product-item:last-child {
  border-bottom: none;
}
</style>
