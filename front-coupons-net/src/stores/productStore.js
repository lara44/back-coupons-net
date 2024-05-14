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
        const response = await axios.get('/api/products/full');
        if (response.data.data) {
          this.listProducts =  response.data.data
          console.log("respuesta", response.data.data, this.listProducts)
        }
      } catch (error) {
        console.error(error);
      }
    },

    async createProduct(newProduct) {
      try {
        const response = await axios.post('/api/products', newProduct);
        if(response.data.success){
          await this.getProducts(); 
        }
      } catch (error) {
        console.error(error);
      }
    },
    
    async updateProduct(updatedProduct) {
      try {
        const response = await axios.put(`/api/products/${updatedProduct.id}`);
        if(response.data.success){
          await this.getProducts();
        }
      } catch (error) {
        console.error(error);
      }
    },

    async deleteProduct(deleteProduct) {
      try {
        const response = await axios.delete(`/api/products/${deleteProduct.id}`);
        if(response.data.success){
          await this.getProducts();
        }
      } catch (error) {
        console.error(error);
      }
    },

  },
});
