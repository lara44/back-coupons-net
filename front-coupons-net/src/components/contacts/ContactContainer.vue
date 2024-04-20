<template>
  <v-container>
    <v-dialog v-model="dialog" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="headline">{{ selectedContact ? 'Editar Contacto' : 'Crear Contacto' }}</span>
        </v-card-title>
        <v-card-text>
          <v-form @submit.prevent="submitForm">
            <v-text-field v-model="newContact.name" label="Nombre" 
              :rules="[requiredRule('Nombre')]" required></v-text-field>

              <v-text-field v-model="newContact.phone" label="Teléfono" 
              :rules="[requiredRule('Teléfono'), numberdRule]" required></v-text-field>

              <v-text-field v-model="newContact.address" label="Dirección" 
              :rules="[requiredRule('Dirección')]" required></v-text-field>

              <v-text-field v-model="newContact.email" label="Correo electrónico"
              :rules="[requiredRule('Correo electrónico'), emailRule]" required></v-text-field>

              <v-text-field v-model="newContact.companyId" label="Empresa" 
              :rules="[requiredRule('Empresa'), numberdRule]" required></v-text-field>

          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn color="blue darken-1" text @click="closeModal">Cancelar</v-btn>
          <v-btn color="primary" @click="submitForm">{{ selectedContact ? 'Actualizar' : 'Guardar' }}</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <!-- Tabla HTML para mostrar los usuarios -->
    <v-card>
      <v-card-title>
        <v-row>
          <v-col md="6" sm="6" cols="12">
            <span class="headline">Lista de Contactos registrados</span>
          </v-col>
          <v-col md="6" sm="6" cols="12">
            <v-text-field v-model="search" append-icon="mdi-magnify" label="Buscar Contacto" single-line hide-details
              variant="underlined"></v-text-field>
          </v-col>
        </v-row>
      </v-card-title>
      <v-btn class="ma-2" color="primary" dark @click="openModal" >Nuevo</v-btn>
      <v-card-text>
        <v-table density="compact">
          <thead>
            <tr>
              <th>ID</th>
              <th>Nombre</th>
              <th>Telefono</th>
              <th>Dirección</th>
              <th>Email</th>
              <th>Empresa</th>
              <th>Company</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="contact in filteredContacts" :key="contact.id">
              <td>{{ contact.id }}</td>
              <td>{{ contact.name }}</td>
              <td>{{ contact.phone }}</td>
              <td>{{ contact.address }}</td>
              <td>{{ contact.email }}</td>
              <td>{{ contact.companyId }}</td>
              <td>{{ contact.company }}</td>
              <td>
                <v-icon @click="editContact(contact)" color="primary">mdi-pencil</v-icon>
              </td>
            </tr>
            <tr v-if="!filteredContacts.length">
              <td colspan="4">No se encontraron Contactos</td>
            </tr>
          </tbody>
        </v-table>
      </v-card-text>
      <v-pagination v-model="currentPage" rounded="circle" :length="totalPages" style="box-shadow: none !important;"></v-pagination>
    </v-card>

    <!-- Snackbar para mostrar el mensaje de éxito -->
    <v-snackbar v-model="successMessageVisible" timeout="3000">
      {{ selectedContact ? 'Contacto actualizadO exitosamente' : 'Contacto creado exitosamente' }}
    </v-snackbar>
  </v-container>
</template>

<script>
import { ref, reactive, computed } from 'vue';
import { useContactStore } from '../../stores/contactStore';

export default {
  name: 'ContactDataTable',
  setup() {
    const currentPage = ref(1); // Página actual
    const itemsPerPage = 10; // Número de usuarios por página
    const contactStore = useContactStore();
    const successMessageVisible = ref(false);
    const search = ref('');
    const newContact = reactive({
        id: "",
        name: "",
        phone: "",
        address: "",
        companyId: "",
        company: "",
    });
    const selectedContact = ref(null);
    const dialog = ref(false);

    const totalContacts = computed(() => contactStore.listContacts.length);
    const totalPages = computed(() => Math.ceil(totalContacts.value / itemsPerPage));

    const filteredContacts = computed(() => {
      const startIndex = (currentPage.value - 1) * itemsPerPage;
      const endIndex = startIndex + itemsPerPage;
      const contactList = contactStore.listContacts;
      return contactList
        .filter(contact => contact.name.toLowerCase().includes(search.value.toLowerCase()))
        .slice(startIndex, endIndex);
    });

    const requiredRule = (fieldName) => (value) => !!value || `El campo "${fieldName}" es obligatorio`;

    const numberdRule = (value) => {
      const numberRegex = /^[0-9]+$/;
      return numberRegex.test(value) || 'El campo debe ser un número válido';
    };

    const emailRule = (value) => {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      return emailRegex.test(value) || 'El campo "Correo electrónico" debe ser un correo electrónico válido';
    };


    const openModal = () => {
      dialog.value = true;
      selectedContact.value = null;
      newContact.name = '';
      newContact.phone = '';
      newContact.address = '';
      newContact.email = '';
      newContact.companyId = '';
      newContact.company = '';
    };

    const submitForm = async () => {
      if (!newContact.name || !newContact.phone || !newContact.address || !newContact.email || !newContact.companyId) {
        return;
      }
      if (selectedContact.value) {
        await contactStore.updateContact({ ...selectedContact.value, ...newContact });
      } else {
        await contactStore.createContact(newContact);
      }

      await contactStore.getContacts();

      newContact.name = '';
      newContact.phone = '';
      newContact.address = '';
      newContact.email = '';
      newContact.companyId = '';
      newContact.company = '';

      successMessageVisible.value = true;

      setTimeout(() => {
        successMessageVisible.value = false;
      }, 3000);

      dialog.value = false;
    };

    const editContact = (contact) => {
      selectedContact.value = { ...contact };
      newContact.name = selectedContact.value.name;
      newContact.phone = selectedContact.value.phone;
      newContact.address = selectedContact.value.address;
      newContact.email = selectedContact.value.email;
      newContact.companyId = selectedContact.value.companyId;
      newContact.company = selectedContact.value.company;
      dialog.value = true;
    };

    const closeModal = () => {
      dialog.value = false;
      selectedContact.value = null;
      newContact.name = '';
      newContact.phone = '';
      newContact.address = '';
      newContact.email = '';
      newContact.companyId = '';
      newContact.company = '';
    };

    return {
      search,
      currentPage,
      emailRule,
      itemsPerPage,
      filteredContacts,
      newContact,
      successMessageVisible,
      selectedContact,
      dialog,
      submitForm,
      requiredRule,
      numberdRule,
      editContact,
      openModal,
      closeModal,
      totalPages
    };
  },

  async mounted() {
    try {
      await useContactStore().getContacts();
    } catch (error) {
      console.error(error);
    } finally {
      this.loading = false;
    }
  },
};
</script>