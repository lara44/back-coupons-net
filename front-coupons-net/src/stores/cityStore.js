import { defineStore } from "pinia";
import axios from "axios";

export const useCityStore = defineStore("cityStore", {
  state: () => {
    return {
      listCities: [],
      city: {
        id: "",
        name: "",
        stateId: "",
        states: "",
      },
    };
  },

  actions: {

    async getCities() {
      try {
        const response = await axios.get('/api/cities/full');
        if (response.data.data) {
          this.listCities =  response.data.data
          console.log("respuesta", response.data.data, this.listCites)
        }
      } catch (error) {
        console.error(error);
      }
    },

    async createCity(newCity) {
      try {
        const response = await axios.post('/api/cities', newCity);
        if(response.data.success){
          await this.getCities(); 
        }
      } catch (error) {
        console.error(error);
      }
    },
    
    async updateCity(updatedCity) {
      try {
        const response = await axios.put(`/api/cities/${updatedCity.id}`, updatedCity);
        if(response.data.success){
          await this.getCities();
        }
      } catch (error) {
        console.error(error);
      }
    },

    async deleteCity(deleteCity) {
      try {
        const response = await axios.delete(`/api/cities/${deleteCity.id}`, deleteCity);
        if(response.data.success){
          await this.getCities();
        }
      } catch (error) {
        console.error(error);
      }
    },

  },
});
