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

    createRoute(){
        if (this.route != undefined) {
            console.log("Route: ", this.route)
            this.routesClient.Create(this.route.name)
            .then(newRoute => {
                if(newRoute != null){
                    this.routesClient.AddStops(newRoute.id, this.route!.stops.map(r => r.id));
                }
            });
        }
    }

    openStopComponent() {
        this.$emit('open')
    }

    closeRouteComponent() {
        this.$emit('close')
    }
}