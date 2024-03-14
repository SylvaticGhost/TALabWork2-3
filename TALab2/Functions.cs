using System.Globalization;
using NumSharp;
using NumSharp.Generic;

namespace TALab2;

public static class Functions
{
    public static void PrintList<T>(List<T> list)
    {
        foreach (T obj in list)
        {
            Console.WriteLine(obj);
        }
    }


    public static string CollectionToString<T>(IEnumerable<T> collection)
    {
        string result = "";

        foreach (T obj in collection)
        {
            result += obj!.ToString() + ' ';
        }
        result += '\n';
        return result;
    }
    
    
    public static string WayListToString<T>(IEnumerable<T> collection)
    {
        string result = "";
        

        foreach (T obj in collection)
        {
            result += obj!.ToString() + " -> ";
        }
        
        if(result.Length >= 4)
            result = result[..^4];

        return result;
    }


    public static string SccListToString(List<List<char>> collection)
    {
        string result = "";
        
        for(int i = 0; i < collection.Count; i++)
        {
            result += $"SCC {i + 1}: {WayListToString(collection[i])}\n";
        }
        
        return result;
    }
}