namespace TALab2;

public class Vertex
{
    /// <summary>
    /// Letter Or Number for short signing
    /// </summary>
    public char Sign { get; set; }
    /// <summary>
    /// meaningfull name
    /// </summary>
    public string Name { get; set; }
    public double CurrentWeightInGraph { get; set; }
    public Cords Cords { get; }
    
    public Vertex(char sign,Cords cords,string name = "")
    {
        Sign = sign;
        Name = name;
        CurrentWeightInGraph = 0;
        Cords = cords;
    }

    public override string ToString()
    {
        return $"{Sign} Cords: {Cords}, Name: {Name}";
    }
    
}