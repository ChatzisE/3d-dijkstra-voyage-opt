import L from 'leaflet';
const mapHelper = {
    fontAwesomeIcon: L.divIcon({
        html: '<i class="fa fa-map-marker fa-4x"></i>',
        iconSize: [20, 20],
        className: 'myDivIcon'
    })
};
export default mapHelper;