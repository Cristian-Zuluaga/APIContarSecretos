
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LibroController : ControllerBase
{
    private readonly ILibroService _libroService;

    public LibroController(ILibroService libroService)
    {
        _libroService = libroService;
    }

    [HttpGet]
    [Route("GetById")]
    public async Task<ActionResult> GetById(int id)
    {
        var libros = await _libroService.FindById(id);
        return Ok(libros);
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<ActionResult> GetAll()
    {
        var libros = await _libroService.GetAll();
        return Ok(libros);
    }

    [HttpPost]
    [Route("GetAllFilter")]
    public async Task<ActionResult> GetAllFilter(RequestFilterLibroDTO requestFilterLibroDTO)
    {
        var libros = await _libroService.GetAllFilter(requestFilterLibroDTO);
        return Ok(libros);
    }

    [HttpPost]
    [Route("AddLibro")]
    public async Task<IActionResult> AddLibro(Libro libro)
    {   // var response puede ir en vez de var libros
        var libros = await _libroService.AddLibro(libro);
        return Ok(libros);
    }

    [HttpPut]
    [Route("UpdateLibro")]
    public async Task<IActionResult> UpdateLibro(Libro libro)
    {   // var response puede ir en vez de var libros
        var libros = await _libroService.UpdateLibro(libro);
        return Ok(libros);
    }

    

}
