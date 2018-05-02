import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';
import BusesClient from '../../clients/BusesClient';

@Component
export default class AdminBusInfoComponent extends Vue {

    @Prop() isOptionalComponent?: boolean
    @Prop() busDefault?: busVM

    bus?: busVM = this.busDefault

    busClient: BusesClient = new BusesClient()

    get computedBusReady() {
        let isReady = false;
        if (this.bus != undefined) {
            isReady = this.checkLicenseFormat(this.bus.licensePlate);
        }
        console.log("is Ready?", isReady, this.bus)
        return {
            'disabled': !isReady
        }
    }

    createBus() {
        if (this.bus != undefined) {
            // this.busClient.SetPlates(this.bus)
            console.log("Created bus", this.bus)
            alert("Camion creado con placas: " + this.bus.licensePlate)
            this.clearComponent()
        }
    }

    updated() {
        // console.log(this.bus!.licensePlate)
    }

    checkLicenseFormat(plate: string) {
        var format = /^[0-9]{3}-[0-9]{3}-[A-Z]{1}$/i;
        if (plate != undefined) {
            if (plate.length != 9 && !format.test(plate)) {
                console.log("INCORRECT FORMAT")
                return false
            }
        }
        console.log("CORRECT FORMAT")
        return true
      }

    clearComponent(){
        let newBus: busVM = {
            id: "",
            licensePlate: "",
            status: 0,
            location: { latitude: 0, longitude: 0}
        }
        this.bus = newBus
    }

    closeBusComponent() {
        this.$emit('close')
    }
}