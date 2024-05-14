import axios from 'axios'
import { defineStore } from 'pinia'
import router from '../router'

export const useLoginStore = defineStore('login', {
    state: () => {
      return { 
          errorLogin: false,
          errorLoginMessages: [],
          message: '',
          token: '',
          user:{
            email: '',
            password: '',
          }          
        }
    },

    actions: {
      cleanLogin(){
        this.errorLoginMessages = []
      },

      async login($user) {
        try {
          
          console.log($user);

          const response = await axios.post('/api/users/Login', $user)
          
          if(response.data.token) {
            this.token = response.data.token
            localStorage.setItem('spa_token', this.token)
            this.setHeaders();
            axios.defaults.headers.common['X-Requested-With'] = 'XMLHtpRequest'
          }
          return response
        } catch (error) {
          this.message = error.message
          return error
        }
      },

      async getAuth(token) {
        if (token) {
            this.token = token
        }

        if (!this.token) {
            return
        }

        try {
            this.setHeaders();
        } catch (e) {
            console.log("error: ", e)
        }
      },

      async logout() {
        localStorage.removeItem("spa_token");
        this.token = ''
        router.push({name: 'login'})
      },

      async setHeaders() {
        axios.defaults.headers.common['Authorization'] = `Bearer ${this.token}`;
        axios.defaults.headers.common['Access-Control-Allow-Origin'] = '*';
        axios.defaults.headers.common['Access-Control-Allow-Methods'] = 'GET, POST, PATCH, PUT, DELETE, OPTIONS'
        axios.defaults.headers.common['Content-Type'] = 'application/json'
        axios.defaults.headers.common['X-Requested-With'] = 'XMLHtpRequest'
      }
    },
  })