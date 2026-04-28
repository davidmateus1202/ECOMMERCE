import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import router from './routes'
import Vue3Toastify, { type ToastContainerOptions } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';

const app = createApp(App)

app.use(router)
    .use(Vue3Toastify, {
        autoClose: 3000,
        position: 'top-right',
        pauseOnHover: true,
        draggable: true,
        hideProgressBar: false,
    } as ToastContainerOptions)
    .mount('#app')
