import { Component } from "vue-property-decorator";
import Vue from "vue";
import Authentication from "../../authentication/Authentication";

@Component
export default class AdminNavMenu extends Vue {
    signout() {
        let auth = new Authentication();
        auth.signOut();
        console.log(auth.isAuthenticated());
    }
}