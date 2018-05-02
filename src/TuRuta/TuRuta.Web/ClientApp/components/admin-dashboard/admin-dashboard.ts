import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';
import StopsClient from '../../clients/StopsClient';
import BusesClient from '../../clients/BusesClient';
import Authentication from '../../authentication/Authentication';

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

    stopResult: any = null
    stopsClient: StopsClient = new StopsClient()

    bladeCounter: number = 1


    get computedEditActive() {
        return {
            'big': this.bladeCounter == 1,
            'medium': this.bladeCounter == 2,
            'small': this.bladeCounter == 3
        }
    }

    get computedStopActive(){
        return {
            'second': this.bladeCounter == 2, 
            'third': this.bladeCounter == 3, 
            'hide-container':  this.bladeCounter < 2
        }
    }

    get computedStop() { return (this.isRouteEditActive || this.isBusEditActive || this.isStopEditActive || this.isStopCreateActive) }

    beforeMount() {
        //let user = authentication.initialize();
        //console.log(user!.userName);
    }

    mounted() {
        let authentication = new Authentication();
        let user = authentication.initialize();
        if (authentication.isAuthenticated()) {
            console.log("logedin")
        }
    }
    
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
        this.getRoute(name).then(() => { 
            this.isRouteEditActive = !this.isRouteEditActive; 
            if (this.isRouteEditActive) {
                this.bladeCounter++;
            }
            else {
                this.bladeCounter--;
            }
        });
    }

    toggleStopInfoComponent(blade: number, name: string){
        if (blade == 1) {
            if (this.isStopEditActive) {
                this.stopResult = null
                this.isStopEditActive = false;
                this.bladeCounter--;
            }
            else{
                this.getStop(name).then(() => {
                    this.isStopEditActive = true;
                    this.bladeCounter++;
                });
            }
        }
        else {
            this.stopResult = {
                name: "",
                id: "",
                location: { latitude: 0, longitude: 0 }
            }
            if (this.isStopCreateActive) {
                this.isStopCreateActive = false;
                this.bladeCounter--;
            }
            else {
                this.isStopCreateActive = true;
                this.bladeCounter++;
            }
        }
    }

    toggleBusInfoComponent(id: string){
        this.getBus(id).then(() => {
            this.isBusEditActive = !this.isBusEditActive;
            if (this.isBusEditActive) {
                this.bladeCounter++;
            }
            else {
                this.bladeCounter--;
            }
        });
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