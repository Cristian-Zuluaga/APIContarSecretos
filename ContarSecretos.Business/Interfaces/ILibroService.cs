using System.Collections.Generic;
using System.Threading.Tasks;
//using ContarSecretos.Business.Models; // 

public interface ILibroService
{
    Task<Libro> AgregarLibroAsync(Libro libro);
    Task<Libro> ObtenerLibroPorIdAsync(int id);
    Task<IEnumerable<Libro>> ObtenerTodosLosLibrosAsync();
    Task<Libro> EditarLibroAsync(int id, Libro libroActualizado);
    Task<bool> EliminarLibroAsync(int id);
    Task<IEnumerable<Libro>> BuscarLibrosAsync(string genero, string autor, string nombre);
}