import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import PubNub from 'pubnub';

interface Message {
    latitude: number;
    longitude: number;
    busId: string;
    nextStop: Stop;
}

interface Route {
    routeId: string;
    name: string;
    stops: Stop[];
    buses: Bus[];
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

@Component
export default class SingleRouteComponent extends Vue {
    // Public props

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
    public lastStop: number = this.route.stops.length-1;
    public currentStop: Stop = this.messages[this.messages.length-1].nextStop;
    public currentStopIndex: number = this.findCurrentIndex(this.route, this.currentStop);

    // Private props
    private listener = {
        status: function (statusEvent: any) {
            if (statusEvent.category === "PNConnectedCategory") {
                console.log("Conectado");
            }
        },
        message: this.messageReceived
    }

    // Lifecycle Hooks
    mounted() {        
        // Gets config of pubnub
        fetch('/api/config/pubnub')
            .then(response => response.json() as Promise<any>)
            .then(data => {
                var pubnub = new PubNub({
                    subscribeKey: data.SubKey,
                    ssl: true
                })
                this.messages
                pubnub.addListener(this.listener)
                pubnub.subscribe({
                    channels: ['client'],
                });
            });
    }

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

    // Functions
    messageReceived(message: any){
        console.log(message);
        this.messages.push(message);
    }

    formatRouteName() {
        var name = this.$route.params.route
        var formated = "Ruta " + name.toUpperCase().replace(/-/g," ")
        return formated
    }

    get currentCoordinates(){
        return '(' + this.currentStop.latitude + ', ' + this.currentStop.longitude + ')';
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