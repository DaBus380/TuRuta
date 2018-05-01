import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';

@Component
export default class AdminStopInfoComponent extends Vue {

    @Prop() isOptionalComponent?: boolean;
    @Prop() isOptionalComponentEdit?: boolean;
    @Prop() stopLocation?: point;
    @Prop() stop?: stopVM;
    
    closeStopComponent(){
        this.$emit('close')
    }

    closeStopEditComponent(){
        this.$emit('closeEdit')
    }
}