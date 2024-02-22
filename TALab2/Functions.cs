using NumSharp;
using NumSharp.Generic;

namespace TALab2;

public class Functions
{
    public Functions() {}

    public static List<List<double>> ConvertNdArrayToList(NDArray array)
    {
        List<List<double>> res = [];

        for (int i = 0; i < array.shape[0]; i++)
        {
            List<double> row = new List<double>();
            for (int j = 0; j < array.shape[1]; j++)
            {
                row.Add(array[i, j]);
            }
            res.Add(row);
        }

        return res;
    }


    public static void PrintList<T>(List<T> list)
    {
        foreach (T obj in list)
        {
            Console.WriteLine(obj);
        }
    }


    public static string EnumerableToString<T>(IEnumerable<T> collection)
    {
        string result = "";

        foreach (T obj in collection)
        {
            result += obj!.ToString() + '\n';
        }

        return result;
    }
}