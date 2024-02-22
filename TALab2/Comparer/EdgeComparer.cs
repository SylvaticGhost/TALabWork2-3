namespace TALab2.Comparer;

public class EdgeComparer : IEqualityComparer<Edge>
{
    public bool Equals(Edge? x, Edge? y)
    {
        if (x.Destinations == y.Destinations)
            return true;



        if (!x.Oriented && !y.Oriented)
        {
            if (x.Destinations == (y.Destinations.Item2, y.Destinations.Item1))
                return true;
        }


        return false;
    }


    public int GetHashCode(Edge obj)
    {
        //throw new NotImplementedException();
        return 0;
    }


    // private (T, T) SwapElementInPair<T>((T, T) pair)
    // {
    //     return (pair.Item2, pair.Item1);
    // }
}