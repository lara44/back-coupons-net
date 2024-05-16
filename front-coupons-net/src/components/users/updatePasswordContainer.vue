<template>
    <v-container >
        <v-row justify="center">
            <v-col cols="12" sm="8" md="4">
        <v-card>
            <v-card-title>
                <v-row>
                    <v-col md="6" sm="6" cols="12">
                        <span class="headline">Cambiar Contraseña</span>
                    </v-col>
                </v-row>
            </v-card-title>
            <v-card-text>
                <v-form>
                    <v-text-field 
                        label="Contraseña Actual" 
                        prepend-icon="mdi-lock" 
                        type="password"
                        v-model="changePasword.currentPassword"
                        :rules="[requiredRule('Contraseña Actual'), passwordRule]"
                    >
                    </v-text-field>

                    <v-text-field 
                        label="Contraseña Nueva" 
                        prepend-icon="mdi-lock" 
                        type="password"
                        v-model="changePasword.newPassword"
                        :rules="[requiredRule('Contraseña Nueva'), passwordRule]"
                    >
                    </v-text-field>

                    <v-text-field 
                        label="Confirmar Contraseña" 
                        prepend-icon="mdi-lock" 
                        type="password"
                        v-model="changePasword.confirm"
                        :rules="[requiredRule('Confirmar Contraseña'), passwordRule, passwordMatchRule]"
                    >
                    </v-text-field>

                    <div v-if="userStore.errorMessages.length > 0" style="color: red; text-align: center">
                        <ul>
                            <li v-for="error in userStore.errorMessages" :key="error">
                                {{ error }}
                            </li>
                        </ul>
                    </div>
                </v-form>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn class="font-weight-bold" color="#324c6e" text @click="submitForm">
                    <v-icon>mdi-lock</v-icon>Cambiar Contraseña
                </v-btn>
            </v-card-actions>
        </v-card>
            </v-col>
        </v-row>
        <v-snackbar v-model="successMessageVisible" timeout="3000">
            {{ notification }}
        </v-snackbar>
    </v-container>
</template>

<script setup>
import { ref, reactive, computed, onMounted } from "vue";
import { useUserStore } from '../../stores/userStore';

const userStore = useUserStore();
const notification = ref('');
const successMessageVisible = ref(false);
const changePasword = reactive({
    currentPassword: "",
    newPassword: "",
    confirm: ""
});


const requiredRule = (fieldName) => (value) => !!value || `El campo "${fieldName}" es obligatorio`;

const passwordRule = (value) => value.length >= 6 || "La contraseña debe tener al menos 6 caracteres";

const passwordMatchRule = (value) => value === changePasword.newPassword || "Las contraseñas no coinciden";


const submitForm = () => {

    if ( !changePasword.currentPassword || !changePasword.newPassword || !changePasword.confirm )
    {
      return;
    }

    userStore.changePassword(changePasword).then(response => {
      if(response.status === 400){
        if (response.data.errors) {
            if(response.data?.errors["NewPassword"]) userStore.errorMessages.push(response.data.errors["NewPassword"]);
            if(response.data?.errors["CurrentPassword"]) userStore.errorMessages.push(response.data.errors["CurrentPassword"]);
            if(response.data?.errors["Confirm"]) userStore.errorMessages.push(response.data.errors["Confirm"]);
        } else {
            userStore.errorMessages.push(response.data);
        } 
      }else{
        notification.value = "Contraseña Cambiada Exitosamente";
        successMessage();
        userStore.cleanErrors();
      }
    });

}

const successMessage = () =>{
  successMessageVisible.value = true;
  setTimeout(() => {
    successMessageVisible.value = false;
  }, 3000);
}

</script>