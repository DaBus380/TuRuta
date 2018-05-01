import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';

@Component
export default class AdminRouteInfoComponent extends Vue {

    @Prop() isButtonActive?: boolean
    @Prop() isOptionalComponent?: boolean
    @Prop() route?: routeVM

    openStopComponent() {
        this.$emit('open')
    }

    closeRouteComponent() {
        this.$emit('close')
    }
}