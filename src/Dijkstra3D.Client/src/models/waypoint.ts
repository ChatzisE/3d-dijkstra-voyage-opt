interface Waypoint {
    lat: number;
    lon: number;
    timestamp: Date;
    perpendiculars: [Waypoint];
}

export default Waypoint;