import { defineStore } from "pinia";
import axios from "axios";

export const useUserStore = defineStore("userStore", {
  state: () => {
    return {
      issetErrorMessages:false,
      listUsers: [],
      errorMessages:[],
      user: [],
    };
  },

  actions: {
    
    async getUsers() {
      try {
        const response = await axios.get("/api/users/GetUserPaginationAsync");
        if (response.data.data) {
          this.listUsers = response.data.data;
        }
        console.log(response.data.data);
        return response;
      } catch (error) {
        return error.response
      }
    },

    async createUser(newUser) {
      try {
        const response = await axios.post("/api/users/CreateUser", newUser);
        return response;
      } catch (error) {
        return error.response;
      }
    },

    async updateUser(updatedUser) {
      try {
        const response = await axios.put('/api/users/', updatedUser);
        return response;
      } catch (error) {
        return error.response;
      }
    },

    async changePassword(user) {
      try {
        const response = await axios.post('/api/users/changePassword', user);
        return response;
      } catch (error) {
        return error.response;
      }
    },

    async resetPassword(user) {
      try {
        const response = await axios.post('/api/users/ResetPassword', user);
        return response;
      } catch (error) {
        return error.response;
      }
    },

    async recoverPassword(user) {
      try {
        const response = await axios.post('/api/users/RecoverPassword', user);
        return response;
      } catch (error) {
        return error.response;
      }
    },


    async deleteUser(deleteUser) {
      try {
        const response = await axios.delete(`/api/users/${deleteUser.id}`);
        if (response.data.success) {
          await this.getUsers();
        }
      } catch (error) {
        console.error(error);
      }
    },

    async resedToken(user) {
      try {
        await axios.post('/api/users/ResedToken', user);
      } catch (error) {
        console.error(error);
      }
    },

    cleanErrors(){
      this.errorMessages = []
    },

  },
});
