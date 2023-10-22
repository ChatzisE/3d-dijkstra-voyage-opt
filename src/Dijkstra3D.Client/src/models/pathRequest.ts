import Waypoint from "./waypoint";
interface PathRequest {
    departure: Waypoint;
    arrival: Waypoint;
    step: number;
  }
export default PathRequest;