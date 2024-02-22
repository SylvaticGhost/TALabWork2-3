namespace TALab2;

public class DataProvider
{
    public List<Vertex> Vertixes =
    [
        new Vertex('A', new Cords(50.451054, 30.465649), "KPI"), //0
        new Vertex('B', new Cords(50.442556, 30.511684), "Red University"), //1
        new Vertex('C', new Cords(50.448916, 30.513862), "Golden gate"), //2
        new Vertex('D', new Cords(50.452632, 30.514738), "Sophia Church"), //3
        new Vertex('E', new Cords(50.444646, 30.521361), "Fontan"), //4
        new Vertex('F', new Cords(50.450841, 30.522879), "Liadski gate"), //5
        new Vertex('G', new Cords(50.458816, 30.517610), "Andriy Church"), //6
        new Vertex('H', new Cords(50.462012, 30.517541), "One street museum"), //7
        new Vertex('I', new Cords(50.457803, 30.523566), "Funikuler"), //8
        new Vertex('J', new Cords(50.455803, 30.522940), "Mykhail Church"), //9
        new Vertex('K', new Cords(50.453411, 30.527816), "Philarmonia") //10
    ];

    private Vertex GetVertix(char sign)
    {
        return Vertixes.Where(v => v.Sign == sign).FirstOrDefault();
    }
    

    public List<Edge> Edges { get; set; }

    public DataProvider()
    {
        Edges =
        [
            new Edge(GetVertix('A'), GetVertix('B')),
            new Edge(GetVertix('B'), GetVertix('C')),
            new Edge(GetVertix('C'), GetVertix('D')),
            new Edge(GetVertix('C'), GetVertix('E'), true),
            new Edge(GetVertix('D'), GetVertix('F'), true),
            new Edge(GetVertix('F'), GetVertix('E')),
            new Edge(GetVertix('D'), GetVertix('G')),
            new Edge(GetVertix('D'), GetVertix('I')),
            new Edge(GetVertix('J'), GetVertix('D'), true),
            new Edge(GetVertix('G'), GetVertix('H')),
            new Edge(GetVertix('G'), GetVertix('I')),
            new Edge(GetVertix('G'), GetVertix('J'), true),
            new Edge(GetVertix('H'), GetVertix('I'), true),
            new Edge(GetVertix('I'), GetVertix('J')),
            new Edge(GetVertix('J'), GetVertix('K')),
            new Edge(GetVertix('F'), GetVertix('J'), true),
            new Edge(GetVertix('K'), GetVertix('F')),
            new Edge(GetVertix('F'), GetVertix('E'))
        ];
    }

    
}