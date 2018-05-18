import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';

@Component
export default class AdminBusComponent extends Vue {

    busDefault: busVM = {
        id: "",
        licensePlate: "",
        status: 0,
        location: { latitude: 0, longitude: 0 },
    }

}