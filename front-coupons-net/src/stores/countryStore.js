import { defineStore } from "pinia";
import axios from "axios";

export const useCountryStore = defineStore("countryStore", {
  state: () => {
    return {
      listCountries: [],
      country: {
        id: "",
        name: "",
      },
    };
  },

  actions: {
    async getCountries() {
      try {
        const response = await axios.get("/api/countries/full");
        if (response.data.data) {
          this.listCountries = response.data.data;
        }
      } catch (error) {
        console.error(error);
      }
    },

    async createCountry(newCountry) {
      try {
        const response = await axios.post("/api/countries", newCountry);
        if (response.data.success) {
          await this.getCountries();
        }
      } catch (error) {
        console.error(error);
      }
    },

    async updateCountry(updatedCountry) {
      try {
        const response = await axios.put(
          `/api/countries/${updatedCountry.id}`,
          updatedCountry
        );
        if (response.data.success) {
          await this.getCountries();
        }
      } catch (error) {
        console.error(error);
      }
    },

    async deleteCountry(deleteCountry) {
      try {
        return await axios.delete(`/api/countries/${deleteCountry.id}`);
      } catch (error) {
        console.error(error);
      }
    },
  },
});