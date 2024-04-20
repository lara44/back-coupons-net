import { defineStore } from "pinia";
import axios from "axios";

export const useCompanyStore = defineStore("companyStore", {
  state: () => {
    return {
      listCompanies: [],
      company: {
        id: "",
        nit: "",
        name: "",
        address: "",
        email: "",
        phone: "",
        contacts: "",
      },
    };
  },

  actions: {

    async getCompanies() {
      try {
        const response = await axios.get('/api/companies/full');
        if (response.data.data) {
          this.listCompanies =  response.data.data
          console.log("respuesta", response.data.data, this.listCompanies)
        }
      } catch (error) {
        console.error(error);
      }
    },

    async createCompany(newCompany) {
      try {
        const response = await axios.post('/api/companies', newCompany);
        if(response.data.success){
          await this.getCompanies(); 
        }
      } catch (error) {
        console.error(error);
      }
    },
    
    async updateCompany(updatedCompany) {
      try {
        const response = await axios.put(`/api/companies/${updatedCompany.id}`, updatedCompany);
        if(response.data.success){
          await this.getCompanies();
        }
      } catch (error) {
        console.error(error);
      }
    },
  },
});
