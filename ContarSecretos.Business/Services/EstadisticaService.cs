
using System.Net;

public class EstadisticaService : IEstadisticaService
{
    private readonly IUnitOfWork _unitOfWork;

    public EstadisticaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<BaseMessage<Estadistica>> AddEstadistica(Estadistica estadistica)
    {
        try{
            
            if(estadistica.AudioLibroId != null){
                estadistica.AudioLibro = await _unitOfWork.AudioLibroRepository.FindAsync((int)estadistica.AudioLibroId);
            }

            if(estadistica.LibroId != null){
                estadistica.Libro = await _unitOfWork.LibroRepository.FindAsync((int)estadistica.LibroId);
            }

            await _unitOfWork.EstadisticaRepository.AddAsync(estadistica);
            await _unitOfWork.SaveAsync();
        }
        catch(Exception ex)
        {
            return new BaseMessage<Estadistica>() {
                Message = $"[Exception]: {ex.Message}",
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                ResponseElements = new ()
            };
        }
        
        
        return new BaseMessage<Estadistica>() {
            Message = "",
            StatusCode = System.Net.HttpStatusCode.OK,
            ResponseElements = new List<Estadistica>{estadistica}
        };
    
    }

    public async Task<bool> DeleteEstadistica(int? audioLibroId,int? libroId)
    {
        try{

            Estadistica estadistica = new();

            if(audioLibroId != null){
                var listado = await _unitOfWork.EstadisticaRepository.GetAllAsync(x=>x.AudioLibroId == audioLibroId);
                if(listado.Count() > 0){

                    estadistica = listado.First();  
                }   
            }

            if(libroId != null){
                var listado = await _unitOfWork.EstadisticaRepository.GetAllAsync(x=>x.LibroId == libroId);
                if(listado.Count() > 0){

                    estadistica = listado.First();  
                }   
            }

            await _unitOfWork.EstadisticaRepository.Delete(estadistica.Id);
            await _unitOfWork.SaveAsync();
            
            return true;
        }
        catch(Exception)
        {
            return false;
        }
    }

    public async Task<BaseMessage<Estadistica>> GetEstadistica(int? audioLibroId, int? libroId)
    {
         try{

            Estadistica estadistica = new();

            if(audioLibroId != null){
                var listado = await _unitOfWork.EstadisticaRepository.GetAllAsync(x=>x.AudioLibroId == audioLibroId);
                if(listado.Count() > 0){

                    estadistica = listado.First();  
                }   
            }

            if(libroId != null){
                var listado = await _unitOfWork.EstadisticaRepository.GetAllAsync(x=>x.LibroId == libroId);
                if(listado.Count() > 0){

                    estadistica = listado.First();  
                }   
            }
            
            return estadistica.Id != 0 ?  
            this.BuildResponse(new List<Estadistica>(){estadistica}, "", HttpStatusCode.OK) : 
            this.BuildResponse(new List<Estadistica>(), "", HttpStatusCode.NotFound);
        }
        catch(Exception ex)
        {
            return new BaseMessage<Estadistica>() {
                Message = $"[Exception]: {ex.Message}",
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                ResponseElements = new ()
            };
        }
   
    }

    
    private BaseMessage<Estadistica> BuildResponse(List<Estadistica> lista, string message = "", HttpStatusCode status = HttpStatusCode.OK)
    {
        return new BaseMessage<Estadistica>(){
            Message = message,
            StatusCode = status,
            ResponseElements = lista
        };
    }

    public async Task<BaseMessage<Estadistica>> UpdateEstadistica(Estadistica estadistica)
    {
         try{

            await _unitOfWork.EstadisticaRepository.Update(estadistica);
            await _unitOfWork.SaveAsync();
        }
        catch(Exception ex)
        {
            return new BaseMessage<Estadistica>() {
                Message = $"[Exception]: {ex.Message}",
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                ResponseElements = new ()
            };
        }
        
        
        return new BaseMessage<Estadistica>() {
            Message = "",
            StatusCode = System.Net.HttpStatusCode.OK,
            ResponseElements = new List<Estadistica>{estadistica}
        };
    }
}