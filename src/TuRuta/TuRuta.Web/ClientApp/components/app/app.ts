import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component({
    components: {
        MenuComponent: require('../navmenu/navmenu.vue.html'),
        SearchComponent: Vue.component('search-component', require('../search/search.vue.html'))
    }
})
export default class AppComponent extends Vue {
}
