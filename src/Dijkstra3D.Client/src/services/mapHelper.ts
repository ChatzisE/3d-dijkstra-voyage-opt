import L from 'leaflet';
import PathRequest from "./models/pathRequest";
import pathRequest from "../models/pathRequest.ts";

class mapHelper {
    circleMarkers: L.LayerGroup;
    departureMarker: L.Marker;
    arrivalMarker: L.Marker;
    greatCircle: L.Polyline;
    departureIcon: L.DivIcon;
    arrivalIcon: L.DivIcon;
    map: L.Map;
    fontAwesomeIcon: L.divIcon = L.divIcon({
        html: '<i class="fa fa-map-marker fa-4x"></i>',
        iconSize: [20, 20],
        className: 'myDivIcon'
    });

    constructor(mapId: string) {
        const mapElement = document.getElementById(mapId);
        if (mapElement) {
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
    }

    addMarker(lat: number, lon: number, type: string): void {
        switch (type) {
            case "departure":
                this.departureMarker = L.marker(
                    [departure.lat, departure.lon],
                    {icon: this.fontAwesomeIcon}
                ).addTo(this.map);
                break;
            case "arrival":
                this.arrivalMarker = L.marker([arrival.lat, arrival.lon],
                    {icon: this.fontAwesomeIcon}).addTo(this.map);
                break;
            case "circle":
                const circleMarker = L.circleMarker(L.latLng(p.lat, p.lon), {
                    radius: 3,
                    color: '#121A1DFF',
                    fillOpacity: 1
                });
                this.circleMarkers.addLayer(circleMarker);
        }
    }

    drawPath(path: pathRequest): void {
        path.forEach((p) => {
            this.greatCircle.addLatLng(L.latLng(p.lat, p.lon));
            this.addMarker(p.lat, p.lon, "circle");
        });
        this.greatCircle.addTo(this.map);
        this.markers.addTo(this.map);
        this.map.fitBounds(this.greatCircle.getBounds());
    }
}