import Vue from 'vue'
import { Component, Prop } from 'vue-property-decorator'
import { } from "@types/googlemaps"

@Component
export default class MapComponent extends Vue {

    // Data
    map: any = null
    city: string = ''
    markers: google.maps.Marker[] = []

    // Properties
    @Prop() stops?: stopVM[]

    // Lifecycle
    mounted() {
        this.initMap()
        if(this.stops != undefined && this.stops.length != 0) {
            this.iniMarkers()
        }
    }

    // Methods
    initMap(){
        let element = document.getElementById("mapDiv")
        let latLon = new google.maps.LatLng(20.6736, -103.344)
        let options = {
            zoom: 14,
            center: latLon,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        }
        this.map = new google.maps.Map(element, options)
    }

    iniMarkers() {
        var newMarkers = new Array<google.maps.Marker>()
        if (this.stops != undefined && this.stops.length != 0) {
            this.stops.forEach(stop => {
                let location = { 
                    lat: stop.location.latitude, 
                    lng: stop.location.longitude
                }
                let marker = new google.maps.Marker({
                    position: location,
                    map: this.map,
                    title: stop.name
                })
                newMarkers.push(marker)
            })
            this.markers = newMarkers
            var centerPosition = Math.floor( this.markers.length / 2 )
            this.map.center = this.markers[centerPosition].getPosition()
        }
    }
}