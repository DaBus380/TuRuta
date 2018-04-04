import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component({
    components: {
        MenuComponent: require('../navmenu/navmenu.vue.html'),
        SearchComponent: Vue.component('search-component', require('../search/search.vue.html')),
        MapComponent: Vue.component('map-component', require('../map/map.vue.html')),
        RouteInfoComponent: Vue.component('route-info-component', require('../route-info/route-info.vue.html'))
    }
})
export default class AppComponent extends Vue {
}
