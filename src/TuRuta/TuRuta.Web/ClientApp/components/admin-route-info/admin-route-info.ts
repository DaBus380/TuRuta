import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';
import StopsClient from '../../clients/StopsClient';

@Component
export default class AdminRouteInfoComponent extends Vue {

    @Prop() isButtonActive?: boolean
    @Prop() isOptionalComponent?: boolean
    @Prop() routeDefault?: routeVM
   
    routesClient = new RoutesClient();
    route?: routeVM = this.routeDefault;

    stopsClient: StopsClient = new StopsClient();
    stopResult: any = null

    get computedRouteReady() {
        let isReady = false;
        if (this.route != undefined) {
            isReady = this.route.name != "" && this.route.stops.length > 0;
        }
        return {
            'disabled': !isReady
        }
    }

    createRoute() {
        if (this.route != undefined) {
            this.routesClient.Create(this.route.name)
                .then(newRoute => {
                    if(newRoute != null && this.route != null){
                        console.log(this.route)
                        this.routesClient.AddStops(newRoute.id, this.route!.stops.map(r => r.id));
                        alert("Ruta creada: " + this.route.name)
                        this.clearComponent()
                    }
                })
        }
    }

    saveRoute() {
        if (this.route != undefined) {
            alert("Ruta guardada: " + this.route.name)
            this.clearComponent()
            this.closeRouteComponent()
        }
    }

    // NOT WORKING
    addStopResult(stopResult: any){
        if (stopResult != undefined) {
            this.stopsClient.GetByName(stopResult.name)
                .then( newStop => {
                    if(newStop != null && this.route != undefined){
                        this.route.stops.push(newStop)
                    }
                });
        }
    }

    deleteStop(index: number){
        if (this.route != undefined) {
            if (index > -1) {
                this.route.stops.splice(index, 1);
            }
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