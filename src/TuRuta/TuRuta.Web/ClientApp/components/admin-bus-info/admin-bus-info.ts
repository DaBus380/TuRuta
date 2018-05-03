import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';
import BusesClient from '../../clients/BusesClient';

@Component
export default class AdminBusInfoComponent extends Vue {

    @Prop() isOptionalComponent?: boolean
    @Prop() busDefault?: busVM

    bus?: busVM = this.busDefault
    busesNoPlates: string[] = []
    busRoute: string = ""

    busClient: BusesClient = new BusesClient();
    routesClient: RoutesClient = new RoutesClient();



    get computedBusReady() {
        let isReady = false;
        if (this.bus != undefined) {
            isReady = this.bus.id != "" && this.busRoute != "" && this.checkLicenseFormat(this.bus.licensePlate);
        }
        console.log("is Ready?", isReady, this.bus)
        return {
            'disabled': !isReady
        }
    }

    mounted() {
        this.getBusesNotConfigured()
    }

    selectBus(id: string){
        if (this.bus != undefined) {
            this.bus.id = id
        }
    }

    addRouteResult(routeResult: any){
        if (routeResult != undefined) {
            console.log("Route: ", routeResult.name)
            this.busRoute = routeResult.name
        }
    }

    getBusesNotConfigured() {
        this.busClient.GetNoPlates()
            .then( (buses) => {
                console.log("BUSES NO PLATES", buses)
                this.busesNoPlates = buses
            })
    }


    addBusInfo() {
        if (this.bus != undefined) {
            this.busClient.SetPlates(this.bus.id, this.bus.licensePlate)
            .then( () => {
                console.log("Added plates to bus", this.bus)
            })

            var route = this.routesClient.Get(this.busRoute)
            .then(route => {
                console.log(route!.id);
                this.busClient.SetRoute(this.bus!.id, route!.id);
            })
            .then( () => {
                this.clearComponent()
                this.getBusesNotConfigured()
            });
        }
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