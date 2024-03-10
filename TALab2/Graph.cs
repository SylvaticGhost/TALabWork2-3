using System.Collections;
using TALab2.Comparer;
using System;
using Vertex = TALab2.Vertex;

namespace TALab2;

public class Graph
{
    public IEnumerable<Vertex> Vertices { get; set; }

    public IEnumerable<Edge> Edges { get; set; }

    private readonly GraphOperations _graphOperations;
    private readonly List<Edge> _checkedEdges;
    public Vertex this[char sign] => GetVertex(sign);
    
    public Dictionary<char, List<char>>? NeighbourVertices { get; set; }

    public Graph(IEnumerable<Vertex> vertices, IEnumerable<Edge> edges)
    {
        Vertices = vertices.OrderBy(v => v.Sign);
        Edges = edges;
        _graphOperations = new();
        _checkedEdges = [];
        InitNeighbourVerticesDictionary();
    }


    public WayToPoint DijkstraAlgorithm(Vertex start, Vertex end)
    {
        Queue<Vertex> queue = new();
        
        start.CurrentRoadInGraph = new();
        start.CurrentRoadInGraph.AddLast(start.Sign);
        
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            Vertex currentVertex = queue.Dequeue();

            if (currentVertex.Sign == end.Sign)
                continue;

            Console.WriteLine($"Current vertex: {currentVertex}, cost: {currentVertex.CurrentWeightInGraph}");

            IReadOnlyCollection<Edge> availableEdges = GetEdgesForVertex(currentVertex).ToList();
            Console.Write($"Available edges for {currentVertex.Sign}: ");

            foreach (Edge e in availableEdges)
            {
                Console.Write($" {e},");
            }

            Console.WriteLine();

            foreach (Edge edge in availableEdges)
            {
                Console.WriteLine($"Checked edge now: {edge}");

                Vertex? secondVertex = edge.GetVertixOppositeTo(currentVertex);
                Console.WriteLine($"Second Vertex is: {secondVertex}");

                //if new discovered way is better --> update cost of second vertex
                if (currentVertex.CurrentWeightInGraph + edge.Weight < secondVertex?.CurrentWeightInGraph
                    || secondVertex!.CurrentWeightInGraph == 0) // if vertex costs 0 -> it hasn't been discovered
                {
                    secondVertex.CurrentWeightInGraph = currentVertex.CurrentWeightInGraph + edge.Weight;
                    
                    LinkedList<char> newRoad = new(currentVertex.CurrentRoadInGraph);
                    newRoad.AddLast(secondVertex.Sign);
                    secondVertex.CurrentRoadInGraph = newRoad;
                    
                    Console.WriteLine($"Updating cost, now at {secondVertex.Sign}: {secondVertex.CurrentWeightInGraph}");
                    Console.WriteLine($"Road to {secondVertex.Sign}: {Functions.WayListToString(secondVertex.CurrentRoadInGraph)}");
                }

                _checkedEdges.Add(edge);
                queue.Enqueue(secondVertex);

            }
            Console.WriteLine();
        }

        Console.WriteLine("End of algorithm");
        _checkedEdges.Clear();

        Console.WriteLine($"Road from {start.Sign} to {end.Sign}: {Functions.CollectionToString(end.CurrentRoadInGraph)}");
        
