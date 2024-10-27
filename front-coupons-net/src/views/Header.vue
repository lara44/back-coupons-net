<template>
  <v-app-bar color="#1d3557">
    <v-img
      max-height="80"
      max-width="150"
      src="/neocode.png"
      alt="Neocode"
      style="margin-left: 2em"
    ></v-img>
    <v-spacer></v-spacer>
    <Menu />
  </v-app-bar>
</template>
<script setup>
import Menu from "./Menu.vue";
</script>
