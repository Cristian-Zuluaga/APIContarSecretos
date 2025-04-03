
using System.Net;

public class AudioLibroService : IAudioLibroService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    private readonly IEstadisticaService _estadisticaService;   

    public AudioLibroService(IUnitOfWork unitOfWork, IFileService fileService,IEstadisticaService estadisticaService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _estadisticaService = estadisticaService;
    }

    public async Task<BaseMessage<AudioLibro>> AddAudioLibro(RequestAudioLibroDTO audioLibro)
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

            //Crea el registro de estadistica
            await SaveEstadisticaAsync(audioLibroSave.Id);

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

    private async Task SaveEstadisticaAsync(int idAudioLibro){
        try{
            Estadistica estadistica = new (){
                AudioLibroId = idAudioLibro,
                LibroId = null,
                Libro = null,
                CountLeido = 0,
                CountDescargas = 0,
                CountEscuchado = 0
            };

            await _estadisticaService.AddEstadistica(estadistica);
        }catch(Exception){
            throw;
        }
    }

    private async Task DeleteEstadisticaAsync(int idAudioLibro){
        try{
            await _estadisticaService.DeleteEstadistica(idAudioLibro,null);
        }catch(Exception){
            throw;
        }
    }
    private string ValidateModel(RequestAudioLibroDTO audioLibro){
        string message = string.Empty;
        
        if(string.IsNullOrEmpty(audioLibro.Titulo))
        {
            message += "El titulo es requerido";
        }
        if(audioLibro.NarradorId == 0)
        {
            message += "El narrador es requerido";
        }
        if(string.IsNullOrEmpty(audioLibro.Duracion))
        {
            message += "La duración es requerida";
        }
        if(string.IsNullOrEmpty(audioLibro.Base64File)){
            message += "El archivo es requerido";
        }
        return message;
    }


    public async Task<BaseMessage<ResponseAudioLibroDTO>> FindById(int id)
    {
           try{
            ResponseAudioLibroDTO? audioLibro = new();
            var audioLibroBD = await _unitOfWork.AudioLibroRepository.FindAsync(id);
            
            if(audioLibroBD == null){
                return new BaseMessage<ResponseAudioLibroDTO>() {
                    Message = "Elemento buscado no existe",
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    ResponseElements = new ()
                };
            }

            audioLibro.Titulo = audioLibroBD.Titulo;
            audioLibro.AutorId = audioLibroBD.AutorId;
            audioLibro.NarradorId = audioLibroBD.AutorId;
            audioLibro.Path = audioLibroBD.Path;
            audioLibro.Duracion = audioLibroBD.Duracion;
            audioLibro.Genero =audioLibroBD.Genero;
            audioLibro.Tamanio = audioLibroBD.Tamanio;
            audioLibro.Base64 = _fileService.GetFileByName(audioLibroBD.Path);

            return !string.IsNullOrEmpty(audioLibro.Titulo) ?  
            this.BuildResponseAudioLibroDTO(new List<ResponseAudioLibroDTO>(){audioLibro}, "", HttpStatusCode.OK) : 
            this.BuildResponseAudioLibroDTO(new List<ResponseAudioLibroDTO>(), "", HttpStatusCode.NotFound);
        }catch(Exception ex){
                return new BaseMessage<ResponseAudioLibroDTO>() {
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

    public async Task<BaseMessage<AudioLibro>> UpdateAudioLibro(RequestAudioLibroDTO audioLibro)
    {
        var isValid = ValidateModel(audioLibro);
        if(!string.IsNullOrEmpty(isValid))
        {
            return this.BuildResponse(null, isValid, HttpStatusCode.BadRequest);
        }

        AudioLibro? audioLibroUpdate = null;

        try{

            var audioLibroBD = await _unitOfWork.AudioLibroRepository.FindAsync(x => x.Id == audioLibro.Id);

            if(audioLibroBD == null){
                return this.BuildResponse(null, "Audio libro no encontrado", HttpStatusCode.BadRequest);
            }

            var autor = await _unitOfWork.AutorRepository.FindAsync(audioLibro.AutorId);

            if(autor == null){
                return this.BuildResponse(null, "Autor no valido", HttpStatusCode.BadRequest);
            }

            var resultFile = _fileService.SaveFileAudioLibroBase64(audioLibro.Base64File);

            if(resultFile == null){
                return this.BuildResponse(null, "Archvio no procesado", HttpStatusCode.BadRequest);
            }

            var resultDeleteFile = _fileService.DeleteFile(audioLibroBD.Path);

            if(!resultDeleteFile){
                return this.BuildResponse(null, "Error actualizando audio libro", HttpStatusCode.BadRequest);
            }

            audioLibroUpdate = new(){
                Id = audioLibro.Id,
                Titulo = audioLibro.Titulo,
                AutorId = audioLibro.AutorId,
                Autor = autor,
                Genero = audioLibro.Genero,
                NarradorId = audioLibro.NarradorId,
                Duracion = audioLibro.Duracion,
                Path = resultFile.Path,
                Tamanio = resultFile.Size
            };

            await _unitOfWork.AudioLibroRepository.Update(audioLibroUpdate);
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
            ResponseElements = new List<AudioLibro>{audioLibroUpdate}
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

    public async Task<BaseMessage<ResponseAudioLibroDTO>> GetAllFilter(RequestFilterAudioLibroDTO requestFilterAudioLibroDTO)
    {

        try{
            var listaBD = await _unitOfWork.AudioLibroRepository.GetAllAsync(x => 
                                                                                (requestFilterAudioLibroDTO.Titulo == null || x.Titulo.ToLower().Contains(requestFilterAudioLibroDTO.Titulo.ToLower()))
                                                                                &&  (requestFilterAudioLibroDTO.Genero == null || x.Genero.ToLower().Contains(requestFilterAudioLibroDTO.Genero.ToLower()))
                                                                                && (requestFilterAudioLibroDTO.NarradorId == null || x.NarradorId == requestFilterAudioLibroDTO.NarradorId)
                                                                                && (requestFilterAudioLibroDTO.AutorId == null || x.AutorId == requestFilterAudioLibroDTO.AutorId)
                                                                            );
            List<ResponseAudioLibroDTO> listRespuesta = new();

            foreach(var audio in listaBD){
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

    public async Task<bool> DeleteAudioLibro(int id)
    {
        try{

            //Elimina estadistica
            await DeleteEstadisticaAsync(id);
            
            await _unitOfWork.AudioLibroRepository.Delete(id);
            await _unitOfWork.SaveAsync();  

            return true;
        }
        catch(Exception)
        {
            return false;
        }
    }
}