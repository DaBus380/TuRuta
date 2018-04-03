import Vue, { VNode } from 'vue';
import { Component, Prop } from 'vue-property-decorator';

@Component
export default class SearchComponent extends Vue {
    // Props
    
    search_input: string = '';
   
    @Prop()
    title?: string;
    placeholder?: string;
    content?: string;
    
    // Functions
    search(input: string) {
        var path = input.toLowerCase().replace(/ /g,"-");
        this.$router.push("ruta/" + path)
    }
}