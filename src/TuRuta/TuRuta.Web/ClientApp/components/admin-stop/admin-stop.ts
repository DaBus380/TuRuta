import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';

@Component
export default class AdminStopComponent extends Vue {

    stopDefault: stopVM = {
        name: "",
        id: "",
        location: { latitude: 0, longitude: 0 }
    }

    resetStop() {
        let emptyStop = {
            name: "",
            id: "",
            location: { latitude: 0, longitude: 0 }
        }
        this.stopDefault = emptyStop
    }

    receivePosition(point: point) {
        this.stopDefault.location = point
    }
}