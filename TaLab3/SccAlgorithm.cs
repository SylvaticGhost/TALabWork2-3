using System.Text;
using TALab2;

namespace TaLab3;

public static class SccAlgorithm
{
    public static List<List<char>> TarjanAlgorithm(Graph graph)
    {
        List<List<char>> sccs = [];

        List<char> notVisited = graph.Vertices.Select(v => v.Sign).ToList();
        List<char> visitedAllNeighbours = [];

        while (notVisited.Count > 0)
        {
            Stack<Vertex> stack = new();
            stack.Push(graph[notVisited.First()]);
            do
            {
                var vertex = stack.Peek();

                var neighbours = graph.NeighbourVertices![vertex.Sign]
                    .Where(v => !stack.Contains(graph[v]) && notVisited.Contains(v)).ToList();

                notVisited.Remove(vertex.Sign);

                if (neighbours.Count == 0)
                {
                    visitedAllNeighbours.Add(vertex.Sign);
                    break;
                }
                else
                {
                    stack.Push(graph[neighbours.First()]);
                }


            } while (stack.Count > 0);

            sccs.Add(stack.Select(v => v.Sign).ToList());

        }

        return sccs;
    }


    public static List<List<char>> AlgorithmCCS(Graph graph)
    {
        var sccs1 = GraphOperations.DfsSearch(graph, graph.Vertices.First().Sign);

        graph.Transpose();

        var sccs2 = GraphOperations.DfsSearch(graph, graph.Vertices.First().Sign);

        int x = 0;
        
        var commonNodes = CommonNodes(sccs1, sccs2);

        return TarjanAlgorithm(graph);

    }
    
    
    private static List<char> CommonNodes(Dictionary<char, List<char>> sccs1, Dictionary<char, List<char>> sccs2)
    {
        List<List<char>> commonNodes = [[]];

        List<List<char>> sccs1List = [[sccs1.First().Key]];
        
        char previous = sccs1.First().Key;

        int i = 0;
        
        while (true)
        {
            var next = sccs1[previous].FirstOrDefault(v => sccs1List[i].Contains(v));
            
            if (next != default(char))
            {
                sccs1List[i].Add(next);
                previous = next;
            }
            else
            {
                i++;
                sccs1List.Add([]);
            }
        }
    }
    
    
    public static string PrintSccs(List<List<char>> sccs)
    {
        StringBuilder sb = new();
        
        foreach (List<char>? scc in sccs)
        {
            if(scc is { Count: > 0 })
            {
                sb.AppendLine(string.Join(", ", scc));
                sb.AppendLine("        ");
            }
        }

        sb.AppendLine();
        return sb.ToString();
    }

}