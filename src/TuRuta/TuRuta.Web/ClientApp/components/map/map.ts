import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { } from "@types/googlemaps";

@Component
export default class MapComponent extends Vue {

    map?: google.maps.Map;
    mounted() {
        let element = document.getElementById("mapDiv");
        let latLon = new google.maps.LatLng(20.6736, -103.344);
        let options = {
            zoom: 13,
            center: latLon,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        this.map = new google.maps.Map(element, options);
    }
}