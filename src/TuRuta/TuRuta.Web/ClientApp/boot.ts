import './css/site.css';
import 'bootstrap';
import Vue from 'vue';
import {ComponentOptions} from 'vue';
import VueRouter from 'vue-router';
Vue.use(VueRouter);

const routes = [
    { path: '/', component: require('./components/home/home.vue.html') },
    { path: '/rutas', component: require('./components/all-routes/all-routes.vue.html') },
    { path: '/ruta/:route', component: require('./components/single-route/single-route.vue.html') },
    { path: "/maptest", component: require("./components/testmaps/testmaps.vue.html") }
];

new Vue({
    el: '#app-root',
    router: new VueRouter({ mode: 'history', routes: routes }),
    render: h => h(require('./components/app/app.vue.html'))
});