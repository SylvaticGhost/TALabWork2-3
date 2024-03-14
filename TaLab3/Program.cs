// See https://aka.ms/new-console-template for more information

using TALab2;
using TaLab3;

DataProvider dataProvider = new();

Graph graph = new(dataProvider.Vertixes, dataProvider.OrientedEdges);

List<List<char>> sccs = SccAlgorithm.TarjanAlgorithm(graph);

Console.WriteLine("Graph:");
Console.WriteLine(graph);

Console.WriteLine(Functions.SccListToString(sccs));

Console.WriteLine("Transposed graph:");
graph.Transpose();
Console.WriteLine(graph);


SccAlgorithm.AlgorithmCCS(graph);