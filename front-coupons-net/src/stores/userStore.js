import { defineStore } from "pinia";
import axios from "axios";

export const useUserStore = defineStore("userStore", {
  state: () => {
    return {
      listUsers: [],
      user: [],
    };
  },

  actions: {
    
    async getUsers() {
      try {
        const response = await axios.get("/api/users/1234");
        if (response.data) {
          this.listUsers = response.data;
          console.log(this.listUsers);
        }
      } catch (error) {
        console.error(error);
      }
    },

    async createUser(newUser) {
      try {
        const response = await axios.post("/api/users/CreateUser", newUser);
        return response
      } catch (error) {
        return error.response
      }
    },

    async updateUser(updatedUser) {
      try {
        const response = await axios.put(
          `/api/users/${updatedUser.id}`,
          updatedUser
        );
        if (response.data.success) {
          await this.getUsers();
        }
      } catch (error) {
        console.error(error);
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
  },
});
