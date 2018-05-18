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

    @Watch('stopDefault')
    onStopDefaultChanged(val: string, oldVal: string) { 
        this.stop = this.stopDefault
    }

    get computedStopReady() {
        let isReady = false;
        if (this.stop != undefined) {
            isReady = this.stop.name != "" && this.stop.location.latitude != 0 && this.stop.location.longitude != 0;
        }
        return {
            'disabled': !isReady
        }
    }

    addStop() {
        if (this.stop != undefined) {
            if (this.stop.name != "" && this.stop.location.latitude != 0 && this.stop.location.longitude != 0) {
                this.stopClient.Create(this.stop)
                    .then( newStop => {
                        this.$emit('close', newStop);
                    })
            }
        }    
    }

    createStop(){
        if (this.stop != undefined) {
            if (this.stop.name != "" && this.stop.location.latitude != 0 && this.stop.location.longitude != 0) {
                this.stopClient.Create(this.stop)
                    .then( (stop) => {
                        this.$emit('close')
                        console.log("Stop Created", stop)
                        alert("Parada creada: " + stop!.name)
                        this.clearComponent()
                    })
            }
        }
    }

    saveStop() {
        if (this.stop != undefined) {
            if (this.stop.name != "" && this.stop.location.latitude != 0 && this.stop.location.longitude != 0) {
                this.stopClient.UpdateStop(this.stop)
                    .then( (stop) => {
                        if (stop != null) {
                            this.$emit('closeEdit')
                            console.log("STOP GUARDADO", stop)
                            alert("Parada guardada: " + stop.name)
                        }
                    })
            }
        }
    }


    clearComponent(){
        this.$emit('close')
    }

    closeStopComponent(){
        this.$emit('close')
    }

    closeStopEditComponent(){
        this.$emit('closeEdit')
    }
}