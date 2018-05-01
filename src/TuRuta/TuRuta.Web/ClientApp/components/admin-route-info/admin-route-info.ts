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
        
    }

    setRouteStops(name: string) {
        if (this.route != undefined) {
            console.log("NAME: ", name)
            this.route.name = name
        }
    }

    openStopComponent() {
        this.$emit('open')
    }

    closeRouteComponent() {
        this.$emit('close')

        // this.routesClient.
    }
}