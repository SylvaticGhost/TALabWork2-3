namespace TALab2;

public class WaysTable
{
    public List<List<List<char>>> Table { get; private set; }
    
    private readonly Graph _graph;
    private readonly int _size;

    public List<char> this[int i, int j]
    {
        get => Table[i][j];
        set => Table[i][j] = value;
    }
    
    public WaysTable(Graph graph, Matrix adjacencyMatrix)
    {
        _graph = graph;
        
        int size = graph.Vertices.Count();
        _size = size;
        List<List<List<char>>> table = [];
        
        for (int i = 0; i < size; i++)
        {
            List<List<char>> row = [];
        
            for (int j = 0; j < size; j++)
            {
                List<char> cell = [];
                row.Add(cell);
            }
            
            table.Add(row);
        }
        
        Table =  table;
        
        var matrix = adjacencyMatrix.Containing;
        
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (matrix[i][j] > 0 && !double.IsInfinity(matrix[i][j]))
                {
                    table[i][j].Add(Graph.GetSignFromIndex(i));
                    table[i][j].Add(Graph.GetSignFromIndex(j));
                }
                else 
                {
                    table[i][j].Add(Graph.GetSignFromIndex(i));
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
                result += Functions.WayListToString(cell) + "; ";
            }
            result += '\n';
        }
        
        return result;
    }


    public void FinishTable()
    {
        for (int i = 0; i < _size; i++)
        {
            for (int j = 0; j < _size; j++)
            {
                //checking cell
                if (i != j)
                {
                    char start = Graph.GetSignFromIndex(i);
                    char end = Graph.GetSignFromIndex(j);
                    
                    if(this[i, j].Last() == end)
                        continue;
                    
                    this[i, j].Add(end);

                    if (!_graph.CheckIfVerticesAreNeighbours(start, end))
                    {
                        if (this[i, j].Count > 2 && this[i, j].Last() != end)
                        {
                            this[i, j].RemoveRange(1, this[i,j].Count - 2);
                        }
                        
                        
                        for (int k = 0; k < this[i, j].Count - 1; k++)
                        {
                            if(!_graph.CheckIfVerticesAreNeighbours(this[i, j].ElementAt(k), this[i, j].ElementAt(k + 1)))
                            {
                                int index1 = Graph.GetIndexFromSign(this[i, j].ElementAt(k));
                                int index2 = Graph.GetIndexFromSign(this[i, j].ElementAt(k + 1));

                                List<char> addRoad = new(this[index1, index2]);
                                
                                if(addRoad.Count < 2)
                                    continue;
                                
                                addRoad.RemoveAt(0);
                                addRoad.RemoveAt(addRoad.Count - 1);
                                if(this[i, j].Except(addRoad).Any())
                                    this[i, j].InsertRange(k + 1, addRoad);

                                k--;
                            }
                        }
                        
                        
                    }
                    
                    //simplify way if it contains loops
                    for (int k = 0; k < this[i, j].Count - 1; k++)
                    {
                        for (int l = k + 2; l < this[i, j].Count; l++)
                        {
                            if(_graph.CheckIfVerticesAreNeighbours(this[i, j].ElementAt(k), this[i, j].ElementAt(l)))
                            {
                                this[i, j].RemoveRange(k + 1, l - k - 1);
                                l = k + 2;
                            }
                        }
                    }
                }
            }
        }
    }
    
}