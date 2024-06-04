import { defineStore } from "pinia";
import axios from "axios";

export const useCouponStore = defineStore("couponStore", {
  state: () => {
    return {
      listCoupons: [
        {id:1, nombre:"cupon prueba1", fechainicio:"22/02/1992", fechafin:"22/02/2300", estado: "activo", cantidad:"20", porcentaje:"16"},
        
      ],
      coupon: {
        id: "",
        name: "",
      },
    };
  },

  actions: {
    async getCoupons() {
      try {
        // const response = await axios.get("/api/coupons/full");
        // if (response.data.data) {
        //   this.listCoupons = response.data.data;
          console.log("respuesta correcta de cupones");
        //}
      } catch (error) {
        console.error(error);
      }
    },

    async createCoupon(newCoupon) {
      try {
        const response = await axios.post("/api/coupons", newCoupon);
        if (response.data.success) {
          await this.getCoupons();
        }
      } catch (error) {
        console.error(error);
      }
    },

    async updateCoupon(updatedCoupon) {
      try {
        const response = await axios.put(
          `/api/coupons/${updatedCoupon.id}`,
          updatedCoupon
        );
        if (response.data.success) {
          await this.getCoupons();
        }
      } catch (error) {
        console.error(error);
      }
    },

    async deleteCoupon(deleteCoupon) {
      try {
        const response = await axios.delete(
          `/api/coupons/${deleteCoupon.id}`,
          deleteCoupon
        );
        if (response.data.success) {
          await this.getCoupons();
        }
      } catch (error) {
        console.error(error);
      }
    },
  },
});