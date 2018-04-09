import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';

@Component
export default class SingleRouteComponent extends Vue {

    // Data
    route: any = null
    isRouteLoaded: boolean = false
    private routesClient = new RoutesClient()

    // Lifecycle Hooks
    created() {
        this.createTempRoute()
    }

    mounted() {
        this.getRoute()
    }


    // Methods
    getRoute(){
        let name = this.getRequestFromURL()
        this.fetchRoute(name)
            .then( res => { 
                this.route = res 
                this.route.name = this.formatRouteName(this.route.name)
                this.isRouteLoaded = true
            })
    }

    getRequestFromURL() {  return this.$route.params.route }

    formatRouteName(name: string) { return "Ruta " + name.toUpperCase().replace(/-/g," ") }


    // Async Methods
    async createTempRoute() {
        var newRoute = await this.routesClient.Create(this.getRequestFromURL())
    }

    async fetchRoute(name: string) { return await this.routesClient.Get(name) }
}