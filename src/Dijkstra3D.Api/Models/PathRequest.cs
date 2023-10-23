namespace Dijkstra3D.Api.Models;

public class PathRequest
{
    public Waypoint Departure { get; set; }
    public Waypoint Arrival { get; set; }
    public double Step { get; set; }
    public double SpeedOverGround { get; set; }
}