namespace TALab2;

public class MapHelpers
{
    private readonly DataProvider _dataProvider;
    
    public MapHelpers(DataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }
    
    public bool CheckIfVerticesIsNeighbours(char first, char second)
    {
        foreach (var edge in _dataProvider.Edges)
        {
            if(edge.Vertix1.Sign == first && edge.Vertix2.Sign == second)
                return true;

            if (!edge.Oriented)
            {
                if(edge.Vertix2.Sign == first && edge.Vertix1.Sign == second)
                    return true;
            }
        }
        
        return false;
    }
}