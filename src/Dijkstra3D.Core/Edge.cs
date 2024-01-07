namespace Dijkstra3D.Core;

public class Edge
{
    public string Id { set; get; }
    public Node Start { set; get; }
    public Node End { set; get; }
    public double Cost { set; get; }

    public Edge(string id, Node start, Node end, double cost)
    {
        Id = id;
        Start = start;
        End = end;
        Cost = cost;
    }
}