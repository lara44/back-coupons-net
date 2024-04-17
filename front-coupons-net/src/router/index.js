
import { createRouter, createWebHistory } from 'vue-router'
import home from '../views/Home.vue';
import login from '../views/Login.vue';
import user from '../components/users/UserContainer.vue';
import product from '../components/products/ProductContainer.vue';
import role from '../components/roles/RoleContainer.vue';
import { useLoginStore } from '../stores/loginStore';

const requiredAuth = async() =>{
    const loginStore = useLoginStore();
    const authToken = await localStorage.getItem('spa_token')
    if(authToken) {
        loginStore.token = authToken
        const auth = await loginStore.getAuth(loginStore.token)
        console.log("entró", auth, loginStore.token)
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
        //beforeEnter: requiredAuth, 
        children: [
            {
                path: '/users',
                name: 'users',
                component: user,
                //beforeEnter: requiredAuth
            },
            {
                path: '/products',
                name: 'products',
                component: product,
                //beforeEnter: requiredAuth
            },
            {
                path: '/roles',
                name: 'roles',
                component: role,
                //beforeEnter: requiredAuth
            },

        ]
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

export default router