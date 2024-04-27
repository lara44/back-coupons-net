import { defineStore } from "pinia";
import axios from "axios";

export const useContactStore = defineStore("contactStore", {
  state: () => {
    return {
      listContacts: [],
      contact: {
        id: "",
        name: "",
        phone: "",
        address: "",
        companyId: "",
        company: ""
      },
    };
  },

  actions: {

    async getContacts() {
      try {
        const response = await axios.get('/api/contacts/full');
        if (response.data.data) {
          this.listContacts =  response.data.data
          console.log("respuesta", response.data.data, this.listCites)
        }
      } catch (error) {
        console.error(error);
      }
    },

    async createContact(newContact) {
      try {
        const response = await axios.post('/api/contacts', newContact);
        if(response.data.success){
          await this.getContacts(); 
        }
      } catch (error) {
        console.error(error);
      }
    },
    
    async updateContact(updatedContact) {
      try {
        const response = await axios.put(`/api/contacts/${updatedContact.id}`, updatedContact);
        if(response.data.success){
          await this.getContacts();
        }
      } catch (error) {
        console.error(error);
      }
    },

    async deleteContact(deleteContact) {
      try {
        const response = await axios.delete(`/api/contacts/${deleteContact.id}`, deleteContact);
        if(response.data.success){
          await this.getContacts();
        }
      } catch (error) {
        console.error(error);
      }
    },

  },
});
