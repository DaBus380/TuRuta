import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';

@Component
export default class Test extends Vue {
    mounted() {
        var routesClient = new RoutesClient();
        routesClient.Search("380")
            .then(result => console.log(result));
    }
}