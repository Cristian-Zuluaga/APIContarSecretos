
using System.Net;

public class AudioLibroService : IAudioLibroService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    public AudioLibroService(IUnitOfWork unitOfWork, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task<BaseMessage<AudioLibro>> AddAudioLibro(RequestAudioLibroAddDTO audioLibro)
    {
        var isValid = ValidateModel(audioLibro);
        if(!string.IsNullOrEmpty(isValid))
        {
            return this.BuildResponse(null, isValid, HttpStatusCode.BadRequest);
        }

        AudioLibro? audioLibroSave = null;

        try{


            var autor = await _unitOfWork.AutorRepository.FindAsync(audioLibro.AutorId);

            if(autor == null){
                return this.BuildResponse(null, "Autor no valido", HttpStatusCode.BadRequest);
            }

            var resultFile = _fileService.SaveFileAudioLibroBase64(audioLibro.Base64File);

            if(resultFile == null){
                return this.BuildResponse(null, "Archvio no procesado", HttpStatusCode.BadRequest);
            }

            audioLibroSave = new(){
                Titulo = audioLibro.Titulo,
                AutorId = audioLibro.AutorId,
                Autor = autor,
                Genero = audioLibro.Genero,
                NarradorId = audioLibro.NarradorId,
                Duracion = audioLibro.Duracion,
                Path = resultFile.Path,
                Tamanio = resultFile.Size
            };

            await _unitOfWork.AudioLibroRepository.AddAsync(audioLibroSave);
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
            ResponseElements = new List<AudioLibro>{audioLibroSave}
        };
    
    }


    private string ValidateModel(RequestAudioLibroAddDTO audioLibro){
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

    public async Task<BaseMessage<ResponseAudioLibroDTO>> GetAll()
    {
        try 
        {
            IEnumerable<AudioLibro> audioLibros= await _unitOfWork.AudioLibroRepository.GetAllAsync();

            List<ResponseAudioLibroDTO> listRespuesta = new();

            foreach(var audio in audioLibros){
                listRespuesta.Add(new(){
                    Titulo = audio.Titulo,
                    Genero = audio.Genero,
                    NarradorId = audio.NarradorId,
                    Duracion = audio.Duracion,
                    Tamanio = audio.Tamanio,
                    Path = audio.Path,
                    AutorId = audio.AutorId,
                    Base64 = _fileService.GetFileByName(audio.Path)
                });
            }

            return listRespuesta.Any() ?  BuildResponseAudioLibroDTO(listRespuesta.ToList(), "", HttpStatusCode.OK) : 
            BuildResponseAudioLibroDTO(listRespuesta.ToList(), "", HttpStatusCode.NotFound);
        }
        catch (Exception ex){
            return new BaseMessage<ResponseAudioLibroDTO>() {
                Message = $"[Exception]: {ex.Message}",
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                ResponseElements = new ()
            };
        }
    }

    public async Task<BaseMessage<AudioLibro>> UpdateAudioLibro(AudioLibro audioLibro)
    {
        /*
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
        */

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

    private BaseMessage<ResponseAudioLibroDTO> BuildResponseAudioLibroDTO(List<ResponseAudioLibroDTO> lista, string message = "", HttpStatusCode status = HttpStatusCode.OK)
    {
        return new BaseMessage<ResponseAudioLibroDTO>(){
            Message = message,
            StatusCode = status,
            ResponseElements = lista
        };
    }

    public async Task<BaseMessage<AudioLibro>> GetAllFilter(RequestFilterAudioLibroDTO requestFilterAudioLibroDTO)
    {
        var lista = await _unitOfWork.AudioLibroRepository.GetAllAsync(x => 
                                                                    (requestFilterAudioLibroDTO.Titulo == null || x.Titulo.ToLower().Contains(requestFilterAudioLibroDTO.Titulo.ToLower()))
                                                                    &&  (requestFilterAudioLibroDTO.Genero == null || x.Genero.ToLower().Contains(requestFilterAudioLibroDTO.Genero.ToLower()))
                                                                    && (requestFilterAudioLibroDTO.NarradorId == null || x.NarradorId == requestFilterAudioLibroDTO.NarradorId)
                                                                    && (requestFilterAudioLibroDTO.AutorId == null || x.AutorId == requestFilterAudioLibroDTO.AutorId)
                                                                );
        return lista.Any() ?  BuildResponse(lista.ToList(), "", HttpStatusCode.OK) : 
            BuildResponse(lista.ToList(), "", HttpStatusCode.NotFound);
    }
}