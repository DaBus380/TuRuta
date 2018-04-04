import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';

@Component
export default class SingleRouteComponent extends Vue {

    @Prop()
    route?: routeVM;

    // Private props
    private routesClient = new RoutesClient();

    // Lifecycle Hooks
    created() {
        // Temporary functionality
        this.createTempRoute();
    }

    mounted() {
        this.getRoute()
            .then( res => { 
                this.$props.route = res 
                this.$props.route.name = this.formatRouteName(this.$props.route.name)
                console.log("Prop: ", this.$props.route)
            });
    }

    getRequestFromURL() {  return this.$route.params.route }

    formatRouteName(name: string) {
        var formated = "Ruta " + name.toUpperCase().replace(/-/g," ")
        return formated
    }

    // Async Methods
    async createTempRoute() {
        var newRoute = await this.routesClient.Create(this.getRequestFromURL());
    }

    async getRoute() { return await this.routesClient.Get(this.getRequestFromURL()); }
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