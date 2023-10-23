import Waypoint from "./waypoint";

interface PathRequest {
    departure: Waypoint;
    arrival: Waypoint;
    step: number;
    speedOverGround: number;
}

export default PathRequest;