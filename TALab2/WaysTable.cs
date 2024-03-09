namespace TALab2;

public class WaysTable
{
    public List<List<LinkedList<char>>> Table { get; private set; }
    
    private readonly Graph _graph;


    public LinkedList<char> this[int i, int j]
    {
        get => Table[i][j];
        set => Table[i][j] = value;
    }
    
    public WaysTable(Graph graph, Matrix adjacencyMatrix)
    {
        _graph = graph;
        
        int size = graph.Vertices.Count();
        
        List<List<LinkedList<char>>> table = [];

        for (int i = 0; i < size; i++)
        {
            List<LinkedList<char>> row = [];

            for (int j = 0; j < size; j++)
            {
                LinkedList<char> cell = [];
                row.Add(cell);
            }
            
            table.Add(row);
        }
        
        Table = table;
        
        var matrix = adjacencyMatrix.Containing;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (i == j)
                {
                    table[i][j].AddLast(Graph.GetSignFromIndex(i));
                }
                else if (matrix[i][j] > 0 && !double.IsInfinity(matrix[i][j]))
                {
                    table[i][j].AddLast(Graph.GetSignFromIndex(i));
                    table[i][j].AddLast(Graph.GetSignFromIndex(j));
                }
            }
        }
    }


    public override string ToString()
    {
        string result = "";

        foreach (var row in Table)
        {
            foreach (var cell in row)
            {
                result += Functions.LinkedListToString(cell) + "; ";
            }
            result += '\n';
        }
        
        return result;
    }
}