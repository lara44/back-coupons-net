import { createApp } from 'vue'
import router from './router'
import { createPinia } from 'pinia'
import axios from 'axios';


// Configurar baseURL para Axios
//axios.defaults.baseURL = 'https://localhost:7097';
axios.defaults.baseURL = 'http://48.216.136.52';

import App from './App.vue'

const pinia = createPinia()

// Vuetify
import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

const vuetify = createVuetify({
    components,
    directives,
})

createApp(App)
    .use(router)
    .use(vuetify)
    .use(pinia)
    .mount('#app')
