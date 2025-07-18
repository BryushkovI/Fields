using Aspose.Gis.Geometries;
using Microsoft.AspNetCore.Mvc;
using Fields.Model;
using Fields.Services;

namespace Fields.Controllers
{
    [ApiController]
    public class DistanceController : ControllerBase
    {
        [HttpGet]
        [Route("[controller]")]
        public async Task<double> Get(decimal lat, decimal lng, int id )
        {
            double dist = Model.Geometry.Distance2Points(new Model.Point()
            {
                Lat = lat,
                Lng = lng
            }, (DataProvider.ParseFieldKML(id).Locations.Center));
            return dist;
        }
    }
}
