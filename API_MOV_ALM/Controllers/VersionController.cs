using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API_MOV_ALM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(Summary = "Obtiene version actual del aplicativo",
                  Description = "Devuelve la version que esta en produccion del app FAB083.")]
        public async Task<IActionResult> GetVersion()
        {
            return Ok(new { version = "1.0.4", url = "http://10.0.2.2:8088/app-debug.apk" });
        }
    }
}
