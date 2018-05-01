import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';
import StopsClient from '../../clients/StopsClient';
import BusesClient from '../../clients/BusesClient';

type Results = {
    name: string,
    type: string
};

@Component
export default class SearchComponent extends Vue {
    // Props 
    searchInput: string = ''
    routesClient: RoutesClient = new RoutesClient()
    stopsClient: StopsClient = new StopsClient()
    busesClient: BusesClient = new BusesClient()
    searchResults: Results[] = []
   
    @Prop() title?: string
    @Prop() placeholder?: string
    @Prop() content?: string

    @Prop() findRoutes?: boolean
    @Prop() findStops?: boolean
    @Prop() findBuses?: boolean

    // Functions
    search(input: string){
        this.searchResults = []
        if (input.length > 1) {
            this.fetchAll(input)
                .then(() => console.log(this.searchResults));
        }
    }

    fetchAll(input: string){
        return new Promise<any>((resolved, rejected) =>{
            let promiseArr = new Array<Promise<any>>();
            if (this.$props.findRoutes) {
                let promise = this.routesClient.Search(input)
                    .then( results => {
                        this.addToResults(results, "Ruta")
                    });

                promiseArr.push(promise);
            }
            if (this.$props.findStops) {
                let promise = this.stopsClient.Find(input)
                    .then( results => {
                        this.addToResults(results, "Parada")
                    });

                promiseArr.push(promise);
            }
            if (this.$props.findBuses) {
                let promise = this.busesClient.FindBus(input)
                    .then(results => {
                        this.addToResults(results, "Camión")
                    });
                
                promiseArr.push(promise);
            }

            return Promise.all(promiseArr).then(() => resolved());
        });
    }

    addToResults(results: string[] | null, type: string){
        let clean: Results[] = []
        if (results != null) {
            for (let i = 0; i < results.length; i++) {
                if (results[i] != null) {
                    let res: Results = {
                        name: results[i],
                        type: type
                    };
                    clean.push(res)
                }
            }
            this.searchResults = this.searchResults.concat(...clean)
        }
    }

    getRoute(input: string) {
        var path = input.toLowerCase().replace(/ /g,"-");
        this.$router.push("route/" + path)
    }
}