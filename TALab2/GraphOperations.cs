using System.Numerics;

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


    public Matrix CreateDistanceMatrix(Matrix previousDistanceM, int vertex)
    {
        Matrix distanceMatrix = Matrix.CreateEmptyMatrix(previousDistanceM.NumRows, previousDistanceM.NumCols);


        for (int i = 0; i < previousDistanceM.NumRows; i++)
        {
            for (int j = 0; j < previousDistanceM.NumCols; j++)
            {
                double a = previousDistanceM[i][vertex] + previousDistanceM[vertex][j];
                if (i == j)
                    distanceMatrix[i][j] = 0;
                else if ((a < previousDistanceM[i][j] && previousDistanceM[i][j] >= 0) ||
                    (previousDistanceM[i][j] <= 0 && a > 0))
                    distanceMatrix[i][j] = a;
                else
                    distanceMatrix[i][j] = previousDistanceM[i][j];
            }
        }

        return distanceMatrix;
    }
}