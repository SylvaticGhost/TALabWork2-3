namespace TALab2;

public record Destination(char Sign, string Name, WayToPoint Way)
{
    public override string ToString()
    {
        return $"Sign: {Sign}, Name: {Name}, Distance: {Math.Round(Way.Distance, 3)} km";
    }
}
