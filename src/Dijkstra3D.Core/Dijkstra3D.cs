using System.Diagnostics.Contracts;

namespace Dijkstra3D.Core;

public class Dijkstra3D : IDijkstra3D
{
    private Calculations _calculations { get; } = new();

    public List<Waypoint> GetGreatCirclePath(Waypoint departure,
        Waypoint arrival,
        double step = 3.0,
        double speedOverGround = 12.0)
    {
        var path = new List<Waypoint>();
        var totalDistance = _calculations.CalcDistGC(departure.Lon, departure.Lat, arrival.Lon, arrival.Lat);
        var distanceStep = speedOverGround * step;
        var startPoint = departure;
        path.Add(departure);
        while (totalDistance > 0)
        {
            var newPoint = _calculations.FindNewPointOnGC(startPoint.Lon,
                startPoint.Lat,
                arrival.Lon,
                arrival.Lat,
                distanceStep);
            path.Add(newPoint);
            startPoint = newPoint;
            totalDistance -= distanceStep;
        }

        return path;
    }
}