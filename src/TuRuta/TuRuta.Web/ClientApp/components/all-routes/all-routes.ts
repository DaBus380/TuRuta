import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import StopsClient from '../../clients/StopsClient';

@Component
export default class SingleRouteComponent extends Vue {

    // Data
    stopClient: StopsClient = new StopsClient();
    stopResult: any = null

    mounted() {
        this.getStops()
    }

    get computedStops() {
        return this.stopResult != null
    }

    getStops(){
        this.stopClient.Get()
            .then( stops => {
                console.log("Something here", stops)
                this.stopResult = stops
            });
    }

}