export default class BusesClient {
    async GetNoPlates() {
        var response = await fetch("api/buses/noplates");
        if (response.ok) {
            var json = await response.json();
            return json as string[];
        } 

        return Array<string>();
    }

    async GetNoRoute() {
        var response = await fetch("api/config/noconfig");
        if (response.ok) {
            var json = await response.json();
            return json as string[];
        }

        return Array<string>();
    }

    async FindBus(plates: string) {
        var response = await fetch('api/buses/find/' + plates);
        if (response.ok) {
            var json = await response.json();
            return json as string[];
        }

        return Array<string>();
    }

    async SetRoute(busId: string, routeId: string) {
        var response = await fetch('api/buses/setroute/' + busId + '/' + routeId);
        return response.ok;
    }

    async SetPlates(busId: string, plates: string) {
        var response = await fetch('api/buses/setplates/' + busId + '/' + plates);
        return response.ok;
    }

    async GetBusInfo(busId: string) {
        var response = await fetch('api/buses/' + busId);
        if (response.ok) {
            var json = await response.json();
            return json as busInfoVM;
        }

        return null;
    }
}