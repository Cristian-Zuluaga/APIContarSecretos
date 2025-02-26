
using System.Net;

public class AutorService : IAutorService
{

    private readonly IUnitOfWork _unitOfWork;

    public AutorService (IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseMessage<Autor>> AddAutor(Autor autor)
    {
        var isValid = ValidateModel(autor);
        if(!string.IsNullOrEmpty(isValid))
        {
            return this.BuildResponse(null, isValid, HttpStatusCode.BadRequest);
        }

        try{

            await _unitOfWork.AutorRepository.AddAsync(autor);
            await _unitOfWork.SaveAsync();
        }
        catch(Exception ex)
        {
            return new BaseMessage<Autor>() {
                Message = $"[Exception]: {ex.Message}",
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                ResponseElements = new ()
            };
        }
        
        
        return new BaseMessage<Autor>() {
            Message = "",
            StatusCode = System.Net.HttpStatusCode.OK,
            ResponseElements = new List<Autor>{autor}
        };
    }

    public async Task<BaseMessage<Autor>> FindById(int id)
    {
        try{
            Autor? autor = new();
            autor = await _unitOfWork.AutorRepository.FindAsync(id);
            
            return autor != null ?  
            this.BuildResponse(new List<Autor>(){autor}, "", HttpStatusCode.OK) : 
            this.BuildResponse(new List<Autor>(), "", HttpStatusCode.NotFound);
        }catch(Exception ex){
                return new BaseMessage<Autor>() {
                Message = $"[Exception]: {ex.Message}",
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                ResponseElements = new ()
            };
        }   
    }


    private string ValidateModel(Autor autor){
        string message = string.Empty;

        if(string.IsNullOrEmpty(autor.Nombre))
        {
            message += "El nombre es requerido";
        }
        return message;
    }

    private BaseMessage<Autor> BuildResponse(List<Autor> lista, string message = "", HttpStatusCode status = HttpStatusCode.OK)
    {
        return new BaseMessage<Autor>(){
            Message = message,
            StatusCode = status,
            ResponseElements = lista
        };
    }

}