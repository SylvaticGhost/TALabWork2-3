using System.Collections;

namespace TALab2.Comparer;

public class VertexComparer : IEqualityComparer<Vertex>
{
    public bool Equals(Vertex? x, Vertex? y)
    {
        if(x.Sign == y.Sign)
            return true;

        return false;
    }

    public int GetHashCode(Vertex obj)
    {
        //throw new NotImplementedException();
        return 0;
    }
}