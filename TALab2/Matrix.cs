using System.Globalization;

namespace TALab2;

public class Matrix
{
    public List<List<double>> Containing { get; set; }
    /// <summary>
    /// Number of rows in the matrix
    /// </summary>
    public int NumRows { get; set; }
    
    /// <summary>
    /// Numbers of columns in the matrix
    /// </summary>
    public int NumCols { get; set; }

    public List<double> this[int i]
    {
        get => Containing[i];
        set => Containing[i] = value;
    }

    public double this[int i, int j]
    {
        get => Containing[i][j];
        set => Containing[i][j] = value;
    }
    private readonly Functions _functions;
    
    public Matrix(int r, int c)
    {
        NumCols = c;
        NumRows = r;
        _functions = new Functions();
    }


    public Matrix(List<List<double>> containing)
    {
        NumCols = containing.First().Count;
        NumRows = containing.Count;
        _functions = new Functions();
        Containing = containing;
    }


    public static Matrix CreateEmptyMatrix(int n, int m = 0)
    {
        if (m == 0)
            m = n;

        List<List<double>> cont = [];

        for (int i = 0; i < n; i++)
        {
            List<double> row = [];

            for (int j = 0; j < m; j++)
            {
                row.Add(0);
            }

            cont.Add(row);
        }

        return new Matrix(cont);
    }



    public override string ToString()
    {
        if (Containing is null || Containing.Count < 1)
        {
            return "Empty object type Matrix";
        }

        string result = "";

        foreach (List<double> row in Containing)
        {
            result = row.Aggregate(result, (current, d) => current + (Math.Round(d, 3).ToString(CultureInfo.CurrentCulture) + ' ' + '\t'));

            result += '\n';
        }

        return result;
    }
}