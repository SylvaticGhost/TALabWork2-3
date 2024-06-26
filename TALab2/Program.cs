﻿using System.Globalization;
using TALab2;
CultureInfo culture = CultureInfo.GetCultureInfo("de-DE");


DataProvider dataProvider = new DataProvider();

Graph graph =  new Graph(dataProvider.Vertixes, dataProvider.Edges);

char signStart;

char secondSign = ' ';
TypeOfAlgorithm algorithm;
FunctionType function;


do
{
    signStart = ChoosingAPoint();
    function = ChooseFunction();

    if (function is FunctionType.DistanceToPoint)
    {
        Console.WriteLine("Choose second point");
        secondSign = ChoosingAPoint();
    }

    algorithm = ChoseAlgorithm();

    Calculation();

} while (EndOrContinue());



bool EndOrContinue()
{
    while (true)
    {
        Console.WriteLine("Choose if you want to continue Y/N");

        string? input = Console.ReadLine();

        if (input == "Y")
        {
            Console.WriteLine("\n Continue... \n");
            graph.ResetVertices();
            return true;
        }

        if (input == "N")
        {
            Console.WriteLine("Good bye!");
            return false;
        }

        Console.WriteLine("Incorrect input");
    }
}


char ChoosingAPoint() //starting block in schem
{
    while (true)
    {
        Console.WriteLine("Available points:");
        Functions.PrintList(dataProvider.Vertixes);
        Console.WriteLine();

        Console.Write("Choose a point: ");
        string? input = Console.ReadLine();

        if (char.TryParse(input, out char result))
        {
            if (result > dataProvider.Vertixes.Last().Sign || result < dataProvider.Vertixes.First().Sign)
            {
                Console.WriteLine("This sign aren't exist");
            }
            else
            {
                Console.WriteLine($"You've chosen a point: {graph[result]}");
                return result;

            }
        }
        else
            Console.WriteLine("Is not a sign");
    }
}


FunctionType ChooseFunction()
{
    while (true)
    {
        Console.WriteLine("Chose the function: \n 1) Calculate distance to the specific point \n 2) Get list of closest point to your");

        Console.Write("Type:");
        string? input = Console.ReadLine();

        switch (input)
        {
            case "1":
                return FunctionType.DistanceToPoint;
            case "2":
                return FunctionType.List;
            default:
                Console.WriteLine("Invalid input");
                break;
        }
    }
}


TypeOfAlgorithm ChoseAlgorithm()
{
    while (true)
    {
        Console.WriteLine("Write a algorithm, what you want to use\n 1) Djikstra \n 2)Floyd-Warshall");
        Console.Write("Type:");
        string? input = Console.ReadLine();

        if (input == "1")
        {
            return TypeOfAlgorithm.DjikstraAlgorithm;

        }

        if (input == "2")
        {
           return TypeOfAlgorithm.FloydWarshallAlgorithm;

        }

        Console.WriteLine("Invalid input");
    }
}


void Calculation()
{
    if (function == FunctionType.List) // list of shortest destination
    {
        IEnumerable<Destination> points =
            graph.GetListOfShortest(graph[signStart], algorithm);
        
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"List of closest points to : {graph[signStart].Name}");
        Console.ResetColor();
        Console.WriteLine();
        
        foreach (var point in points)
        {
            Console.WriteLine(point.Way.ToStringLong(graph));
            Console.WriteLine();
        }
    }

    if (function == FunctionType.DistanceToPoint)
    {
        WayToPoint way;

        if (algorithm == TypeOfAlgorithm.DjikstraAlgorithm)
            way = graph.DijkstraAlgorithm(graph[signStart], graph[secondSign]);
        else
            way = graph.FloydWarshallAlgorithm(graph[signStart], graph[secondSign]);

        Console.WriteLine(way.ToStringLong(graph));
    }
}

