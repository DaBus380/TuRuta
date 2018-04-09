import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import PubNub from 'pubnub';

interface Message {
    latitude: number;
    longitude: number;
    busId: string;
    nextStop: stopVM;
}

@Component
export default class RouteInfoComponent extends Vue {

    // Props
    @Prop() messages!: Message[]
    @Prop() name!: string
    @Prop() stops!: stopVM[]
    
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
        this.fetchPubNub()
    }

    // Methods
    messageReceived(message: any){
        console.log(message);
        // this.messages.push(message);
    }

    fetchPubNub(){
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
}