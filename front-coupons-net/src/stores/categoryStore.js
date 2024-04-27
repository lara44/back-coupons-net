import { defineStore } from "pinia";
import axios from "axios";

export const useCategoryStore = defineStore("categoryStore", {
  state: () => {
    return {
      listCategories: [],
      category: {
        id: "",
        name: "",
      },
    };
  },

  actions: {

    async getCategories() {
      try {
        const response = await axios.get('/api/categories/full');
        if (response.data.data) {
          this.listCategories =  response.data.data
          console.log("respuesta", response.data.data, this.listCategories)
        }
      } catch (error) {
        console.error(error);
      }
    },

    async createCategory(newCategory) {
      try {
        const response = await axios.post('/api/categories', newCategory);
        if(response.data.success){
          await this.getCategories(); 
        }
      } catch (error) {
        console.error(error);
      }
    },
    
    async updateCategory(updatedCategory) {
      try {
        const response = await axios.put(`/api/categories/${updatedCategory.id}`, updatedCategory);
        if(response.data.success){
          await this.getCategories();
        }
      } catch (error) {
        console.error(error);
      }
    },

    async deleteCategory(deleteCategory) {
      try {
        const response = await axios.delete(`/api/categories/${deleteCategory.id}`, deleteCategory);
        if(response.data.success){
          await this.getCategories();
        }
      } catch (error) {
        console.error(error);
      }
    },

  },
});
