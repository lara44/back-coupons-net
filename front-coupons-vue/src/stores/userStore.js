import { defineStore } from "pinia"
import axios from "axios";

export const useUserStore = defineStore('userStore', {
    state: () => {
        return {
            listUsers: [],
            user: {
                id: '',
                name: '',
                email: '',
            }
        } 
    }, 

    actions: {
        async getUsers(){
            try {
                console.log("entró")
                const response = await axios.get('https://localhost:7097/api/users')
                if(response.data.users){
                    this.listUsers =  response.data.users
                    console.log("respuesta", response.data.users, this.listUsers)
                }
            } catch (error) {
                console.log(error)
            }
        }
    }
})