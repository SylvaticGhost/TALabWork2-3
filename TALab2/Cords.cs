using System.Globalization;

namespace TALab2;

public class Cords
{
    /// <summary>
    ///  Latitude describes the north-south position on the globe
    /// </summary>
    public double Latitude { get; }
    /// <summary>
    /// longitude describes the east-west position.
    /// </summary>
    public double Longitude { get; }
    
    private double R = 6371e3; // Earth's radius in meters

    public Cords(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public Cords(string cordInString)
    {
        (double, double) c = GetCordsFromString(cordInString);

        Latitude = c.Item1;
        Longitude = c.Item2;
    }
    
    
    /// <summary>
    /// Calculate the distance from source point to passed
    /// </summary>
    /// <param name="cords2">Cords of the destination</param>
    /// <returns>The distance in km</returns>
    public double CalculateDistanceToPoint(Cords cords2)
    {
        double dLat = (cords2.Latitude - Latitude) * Math.PI / 180;
        double dLon = (cords2.Longitude - Longitude) * Math.PI / 180;
        
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(Math.PI / 180 * Latitude) * Math.Cos(Math.PI / 180 * cords2.Latitude) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        
        double c = 2 * Math.Asin(Math.Sqrt(a));

        return (R * c)/1000;
    }

    
    public double CalculateDistanceToPointThrowOther(Cords cords2, params Cords[] intermediateCords)
    {
        double result = 0;

        result += new Cords(Latitude, Longitude).CalculateDistanceToPoint(intermediateCords.First());

        for (int i = 1; i < intermediateCords.Length - 1; i++)
        {
            result += intermediateCords[i - 1].CalculateDistanceToPoint(intermediateCords[i]);
        }

        result += intermediateCords.Last().CalculateDistanceToPoint(cords2);

        return result;
    }


    public override string ToString()
    {
        return Latitude.ToString() + ' ' + Longitude;
    }


    private (double, double) GetCordsFromString(string s)
    {
        CultureInfo culture = CultureInfo.GetCultureInfo("de-DE");
        
        string[] sep = s.Split(", ");
        
        return (
            double.Parse(sep.First().Replace('.', ','), culture), 
            double.Parse(sep.Last().Replace('.', ','), culture));
    }
}