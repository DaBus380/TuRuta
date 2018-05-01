import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';

@Component
export default class AdminRouteComponent extends Vue {

    isStopPanelActive: boolean = false;
    routeDefault: routeVM = {
        id: "",
        name: "",
        buses: [],
        stops: [],
        incidents: [],
    }
    stopDefault: stopVM = {
        name: "",
        id: "",
        location: { latitude: 0, longitude: 0 }
    }

    // Methods
    toggleStopInfoComponent (result: any) {
        
        this.isStopPanelActive = !this.isStopPanelActive

        if (result != undefined) {
            this.routeDefault.stops.push(result)
            let emptyStop = {
                name: "",
                id: "",
                location: { latitude: 0, longitude: 0 }
            }
            this.stopDefault = emptyStop
        }
    }

    cleanStopComponent (){
        this.stopDefault = {
            name: "",
            id: "",
            location: { latitude: 0, longitude: 0 }
        }
    }

    receivePosition(point: point) {
        this.stopDefault.location = point
    }
}