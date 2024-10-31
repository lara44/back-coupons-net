import { createApp } from "vue";
import router from "./router";
import { createPinia } from "pinia";
import axios from "axios";
import Toast, { POSITION } from "vue-toastification";
import "vue-toastification/dist/index.css";

// Configurar baseURL para Axios
axios.defaults.baseURL = "http://localhost:5163";
// axios.defaults.baseURL = 'http://48.216.136.52';

import App from "./App.vue";

const pinia = createPinia();

// Vuetify
import "vuetify/styles";
import "./assets/main.css";
import { createVuetify } from "vuetify";
import * as components from "vuetify/components";
import * as directives from "vuetify/directives";

const vuetify = createVuetify({
  components,
  directives,
});

const app = createApp(App);

// Configuración de Toastification
app.use(Toast, {
  position: POSITION.TOP_RIGHT,
  timeout: 5000,
  closeOnClick: true,
  pauseOnHover: true,
});

// Montar la aplicación
app.use(router).use(vuetify).use(pinia).mount("#app");
