using System.Diagnostics.Contracts;

namespace Dijkstra3D.Core;

public class Dijkstra3D : IDijkstra3D
{
    private Calculations _calculations { get;} = new();

    public List<Waypoint> GetGreatCirclePath(Waypoint departure, Waypoint arrival, double step = 3.0)
    {
        var path = new List<Waypoint>();
        var distance = _calculations.CalcDistGC(departure.Lon, departure.Lat, arrival.Lon, arrival.Lat);
        var numberOfWaypoints = (int)Math.Ceiling(distance / step);
        for (var i = 0; i < numberOfWaypoints; i++)
        {
            var lon = departure.Lon + (arrival.Lon - departure.Lon) * i / numberOfWaypoints;
            var lat = departure.Lat + (arrival.Lat - departure.Lat) * i / numberOfWaypoints;
            var waypoint = new Waypoint { Lon = lon, Lat = lat };
            path.Add(waypoint);
        }
        return path;
    }
}
