namespace Fields.Model
{
    public struct Locations
    {

        private IList<Point> _polygon;

        private Point _center;

        public IList<Point> Polygon
        {
            get => _polygon;
            set => _polygon = value;
        }
        public Point Center
        {
            get => _center;
            set => _center = value;
        }

        public void SetCenter(Point coordinates)
        {
            _center = coordinates;
        }
    }

    public struct Point
    {
        decimal _lat;
        decimal _lng;
        public decimal Lat
        {
            get => _lat;
            set => _lat = value;
        }

        public decimal Lng
        {
            get => _lng;
            set => _lng = value;
        }
    }    
}
