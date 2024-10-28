<template>
  <v-layout>
    <v-app-bar color="#1d3557" prominent>
      <v-app-bar-nav-icon
        variant="text"
        @click.stop="drawer = !drawer"
        color="#FDD5A8"
      ></v-app-bar-nav-icon>
      <v-img
        max-height="80"
        max-width="140"
        src="/neocode.png"
        alt="Vuetify"
      ></v-img>
      <!-- <v-toolbar-title class="title">NEW SPA</v-toolbar-title> -->
      <v-spacer></v-spacer>
      <v-avatar>
        <v-img alt="User Photo" :src="user.photo"></v-img>
      </v-avatar>

      <p style="margin-right: 20px">Bienvenido {{ user.fullName }}</p>
    </v-app-bar>

    <v-navigation-drawer v-model="drawer" floating permanent>
      <v-list nav dense>
        <v-list-item
          link
          prepend-icon="mdi-home-city mdi-18px"
          title="Home"
        ></v-list-item>
        <v-list-item
          link
          prepend-icon="mdi-view-dashboard mdi-18px "
          title="Dashboard"
          to="/dashboard"
        ></v-list-item>
        <v-list-group dense>
          <template v-slot:activator="{ props }">
            <v-list-item
              v-bind="props"
              prepend-icon="mdi-account mdi-18px"
              title="Empresa"
              value="Empresa"
            ></v-list-item>
          </template>
          <v-list-item
            link
            style="font-size: 10px !important"
            prepend-icon="mdi-package-variant-closed mdi-18px"
            title="Productos"
            to="/products"
          ></v-list-item>
          <v-list-item
            link
            style="font-size: 10px !important"
            prepend-icon="mdi-clipboard-outline mdi-18px"
            title="Categorias"
            to="/categories"
          ></v-list-item>
          <v-list-item
            link
            style="font-size: 10px !important"
            prepend-icon="mdi-account-group-outline mdi-18px"
            title="Cupones"
            to="/coupons"
          ></v-list-item>
        </v-list-group>
        <v-list-group v-if="user.role === 'Admin'" dense>
          <template v-slot:activator="{ props }">
            <v-list-item
              v-bind="props"
              prepend-icon="mdi-account mdi-18px"
              title="Admin"
              value="Admin"
            ></v-list-item>
          </template>
          <v-list-item
            link
            style="font-size: 10px !important"
            prepend-icon="mdi-store-24-hour mdi-18px"
            title="Empresas"
            to="/companies"
          ></v-list-item>
          <v-list-item
            link
            style="font-size: 10px !important"
            prepend-icon="mdi-contacts mdi-18px"
            title="Contactos"
            to="/contacts"
          ></v-list-item>
          <v-list-item
            link
            style="font-size: 10px !important"
            prepend-icon="mdi-google-maps mdi-18px"
            title="Paises"
            to="/countries"
          ></v-list-item>
          <v-list-item
            link
            style="font-size: 10px !important"
            prepend-icon="mdi-map mdi-18px"
            title="Departamentos"
            to="/states"
          ></v-list-item>
          <v-list-item
            link
            style="font-size: 10px !important"
            prepend-icon="mdi-city mdi-18px"
            title="Ciudades"
            to="/cities"
          ></v-list-item>
          <v-list-item
            link
            style="font-size: 10px !important"
            prepend-icon="mdi-account-group-outline mdi-18px"
            title="Users"
            to="/users"
          ></v-list-item>
        </v-list-group>
        <v-list-item
          link
          prepend-icon="mdi-account-key mdi-18px"
          title="Cambiar Contraseña"
          to="/user/updatePassword"
        ></v-list-item>
        <v-list-item
          link
          prepend-icon="mdi-logout-variant mdi-18px"
          title="Logout"
          @click="loginStore.logout"
        ></v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-main style="background: #eceff1; min-height: 100vh">
      <!-- <pre>{{ loginStore.user }}</pre>
            <p>token: {{ loginStore.token }}</p> -->
      <v-container>
        <router-view />
      </v-container>
    </v-main>
  </v-layout>
  <!-- <v-footer dense bottom absolute width="100%"> -->
  <v-footer>
    <span style="color: #1d3557 !important; font-weight: 400"
      >Neocode — &copy; {{ new Date().getFullYear() }}</span
    >
  </v-footer>
</template>

<script lang="ts" setup>
import { watch, ref, reactive, computed, onMounted } from "vue";
import { useLoginStore } from "../stores/loginStore";
import { useUserStore } from "../stores/userStore";

const loginStore = useLoginStore();
const userStore = useUserStore();

const user = reactive({
  fullName: "",
  authToken: "",
  decodedToken: "",
  role: "",
  photo: "",
  companyId: "",
});

const drawer = ref(true);

watch(
  () => drawer,
  () => {
    drawer.value = false;
  }
);

onMounted(() => {
  user.authToken = localStorage.getItem("spa_token");
  user.decodedToken = loginStore.decodeToken(user.authToken);
  user.role =
    user.decodedToken[
      "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    ];
  user.fullName =
    user.decodedToken["FirstName"] + " " + user.decodedToken["LastName"];
  user.photo = user.decodedToken["Photo"];
  user.email =
    user.decodedToken[
      "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
    ];
  user.companyId = user.decodedToken["CompanyId"];

  userStore.setUser(user);
});
</script>

<style scoped>
.title {
  color: #fff;
}
</style>
