using Fields.Model;
using Microsoft.AspNetCore.Mvc;

namespace Fields.Controllers
{
    [ApiController]
    public class LocationsController : ControllerBase
    {
        [HttpGet]
        [Route("[controller]")]
        public async Task<IResult> Index()
        {
            IEnumerable<Field> fields = [];
            return Results.Json(fields) ;
        }
    }
}
