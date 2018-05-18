import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component({
    components: {
        MenuComponent: require('../navmenu/navmenu.vue.html'),
        AdminMenuComponent: Vue.component('admin-navmenu', require('../admin-navmenu/admin-navmenu.vue.html')),
        SearchComponent: Vue.component('search-component', require('../search/search.vue.html')),
        MapComponent: Vue.component('map-component', require('../map/map.vue.html')),
        
        RouteInfoComponent: Vue.component('route-info-component', require('../route-info/route-info.vue.html')),

        AdminDashboardComponent: Vue.component('admin-dashboard-component', require('../admin-dashboard/admin-dashboard.vue.html')),
        AdminDashboardInfoComponent: Vue.component('admin-dashboard-info-component', require('../admin-dashboard-info/admin-dashboard-info.vue.html')),

        AdminRouteComponent: Vue.component('admin-route-component', require('../admin-route/admin-route.vue.html')),
        AdminRouteInfoComponent: Vue.component('admin-route-info-component', require('../admin-route-info/admin-route-info.vue.html')),

        AdminStopComponent: Vue.component('admin-stop-component', require('../admin-stop/admin-stop.vue.html')),
        AdminStopInfoComponent: Vue.component('admin-stop-info-component', require('../admin-stop-info/admin-stop-info.vue.html')),
        
        AdminBusComponent: Vue.component('admin-bus-component', require('../admin-bus/admin-bus.vue.html')),
        AdminBusInfoComponent: Vue.component('admin-bus-info-component', require('../admin-bus-info/admin-bus-info.vue.html'))
    }
})
export default class AppComponent extends Vue {
}
