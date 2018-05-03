import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import RoutesClient from '../../clients/RoutesClient';
import StopsClient from '../../clients/StopsClient';
import BusesClient from '../../clients/BusesClient';

type Result = {
    name: string,
    type: number
};

@Component
export default class SearchComponent extends Vue {
    // Props 
    searchInput: string = ''
    routesClient: RoutesClient = new RoutesClient()
    stopsClient: StopsClient = new StopsClient()
    busesClient: BusesClient = new BusesClient()
    searchResults: Result[] = []
   
    @Prop() title?: string
    @Prop() placeholder?: string
    @Prop() content?: string

    @Prop() findRoutes?: boolean
    @Prop() findStops?: boolean
    @Prop() findBuses?: boolean

    @Prop() hasAuth?: boolean

    // Functions
    search(input: string){
        this.searchResults = []
        if (input.length > 1) {
            this.fetchAll(input);
        }
    }

    fetchAll(input: string){
        return new Promise<any>((resolved, rejected) =>{
            let promiseArr = new Array<Promise<any>>();
            if (this.$props.findRoutes) {
                let promise = this.routesClient.Search(input)
                    .then( results => {
                        this.addToResults(results, 1)
                    });

                promiseArr.push(promise);
            }
            if (this.$props.findStops) {
                let promise = this.stopsClient.Find(input)
                    .then( results => {
                        this.addToResults(results, 2)
                    });

                promiseArr.push(promise);
            }
            if (this.$props.findBuses) {
                let promise = this.busesClient.FindBus(input)
                    .then(results => {
                        this.addToResults(results, 3)
                    });
                
                promiseArr.push(promise);
            }

            return Promise.all(promiseArr).then(() => resolved());
        });
    }

    addToResults(results: string[] | null, type: number){
        let clean: Result[] = []
        if (results != null) {
            for (let i = 0; i < results.length; i++) {
                if (results[i] != null) {
                    let res: Result = {
                        name: results[i],
                        type: type
                    };
                    clean.push(res)
                }
            }
            this.searchResults = this.searchResults.concat(...clean)
        }
    }

    getResultType(result: number){
        let type = ""
        switch (result) {
            case 1:
                type = "Ruta";
                break;
            case 2:
                type = "Parada";
                break;
            case 3:
                type = "Camión";
                break;
            default:
                break;
        }
        return type
    }

    onResultClicked(result: any){
        this.searchResults = [];
        if (this.hasAuth) {
            this.searchInput = result.name;
            this.$emit("resultClicked", result);
        }
        else {
            if (result.type == 1) {
                this.getRoute(result.name);
            }
        }

    }

    onResultDelete(result: any) {
        this.searchResults = [];
        if (this.hasAuth) {
            this.$emit("resultDelete", result);
        }
    }

    getRoute(route: string) {
        if (!this.hasAuth) {
            var path = route.toLowerCase().replace(/ /g,"-");
            this.$router.push("route/" + path)
        }
    }
}