import { defineStore } from "pinia";
import axios from "axios";

export const useStateStore = defineStore("stateStore", {
  state: () => {
    return {
      listStates: [],
      state: {
        id: "",
        name: "",
        countryId: "",
      },
    };
  },

  actions: {

    async getStates() {
      try {
        const response = await axios.get('/api/states/full');
        if (response.data.data) {
          this.listStates =  response.data.data
          console.log("respuesta", response.data.data, this.listCites)
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
  },
});
