

//public interface ILibroService
//{
    //interfaces del crud libro
    //Task<BaseMessage<Libro>> AddLibro(RequestLibroAddDTO libro);
    //Task<BaseMessage<Libro>> UpdateLibro(RequestLibroUpdateDTO libro);
    //Task<BaseMessage<bool>> DeleteLibro(int id);

    // interfaces para obtener un libro o todos los libros
   // Task<BaseMessage<Libro>> FindById(int id);
   // Task<BaseMessage<Libro>> GetAll(int page, int pageSize);
    //Task<BaseMessage<Libro>> GetAllFilter(RequestFilterLibroDTO filters, int page, int pageSize);

    // interfaz para descarga y lectura
   // Task<FileDTO?> DownloadLibro(int id);
   // Task<string?> ReadLibroOnline(int id);
//}
 

public interface ILibroService
{
    Task<BaseMessage<Libro>> GetAll();
    Task<BaseMessage<Libro>> AddLibro(Libro libro);
    Task<BaseMessage<Libro>> UpdateLibro(Libro libro);
    Task<BaseMessage<Libro>> FindById(int id);
    Task<BaseMessage<Libro>> GetAllFilter(RequestFilterLibroDTO requestFilterLibroDTO);
    //se añaden estas 3 interfaces
    Task<BaseMessage<byte[]>> DownloadLibro(int id);
    Task<BaseMessage<Libro>> DesactivarLibro(int id);
    Task<BaseMessage<Libro>> ActivarLibro(int id);  




}