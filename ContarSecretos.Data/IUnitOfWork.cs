public interface IUnitOfWork
{
    IRepository<int, Autor> AutorRepository{get;}
    IRepository<int, Libro> LibroRepository{get;}
    IRepository<int, AudioLibro> AudioLibroRepository{get;}
    IRepository<int, Estadistica> EstadisticaRepository{get;}
    Task SaveAsync();
}