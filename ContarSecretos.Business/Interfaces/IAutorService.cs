public interface IAutorService{

    Task<BaseMessage<Autor>> FindById(int id);
    Task<BaseMessage<Autor>> AddAutor(Autor autor);
    Task<BaseMessage<Autor>> GetAll();
    Task<BaseMessage<Autor>> UpdateAutor(Autor autor);
    Task<BaseMessage<Autor>> UpdateStateAutor(Autor autor);
    Task<BaseMessage<Autor>> GetAllFilter(RequestFilterAutorDTO requestFilterAutorDTO);
}