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
        console.log(isReady, this.stop)
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
                        console.log("Stop Created", stop);
                        alert("Parada creada: " + newStop!.name)
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
                // AQUI VA EL UPDATE
                this.$emit('closeEdit')
                alert("Parada guardada: " + this.stop.name)
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