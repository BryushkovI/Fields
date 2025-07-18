using System.Drawing;

namespace Fields.Model
{
    public class Field
    {
        public Locations _locations;

        public int Id { get; init; }
        public string Name { get; init; }
        
        public Locations Locations
        {
            get => _locations;
            set => _locations = value;
        }

        public decimal Size { get; set; }

        public void SetCenter(Point coordinates)
        {
            _locations.SetCenter(coordinates);
        }
    }
}
