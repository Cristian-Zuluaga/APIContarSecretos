using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using ContarSecretos.Business.Models;

public class LibroService : ILibroService
{
    private readonly List<Libro> _libros = new(); // Lista temporal en memoria

    public async Task<Libro> AgregarLibroAsync(Libro libro)
    {
        libro.Id = _libros.Count + 1; // Simula un ID autoincremental
        _libros.Add(libro);
        return await Task.FromResult(libro);
    }

    public async Task<Libro> ObtenerLibroPorIdAsync(int id)
    {
        var libro = _libros.FirstOrDefault(l => l.Id == id);
        return await Task.FromResult(libro);
    }

    public async Task<IEnumerable<Libro>> ObtenerTodosLosLibrosAsync()
    {
        return await Task.FromResult(_libros);
    }

    public async Task<Libro> EditarLibroAsync(int id, Libro libroActualizado)
    {
        var libro = _libros.FirstOrDefault(l => l.Id == id);
        if (libro != null)
        {
            libro.Titulo = libroActualizado.Titulo;
            libro.Autor = libroActualizado.Autor;
            libro.AnioPublicacion = libroActualizado.AnioPublicacion;
        }
        return await Task.FromResult(libro);
    }

    public async Task<bool> EliminarLibroAsync(int id)
    {
        var libro = _libros.FirstOrDefault(l => l.Id == id);
        if (libro != null)
        {
            _libros.Remove(libro);
            return await Task.FromResult(true);
        }
        return await Task.FromResult(false);
    }

    public async Task<IEnumerable<Libro>> BuscarLibrosAsync(string genero, string autor, string nombre)
    {
        var librosFiltrados = _libros.Where(l =>
            (string.IsNullOrEmpty(genero) || l.Genero == genero) &&
            //(string.IsNullOrEmpty(autor) || l.Autor == autor) &&
            (string.IsNullOrEmpty(nombre) || l.Titulo.Contains(nombre))
        ).ToList();

        return await Task.FromResult(librosFiltrados);
    }
}