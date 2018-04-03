import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { } from "@types/googlemaps";

@Component
export default class TestMaps extends Vue {

    map: google.maps.Map;
    mounted() {
        let element = document.getElementById("mapDiv");
        let latLon = new google.maps.LatLng(20.6736, -103.344);
        let options = {
            zoom: 8,
            center: latLon,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        this.map = new google.maps.Map(element, options);
    }
}