import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';

@Component
export default class AdminRouteInfoComponent extends Vue {

    @Prop() isButtonActive?: boolean
    @Prop() isOptionalComponent?: boolean
    @Prop() routeDefault?: routeVM
   
    routesClient = new RoutesClient();
    route?: routeVM = this.routeDefault;

    get computedRouteReady() {
        let isReady = false;
        if (this.route != undefined) {
            isReady = this.route.name != "" && this.route.stops.length > 0;
        }
        console.log("is Ready?", isReady, this.route)
        return {
            'disabled': !isReady
        }
    }

    createRoute() {
        if (this.route != undefined) {
            this.routesClient.Create(this.route.name)
            .then(newRoute => {
                if(newRoute != null){
                    this.routesClient.AddStops(newRoute.id, this.route!.stops.map(r => r.id));
                }
            });
            console.log("Created route", this.route)
            alert("Ruta creada: " + this.route.name)
            this.clearComponent()
        }
    }

    saveRoute() {
        if (this.route != undefined) {
            console.log("Saved route", this.route)
            alert("Ruta guardada: " + this.route.name)
            this.clearComponent()
            this.closeRouteComponent()
            // this.routesClient.Create(this.route.name)
            // this.routesClient.SetName(this.route.name)
            /* .then(newRoute => {
                if(newRoute != null){
                    this.routesClient.AddStops(newRoute.id, this.route!.stops.map(r => r.id));
                }
            });*/
        }
    }

    clearComponent(){
        let newRoute = {
            id: "",
            name: "",
            buses: [],
            stops: [],
            incidents: [],
        }
        this.route = newRoute
    }

    openStopComponent() {
        this.$emit('open')
    }

    closeRouteComponent() {
        this.$emit('close')
    }
}