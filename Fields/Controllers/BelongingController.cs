using Microsoft.AspNetCore.Mvc;

namespace Fields.Controllers
{
    [ApiController]
    public class BelongingController : ControllerBase
    {
        [HttpGet]
        [Route("[controller]")]
        public async Task<string> Index(decimal? lat, decimal? lng, int id, string name)
        {
            return "View()";
        }
    }
}
