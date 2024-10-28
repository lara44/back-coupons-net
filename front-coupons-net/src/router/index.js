import { createRouter, createWebHistory } from "vue-router";
import home from "../views/Home.vue";
import login from "../views/Login.vue";
import dashboard from "../views/Dashboard.vue";
import updatePassword from "../components/users/updatePasswordContainer.vue";
import forgotPasword from "../components/users/forgotPasswordContainer.vue";
import resetPassword from "../components/users/resetPasswordContainer.vue";
import confirmEmail from "../components/users/confirmEmailContainer.vue";
import redeemCoupon from "../components/coupons/RedeemCuponContainer.vue";
import user from "../components/users/UserContainer.vue";
import product from "../components/products/ProductContainer.vue";
import coupon from "../components/coupons/CouponsContainer.vue";
import inicio from "../views/Index.vue";
import category from "../components/categories/CategoryContainer.vue";
import city from "../components/cities/CityContainer.vue";
import country from "../components/countries/CountryContainer.vue";
import contact from "../components/contacts/ContactContainer.vue";
import company from "../components/companies/CompanyContainer.vue";
import state from "../components/states/StateContainer.vue";
import role from "../components/roles/RoleContainer.vue";
import { useLoginStore } from "../stores/loginStore";
import consultarCupones from "../components/coupons/ConsultarCupones.vue";

const requiredAuth = async () => {
  const loginStore = useLoginStore();
  const authToken = await localStorage.getItem("spa_token");
  if (authToken) {
    loginStore.token = authToken;
    await loginStore.getAuth(loginStore.token);
  } else {
    router.push("/login");
  }
};

const requiredAuthAdmin = async () => {
  const loginStore = useLoginStore();
  const authToken = await localStorage.getItem("spa_token");
  if (authToken) {
    loginStore.token = authToken;
    const decodedToken = loginStore.decodeToken(loginStore.token);
    const role =
      decodedToken[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ];
    await loginStore.getAuth(loginStore.token);
    if (decodedToken && role === "Admin") {
      // assuming 'role' is the field that holds the user type
      return;
    } else {
      router.push("/home");
    }
  } else {
    router.push("/login");
  }
};

const routes = [
  {
    path: "/",
    name: "inicio",
    component: inicio,
  },
  {
    path: "/login",
    name: "login",
    component: login,
  },
  {
    path: "/consultar-cupones",
    name: "consultar-cupones",
    component: consultarCupones,
  },
  {
    path: "/forgotPasword",
    name: "forgotPasword",
    component: forgotPasword,
  },
  {
    path: "/api/users/ResetPassword",
    name: "resetPassword",
    component: resetPassword,
  },
  {
    path: "/api/users/ConfirmEmail",
    name: "confirmEmail",
    component: confirmEmail,
  },

  {
    path: "/coupons/redeem",
    name: "redeemCoupon",
    component: redeemCoupon,
    beforeEnter: requiredAuth,
  },

  {
    path: "/home",
    name: "home",
    component: home,
    beforeEnter: requiredAuth,
    children: [
      {
        path: "/dashboard",
        name: "dashboard",
        component: dashboard,
        beforeEnter: requiredAuth,
      },
      {
        path: "/users",
        name: "users",
        component: user,
        beforeEnter: requiredAuthAdmin,
      },
      {
        path: "/products",
        name: "products",
        component: product,
        beforeEnter: requiredAuth,
      },
      {
        path: "/coupons",
        name: "coupons",
        component: coupon,
        beforeEnter: requiredAuth,
      },
      {
        path: "/categories",
        name: "categories",
        component: category,
        beforeEnter: requiredAuth,
      },
      {
        path: "/cities",
        name: "cities",
        component: city,
        beforeEnter: requiredAuthAdmin,
      },
      {
        path: "/contacts",
        name: "contacts",
        component: contact,
        beforeEnter: requiredAuthAdmin,
      },
      {
        path: "/companies",
        name: "companies",
        component: company,
        beforeEnter: requiredAuthAdmin,
      },
      {
        path: "/countries",
        name: "countries",
        component: country,
        beforeEnter: requiredAuthAdmin,
      },
      {
        path: "/states",
        name: "states",
        component: state,
        beforeEnter: requiredAuthAdmin,
      },
      {
        path: "/roles",
        name: "roles",
        component: role,
        beforeEnter: requiredAuthAdmin,
      },
      {
        path: "/user/updatePassword",
        name: "updatePassword",
        component: updatePassword,
        beforeEnter: requiredAuth,
      },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
