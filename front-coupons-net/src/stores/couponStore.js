import { defineStore } from "pinia";
import axios from "axios";

export const useCouponStore = defineStore("couponStore", {
  state: () => {
    return {
      listCoupons: [],
      coupon: {
        id: "",
        name: "",
      },
    };
  },

  actions: {
    async getCoupons() {
      try {
        const response = await axios.get("/api/coupons/full");
          if (response.data.data) {
            this.listCoupons = response.data.data;
            console.log(response.data.data);
          }
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

    async redeemCoupon(redeem) {
      try {
        const response = await axios.get(`/api/coupons/redeem?code=${redeem.code}`);
        return response;
      } catch (error) {
        return error.response;
      }
    },

  },
});