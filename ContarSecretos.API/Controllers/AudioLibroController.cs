using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AudioLibroController : ControllerBase {

    private readonly IAudioLibroService _audioLibroService;

    public AudioLibroController (IAudioLibroService audioLibroService){
        _audioLibroService = audioLibroService;
    }

    [HttpGet]
    [Route("GetById")]
    [AllowAnonymous]
    public async Task<ActionResult> GetById(int id){
        var audioLibro = await _audioLibroService.FindById(id);
        return Ok(audioLibro);
    }

    [HttpGet]
    [Route("GetAll")]
    [AllowAnonymous]
    public async Task<ActionResult> GetAll(){
        var audioLibros = await _audioLibroService.GetAll();
        return Ok(audioLibros);
    }
   
    [HttpPost]
    [Route("GetAllFilter")]
    public async Task<ActionResult> GetAllFilter(RequestFilterAudioLibroDTO requestFilterAudioLibroDTO){
        var audioLibros = await _audioLibroService.GetAllFilter(requestFilterAudioLibroDTO);
        return Ok(audioLibros);
    }

    [HttpPost]
    [Route("AddAudioLibro")]
    public async Task<IActionResult> AddAudioLibro(RequestAudioLibroDTO audioLibro){
        var response = await  _audioLibroService.AddAudioLibro(audioLibro);
        return Ok(response);
    }
    
    [HttpPut]
    [Route("UpdateAudioLibro")]
    public async Task<IActionResult> UpdateAudioLibro(RequestAudioLibroDTO audioLibro){
        var response = await  _audioLibroService.UpdateAudioLibro(audioLibro);
        return Ok(response);
    }

    [HttpDelete]
    [Route("DeleteAudioLibro")]
    public async Task<IActionResult> DeleteAudioLibro(int id){
        BaseMessage<ResponseAudioLibroDTO> responseGet = await _audioLibroService.FindById(id);
        if (responseGet.StatusCode != HttpStatusCode.OK){
            return Ok(responseGet);
        }
        var respuesta = await _audioLibroService.DeleteAudioLibro(id);
        return Ok(respuesta);
    }
}