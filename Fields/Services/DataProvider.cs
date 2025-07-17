using Aspose.Gis;
using Aspose.Gis.Geometries;
using Fields.Model;
using System.Globalization;
using System.Text;
namespace Fields.Services
{
    public class DataProvider
    {
        static string _fieldsPath = "C:\\Users\\Илья\\Downloads\\fields.kml";
        static public string ParseFieldsKML() 
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(_fieldsPath))))
            using (var layer = VectorLayer.Open(AbstractPath.FromStream(memoryStream), Drivers.Kml))
            {
                
                Console.WriteLine(layer.Count); // 2
                Console.WriteLine(layer[1].GetValue<string>("name")); // Mary
            }
            return "";
        }

        static public Field ParseFieldKML(int fid)
        {
            Field field;
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(_fieldsPath))))
            using (var layer = VectorLayer.Open(AbstractPath.FromStream(memoryStream), Drivers.Kml))
            {

                field = new()
                {
                    fid = layer[fid - 1].GetValue<int>("fid"),
                    Name = layer[fid - 1].GetValue<string>("name"),
                    Cordinates = []
                };
                var coordinates = layer[fid - 1].Geometry.ToString();
                coordinates = coordinates.Replace("POLYGON ((", "").Replace("))", "");

                string[] pairs = coordinates.Split(',');

                foreach (string pair in pairs)
                {
                    string[] coords = pair.Trim().Split(' ');

                    if (coords.Length == 2 &&
                        decimal.TryParse(coords[0], NumberStyles.Float, CultureInfo.InvariantCulture, out decimal lng) &&
                        decimal.TryParse(coords[1], NumberStyles.Float, CultureInfo.InvariantCulture, out decimal lat))
                    {
                        field.Cordinates.Add((lat, lng));
                    }
                }
            }
            return field;
        }
    }
}
