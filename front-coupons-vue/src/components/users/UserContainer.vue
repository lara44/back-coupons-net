<template>
    <v-card>
        <v-card-title>
            <v-row>
                <v-col md="8" sm="6" cols="12">
                    List of users
                </v-col>
            </v-row>
        </v-card-title>
        <v-card-text>
            <v-table density="compact">
                <thead>
                    <tr>
                        <th class="text-left">Name</th>
                        <th class="text-left">Email</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="user in userStore.listUsers" :key="user.id">
                        <td>{{ user.name }}</td>
                        <td>{{ user.email }}</td>
                    </tr>
                </tbody>
            </v-table>
        </v-card-text>
    </v-card>

    
</template>

<script setup lang="ts">
import { useUserStore } from '../../stores/userStore';
const userStore = useUserStore();

userStore.getUsers();

</script>