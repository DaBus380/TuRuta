import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';


@Component
export default class AdminDashboardInfoComponent extends Vue {

    onResultEmitted(result: any){
        this.$emit("resultEmitted", result);
    }

    onDeleteEmitted(result: any){
        this.$emit("deleteEmitted", result);
    }
}