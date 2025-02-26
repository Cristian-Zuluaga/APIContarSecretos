using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
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

    [HttpPost]
    [Route("AddAutor")]
    public async Task<IActionResult> AddAutor(Autor autor){
        var response = await  _autorService.AddAutor(autor);
        return Ok(response);
    }
    
}