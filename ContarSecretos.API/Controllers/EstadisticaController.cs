
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EstadisticaController : ControllerBase {

    public EstadisticaController()
    {
        
    }

    [HttpPost]
    [Route("DescargoAudioLibro")]
    public async Task<IActionResult> DescargoAudioLibro([FromQuery] int id){
        
        return Ok(id);
    }
}