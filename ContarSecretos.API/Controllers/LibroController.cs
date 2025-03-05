using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/libros")]
[ApiController]
public class LibroController : ControllerBase
{
    private readonly ILibroService _libroService;

    public LibroController(ILibroService libroService)
    {
        _libroService = libroService;
    }

    // Crear un nuevo libro
    [HttpPost]
    public async Task<ActionResult<Libro>> AgregarLibro([FromBody] Libro libro)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var nuevoLibro = await _libroService.AgregarLibroAsync(libro);
        return CreatedAtAction(nameof(ObtenerLibroPorId), new { id = nuevoLibro.Id }, nuevoLibro);
    }

    // Obtener un libro por ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Libro>> ObtenerLibroPorId(int id)
    {
        var libro = await _libroService.ObtenerLibroPorIdAsync(id);
        if (libro == null)
            return NotFound();

        return Ok(libro);
    }

    // Obtener todos los libros
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Libro>>> ObtenerTodosLosLibros()
    {
        var libros = await _libroService.ObtenerTodosLosLibrosAsync();
        return Ok(libros);
    }

    // Editar un libro
    [HttpPut("{id}")]
    public async Task<ActionResult<Libro>> EditarLibro(int id, [FromBody] Libro libroActualizado)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var libroEditado = await _libroService.EditarLibroAsync(id, libroActualizado);
        return Ok(libroEditado);
    }

    // Eliminar un libro
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarLibro(int id)
    {
        var eliminado = await _libroService.EliminarLibroAsync(id);
        if (!eliminado)
            return NotFound();

        return NoContent();
    }

    // Buscar libros por filtros
    [HttpGet("buscar")]
    public async Task<ActionResult<IEnumerable<Libro>>> BuscarLibros(
        [FromQuery] string genero, [FromQuery] string autor, [FromQuery] string nombre, 
        [FromQuery] string idioma, [FromQuery] string formato, [FromQuery] int? anio, 
        [FromQuery] string editorial, [FromQuery] string isbn)
    {
        var libros = await _libroService.BuscarLibrosAsync(genero, autor, nombre, idioma, formato, anio, editorial, isbn);
        return Ok(libros);
    }
}