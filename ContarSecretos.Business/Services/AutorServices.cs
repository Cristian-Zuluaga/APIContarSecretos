
using System.Net;

public class AutorService : IAutorService
{

    private readonly IUnitOfWork _unitOfWork;

    public AutorService (IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseMessage<Autor>> GetAll()
    {
        try 
        {
            IEnumerable<Autor> autores= await _unitOfWork.AutorRepository.GetAllAsync();

            return autores.Any() ?  BuildResponse(autores.ToList(), "", HttpStatusCode.OK) : 
            BuildResponse(autores.ToList(), "", HttpStatusCode.NotFound);
        }
        catch (Exception ex){
            return new BaseMessage<Autor>() {
                Message = $"[Exception]: {ex.Message}",
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                ResponseElements = new ()
            };
        }
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

    

    public async Task<BaseMessage<Autor>> UpdateAutor(Autor autor)
    {
        var isValid = ValidateModel(autor);
        if(!string.IsNullOrEmpty(isValid))
        {
            return this.BuildResponse(null, isValid, HttpStatusCode.BadRequest);
        }

        try{

            await _unitOfWork.AutorRepository.Update(autor);
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
        if(string.IsNullOrEmpty(autor.Apellido))
        {
            message += "El apellido es requerido";
        }
        if(autor.FechaNacimiento == null)
        {
            message += "La fecha de nacimiento es requerida";
        }
        if(string.IsNullOrEmpty(autor.Pais))
        {
            message += "El pais es requerido";
        }
        if(autor.FechaNacimiento.ToDateTime(TimeOnly.MinValue) >= DateTime.Now)
        {
            message += "La fecha de nacimiento no puede ser mayor a hoy";
        }
        if(!autor.EstaVivo && autor.FechaMuerte == 0)
        {
            message += "La fecha de muerte no es valida";
        }

        List<string> idiomasPermitidos = new List<string> { "EN", "ES", "DE", "JP", "PT", "FR", "IT", "RU", "UK" };
        if(idiomasPermitidos.Contains(autor.Idiomas))
        {
            message += "El idioma no es valido";
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