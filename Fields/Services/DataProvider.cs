using Aspose.Gis;
using Aspose.Gis.Geometries;
using Fields.Model;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
namespace Fields.Services
{
    public class DataProvider
    {
        static string _fieldsPath = ".\\wwwroot\\resource\\fields.kml";

        static string _centersPath = ".\\wwwroot\\resource\\centroids.kml";

        static public IList<Field> ParseFieldsKML() 
        {
            IList<Field> fields = [];
            Field field;
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(_fieldsPath))))
            using (var layer = VectorLayer.Open(AbstractPath.FromStream(memoryStream), Drivers.Kml))
            {
                foreach (var item in layer)
                {
                    field = new()
                    {
                        Id = item.GetValue<int>("fid"),
                        Name = item.GetValue<string>("name"),
                        Size = item.GetValue<int>("size"),
                        Locations = new()
                        {
                            Polygon = []
                        },
                    };

                    var coordinates = item.Geometry.ToString();
                    coordinates = coordinates.Replace("POLYGON ((", "").Replace("))", "");

                    string[] pairs = coordinates.Split(',');
                    foreach (string pair in pairs)
                    {
                        string[] coords = pair.Trim().Split(' ');

                        if (coords.Length == 2 &&
                            decimal.TryParse(coords[0], NumberStyles.Float, CultureInfo.InvariantCulture, out decimal lng) &&
                            decimal.TryParse(coords[1], NumberStyles.Float, CultureInfo.InvariantCulture, out decimal lat))
                        {
                            field.Locations.Polygon.Add(new()
                            {
                                Lat = lat,
                                Lng = lng
                            });
                        }
                    }

                    fields.Add(field);
                }
                
            }

            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(_centersPath))))
            using (var layer = VectorLayer.Open(AbstractPath.FromStream(memoryStream), Drivers.Kml))
            {
                foreach (var item in layer)
                {
                    field = fields.FirstOrDefault(e => e.Id == item.GetValue<int>("fid"));
                    var coordinates = item.Geometry.ToString();
                    coordinates = coordinates.Replace("POINT (", "").Replace(")", "");
                    string[] coords = coordinates.Trim().Split(' ');
                    if (coords.Length == 2 &&
                            decimal.TryParse(coords[0], NumberStyles.Float, CultureInfo.InvariantCulture, out decimal lng) &&
                            decimal.TryParse(coords[1], NumberStyles.Float, CultureInfo.InvariantCulture, out decimal lat))
                    {
                        field.SetCenter(new()
                        {
                            Lat = lat,
                            Lng = lng
                        });
                    }
                }
                
            }


            return fields;
        }

        static public Field ParseFieldKML(int fid)
        {
            Field field;
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(_fieldsPath))))
            using (var layer = VectorLayer.Open(AbstractPath.FromStream(memoryStream), Drivers.Kml))
            {

                field = new()
                {
                    Id = layer[fid - 1].GetValue<int>("fid"),
                    Name = layer[fid - 1].GetValue<string>("name"),
                    Size = layer[fid - 1].GetValue<int>("size"),
                    Locations = new()
                    {
                        Polygon = []
                    },
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
                        field.Locations.Polygon.Add(new()
                        {
                            Lat = lat,
                            Lng = lng
                        });
                    }
                }
            }

            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(_centersPath))))
            using (var layer = VectorLayer.Open(AbstractPath.FromStream(memoryStream), Drivers.Kml))
            {
                var coordinates = layer[fid - 1].Geometry.ToString();
                coordinates = coordinates.Replace("POINT (", "").Replace(")", "");
                string[] coords = coordinates.Trim().Split(' ');
                if (coords.Length == 2 &&
                        decimal.TryParse(coords[0], NumberStyles.Float, CultureInfo.InvariantCulture, out decimal lng) &&
                        decimal.TryParse(coords[1], NumberStyles.Float, CultureInfo.InvariantCulture, out decimal lat))
                {
                    field.SetCenter(new()
                    {
                        Lat = lat,
                        Lng = lng
                    });
                }
            }
            
            return field;
        }
    }
}
