namespace TALab2;

public record WayToPoint(double Distance, List<char> Path)
{
    public char Start => Path.First();
    public char Finish => Path.Last();
    
    public override string ToString()
    {
        return $"Distance: {Math.Round(Distance, 3)} km, Path: {Functions.WayListToString(Path)}";
    }


    public string ToStringLong(Graph graph)
    {
        return
            $"Way from {graph[Start].Name} to {graph[Finish].Name}, Distance: {Math.Round(Distance, 3)} km, Path: {GetPath(graph)}";
    }

    private string GetPath(Graph graph)
    {
        string result = "";
        
        for(int i = 0; i < Path.Count - 1; i++)
        {
            double distance = graph.GetEdge(Path[i], Path[i + 1]).Weight;
            result += $"{graph[Path[i]].Name} ({Math.Round(distance, 3)}km) --> ";
        }
        
        result += graph[Path.Last()].Name;
        
        return result;
    }
}
