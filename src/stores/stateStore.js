import { defineStore } from "pinia";
import axios from "axios";

export const useStateStore = defineStore("stateStore", {
  state: () => {
    return {
      listStates: [],
    };
  },

  actions: {

    async getStates() {
      try {
        const response = await axios.get('/api/states/full');
        if (response.data.data) {
          this.listStates =  response.data.data
        }
      } catch (error) {
        console.error(error);
      }
    },

    async getStatesByCountry(countryId) {
      try {
        const response = await axios.get(`/api/states/countries/${countryId}/states`);
        if (response.data.data) {
          this.listStates =  response.data.data
        }
      } catch (error) {
        console.error(error);
      }
    },

    async createState(newState) {
      try {
        const response = await axios.post('/api/states', newState);
        if(response.data.success){
          await this.getStates(); 
        }
      } catch (error) {
        console.error(error);
      }
    },
    
    async updateState(updatedState) {
      try {
        const response = await axios.put(`/api/states/${updatedState.id}`, updatedState);
        if(response.data.success){
          await this.getStates();
        }
      } catch (error) {
        console.error(error);
      }
    },

    async deleteState(deleteState) {
      try {
        const response = await axios.delete(`/api/states/${deleteState.id}`);
        if(response.data.success){
          await this.getStates();
        }
      } catch (error) {
        console.error(error);
      }
    },
  },
});
