import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import { } from "@types/googlemaps";

@Component
export default class MapComponent extends Vue {

    @Prop()
    map?: google.maps.Map;
    markers?: google.maps.Marker[];
    stops?: stopVM[];

    mounted() {
        let element = document.getElementById("mapDiv");
        let latLon = new google.maps.LatLng(20.6736, -103.344);
        let options = {
            zoom: 13,
            center: latLon,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        this.map = new google.maps.Map(element, options);

        let myLatLng = {lat: 20.678819, lng: -103.394160};

        this.markers = this.mapStopsToMarkers();   
    }

    mapStopsToMarkers() {
        var tempMarkers = new Array<google.maps.Marker>();
        if (this.stops != undefined) {
            this.stops.forEach(stop => {
                var location = { lat: stop.location.latitude, lng: stop.location.longitude }
                var marker = new google.maps.Marker({
                    position: location,
                    map: this.map,
                    title: stop.name,
                });
                tempMarkers.push()
            });
        }
        return tempMarkers
    }

    /*var marker = new google.maps.Marker({
            position: myLatLng,
            map: this.map,
            title: 'Hello World!'
          });*/
}