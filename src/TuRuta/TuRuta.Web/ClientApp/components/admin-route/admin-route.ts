import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';

@Component
export default class AdminRouteComponent extends Vue {

    marker: point = { latitude: 0, longitude: 0 };
    isStopPanelActive: boolean = false;

    // Methods
    toggleStopInfoComponent () {
        this.isStopPanelActive = !this.isStopPanelActive
    }

    receivePosition(point: point) {
        // console.log(point);
        this.marker = point
    }
}