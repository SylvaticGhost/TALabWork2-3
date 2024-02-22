namespace TALab2;

public class Edge
{
    public Vertex Vertix1 { get; }
    public Vertex Vertix2 { get;  }
    public double Weight { get; } 
    public bool Oriented { get; }
    
    public (char, char) Destinations { get; }

    public Edge(Vertex v1, Vertex v2, bool oriented = false, double weight = 0)
    {
        Vertix1 = v1;
        Vertix2 = v2;
        
        if (weight == 0)
            Weight = v1.Cords.CalculateDistanceToPoint(v2.Cords);
        else
            Weight = weight;
        Oriented = oriented;
        Destinations = (v1.Sign, v2.Sign);
    }

    public override string ToString()
    {
        if(Oriented)
            return $"{Vertix1.Sign} --> {Vertix2.Sign}, distance={Weight}";
        
        return $"{Vertix1.Sign} --- {Vertix2.Sign}, distance={Weight}";
    }
    
    
    public Vertex? GetVertixOppositeTo(Vertex v1)
    {
        if (v1.Sign == Vertix1.Sign)
            return Vertix2;
        
        if (v1.Sign == Vertix2.Sign)
            return Vertix1;

        return null;
    }
    
}