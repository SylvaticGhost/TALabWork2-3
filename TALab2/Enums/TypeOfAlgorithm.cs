namespace TALab2
{
    public enum TypeOfAlgorithm
    {
        DjikstraAlgorithm,
        FloydWarshallAlgorithm
    }

    public static class TypeOfAlgorithmExtensions
    {
        public static bool ToBoolean(this TypeOfAlgorithm algorithm)
        {
            return algorithm == TypeOfAlgorithm.DjikstraAlgorithm;
        }


        public static TypeOfAlgorithm GetOpposite(this TypeOfAlgorithm algorithm) => algorithm is 0 ? (TypeOfAlgorithm)1 : 0;
    }
}
