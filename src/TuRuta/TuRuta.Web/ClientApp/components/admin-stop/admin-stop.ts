import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';

@Component
export default class AdminStopComponent extends Vue {

    marker: point = { latitude: 0, longitude: 0 };
    emptyStop: stopVM = {
        name: "",
        id: "",
        location: this.marker
    }

    receivePosition(point: point) {
        // console.log(point);
        this.marker = point
    }

}