import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';
import Authentication from '../../authentication/Authentication';

@Component
export default class Test extends Vue {
    authentication = new Authentication();

    beforeMount(){
        //this.authentication.signOut();
        //this.authentication.initialize();
    }

    async mounted(){
        /*
        var token = await this.authentication.acquireToken();
        console.log(token);

        console.log(this.authentication.isAuthenticated());
        */
    }
}