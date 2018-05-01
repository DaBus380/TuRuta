import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';

@Component
export default class AdminRouteComponent extends Vue {

    isStopPanelActive: boolean = false;

    // Methods
    toggleStopInfoComponent () {
        this.isStopPanelActive = !this.isStopPanelActive
    }

    receivePosition(point:point) {
        console.log(point);
    }
}