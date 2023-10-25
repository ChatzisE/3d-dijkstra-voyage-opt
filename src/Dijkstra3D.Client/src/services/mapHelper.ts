import L from 'leaflet';

class mapHelper {
    markers: L.LayerGroup;
    departureMarker: L.Marker;
    arrivalMarker: L.Marker;
    greatCircle: L.Polyline;
    departureIcon: L.DivIcon;
    arrivalIcon: L.DivIcon;
    // Normal signature with defaults
    constructor(mapId: string) {
        const mapElement = document.getElementById(mapId);
        if (mapElement) {
            map.value = L.map(mapElement).setView([0, 0], 2);
            map.value.zoomControl.setPosition("topright");
            L.tileLayer(
                "https://a.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}@2x.png",
                {
                    attribution:
                        'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors',
                    maxZoom: 18,
                }//@ts-ignore
            ).addTo(map.value);
            departureMarker.value = L.marker(
                [departure.value.lat, departure.value.lon],
                {
                    draggable: true,
                }//@ts-ignore
            ).addTo(map.value);
            arrivalMarker.value = L.marker([arrival.value.lat, arrival.value.lon], {
                draggable: true,
                //@ts-ignore
            }).addTo(map.value);
            markers.value = new L.LayerGroup();
        }
    }
}