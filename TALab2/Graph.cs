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
    public Vertex this[char sign] => GetVertix(sign);

    public Graph(IEnumerable<Vertex> vertices, IEnumerable<Edge> edges)
    {
        Vertices = vertices.OrderBy(v => v.Sign);
        Edges = edges;
        _graphOperations = new();
        _checkedEdges = [];
    }


    public double DjikstraAlgorithm(Vertex start, Vertex end)
    {
        Queue<Vertex> queue = new();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            Vertex currentVertex = queue.Dequeue();

            if (currentVertex.Sign == end.Sign)
                continue;

            Console.WriteLine($"Current vertex: {currentVertex}, cost: {currentVertex.CurrentWeightInGraph}");

            IEnumerable<Edge> availableEdges = GetEdgesForVertix(currentVertex);
            Console.Write($"Available edges for {currentVertex.Sign}: ");

            foreach (Edge e in availableEdges)
            {
                Console.Write($" {e},");
            }

            Console.WriteLine();

            foreach (Edge edge in availableEdges)
            {
                Console.WriteLine($"Checked edge now: {edge}");

                Vertex? secondVertix = edge.GetVertixOppositeTo(currentVertex);
                Console.WriteLine($"Second Vertex is: {secondVertix}");

                //if new discovered way is better update cost of second vertex
                if (currentVertex.CurrentWeightInGraph + edge.Weight < secondVertix?.CurrentWeightInGraph
                    || secondVertix.CurrentWeightInGraph == 0) // if vertex costs 0 - it hasn't been discovered
                {
                    secondVertix.CurrentWeightInGraph = currentVertex.CurrentWeightInGraph + edge.Weight;
                    Console.WriteLine($"Updating cost, now at {secondVertix.Sign}: {secondVertix.CurrentWeightInGraph}");
                }

                _checkedEdges.Add(edge);
                queue.Enqueue(secondVertix);

            }
            Console.WriteLine();
        }

        Console.WriteLine("End of algorithm");
        _checkedEdges.Clear();
        return end.CurrentWeightInGraph;
    }


    private IEnumerable<Edge> GetEdgesForVertix(Vertex vertex)
    {
        return Edges.Where(edge =>
                edge.Vertix1.Sign == vertex.Sign || (edge.Vertix2.Sign == vertex.Sign && !edge.Oriented))
            .Except(_checkedEdges, new EdgeComparer())
            .OrderBy(edge => edge.Weight);
    }


    private Vertex GetVertix(char sign)
    {
        return Vertices.Where(v => v.Sign == sign).FirstOrDefault();
    }


    private static int GetIndexFromSign(char sign) => sign - 'A';

    private static char GetSignFromIndex(int index) => (char)(index + 'A');

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
        Console.WriteLine("adjacencyMatrix");
        Console.WriteLine(adjacencyMatrix);

        Matrix delta = _graphOperations.CreateFirstDistanceMatrix(adjacencyMatrix);
        Console.WriteLine("Distance 0:");
        Console.WriteLine(delta);

        for (int i = 0; i < Vertices.Count(); i++)
        {
            delta = _graphOperations.CreateDistanceMatrix(delta, i);
            Console.WriteLine("Distance " + i);
            Console.WriteLine(delta);
        }

        return delta;
    }


    public double FloydWarshallAlgorithm(Vertex vertex1, Vertex vertex2)
    {
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
                double distance = DjikstraAlgorithm(start, vertex);

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

}