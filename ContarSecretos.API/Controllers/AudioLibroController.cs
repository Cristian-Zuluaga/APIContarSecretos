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

    /*
    [HttpPost]
    [Route("GetAllFilter")]
    public async Task<ActionResult> GetAllFilter(RequestFilterAutorDTO requestFilterAutorDTO){
        var autores = await _autorService.GetAllFilter(requestFilterAutorDTO);
        return Ok(autores);
    }

    */
    [HttpPost]
    [Route("AddAudioLibro")]
    public async Task<IActionResult> AddAudioLibro(AudioLibro audioLibro){
        var response = await  _audioLibroService.AddAudioLibro(audioLibro);
        return Ok(response);
    }
    
    [HttpPut]
    [Route("UpdateAudioLibro")]
    public async Task<IActionResult> UpdateAudioLibro(AudioLibro audioLibro){
        var response = await  _audioLibroService.UpdateAudioLibro(audioLibro);
        return Ok(response);
    }
    /*
    
    [HttpDelete]
    [Route("DeleteAutor")]
    public async Task<IActionResult> DeleteAutor(int id){
        BaseMessage<Autor> responseGet = await _autorService.FindById(id);
        if (responseGet.StatusCode != HttpStatusCode.OK){
            return Ok(responseGet);
        }

        Autor autor = responseGet.ResponseElements.FirstOrDefault();
        var respuesta = await _autorService.UpdateStateAutor(autor);
        return Ok(respuesta);
    }
    */
}