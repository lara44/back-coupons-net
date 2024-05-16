
import { createRouter, createWebHistory } from 'vue-router'
import home from '../views/Home.vue';
import login from '../views/Login.vue';
import user from '../components/users/UserContainer.vue';
import product from '../components/products/ProductContainer.vue';
import category from '../components/categories/CategoryContainer.vue';
import city from '../components/cities/CityContainer.vue';
import country from '../components/countries/CountryContainer.vue';
import contact from '../components/contacts/ContactContainer.vue';
import company from '../components/companies/CompanyContainer.vue';
import state from '../components/states/StateContainer.vue';
import role from '../components/roles/RoleContainer.vue';
import { useLoginStore } from '../stores/loginStore';

const requiredAuth = async() =>{
    const loginStore = useLoginStore();
    const authToken = await localStorage.getItem('spa_token')
    if(authToken) {
        loginStore.token = authToken
        await loginStore.getAuth(loginStore.token)
    } else {
        router.push('/login');
    }
}

const requiredAuthAdmin = async() =>{
    const loginStore = useLoginStore();
    const authToken = await localStorage.getItem('spa_token')
    if(authToken) {
        loginStore.token = authToken;
        const decodedToken = loginStore.decodeToken(loginStore.token);
        const role = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        await loginStore.getAuth(loginStore.token);
        if (decodedToken && role === "Admin") { // assuming 'role' is the field that holds the user type
            return
        } else {
            router.push('/home');
        }
    } else {
        router.push('/login');
    }
}

const routes = [
    {
        path: '/',
        redirect: 'login',
    },
    {
        path: '/login',
        name: 'login',
        component: login
    },
    {
        path: '/home',
        name: 'home',
        component: home,
        beforeEnter: requiredAuth, 
        children: [
            {
                path: '/users',
                name: 'users',
                component: user,
                beforeEnter: requiredAuthAdmin
            },
            {
                path: '/products',
                name: 'products',
                component: product,
                beforeEnter: requiredAuth
            },
            {
                path: '/categories',
                name: 'categories',
                component: category,
                beforeEnter: requiredAuth
            },
            {
                path: '/cities',
                name: 'cities',
                component: city,
                beforeEnter: requiredAuthAdmin
            },
            {
                path: '/contacts',
                name: 'contacts',
                component: contact,
                beforeEnter: requiredAuthAdmin
            },
            {
                path: '/companies',
                name: 'companies',
                component: company,
                beforeEnter: requiredAuthAdmin
            },
            {
                path: '/countries',
                name: 'countries',
                component: country,
                beforeEnter: requiredAuthAdmin
            },
            {
                path: '/states',
                name: 'states',
                component: state,
                beforeEnter: requiredAuthAdmin
            },
            {
                path: '/roles',
                name: 'roles',
                component: role,
                beforeEnter: requiredAuthAdmin
            },

        ]
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

export default router