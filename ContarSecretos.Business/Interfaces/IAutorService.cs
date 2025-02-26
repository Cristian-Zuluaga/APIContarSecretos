public interface IAutorService{

    public Task<BaseMessage<Autor>> FindById(int id);
    Task<BaseMessage<Autor>> AddAutor(Autor autor);
}