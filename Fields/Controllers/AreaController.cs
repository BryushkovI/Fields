using Fields.Model;
using Fields.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fields.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AreaController : ControllerBase
    {
        
        [HttpGet]
        public async Task<double> Get(int id)
        {

            return Geometry.AreaInMetrs(DataProvider.ParseFieldKML(id).Locations.Polygon);
        }
    }
}
