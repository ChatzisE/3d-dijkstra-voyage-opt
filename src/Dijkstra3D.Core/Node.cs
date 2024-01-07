namespace Dijkstra3D.Core;

public class Node
{
    public string Id { set; get; }
    public List<Edge> EdgesIn { set; get; }

    public List<Edge> EdgesOut { set; get; }

    // --- Dijkstra info ---
    public double CostFromStart { set; get; }
    public Node? PrevNode { set; get; }
    public bool Finalized { set; get; }
    public bool Initialized { set; get; }

    public Node(string id)
    {
        Id = id;
        EdgesIn = new List<Edge>();
        EdgesOut = new List<Edge>();
    }

    public void InitializeNodeForDijkstra()
    {
        CostFromStart = double.MaxValue;
        PrevNode = null;
        Finalized = false;
        Initialized = false;
    }
}