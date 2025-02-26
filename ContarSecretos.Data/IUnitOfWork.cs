public interface IUnitOfWork
{
    IRepository<int, Autor> AutorRepository{get;}
    IRepository<int, Libro> LibroRepository{get;}
    IRepository<int, AudioLibro> AudioLibroRepository{get;}
    Task SaveAsync();
}