import { defineStore } from "pinia";
import axios from "axios";

export const useRoleStore = defineStore("roleStore", {
  state: () => {
    return {
      listRoles: [],
      role: {
        id: "",
        name: "",
        created_at: "",
      },
    };
  },

  actions: {
   
    async getRoles() {
      try {
        const staticRoles = [
          {
            id: 1,
            name: "Administrador",
            created_at: "2024-01-01",
          },
          {
            id: 2,
            name: "Empresa",
            created_at: "2024-02-02",
          },
        ];
    
        console.log("Datos de roles:", staticRoles);
    
        this.listRoles = staticRoles;
      } catch (error) {
        console.error(error);
      }
    },

    // async getRoles() {
    //   try {
    //     const response = await axios.get('http://localhost:8000/laravel9/public/api/auth/roles');
    //     if (response.data.roles) {
    //       this.patchState({
    //         listRoles: response.data.roles.data
    //       });
    //     }
    //   } catch (error) {
    //     console.error(error);
    //   }
    // },

    async createRole(newRole) {
      try {
        const response = await axios.post('http://localhost:8000/laravel9/public/api/auth/roles', newRole);
        if(response.data.success){
          await this.getRoles(); //si fue exitoso actualizo la lista de roles
        }
      } catch (error) {
        console.error(error);
      }
    },
    async updateRole(updatedRole) {
      try {
        const response = await axios.put(`http://localhost:8000/laravel9/public/api/auth/roles/${updatedRole.id}`, updatedRole);
        if(response.data.success){
          await this.getRoles(); //si fue exitoso actualizo la lista de roles
        }
      } catch (error) {
        console.error(error);
      }
    },
  },
});
