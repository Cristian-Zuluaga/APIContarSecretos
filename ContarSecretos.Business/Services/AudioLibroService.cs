
using System.Net;

public class AudioLibroService : IAudioLibroService
{

    private readonly IUnitOfWork _unitOfWork;

    public AudioLibroService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseMessage<AudioLibro>> AddAudioLibro(AudioLibro audioLibro)
    {
        var isValid = ValidateModel(audioLibro);
        if(!string.IsNullOrEmpty(isValid))
        {
            return this.BuildResponse(null, isValid, HttpStatusCode.BadRequest);
        }

        try{


            var autor = await _unitOfWork.AutorRepository.FindAsync(audioLibro.AutorId);

            if(autor == null){
                return this.BuildResponse(null, "Autor no valido", HttpStatusCode.BadRequest);
            }

            audioLibro.Autor = autor;

            await _unitOfWork.AudioLibroRepository.AddAsync(audioLibro);
            await _unitOfWork.SaveAsync();
        }
        catch(Exception ex)
        {
            return new BaseMessage<AudioLibro>() {
                Message = $"[Exception]: {ex.Message}",
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                ResponseElements = new ()
            };
        }
        
        
        return new BaseMessage<AudioLibro>() {
            Message = "",
            StatusCode = System.Net.HttpStatusCode.OK,
            ResponseElements = new List<AudioLibro>{audioLibro}
        };
    
    }


    private string ValidateModel(AudioLibro audioLibro){
        string message = string.Empty;
        
        if(string.IsNullOrEmpty(audioLibro.Titulo))
        {
            message += "El titulo es requerido";
        }
        /*
        if(string.IsNullOrEmpty(audioLibro.Apellido))
        {
            message += "El apellido es requerido";
        }
        if(audioLibro.FechaNacimiento == null)
        {
            message += "La fecha de nacimiento es requerida";
        }
        if(string.IsNullOrEmpty(audioLibro.Pais))
        {
            message += "El pais es requerido";
        }
        if(audioLibro.FechaNacimiento.ToDateTime(TimeOnly.MinValue) >= DateTime.Now)
        {
            message += "La fecha de nacimiento no puede ser mayor a hoy";
        }
        if(!audioLibro.EstaVivo && audioLibro.FechaMuerte == 0)
        {
            message += "La fecha de muerte no es valida";
        }

        List<string> idiomasPermitidos = new List<string> { "EN", "ES", "DE", "JP", "PT", "FR", "IT", "RU", "UK" };
        if(!idiomasPermitidos.Contains(audioLibro.Idiomas))
        {
            message += "El idioma no es valido";
        }
        */
        return message;
    }


    public async Task<BaseMessage<AudioLibro>> FindById(int id)
    {
           try{
            AudioLibro? audioLibro = new();
            audioLibro = await _unitOfWork.AudioLibroRepository.FindAsync(id);
            
            return audioLibro != null ?  
            this.BuildResponse(new List<AudioLibro>(){audioLibro}, "", HttpStatusCode.OK) : 
            this.BuildResponse(new List<AudioLibro>(), "", HttpStatusCode.NotFound);
        }catch(Exception ex){
                return new BaseMessage<AudioLibro>() {
                Message = $"[Exception]: {ex.Message}",
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                ResponseElements = new ()
            };
        }   
    }

    public async Task<BaseMessage<AudioLibro>> GetAll()
    {
        try 
        {
            IEnumerable<AudioLibro> audioLibros= await _unitOfWork.AudioLibroRepository.GetAllAsync();

            return audioLibros.Any() ?  BuildResponse(audioLibros.ToList(), "", HttpStatusCode.OK) : 
            BuildResponse(audioLibros.ToList(), "", HttpStatusCode.NotFound);
        }
        catch (Exception ex){
            return new BaseMessage<AudioLibro>() {
                Message = $"[Exception]: {ex.Message}",
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                ResponseElements = new ()
            };
        }
    }

    public async Task<BaseMessage<AudioLibro>> UpdateAudioLibro(AudioLibro audioLibro)
    {
        var isValid = ValidateModel(audioLibro);
        if(!string.IsNullOrEmpty(isValid))
        {
            return this.BuildResponse(null, isValid, HttpStatusCode.BadRequest);
        }

        try{

            var autor = await _unitOfWork.AutorRepository.FindAsync(audioLibro.AutorId);

            if(autor == null){
                return this.BuildResponse(null, "Autor no valido", HttpStatusCode.BadRequest);
            }

            audioLibro.Autor = autor;

            await _unitOfWork.AudioLibroRepository.Update(audioLibro);
            await _unitOfWork.SaveAsync();
        }
        catch(Exception ex)
        {
            return new BaseMessage<AudioLibro>() {
                Message = $"[Exception]: {ex.Message}",
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                ResponseElements = new ()
            };
        }
        
        
        return new BaseMessage<AudioLibro>() {
            Message = "",
            StatusCode = System.Net.HttpStatusCode.OK,
            ResponseElements = new List<AudioLibro>{audioLibro}
        };
    
    }

    private BaseMessage<AudioLibro> BuildResponse(List<AudioLibro> lista, string message = "", HttpStatusCode status = HttpStatusCode.OK)
    {
        return new BaseMessage<AudioLibro>(){
            Message = message,
            StatusCode = status,
            ResponseElements = lista
        };
    }

}