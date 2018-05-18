import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';
import Authentication from '../../authentication/Authentication';

@Component
export default class Test extends Vue {
    authentication = new Authentication();
    headers = new Headers();

    beforeMount() {
        this.authentication.initialize();
        if(this.authentication.isAuthenticated()){
            this.authentication.acquireToken().then(token => {
                this.headers.set("Authorization", "Bearer "+token);
            });
        }
    }

    clicked(){
        fetch("/api/SampleData/WeatherForecasts", {
            headers: this.headers
        })
        .then(response => {
            console.log(response.status);
        });
    }
}