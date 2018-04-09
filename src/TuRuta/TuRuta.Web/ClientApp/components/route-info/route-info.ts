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
    @Prop() messages: Message[];
    @Prop() name: string;
    @Prop() stops: stopVM[];
    

    mounted() {
    }
}