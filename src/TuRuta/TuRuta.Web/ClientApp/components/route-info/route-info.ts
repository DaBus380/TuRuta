import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import PubNub from 'pubnub';

@Component
export default class RouteInfoComponent extends Vue {
    
    // Props
    @Prop() name: string;
    @Prop() stops: stopVM[];
    

    mounted() {
    }
}