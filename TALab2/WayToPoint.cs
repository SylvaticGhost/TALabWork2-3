namespace TALab2;

public record WayToPoint(double Distance, LinkedList<char> Path)
{
    public char Start => Path.First!.Value;
    public char Finish => Path.Last!.Value;
    
    public override string ToString()
    {
        return $"Distance: {Math.Round(Distance, 3)} km, Path: {Functions.LinkedListToString(Path)}";
    }
}
