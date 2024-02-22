namespace TALab2;

public record Destination(char Sign, string Name, double Distance)
{
    public override string ToString()
    {
        return $"Sign: {Sign}, Name: {Name}, Distance: {Distance}";
    }
}
