using Microsoft.AspNetCore.Mvc;

namespace Fields.Controllers
{
    [ApiController]
    public class DistanceController : ControllerBase
    {
        [HttpGet]
        [Route("[controller]")]
        public async Task<decimal> Get(decimal? lat, decimal? lng, int id )
        {
            return 456;
        }
    }
}
