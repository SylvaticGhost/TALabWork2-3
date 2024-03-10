using TALab2;

namespace TestProject1;

public static class TestHelpers
{
    public static bool CompareList(IReadOnlyList<Destination> list1, IReadOnlyList<Destination> list2, double calculationError)
    {
        if (list1.Count() == list2.Count())
        {
            for (int i = 0; i < list1.Count(); i++)
            {
                if (!(Math.Abs(list1[i].Way.Distance - list2[i].Way.Distance) < calculationError) || list1[i].Sign != list2[i].Sign)
                    return false;
            }

            return true;
        }
        
        return false;
    }
    
    
    public static char[] GetCaseForListTest()
    {
        List<char> testCases = [];
    
        for(char c = 'A'; c <= 'K'; c++)
        {
            testCases.Add(c);
        }

        return testCases.ToArray();
    }
    
    
    
}