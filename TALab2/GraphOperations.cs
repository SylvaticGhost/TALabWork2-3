﻿using System.Numerics;

namespace TALab2;

public class GraphOperations
{

    public GraphOperations()
    {
    }

    public Matrix CreateFirstDistanceMatrix(Matrix adjacencyMatrix)
    {
        List<List<double>> cont = new List<List<double>>(adjacencyMatrix.Containing);


        for (int i = 0; i < adjacencyMatrix.NumRows; i++)
        {

            for (int j = 0; j < adjacencyMatrix.NumCols; j++)
            {
                if (adjacencyMatrix[i][j] <= 0)
                    cont[i][j] = double.PositiveInfinity;
            }
        }

        return new Matrix(cont);
    }


    public Matrix CreateDistanceMatrix(Matrix previousDistanceM, int vertex, WaysTable table)
    {
        Matrix distanceMatrix = Matrix.CreateEmptyMatrix(previousDistanceM.NumRows, previousDistanceM.NumCols);


        for (int i = 0; i < previousDistanceM.NumRows; i++)
        {
            for (int j = 0; j < previousDistanceM.NumCols; j++)
            {
                double a = previousDistanceM[i][vertex] + previousDistanceM[vertex][j];
                
                if (i == j) //reduce cases on diagonal as it shows distance between same points
                    distanceMatrix[i][j] = 0;
                
                //if a new distance is shorter than the previous one, update the matrix
                else if ((a < previousDistanceM[i][j] && previousDistanceM[i][j] >= 0) ||
                    (previousDistanceM[i][j] <= 0 && a > 0))
                {
                    distanceMatrix[i][j] = a;
                    table[i, j].Add(Graph.GetSignFromIndex(vertex));
                }
                else
                    distanceMatrix[i][j] = previousDistanceM[i][j];
            }
        }
        
        return distanceMatrix;
    }

    
    public static Dictionary<char, List<char>> DfsSearch(Graph graph, char start)
    {
        Dictionary<char, List<char>> tree = new Dictionary<char, List<char>>();
        List<char> visited = new List<char> { start };

        Stack<char> stack = new Stack<char>();
        stack.Push(start);

        var vertex = start;
        tree[vertex] = new List<char>();

        while (stack.Count > 0)
        {
            var next = graph.NeighbourVertices![vertex].FirstOrDefault(v => !visited.Contains(v));

            if (next != default(char))
            {
                visited.Add(next);
                stack.Push(next);
                tree[vertex].Add(next);
                vertex = next;
                if (!tree.ContainsKey(vertex))
                {
                    tree[vertex] = new List<char>();
                }
            }
            else
            {
                stack.Pop();
                if (stack.Count > 0)
                    vertex = stack.Peek();
            }
        }

        return tree;
    }
    
}