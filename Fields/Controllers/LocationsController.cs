using Aspose.Drawing;
using Aspose.Gis;
using Aspose.Gis.Geometries;
using Fields.Model;
using Fields.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Fields.Controllers
{
    [ApiController]
    public class LocationsController : ControllerBase
    {
        [HttpGet]
        [Route("[controller]")]
        public async Task<IResult> Index()
        {
            IEnumerable<Field> fields = DataProvider.ParseFieldsKML();

            return Results.Json(fields) ;
        }
    }
}
