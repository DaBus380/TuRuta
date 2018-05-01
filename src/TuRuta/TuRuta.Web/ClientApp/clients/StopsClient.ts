export default class StopsClient {
    async GetRoutes(id: string) {
        var response = await fetch('api/stops/getroutes/' + id);
        if (response.ok) {
            var json = await response.json();
            return json as routeVM[];
        }

        return null;
    }

    async Get() {
        var response = await fetch("api/stops");
        if (response.ok) {
            var json = await response.json();
            return json as stopVM[];
        }

        return null;
    }

    async Create(stop: stopVM) {
        var newStop: stopVM = {
            id: "00000000-0000-0000-0000-000000000000",
            name: stop.name,
            location: stop.location
        };
        console.log(JSON.stringify(newStop));

        var response = await fetch("api/stops", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(newStop)
        });
        if (response.ok) {
            var json = await response.json();
            return json as stopVM;
        }

        return null;
    }

    async GetByName(name: string) {
        var response = await fetch('api/stops/' + name);
        if (response.ok) {
            var json = await response.json();
            return json as stopVM;
        }

        return null;
    }

    async Find(hint: string) {
        var response = await fetch('api/stops/find/' + hint);
        if (response.ok) {
            var json = await response.json();
            return json as string[];
        }

        return Array<string>();
    }
}