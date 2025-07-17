namespace Fields.Model
{
    public class Polygon
    {
        public IList<(double, double, double)> Points { get; set; }

        public IList<(double, double, double)> Vectors { get; set; }

        public Polygon(IList<(double, double, double)> points)
        {
            Points = points;
        }
    }
}
