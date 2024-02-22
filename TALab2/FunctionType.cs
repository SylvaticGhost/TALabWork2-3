namespace TALab2
{
    public enum FunctionType
    {
        DistanceToPoint,
        List
    }


    public static class FunctionTypeExtension
    {
        public static FunctionType GetOpposite(this FunctionType algorithm) => algorithm is 0 ? (FunctionType)1 : 0;
    }
}

