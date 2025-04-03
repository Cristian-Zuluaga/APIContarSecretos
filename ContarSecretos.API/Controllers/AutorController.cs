using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AutorController : ControllerBase {

    private readonly IAutorService _autorService;

    public AutorController (IAutorService autorService){
        _autorService = autorService;
    }

    [HttpGet]
    [Route("GetById")]
    public async Task<ActionResult> GetById(int id){
        var autor = await _autorService.FindById(id);
        return Ok(autor);
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<ActionResult> GetAll(){
        var autores = await _autorService.GetAll();
        return Ok(autores);
    }

    [HttpPost]
    [Route("GetAllFilter")]
    public async Task<ActionResult> GetAllFilter(RequestFilterAutorDTO requestFilterAutorDTO){
        var autores = await _autorService.GetAllFilter(requestFilterAutorDTO);
        return Ok(autores);
    }

    [HttpPost]
    [Route("AddAutor")]
    public async Task<IActionResult> AddAutor(Autor autor){
        var response = await  _autorService.AddAutor(autor);
        return Ok(response);
    }

    [HttpPut]
    [Route("UpdateAutor")]
    public async Task<IActionResult> UpdateAutor(Autor autor){
        var response = await  _autorService.UpdateAutor(autor);
        return Ok(response);
    }
    
    [HttpDelete]
    [Route("InactivarAutor")]
    public async Task<IActionResult> DeleteAutor(int id){
        BaseMessage<Autor> responseGet = await _autorService.FindById(id);
        if (responseGet.StatusCode != HttpStatusCode.OK){
            return Ok(responseGet);
        }

        Autor autor = responseGet.ResponseElements.FirstOrDefault();
        var respuesta = await _autorService.UpdateStateAutor(autor);
        return Ok(respuesta);
    }
    
}