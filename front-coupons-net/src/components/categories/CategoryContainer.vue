<template>
  <v-container>
    <v-dialog v-model="dialog" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="headline">{{ selectedCategory ? 'Editar Categoria' : 'Crear Categoria' }}</span>
        </v-card-title>
        <v-card-text>
          <v-form @submit.prevent="submitForm">
            <v-text-field v-model="newCategory.name" label="Nombre" 
              :rules="[requiredRule('Nombre')]" required></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn color="blue darken-1" text @click="closeModal">Cancelar</v-btn>
          <v-btn color="primary" @click="submitForm">{{ selectedCategory ? 'Actualizar' : 'Guardar' }}</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <!-- Tabla HTML para mostrar los usuarios -->
    <v-card>
      <v-card-title>
        <v-row>
          <v-col md="6" sm="6" cols="12">
            <span class="headline">Lista de Categorias registradass</span>
          </v-col>
          <v-col md="6" sm="6" cols="12">
            <v-text-field v-model="search" append-icon="mdi-magnify" label="Buscar Categoria" single-line hide-details
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
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="category in filteredCategories" :key="category.id">
              <td>{{ category.id }}</td>
              <td>{{ category.name }}</td>
              <td>
                <v-icon @click="editCategory(category)" color="primary">mdi-pencil</v-icon>
              </td>
            </tr>
            <tr v-if="!filteredCategories.length">
              <td colspan="4">No se encontraron categorias</td>
            </tr>
          </tbody>
        </v-table>
      </v-card-text>
      <v-pagination v-model="currentPage" rounded="circle" :length="totalPages" style="box-shadow: none !important;"></v-pagination>
    </v-card>

    <!-- Snackbar para mostrar el mensaje de éxito -->
    <v-snackbar v-model="successMessageVisible" timeout="3000">
      {{ selectedCategory ? 'Categoria actualizada exitosamente' : 'Categoria creada exitosamente' }}
    </v-snackbar>
  </v-container>
</template>

<script>
import { ref, reactive, computed } from 'vue';
import { useCategoryStore } from '../../stores/categoryStore';

export default {
  name: 'CategoryDataTable',
  setup() {
    const currentPage = ref(1); // Página actual
    const itemsPerPage = 10; // Número de usuarios por página
    const categoryStore = useCategoryStore();
    const successMessageVisible = ref(false);
    const search = ref('');
    const newCategory = reactive({
      name: '',
    });
    const selectedCategory = ref(null);
    const dialog = ref(false);

    const totalCategories = computed(() => categoryStore.listCategories.length);
    const totalPages = computed(() => Math.ceil(totalCategories.value / itemsPerPage));

    const filteredCategories = computed(() => {
      const startIndex = (currentPage.value - 1) * itemsPerPage;
      const endIndex = startIndex + itemsPerPage;
      const categoryList = categoryStore.listCategories;
      return categoryList
        .filter(category => category.name.toLowerCase().includes(search.value.toLowerCase()))
        .slice(startIndex, endIndex);
    });

    const requiredRule = (fieldName) => (value) => !!value || `El campo "${fieldName}" es obligatorio`;

    const openModal = () => {
      dialog.value = true;
      selectedCategory.value = null;
      newCategory.name = '';
    };

    const submitForm = async () => {
      if (!newCategory.name) {
        return;
      }
      if (selectedCategory.value) {
        await categoryStore.updateCategory({ ...selectedCategory.value, ...newCategory });
      } else {
        await categoryStore.createCategory(newCategory);
      }

      await categoryStore.getCategories();

      newCategory.name = '';

      successMessageVisible.value = true;

      setTimeout(() => {
        successMessageVisible.value = false;
      }, 3000);

      dialog.value = false;
    };

    const editCategory = (category) => {
      selectedCategory.value = { ...category };
      newCategory.name = selectedCategory.value.name;
      dialog.value = true;
    };

    const closeModal = () => {
      dialog.value = false;
      selectedCategory.value = null;
      newCategory.name = '';
    };

    return {
      search,
      currentPage,
      itemsPerPage,
      filteredCategories,
      newCategory,
      successMessageVisible,
      selectedCategory,
      dialog,
      submitForm,
      requiredRule,
      editCategory,
      openModal,
      closeModal,
      totalPages
    };
  },

  async mounted() {
    try {
      await useCategoryStore().getCategories();
    } catch (error) {
      console.error(error);
    } finally {
      this.loading = false;
    }
  },
};
</script>