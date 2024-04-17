<template>
    <v-container fluid class="container-principal">
        <v-row justify="center">
            <v-col cols="12" sm="8" md="4">
                <v-card>
                    <v-toolbar color="#324c6e" dark flat>
                        <v-img style="left: 25px;" max-height="50" max-width="130" src="../src/assets/neocode.png"
                            alt="Vuetify"></v-img>
                    </v-toolbar>
                    <v-card-text>
                        <v-form>
                            <v-text-field label="Email" name="email" prepend-icon="mdi-account" type="text"
                                v-model="loginStore.user.username"></v-text-field>

                            <v-text-field label="Password" name="password" prepend-icon="mdi-lock" type="password"
                                v-model="loginStore.user.password"></v-text-field>
                            <div style="color: red; text-align: center" v-for="error in loginStore.errorLoginMessages"
                                :key="error">
                                <span>{{ error }}</span><br />
                            </div>

                            <!-- <pre>{{ loginStore.user }}</pre> -->
                            <!-- <pre>{{ loginStore.token }}</pre>-->
                            <pre>{{ loginStore.errorLoginMessages }}</pre>

                        </v-form>
                    </v-card-text>
                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn class="font-weight-bold" color="#324c6e" text @click="onLogin()">
                            <v-icon>mdi-arrow-right-bold mdi-24px</v-icon>Ingresar
                        </v-btn>
                    </v-card-actions>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>

<script lang="ts" setup>
import { useRouter } from 'vue-router'
import { useLoginStore } from '../stores/loginStore';

const loginStore = useLoginStore();
const router = useRouter()

const validateCredentials = () => {
    loginStore.cleanLogin()
    if (!loginStore.user?.username) loginStore.errorLoginMessages.push("Ingrese email");
    if (!loginStore.user?.password) loginStore.errorLoginMessages.push("Ingrese password");
    if (loginStore.errorLoginMessages.length > 0) loginStore.errorLogin = true;
    return loginStore.errorLogin;
};

const onLogin = async () => {
    if (validateCredentials()) {
        return
    }

    loginStore.login().then((response: any) => {
        if (response.status === 201) {
            router.push({
                name: 'home'
            })
        }
    })
}
</script>

<style>
.container-principal {
    min-height: 100vh;
    background: #eceff1;
    overflow: hidden;
    display: flex;
    align-items: center;
    justify-content: center;
}
</style>