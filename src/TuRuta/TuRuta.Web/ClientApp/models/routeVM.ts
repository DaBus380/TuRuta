interface routeVM {
    id: string,
    name: string,
    buses: busVM[],
    stops: stopVM[],
    incidents: incidentVM[]
}