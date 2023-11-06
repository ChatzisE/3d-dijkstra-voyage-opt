public class Waypoint
{
    public Waypoint()
    {
        Perpendiculars = new List<Waypoint>();
    }

    public double Lat { get; set; }
    public double Lon { get; set; }
    public DateTime? Timestamp { get; set; }
    public List<Waypoint> Perpendiculars { get; set; }
}