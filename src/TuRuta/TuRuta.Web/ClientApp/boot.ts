import './css/site.css';
import 'bootstrap';
import Vue from 'vue';
import {ComponentOptions} from 'vue';
import VueRouter from 'vue-router';
Vue.use(VueRouter);

const routes = [
    { path: '/', component: require('./components/home/home.vue.html') },
    { path: '/routes', component: require('./components/all-routes/all-routes.vue.html') },
    { path: '/route/:route', component: require('./components/single-route/single-route.vue.html') },
    { path: '/admin/dashboard', name: 'admin', component: require('./components/admin-dashboard/admin-dashboard.vue.html') },
    { path: '/admin/route/create', name: 'admin', component: require('./components/admin-route/admin-route.vue.html') },
    { path: '/admin/stop/create', name: 'admin', component: require('./components/admin-stop/admin-stop.vue.html') },
    { path: '/admin/bus/create', name: 'admin', component: require('./components/admin-bus/admin-bus.vue.html') },
    { path: "/test", component: require("./components/test/test.vue.html")}
];

new Vue({
    el: '#app-root',
    router: new VueRouter({ mode: 'history', routes: routes }),
    render: h => h(require('./components/app/app.vue.html'))
});