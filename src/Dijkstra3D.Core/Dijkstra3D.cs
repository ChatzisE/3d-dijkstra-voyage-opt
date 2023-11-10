using System.Diagnostics.Contracts;
using System.Xml.Linq;

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

    private void GeneratePerpendicularWaypoints(List<Waypoint> path, int numberOfPointsEachSide = 3, double distanceBetweenPoints = 1)
    {
        for (var i = 1; i < path.Count - 1; i++)
        {
            double[] line = _calculations.FindLineFrom2Points(path[i - 1].Lon, path[i - 1].Lat, path[i].Lon, path[i].Lat);
            double[] perp = _calculations.FindLinePerpendicularFromPoint(line, path[i].Lon, path[i].Lat);
            // Add points over the basic ones
            for (int ij = -numberOfPointsEachSide; ij <= numberOfPointsEachSide; ij++)
            {
                double j = ij * distanceBetweenPoints;
                if (j != 0) // For j==0 we already have the basic points
                {
                    double newLon;
                    double newLat;
                    if (perp[0] == 0) // x = -c
                    {
                        newLon = -1 * perp[2];
                        newLat = path[i].Lat + j; // Increase the lat by 1
                    }
                    else
                    {
                        if (perp[1] == 0) // y = c
                        {
                            newLon = path[i].Lon + j; // Increase the lon by 1
                            newLat = perp[2];
                        }
                        else //y = (-1/a)x + c
                        {
                            // ---------------- NEW CALCULATION ------------------------------
                            // Increase the place of the parallel by j
                            // Find intersection between parallel and perp
                            // Parallel y = ax + b
                            // Perp     y = cx + d
                            // x = (d - b)/(a - c) 
                            // https://en.wikipedia.org/wiki/Distance_between_two_parallel_lines
                            double calc1 = j * (Math.Sqrt(Math.Pow((double)line[1], 2) + 1));
                            double b2 = calc1 + line[2];
                            double[] parallel1 = { line[0], line[1], b2 };
                            newLon = (perp[2] - parallel1[2]) / (parallel1[1] - perp[1]);
                            newLat = perp[1] * newLon + perp[2];
                        }
                    }
                    var newPoint = new Waypoint()
                    {
                        Lat = newLat,
                        Lon = newLon
                    };
                    path[i].Perpendiculars.Add(newPoint);

                }
            }

        }
    }
}