using System.Collections;
using System.Diagnostics;
using TALab2;

namespace TestProject1;

[TestFixture]
public class Tests
{
    private DataProvider _dataProvider;
    private Graph _graph;
    private const double calculationError = 0.0000001;

    [SetUp]
    public void Setup()
    {
        _dataProvider = new DataProvider();
        _graph = new Graph(_dataProvider.Vertixes, _dataProvider.Edges);
    }
    
    
    [Test]
    [TestCaseSource(nameof(GetTestCases))]
    public void TestingDistanceToPointFunction(char vertex1, char vertex2)
    {
        Stopwatch stopWatchDijkstra = Stopwatch.StartNew();
        double byDijkstraAlgorithm = _graph.DijkstraAlgorithm(_graph[vertex1], _graph[vertex2]);
        stopWatchDijkstra.Stop();

        Stopwatch stopWatchFloydWarshall = Stopwatch.StartNew();
        double byFloydWarshall = _graph.FloydWarshallAlgorithm(_graph[vertex1], _graph[vertex2]);
        stopWatchFloydWarshall.Stop();
        
        if(Math.Abs(byFloydWarshall - byDijkstraAlgorithm) < calculationError)
        {
            Console.WriteLine("Distance: " + byFloydWarshall);
            Console.WriteLine($"Time for Dijkstra algorithm: {stopWatchDijkstra.ElapsedMilliseconds:F0} ms" );
            Console.WriteLine($"Time for Floyd-Warshall algorithm: {stopWatchFloydWarshall.ElapsedMilliseconds:F0} ms");

            Assert.Pass();
        }
        else
        {
            Console.WriteLine($"Distance by Dijkstra Algorithm: {byDijkstraAlgorithm}, by Floyd-Warshall: {byFloydWarshall}");

            Assert.Fail();
        }
        
    }


    [Test]
    [TestCaseSource(nameof(GetCaseForListTest))]
    public void TestingListFunction(char sign)
    {
        Stopwatch stopWatchDijkstra = Stopwatch.StartNew();
        IEnumerable<Destination> listByDijkstra = _graph.GetListOfShortest(_graph[sign], TypeOfAlgorithm.DjikstraAlgorithm);
        stopWatchDijkstra.Stop();

        Stopwatch stopWatchFloydWarshall = Stopwatch.StartNew();
        IEnumerable<Destination> listByFloydWarshall = _graph.GetListOfShortest(_graph[sign], TypeOfAlgorithm.DjikstraAlgorithm);
        stopWatchFloydWarshall.Stop();

        if (CompareList(listByDijkstra.ToList(), listByFloydWarshall.ToList()))
        {
            Console.WriteLine("Created list:");
            Console.WriteLine(Functions.CollectionToString(listByDijkstra));
            Console.WriteLine($"Time for Dijkstra algorithm: {stopWatchDijkstra.ElapsedMilliseconds} ms");
            Console.WriteLine($"Time for Floyd-Warshall algorithm: {stopWatchFloydWarshall.ElapsedMilliseconds} ms");

            Assert.Pass();
        }
        else
        {
            Console.WriteLine("List by Dijkstra algorithm:");
            Console.WriteLine(Functions.CollectionToString(listByDijkstra));

            Console.WriteLine("List by Floyd-Warshall algorithm");
            Console.WriteLine(Functions.CollectionToString(listByFloydWarshall));

            Assert.Fail();
        }
    }


    private static object[] GetTestCases()
    {
        var testCases = new List<object[]>();

        for (char c1 = 'A'; c1 <= 'K'; c1++)
        {
            for (char c2 = 'A'; c2 <= 'K'; c2++)
            {
                testCases.Add(new object[] { c1, c2 });
            }
        }

        return testCases.ToArray();
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


    private bool CompareList(IReadOnlyList<Destination> list1, IReadOnlyList<Destination> list2)
    {
        if (list1.Count() == list2.Count())
        {
            for (int i = 0; i < list1.Count(); i++)
            {
                if (!(Math.Abs(list1[i].Distance - list2[i].Distance) < calculationError) || list1[i].Sign != list2[i].Sign)
                    return false;
            }

            return true;
        }
        
        return false;
    }
}