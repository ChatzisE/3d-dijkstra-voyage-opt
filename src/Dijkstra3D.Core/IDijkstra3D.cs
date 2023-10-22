namespace Dijkstra3D.Core;

public interface IDijkstra3D
{
    List<Waypoint> GetGreatCirclePath(Waypoint departure, Waypoint arrival, double step = 3.0);
}