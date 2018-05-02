import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';

@Component
export default class AdminBusInfoComponent extends Vue {

    @Prop() isOptionalComponent?: boolean
    @Prop() bus?: string[]

    closeBusComponent() {
        this.$emit('close')
    }
}