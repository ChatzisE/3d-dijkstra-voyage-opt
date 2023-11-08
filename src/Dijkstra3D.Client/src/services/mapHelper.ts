import L from 'leaflet';
import waypoint from "../models/waypoint.ts";

export default class MapHelper {
    circleMarkers: L.LayerGroup;
    departureMarker: L.Marker;
    arrivalMarker: L.Marker;
    pathLine: L.Polyline;
    departureIcon: L.DivIcon;
    arrivalIcon: L.DivIcon;
    map: L.Map;
    fontAwesomeIcon: L.DivIcon = L.divIcon({
        html: '<i class="fa-solid fa-d fa-2x"></i>',
        iconSize: [20, 20],
        className: 'myDivIcon'
    });

    constructor(mapId: string) {
        this.circleMarkers = L.layerGroup();
        this.pathLine = L.polyline([], {color: "#005fa3"});
        this.departureMarker = L.marker([0, 0]);
        this.arrivalMarker = L.marker([0, 0]);
        this.departureIcon = this.fontAwesomeIcon;
        this.arrivalIcon = this.fontAwesomeIcon;
        const mapElement = document.getElementById(mapId);
        if (!mapElement) {
            throw new Error("Map element not found");
        }
        this.map = L.map(mapElement).setView([0, 0], 2);
        this.map.zoomControl.setPosition("topright");
        L.tileLayer(
            "https://a.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}@2x.png",
            {
                attribution:
                    'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors',
                maxZoom: 18,
            }
        ).addTo(this.map);
    }

    addMarker(lat: number, lon: number, type: string): void {
        switch (type) {
            case "departure":
                L.circleMarker(L.latLng(lat, lon), {
                    radius: 9,
                    color: '#ee9265',
                    fillOpacity: 1
                }).addTo(this.map);
                break;
            case "arrival":
                L.circleMarker(L.latLng(lat, lon), {
                    radius: 9,
                    color: '#ee9265',
                    fillOpacity: 1
                }).addTo(this.map);
                break;
            case "circle":
                const circleMarker = L.circleMarker(L.latLng(lat, lon), {
                    radius: 3,
                    color: '#121A1DFF',
                    fillOpacity: 1
                });
                this.circleMarkers.addLayer(circleMarker);
        }
    }

    drawPath(path: waypoint[]): void {
        this.pathLine.removeFrom(this.map);
        this.circleMarkers.removeFrom(this.map);
        path.forEach((p) => {
            this.pathLine.addLatLng(L.latLng(p.lat, p.lon));
            this.addMarker(p.lat, p.lon, "circle");
            p.perpendiculars.forEach((perpendicular) => {
                //this.pathLine.addLatLng(L.latLng(perpendicular.lat, perpendicular.lon));
                this.addMarker(perpendicular.lat, perpendicular.lon, "circle");
            });
        });
        this.pathLine.addTo(this.map);
        this.circleMarkers.addTo(this.map);
        this.map.fitBounds(this.pathLine.getBounds());
    }
}