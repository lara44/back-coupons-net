import { defineStore } from "pinia";
import axios from "axios";

export const useUserStore = defineStore("userStore", {
  state: () => {
    return {
      listUsers: [],
      user: {
        userName: "",
        normalizedUserName: "",
        email: "",
        normalizedEmail: "",
        document: "",
        firstName: "",
        lastName: "",
        address: "",
        userType: "",
        cityId: "",
      },
    };
  },

  actions: {

    async getUsers() {
      try {
        const response = await axios.get('/api/users');
        if (response.data.users) {
          this.listUsers =  response.data.users
          console.log("respuesta", response.data.users, this.listUsers)
        }
      } catch (error) {
        console.error(error);
      }
    },

    async createUser(newUser) {
      try {
        const response = await axios.post('/api/users', newUser);
        if(response.data.success){
          await this.getUsers(); 
        }
      } catch (error) {
        console.error(error);
      }
    },
    
    async updateUser(updatedUser) {
      try {
        const response = await axios.put(`/api/users/${updatedUser.id}`, updatedUser);
        if(response.data.success){
          await this.getUsers();
        }
      } catch (error) {
        console.error(error);
      }
    },

    async deleteUser(deleteUser) {
      try {
        const response = await axios.delete(`/api/users/${deleteUser.id}`);
        if(response.data.success){
          await this.getUsers();
        }
      } catch (error) {
        console.error(error);
      }
    },

  },
});
