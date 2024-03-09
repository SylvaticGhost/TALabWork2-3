namespace TALab2;

public record WayToPoint(double Distance, List<char> Path)
{
    public char Start => Path.First();
    public char Finish => Path.Last();
    
    public override string ToString()
    {
        return $"Distance: {Math.Round(Distance, 3)} km, Path: {Functions.WayListToString(Path)}";
    }


    // public string ToStringLong(Graph graph)
    // {
    //     
    // }
}
