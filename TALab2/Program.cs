using System.Globalization;
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
            graph.ResetVertexWeight();
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

char ChoosingAPoint()
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
    if (function == FunctionType.List)
    {
        IEnumerable<Destination> points =
            graph.GetListOfShortest(graph[signStart], algorithm);

        string list = Functions.CollectionToString(points);

        Console.WriteLine("List: \n" + list);
    }

    if (function == FunctionType.DistanceToPoint)
    {
        double distance;

        if (algorithm == TypeOfAlgorithm.DjikstraAlgorithm)
            distance = graph.DijkstraAlgorithm(graph[signStart], graph[secondSign]);
        else
            distance = graph.FloydWarshallAlgorithm(graph[signStart], graph[secondSign]);

        Console.WriteLine($"From {signStart} to {secondSign}\n distance = {distance}");
    }
}

