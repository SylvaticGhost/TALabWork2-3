using System.Collections;
using TALab2.Comparer;
using System;

namespace TALab2;

public class Graph
{
    public IEnumerable<Vertex> Vertices { get; set; }

    public IEnumerable<Edge> Edges { get; set; }

    private readonly GraphOperations _graphOperations;
    private readonly List<Edge> _checkedEdges;
    public Vertex this[char sign] => GetVertex(sign);

    public Graph(IEnumerable<Vertex> vertices, IEnumerable<Edge> edges)
    {
        Vertices = vertices.OrderBy(v => v.Sign);
        Edges = edges;
        _graphOperations = new();
        _checkedEdges = [];
    }


    public double DijkstraAlgorithm(Vertex start, Vertex end)
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
                    Console.WriteLine($"Road to {secondVertex.Sign}: {Functions.LinkedListToString(secondVertex.CurrentRoadInGraph)}");
                }

                _checkedEdges.Add(edge);
                queue.Enqueue(secondVertex);

            }
            Console.WriteLine();
        }

        Console.WriteLine("End of algorithm");
        _checkedEdges.Clear();

        Console.WriteLine($"Road from {start.Sign} to {end.Sign}: {Functions.CollectionToString(end.CurrentRoadInGraph)}");
        
        return end.CurrentWeightInGraph;
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


    private Matrix CreateDistanceMatrix()
    {
        Matrix adjacencyMatrix = CreateAdjacencyMatrix();
        WaysTable table = new(this, adjacencyMatrix);
        
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

        Console.WriteLine("Ways table:");
        Console.WriteLine(table);
        
        return delta;
    }


    public double FloydWarshallAlgorithm(Vertex vertex1, Vertex vertex2)
    {
        InitStartWayInVertex();
        
        List<List<LinkedList<char>>> ways = new List<List<LinkedList<char>>>();
        
        Matrix distanceMatrix = CreateDistanceMatrix();

        Console.WriteLine(distanceMatrix);

        return distanceMatrix[GetIndexFromSign(vertex1.Sign)][GetIndexFromSign(vertex2.Sign)];
    }


    public IEnumerable<Destination> GetListOfShortest(Vertex start, TypeOfAlgorithm typeOfAlgorithm)
    {
        List<Destination> list = [];
        
        if (typeOfAlgorithm == TypeOfAlgorithm.DjikstraAlgorithm)
        {
            foreach (Vertex vertex in Vertices.Except(new Vertex[] { start }, new VertexComparer()))
            {
                double distance = DijkstraAlgorithm(start, vertex);

                list.Add(new Destination(vertex.Sign, vertex.Name, distance));
            }
        }
        else if (typeOfAlgorithm == TypeOfAlgorithm.FloydWarshallAlgorithm)
        {
            List<double> distances = CreateDistanceMatrix()[GetIndexFromSign(start.Sign)];

            for (int i = 0; i < Vertices.Count(); i++)
            {
                char signOfOther = GetSignFromIndex(i);
                
                if (start.Sign == signOfOther)
                    continue;

                list.Add(new Destination(signOfOther, this[signOfOther].Name, distances[i]));
            }
        }

        return list.OrderBy(d => d.Distance);
    }


    public void ResetVertexWeight()
    {
        foreach (Vertex vertex in Vertices)
        {
            vertex.CurrentWeightInGraph = 0;
        }
    }

    
    private void InitStartWayInVertex()
    {
        for(char c = 'A'; c < 'A' + Vertices.Count(); c++)
        {
            Vertex vertex = this[c];
            LinkedList<char> road = [];
            
            road.AddLast(vertex.Sign);
            vertex.CurrentRoadInGraph = road;
        }
    }

}