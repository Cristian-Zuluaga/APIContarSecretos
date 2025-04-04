
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EstadisticaController : ControllerBase {

    private readonly IEstadisticaService _estadisticaService;

    public EstadisticaController (IEstadisticaService estadisticaService){
        _estadisticaService = estadisticaService;
    }

    [HttpPost]
    [Route("DescargoAudioLibro")]
    public async Task<IActionResult> DescargoAudioLibro([FromQuery] int idAudioLibro){
        
        var estadisticaBD = await _estadisticaService.GetEstadistica(idAudioLibro,null);

        if(estadisticaBD == null || estadisticaBD.ResponseElements.Count == 0)
        {
            return NotFound();
        }

        Estadistica estadistica = estadisticaBD.ResponseElements.First();

        estadistica.CountDescargas = estadistica.CountDescargas + 1;

        var response = await _estadisticaService.UpdateEstadistica(estadistica);

        return Ok(response);
    }

    [HttpPost]
    [Route("EscuchoAudioLibro")]
    public async Task<IActionResult> EscuchoAudioLibro([FromQuery] int idAudioLibro){
        
        var estadisticaBD = await _estadisticaService.GetEstadistica(idAudioLibro,null);

        if(estadisticaBD == null || estadisticaBD.ResponseElements.Count == 0)
        {
            return NotFound();
        }

        Estadistica estadistica = estadisticaBD.ResponseElements.First();

        estadistica.CountEscuchado = estadistica.CountEscuchado + 1;

        var response = await _estadisticaService.UpdateEstadistica(estadistica);

        return Ok(response);
    }

    [HttpPost]
    [Route("LeyoAudioLibro")]
    public async Task<IActionResult> LeyoAudioLibro([FromQuery] int idAudioLibro){
        
        var estadisticaBD = await _estadisticaService.GetEstadistica(idAudioLibro,null);

        if(estadisticaBD == null || estadisticaBD.ResponseElements.Count == 0)
        {
            return NotFound();
        }

        Estadistica estadistica = estadisticaBD.ResponseElements.First();

        estadistica.CountLeido = estadistica.CountLeido + 1;

        var response = await _estadisticaService.UpdateEstadistica(estadistica);

        return Ok(response);
    }







    [HttpPost]
    [Route("DescargoLibro")]
    public async Task<IActionResult> DescargoLibro([FromQuery] int idLibro){
        
        var estadisticaBD = await _estadisticaService.GetEstadistica(null,idLibro);

        if(estadisticaBD == null || estadisticaBD.ResponseElements.Count == 0)
        {
            return NotFound();
        }

        Estadistica estadistica = estadisticaBD.ResponseElements.First();

        estadistica.CountDescargas = estadistica.CountDescargas + 1;

        var response = await _estadisticaService.UpdateEstadistica(estadistica);

        return Ok(response);
    }

    [HttpPost]
    [Route("LeyoLibro")]
    public async Task<IActionResult> LeyoLibro([FromQuery] int idLibro){
        
        var estadisticaBD = await _estadisticaService.GetEstadistica(null,idLibro);

        if(estadisticaBD == null || estadisticaBD.ResponseElements.Count == 0)
        {
            return NotFound();
        }

        Estadistica estadistica = estadisticaBD.ResponseElements.First();

        estadistica.CountLeido = estadistica.CountLeido + 1;

        var response = await _estadisticaService.UpdateEstadistica(estadistica);

        return Ok(response);
    }


}