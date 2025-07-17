using System.Drawing;

namespace Fields.Model
{
    public class Field
    {
        public int fid { get; init; }
        public string Name { get; init; }
        public IList<(decimal lat,decimal lng)> Cordinates { get; init; }
        public (decimal, decimal) CentrCordinates { get; init; }
        public decimal Area { get; set; }
    }
}
