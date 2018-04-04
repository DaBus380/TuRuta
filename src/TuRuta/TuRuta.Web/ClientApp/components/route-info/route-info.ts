import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import PubNub from 'pubnub';

interface Message {
    latitude: number;
    longitude: number;
    busId: string;
    nextStop: stopVM;
}

@Component
export default class RouteInfoComponent extends Vue {

    // Public props
    @Prop()
    messages?: Message[];
    route: routeVM = { name: '', id: '', stops: [], buses: [], incidents: [] };

    // Private props
    private listener = {
        status: function (statusEvent: any) {
            if (statusEvent.category === "PNConnectedCategory") {
                console.log("Conectado");
            }
        },
        message: this.messageReceived
    }

    mounted() {
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

    // Methods
    messageReceived(message: any){
        console.log(message);
        // this.messages.push(message);
    }
}