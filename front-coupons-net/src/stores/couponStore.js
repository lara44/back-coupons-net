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
    async getCoupons(role = null, company = null) {
      try {
        let companyId = "";

        if (role && company) {
          companyId = role === "Admin" ? "" : `?id=${company}`;
        }

        const response = await axios.get(`/api/coupons${companyId}`);
        if (response.data.data) {
          this.listCoupons = response.data.data;
          console.log(response.data.data);
        }
      } catch (error) {
        console.error(error);
      }
    },

    async getCouponsFull() {
      try {
        const response = await axios.get(`/api/coupons/full`);
        if (response.data.data) {
          this.listCoupons = response.data.data.sort((a, b) => b.id - a.id);
          console.log(this.listCoupons);
        }
      } catch (error) {
        console.error(error);
      }
    },

    async createCoupon(newCoupon) {
      try {
        const response = await axios.post("/api/coupons", newCoupon);
        return response;
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
        return response;
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
        return response;
      } catch (error) {
        console.error(error);
      }
    },

    async redeemCoupon(redeem) {
      try {
        const response = await axios.post(`/api/redeem/RedeemCoupon`, redeem);
        return response;
      } catch (error) {
        return error.response;
      }
    },
  },
});
