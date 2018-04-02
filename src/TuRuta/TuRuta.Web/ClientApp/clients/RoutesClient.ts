export default class RoutesClient {
    async AddStops(routeId: string, stopIds: string[]) {
        var response = await fetch('api/routes/addstops/' + routeId, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(stopIds)
        });
        if (response.ok) {
            var json = await response.json();
            return json as routeVM;
        }

        return null;
    }

    async AddStop(routeId:string, stopId: string) {
        var response = await fetch('api/routes/addstop/'+ routeId + '/' + stopId);
        if (response.ok) {
            var json = await response.json();
            return json as routeVM;
        }

        return null;
    }

    async Search(hint: string) {
        var response = await fetch('api/routes/find/' +  hint);
        if (response.ok) {
            var json = await response.json()
            return json as string[];
        }

        return null;
    }

    async Names() {
        var response = await fetch("api/routes/names");
        if (response.ok) {
            var json = await response.json();
            return json as string[];
        }

        return null;
    }

    async Create(name: string) {
        var response = await fetch('api/routes/create/' + name);
        if (response.ok) {
            var json = await response.json();
            return json as routeVM;
        }

        return null;
    }

    async Get(name: string) {
        var response = await fetch('api/routes/' + name);
        if (response.ok) {
            var json = await response.json();
            return json as routeVM;
        }

        return null;
    }
}