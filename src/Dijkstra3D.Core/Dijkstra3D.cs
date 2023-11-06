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
        while (totalDistance > distanceStep)
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

        path.Add(arrival);
        GeneratePerpendicularWaypoints(path);
        return path;
    }

    private void GeneratePerpendicularWaypoints(List<Waypoint> path)
    {
        for (var i = 1; i < path.Count - 1; i++)
        {
            var newPoint = _calculations.CalculatePerpendicularPoint(path[i - 1].Lon,
                path[i - 1].Lat,
                path[i].Lon,
                path[i].Lat);
            path[i].Perpendiculars.Add(newPoint);
        }
    }
}