        return new(end.CurrentWeightInGraph, end.CurrentRoadInGraph.ToList());
    }


    private IEnumerable<Edge> GetEdgesForVertex(Vertex vertex)
    {
        return Edges.Where(edge =>
                edge.Vertix1.Sign == vertex.Sign || (edge.Vertix2.Sign == vertex.Sign && !edge.Oriented))
            .Except(_checkedEdges, new EdgeComparer())
            .OrderBy(edge => edge.Weight);
    }


    private Vertex GetVertex(char sign)
    {
        return Vertices.FirstOrDefault(v => v.Sign == sign)!;
    }


    public static int GetIndexFromSign(char sign) => sign - 'A';

    public static char GetSignFromIndex(int index) => (char)(index + 'A');

    private Matrix CreateAdjacencyMatrix()
    {

        List<List<double>> cont = [];

        for (int i = 0; i < Vertices.Count(); i++)
        {
            List<double> row = [];

            for (int j = 0; j < Vertices.Count(); j++)
                row.Add(0);

            cont.Add(row);
        }

        foreach (Edge edge in Edges)
        {
            cont[GetIndexFromSign(edge.Vertix1.Sign)][GetIndexFromSign(edge.Vertix2.Sign)] = edge.Weight;

            if (!edge.Oriented)
                cont[GetIndexFromSign(edge.Vertix2.Sign)][GetIndexFromSign(edge.Vertix1.Sign)] = edge.Weight;
        }

        return new Matrix(cont);
    }


    private Matrix CreateDistanceMatrix(WaysTable table, Matrix adjacencyMatrix)
    {
        Console.WriteLine("adjacencyMatrix");
        Console.WriteLine(adjacencyMatrix);

        Matrix delta = _graphOperations.CreateFirstDistanceMatrix(adjacencyMatrix);
        Console.WriteLine("Distance 0:");
        Console.WriteLine(delta);

        for (int i = 0; i < Vertices.Count(); i++)
        {
            delta = _graphOperations.CreateDistanceMatrix(delta, i, table);
            Console.WriteLine("Distance " + i);
            Console.WriteLine(delta);
        }
        
        table.FinishTable();
        Console.WriteLine("Ways table:");
        Console.WriteLine(table);
        
        return delta;
    }


    public WayToPoint FloydWarshallAlgorithm(Vertex vertex1, Vertex vertex2)
    {
        Matrix adjacencyMatrix = CreateAdjacencyMatrix();
        WaysTable table = new(this, adjacencyMatrix);
        
        Matrix distanceMatrix = CreateDistanceMatrix(table, adjacencyMatrix);

        Console.WriteLine(distanceMatrix);
        
        int index1 = GetIndexFromSign(vertex1.Sign);
        int index2 = GetIndexFromSign(vertex2.Sign);

        return new WayToPoint(distanceMatrix[index1, index2], table[index1, index2]);
    }


    public IEnumerable<Destination> GetListOfShortest(Vertex start, TypeOfAlgorithm typeOfAlgorithm)
    {
        List<Destination> list = [];
        
        if (typeOfAlgorithm == TypeOfAlgorithm.DjikstraAlgorithm)
        {
            foreach (Vertex vertex in Vertices.Except(new Vertex[] { start }, new VertexComparer()))
            {
                var distance = DijkstraAlgorithm(start, vertex);

                list.Add(new Destination(vertex.Sign, vertex.Name, distance));
            }
        }
        else if (typeOfAlgorithm == TypeOfAlgorithm.FloydWarshallAlgorithm)
        {
            Matrix adjacencyMatrix = CreateAdjacencyMatrix();
            WaysTable table = new(this, adjacencyMatrix);
            List<double> distances = CreateDistanceMatrix(table, adjacencyMatrix)[GetIndexFromSign(start.Sign)];

            for (int i = 0; i < Vertices.Count(); i++)
            {
                char signOfOther = GetSignFromIndex(i);
                
                if (start.Sign == signOfOther)
                    continue;
                
                List<char> way = table[GetIndexFromSign(start.Sign), i];

                list.Add(new Destination(signOfOther, this[signOfOther].Name, new WayToPoint(distances[i], way)));
            }
        }

        return list.OrderBy(d => d.Way.Distance);
    }


    public void ResetVertexWeight()
    {
        foreach (Vertex vertex in Vertices)
        {
            vertex.CurrentWeightInGraph = 0;
        }
    }
    
    private void InitNeighbourVerticesDictionary()
    {
        NeighbourVertices = new();
        
        foreach (Vertex vertex in Vertices)
        {
            List<char> neighbours = new();
            
            foreach (Edge edge in Edges)
            {
                if (edge.Vertix1.Sign == vertex.Sign)
                    neighbours.Add(edge.Vertix2.Sign);
                else if (edge.Vertix2.Sign == vertex.Sign && !edge.Oriented)
                    neighbours.Add(edge.Vertix1.Sign);
            }
            
            NeighbourVertices.Add(vertex.Sign, neighbours);
        }
    }
    
    
    public bool CheckIfVerticesAreNeighbours(char sign1, char sign2) => NeighbourVertices![sign1].Contains(sign2);

    
    public Edge GetEdge(char sign1, char sign2)
    {
        return Edges.First(e => e.Vertix1.Sign == sign1 && e.Vertix2.Sign == sign2 
                                || e.Vertix1.Sign == sign2 && e.Vertix2.Sign == sign1);
    }
}

// 0,779 km // F J I