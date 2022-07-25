import { createApp } from "vue";
import { createPinia } from "pinia";

import App from "./App.vue";
import router from "./router";

import "jquery";
// import "popper.js";
// import "@popperjs/core"
import "bootstrap"
import "bootstrap/dist/css/bootstrap.min.css";
import "@/assets/bootswatch.min.css"

// import "font-awesome/css/font-awesome.min.css";

import "@/assets/main.css"

// const app = createApp(App);

const pinia = createPinia()
const app = createApp(App)

app.use(pinia)
app.use(router)

app.mount("#app");
