using Aspose.Drawing;
using Aspose.Gis;
using Fields.Model;
using Fields.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fields.Controllers
{
    [ApiController]
    public class BelongingController : ControllerBase
    {
        [HttpGet]
        [Route("[controller]")]
        public async Task<string> Get(decimal lat, decimal lng)
        {
            var fields = DataProvider.ParseFieldsKML();
            foreach (var item in fields)
            {
                if (Geometry.IsInnerPoint(item.Locations.Polygon, lat, lng))
                {
                    return $"{item.Id}, {item.Name}";
                }
            }
            return false.ToString();
        }
    }
}
