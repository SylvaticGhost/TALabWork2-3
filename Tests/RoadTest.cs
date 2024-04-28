using TALab2;

namespace TestProject1;

[TestFixture]
public class RoadTest
{
    private const double calculationError = 0.001;
    [SetUp]
    public void Setup()
    {
    }
    
    [TestCaseSource(nameof(GetCaseForListTest))]
    public void TestFloydWarshall(char start)
    {
        DataProvider dataProvider = new DataProvider();
        Graph graph = new Graph(dataProvider.Vertixes, dataProvider.Edges);

        var result = graph.GetListOfShortest(graph[start], TypeOfAlgorithm.FloydWarshallAlgorithm);
        bool failed = false;
        foreach (var way in result)
        {
            double d = 0;
            
            for (int i = 0; i < way.Way.Path.Count - 1; i++)
            {
                d += graph.GetEdge(way.Way.Path[i], way.Way.Path[i + 1]).Weight;
            }
            
            
            if (Math.Abs(d - way.Way.Distance) > calculationError)
            {
                Console.WriteLine("Test failed on case");
                Console.WriteLine(way.Way.ToStringLong(graph));
                Console.WriteLine("Expected distance: " + way.Way.Distance);
                Console.WriteLine("Actual distance: " + d);
                Console.WriteLine("Difference: " + Math.Abs(d - way.Way.Distance));
                failed = true;
            }
        }
        
        if(failed)
            Assert.Fail();
        
        Assert.Pass();
    }
    
    
    private static char[] GetCaseForListTest()
    {
        List<char> testCases = [];
    
        for(char c = 'A'; c <= 'K'; c++)
        {
            testCases.Add(c);
        }

        return testCases.ToArray();
    }
}