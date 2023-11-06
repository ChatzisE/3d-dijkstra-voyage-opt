using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra3D.Core
{
    public class Calculations
    {
        public double CalcDistGC(double lon1, double lat1, double lon2, double lat2)
        {
            return CalcDistGC_ChangeDir(lon1, lat1, lon2, lat2, IdentifyChangeDir_Simple(lon1, lat1, lon2, lat2));
        }

        public DateTime CalcETA(DateTime arrival, double speedOverGround, double distance)
        {
            var calcHours = distance / speedOverGround;
            return arrival.AddHours(calcHours);
        }

        // --- GC CALCULATIONS ---
        public Waypoint FindNewPointOnGC(double lon1, double lat1, double lon2, double lat2, double distanceNM)
        {
            var bearing = CalcBearingGC(lon1, lat1, lon2, lat2);
            var newPoint = FindNewPointWithBearing(lon1, lat1, bearing, distanceNM);
            if (newPoint.Lon > 180)
                newPoint.Lon = (newPoint.Lon - 360);
            else if ((decimal)newPoint.Lon < -180)
                newPoint.Lon = (360 + newPoint.Lon);
            return newPoint;
        }

        public double CalcBearingGC(double lon1, double lat1, double lon2, double lat2)
        {
            //Convert to radians
            double x1 = DegreeToRadian(lon1);
            double y1 = DegreeToRadian(lat1);
            double x2 = DegreeToRadian(lon2);
            double y2 = DegreeToRadian(lat2);

            double bearing = 0;
            double a = Math.Cos(y2) * Math.Sin(x2 - x1);
            double b = Math.Cos(y1) * Math.Sin(y2) - Math.Sin(y1) * Math.Cos(y2) * Math.Cos(x2 - x1);
            double adjust = 0;

            if ((a == 0) && (b == 0))
            {
                bearing = 0;
            }
            else if (b == 0)
            {
                if (a < 0)
                    bearing = 3 * Math.PI / 2;
                else
                    bearing = Math.PI / 2;
            }
            else if (b < 0)
            {
                adjust = Math.PI;
                bearing = RadianToDegree((Math.Atan(a / b) + adjust));
            }
            else
            {
                if (a < 0)
                    adjust = 2 * Math.PI;
                else
                    adjust = 0;

                bearing = RadianToDegree(Math.Atan(a / b) + adjust);
            }

            return bearing;
        }

        public Waypoint FindNewPointWithBearing(double lon, double lat, double bearing, double distanceNM)
        {
            double distanceNMadjusted = distanceNM / 0.29155;
            Waypoint newPoint = new Waypoint();
            double x1deg = Convert.ToDouble(lon);
            double y1deg = Convert.ToDouble(lat);

            //Convert distance to km
            double distance = distanceNMadjusted / 1.852;
            //Radius of the Earth in km
            //double EARTH_RADIUS = 6371;
            double EARTH_RADIUS = EstimateEarthRadius(lat);

            //Convert to radians
            double x1 = DegreeToRadian(x1deg);
            double y1 = DegreeToRadian(y1deg);
            double bearingRadian = DegreeToRadian(bearing);

            // Convert arc distance to radians
            double c = distance / EARTH_RADIUS;

            double ynew =
                RadianToDegree(Math.Asin(Math.Sin(y1) * Math.Cos(c) +
                                         Math.Cos(y1) * Math.Sin(c) * Math.Cos(bearingRadian)));
            double xnew;

            double a = Math.Sin(c) * Math.Sin(bearingRadian);
            double b = Math.Cos(y1) * Math.Cos(c) - Math.Sin(y1) * Math.Sin(c) * Math.Cos(bearingRadian);

            if (b == 0)
                xnew = x1deg;
            else
                xnew = x1deg + RadianToDegree(Math.Atan(a / b));

            newPoint.Lon = xnew;
            newPoint.Lat = ynew;

            // Resolve accuracy issues
            if ((bearing == 0) || (bearing == 180))
                newPoint.Lon = lon;
            else if ((bearing == 90) || (bearing == 270))
                newPoint.Lat = lat;

            return newPoint;
        }

        public double CalcDistGC_ChangeDir(double lon1, double lat1, double lon2, double lat2, bool changeDir)
        {
            //var R = 6371; // The mean radius of the earth in km
            double minLat = Math.Min(lat1, lat2);
            double maxLat = Math.Max(lat1, lat2);
            double difLat = maxLat - minLat;
            double meanLat = minLat + difLat / 2;
            double R = EstimateEarthRadius(meanLat); //Radius of the Earth in km
            if (changeDir)
            {
                lon1 = (360 + lon1) % 360;
                lon2 = (360 + lon2) % 360;
            }

            double dLat = DegreeToRadian(lat1 - lat2);
            double dLon = DegreeToRadian(lon1 - lon2);
            double p1LatRad = DegreeToRadian(lat2);
            double p2LatRad = DegreeToRadian(lat1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(p1LatRad) * Math.Cos(p2LatRad);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = (R * c);
            distance = distance / 1.852; // Convert to NM

            return distance;
        }

        public bool IdentifyChangeDir_Simple(double lon1, double lat1, double lon2, double lat2)
        {
            double absDif = Math.Abs(lon1 - lon2);
            // Checks if it is closer to change directions.
            if (absDif < 360 - absDif)
                return false;
            else
                return true;
        }

        public double EstimateEarthRadius(double lat)
        {
            // Polar radius: 6356.752
            // Equator radius: 6378.137
            // Mean radius: 6371.008
            double radius = 6356.752;
            double difference = 21.385; // Equator - Polar radius
            radius = radius + difference * Math.Cos(lat * (Math.PI / 180));

            return radius;
        }

        public double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public double CalcDistRL(double lon1, double lat1, double lon2, double lat2)
        {
            double diffLat = (lat1 + lat2) / 2;
            double R = EstimateEarthRadius(diffLat) / 1.852; // Convert to nm 
            double latRad1 = DegreeToRadian(lat2);
            double latRad2 = DegreeToRadian(lat1);
            double dLat = DegreeToRadian(lat1 - lat2);
            double dLon = DegreeToRadian(Math.Abs(lon1 - lon2));

            double dPhi = Math.Log(Math.Tan(latRad2 / 2 + Math.PI / 4) / Math.Tan(latRad1 / 2 + Math.PI / 4));
            double q = Math.Cos(latRad1);
            if (dPhi != 0) q = dLat / dPhi; // E-W line gives dPhi=0
            // if dLon over 180° take shorter rhumb across 180° meridian:
            if (dLon > Math.PI) dLon = 2 * Math.PI - dLon;
            double dist = Math.Sqrt(dLat * dLat + q * q * dLon * dLon) * R;

            return dist;
        }

        public double CalculateVesselDirection(double vlon1, double vlat1, double vlon2, double vlat2)
        {
            double angle = 0;
            if (vlon1 == vlon2)
            {
                if (vlat1 <= vlat2)
                    angle = 0;
                else
                    angle = 180;
            }
            else
            {
                double absDif = Math.Abs(vlon1 - vlon2);
                // Checks if it is closer to change directions.
                if (absDif > 360 - absDif)
                {
                    vlon1 = (360 + vlon1) % 360;
                    vlon2 = (360 + vlon2) % 360;
                }

                if (vlat1 == vlat2)
                {
                    if (vlon1 <= vlon2)
                        angle = 90;
                    else
                        angle = 270;
                }
                else
                {
                    double division = (double)((vlat2 - vlat1) / (vlon2 - vlon1));
                    if (vlon1 < vlon2)
                        angle = 90 - RadianToDegree(Math.Atan(division));
                    else // if ((vlon1 > vlon2) && (vlat1 < vlat2))
                        angle = 270 - RadianToDegree(Math.Atan(division));
                }
            }

            return angle;
        }


        public double CalcSTWcurrentsEffectNAV(double vesselSpeed, double vesselDir, double currentSpeed,
            double currentDir)
        {
            double vesselTrigDeg = ConvertVesselSeaDirToTrigonometricalDegrees(vesselDir);
            double currentTrigDeg = ConvertVesselSeaDirToTrigonometricalDegrees(currentDir);
            double stwX = vesselSpeed * Math.Cos(DegreeToRadian(vesselTrigDeg)) -
                          currentSpeed * Math.Cos(DegreeToRadian(currentTrigDeg));
            double stwY = vesselSpeed * Math.Sin(DegreeToRadian(vesselTrigDeg)) -
                          currentSpeed * Math.Sin(DegreeToRadian(currentTrigDeg));
            double stw = Math.Round(Math.Sqrt(Math.Pow(stwX, 2) + Math.Pow(stwY, 2)), 2);
            return stw;
        }

        public double GetRelativeWeather(double bearing, double elementDir)
        {
            return Math.Abs(((elementDir - bearing + 180) % 360) - 180);
        }

        public double RadianToDegree(double rads)
        {
            var result = rads * (180.0 / Math.PI);
            return result;
        }

        public double ConvertVesselSeaDirToTrigonometricalDegrees(double vesselDegrees)
        {
            double trigonometricalDegrees = 90 - vesselDegrees;
            return trigonometricalDegrees;
        }

        public Waypoint CalculatePerpendicularPoint(double x1, double y1, double x2, double y2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;

            // Calculate the coordinates of the perpendicular point
            double px = x1 + dy;
            double py = y1 - dx;

            return new Waypoint()
            {
                Lon = px,
                Lat = py
            };
        }
    }
}