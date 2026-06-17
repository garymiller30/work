using System.Text.Json.Serialization;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    /// <summary>
    /// Point double
    /// </summary>
    public sealed class PointD
    {
        [JsonConstructor]
        public PointD(double x,double y)
        {
            X = x; Y = y;
        }
        public PointD()
        {
            
        }

        public  double X { get; set; }
        public  double Y { get; set; }
    }
}
