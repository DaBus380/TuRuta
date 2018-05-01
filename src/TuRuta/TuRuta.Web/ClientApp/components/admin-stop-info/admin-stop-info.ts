import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';
import StopsClient from '../../clients/StopsClient';

@Component
export default class AdminStopInfoComponent extends Vue {

    @Prop() isOptionalComponent?: boolean;
    @Prop() isOptionalComponentEdit?: boolean;
    @Prop() stopDefault?: stopVM;
    
    stop?: stopVM = this.stopDefault;
    stopClient: StopsClient = new StopsClient();


    get computedStopReady() {
        let isReady = false;
        if (this.stop != undefined) {
            isReady = this.stop.name != "" && this.stop.location.latitude != 0 && this.stop.location.longitude != 0;
        }
        return {
            'disabled': !isReady
        }
    }

    createStop(){
        if (this.stop != undefined) {
            if (this.stop.name != "" && this.stop.location.latitude != 0 && this.stop.location.longitude != 0) {
                console.log("Created Stop", this.stop);
                this.stopClient.Create(this.stop);
            }
        }
    }

    closeStopComponent(){
        this.$emit('close')
    }

    closeStopEditComponent(){
        this.$emit('closeEdit')
    }
}