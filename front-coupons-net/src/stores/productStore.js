import { defineStore } from "pinia";
import axios from "axios";

export const useProductStore = defineStore("productStore", {
  state: () => {
    return {
      listProducts: [],
      product: {
        id: "",
        name: "",
        email: "",
      },
    };
  },

  actions: {
    async getProducts() {
      try {
        const response = await axios.get("/api/products/full");
        if (response.data.data) {
          this.listProducts = response.data.data;
        }
      } catch (error) {
        console.error(error);
      }
    },

    async getProductsByCompany(companyId) {
      try {
        const response = await axios.get(
          `/api/products/GetProductsByCompany?CompanyId=${companyId}`
        );
        if (response.data.data) {
          this.listProducts = response.data.data;
        }
      } catch (error) {
        console.error(error);
      }
    },

    async createProduct(newProduct) {
      try {
        const response = await axios.post("/api/products", newProduct);
        return response;
      } catch (error) {
        console.error(error);
      }
    },

    async updateProduct(updatedProduct) {
      try {
        const response = await axios.put(`/api/products/${updatedProduct.id}`);
        return response;
      } catch (error) {
        console.error(error);
      }
    },

    async deleteProduct(deleteProduct) {
      try {
        const response = await axios.delete(
          `/api/products/${deleteProduct.id}`
        );
        return response;
      } catch (error) {
        console.error(error);
      }
    },
  },
});
