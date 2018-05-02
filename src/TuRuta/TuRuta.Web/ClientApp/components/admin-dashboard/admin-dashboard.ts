import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';
import StopsClient from '../../clients/StopsClient';
import BusesClient from '../../clients/BusesClient';

@Component
export default class AdminDashboardComponent extends Vue {

    // Data
    isRouteEditActive: boolean = false
    isStopEditActive: boolean = false
    isStopCreateActive: boolean = false
    isBusEditActive: boolean = false

    routeResult: any = null
    routesClient: RoutesClient = new RoutesClient()

    busResult: any = null
    busesClient: BusesClient = new BusesClient()

    stopResult: stopVM = {
        name: "",
        id: "",
        location: { latitude: 0, longitude: 0 }
    }
    stopsClient: StopsClient = new StopsClient()


    get computedEditActive() {
        return {
            'big': ( (!this.isRouteEditActive || !this.isStopEditActive || !this.isBusEditActive) && !this.isStopCreateActive ),
            'medium': ( (this.isRouteEditActive || this.isStopEditActive || this.isBusEditActive) && !this.isStopCreateActive ),
            'small': this.isStopCreateActive
        }
    }

    get computedStopActive(){
        return {
            'second': this.isStopEditActive, 
            'third': this.isStopCreateActive, 
            'hide-container': !this.isStopEditActive && !this.isStopCreateActive
        }
    }

    get computedStop() { return (this.isRouteEditActive || this.isBusEditActive || this.isStopEditActive || this.isStopCreateActive) }

    
    editResult(result: any){
        switch (result.type) {
            case 1:
                this.toggleRouteInfoComponent(result.name)
                break;
            
            case 2:
                this.toggleStopInfoComponent(1, result.name)
                break;
            
            case 3:
                this.toggleBusInfoComponent(result.name)
                break;
        
            default:
                break;
        }
    }

    toggleRouteInfoComponent(name: string){
        this.getRoute(name);
        this.isRouteEditActive = !this.isRouteEditActive;
    }

    toggleStopInfoComponent(blade: number, name: string){
        if (blade == 1) {
            this.getStop(name);
            this.isStopEditActive = !this.isStopEditActive;
        }
        else if (blade == 2) {
            this.stopResult = {
                name: "",
                id: "",
                location: { latitude: 0, longitude: 0 }
            }
            this.isStopCreateActive = !this.isStopCreateActive;
        }
    }

    toggleBusInfoComponent(id: string){
        this.getBus(id);
        this.isBusEditActive = !this.isBusEditActive;
    }


    getRoute(name: string) {
        return new Promise<any>((resolved, rejected) =>{
            let promise = this.routesClient.Get(name)
                .then( route => {
                    this.routeResult = route;
                });
            return promise.then(() => resolved());
        });
    }

    getBus(id: string){
        return new Promise<any>((resolved, rejected) =>{
            let promise = this.busesClient.FindBus(id)
                .then( bus => {
                    this.busResult = bus;
                });
            return promise.then(() => resolved());
        });
    }

    getStop(name: string){
        return new Promise<any>((resolved, rejected) =>{
            let promise = this.stopsClient.GetByName(name)
                .then( stop => {
                    if (stop != null) {
                        this.stopResult = stop;
                    }
                });
            return promise.then(() => resolved());
        });
    }

    receivePosition(point: point) {
        this.stopResult.location = point
    }

}