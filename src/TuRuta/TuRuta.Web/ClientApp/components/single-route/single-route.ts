import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import PubNub from 'pubnub';
import RoutesClient from '../../clients/RoutesClient';

interface Message {
    latitude: number;
    longitude: number;
    busId: string;
    nextStop: Stop;
}

interface Route {
    routeId?: string;
    name: string;
    stops?: Stop[];
    buses?: Bus[];
}

interface Bus {
    busId: string;
    licensePlate: string;
    status: number;
}

interface Stop {
    stopId: string;
    name: string;
    latitude: number;
    longitude: number;
}

@Component({})
export default class SingleRouteComponent extends Vue {

    // Public props
    @Prop()
    route?: Route;
    messages?: Message[];

    // Private props
    private listener = {
        status: function (statusEvent: any) {
            if (statusEvent.category === "PNConnectedCategory") {
                console.log("Conectado");
            }
        },
        message: this.messageReceived
    }

    // Temporary functionality
    created() {
        this.createTempRoute();
    }

    // Lifecycle Hooks
    mounted() {
        // Sets route
        this.getRoute()
            .then(res => {
                    this.$props.route = res as Route
                    console.log("Mounted: ", this.$props.route)});   

        // Gets config of pubnub
        fetch('/api/config/pubnub')
            .then(response => response.json() as Promise<any>)
            .then(data => {
                var pubnub = new PubNub({
                    subscribeKey: data.subKey,
                    ssl: true
                })
                pubnub.addListener(this.listener)
                pubnub.subscribe({
                    channels: ['client'],
                });
            });
    }

    // Functions
    messageReceived(message: any){
        console.log(message);
        // this.messages.push(message);
    }

    async createTempRoute() {
        // Creates temporary route
        var routesClient = new RoutesClient();
        var newRoute = await routesClient.Create(this.getRequestFromURL());
        console.log("Created: ", newRoute);
    }

    async getRoute() {
        // Gets route from 'server'
        var routesClient = new RoutesClient();
        var result = await routesClient.Search(this.getRequestFromURL());
        var route: Route = {
            name: this.formatRouteName(result![0])
        };
        return route;
    }

    getRequestFromURL() {  return this.$route.params.route }

    formatRouteName(name: string) {
        var formated = "Ruta " + name.toUpperCase().replace(/-/g," ")
        console.log(formated)
        return formated
    }
}





/*var pubnub = new PubNub({
    subscribeKey: "sub-c-5679dfc0-1135-11e8-bb6e-d6d19ee12a32",
    ssl: true
})
pubnub.addListener(this.listener)
pubnub.subscribe({
    channels: ['client'],
});*/

/*
    public messages: Message[] = [
        {
            latitude: 0,
            longitude: 0,
            busId: "2001",
            nextStop: {
                stopId: "3002",
                name: "Plaza Galerias",
                latitude: 2,
                longitude: -2
            }
        }
    ];
    public route: Route = {
        routeId: "1001",
        name: this.formatRouteName(),
        stops: [
            {
                stopId: "3001",
                name: "Tec de Monterrey",
                latitude: 1,
                longitude: -1
            },
            {
                stopId: "3002",
                name: "Plaza Galerías",
                latitude: 2,
                longitude: -2
            },
            {
                stopId: "3003",
                name: "Minerva",
                latitude: 3,
                longitude: -3
            }],
        buses: [
            {
                busId: "2001",
                licensePlate: "JHJ-1626",
                status: 1
            }
        ]
    }; 
    
     // Find current stop's index
    findCurrentIndex(route: Route, currentStop: Stop){
        console.log(route.stops[0].name);
        for (let index = 0; index < route.stops.length; index++) {
            if (route.stops[index].name == currentStop.name) {
                return index;
            };   
        }
        return -1;
    }
    */