//using System.Net;
//using System.Text.RegularExpressions;

//public class LibroService : ILibroService
//{
//    private readonly IUnitOfWork _unitOfWork;

//    public LibroService(IUnitOfWork unitOfWork)
//    {
//        _unitOfWork = unitOfWork;
//    }

//    public async Task<BaseMessage<Libro>> GetAll()
//    {
//        try
//        {
//            IEnumerable<Libro> libros = await _unitOfWork.LibroRepository.GetAllAsync();
//            return libros.Any() ? BuildResponse(libros.ToList(), "", HttpStatusCode.OK) :
//                                  BuildResponse(libros.ToList(), "", HttpStatusCode.NotFound);
//        }
//        catch (Exception ex)
//        {
//            return new BaseMessage<Libro>()
//            {
//                Message = $"[Exception]: {ex.Message}",
//                StatusCode = HttpStatusCode.InternalServerError,
//                ResponseElements = new()
//            };
//        }
//    }

//    public async Task<BaseMessage<Libro>> AddLibro(Libro libro)
//    {
//        var isValid = ValidateModel(libro);
//        if (!string.IsNullOrEmpty(isValid))
//        {
//            return BuildResponse(null, isValid, HttpStatusCode.BadRequest);
//        }

//        try
//        {
//            await _unitOfWork.LibroRepository.AddAsync(libro);
//            await _unitOfWork.SaveAsync();
//        }
//        catch (Exception ex)
//        {
//            return new BaseMessage<Libro>()
//            {
//                Message = $"[Exception]: {ex.Message}",
//                StatusCode = HttpStatusCode.InternalServerError,
//                ResponseElements = new()
//            };
//        }

//        return BuildResponse(new List<Libro> { libro }, "", HttpStatusCode.OK);
//    }

//    public async Task<BaseMessage<Libro>> UpdateLibro(Libro libro)
//    {
//        var isValid = ValidateModel(libro);
//        if (!string.IsNullOrEmpty(isValid))
//        {
//            return BuildResponse(null, isValid, HttpStatusCode.BadRequest);
//        }

//        try
//        {
//            await _unitOfWork.LibroRepository.Update(libro);
//            await _unitOfWork.SaveAsync();
//        }
//        catch (Exception ex)
//        {
//            return new BaseMessage<Libro>()
//            {
//                Message = $"[Exception]: {ex.Message}",
//                StatusCode = HttpStatusCode.InternalServerError,
//                ResponseElements = new()
//            };
//        }

//        return BuildResponse(new List<Libro> { libro }, "", HttpStatusCode.OK);
//    }

//    public async Task<BaseMessage<Libro>> FindById(int id)
//    {
//        try
//        {
//            Libro? libro = await _unitOfWork.LibroRepository.FindAsync(id);
//            return libro != null ? BuildResponse(new List<Libro> { libro }, "", HttpStatusCode.OK) :
//                                   BuildResponse(new List<Libro>(), "", HttpStatusCode.NotFound);
//        }
//        catch (Exception ex)
//        {
//            return new BaseMessage<Libro>()
//            {
//                Message = $"[Exception]: {ex.Message}",
//                StatusCode = HttpStatusCode.InternalServerError,
//                ResponseElements = new()
//            };
//        }
//    }

//    private string ValidateModel(Libro libro)
//    {
//        string message = string.Empty;

//        if (string.IsNullOrEmpty(libro.Titulo))
//        {
//            message += "El t�tulo es requerido. ";
//        }

//        if (string.IsNullOrEmpty(libro.ISBN13) || !Regex.IsMatch(libro.ISBN13, "^\\d{10}|\\d{13}$"))
//        {
//            message += "El ISBN debe tener 10 o 13 caracteres num�ricos. ";
//        }

//        if (libro.AnioPublicacion < 1450 || libro.AnioPublicacion > 2026)
//        {
//            message += "El a�o de publicaci�n debe estar entre 1450 y 2026. ";
//        }

//        if (string.IsNullOrEmpty(libro.Editorial))
//        {
//            message += "La editorial es requerida. ";
//        }

//        return message;
//    }

//    private BaseMessage<Libro> BuildResponse(List<Libro> lista, string message = "", HttpStatusCode status = HttpStatusCode.OK)
//    {
//        return new BaseMessage<Libro>()
//        {
//            Message = message,
//            StatusCode = status,
//            ResponseElements = lista
//        };
//    }
//}


using System.Net;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class LibroService : ILibroService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEstadisticaService _estadisticaService;

    public LibroService(IUnitOfWork unitOfWork,IEstadisticaService estadisticaService)
    {
        _unitOfWork = unitOfWork;
        _estadisticaService = estadisticaService;
    }

    public async Task<BaseMessage<Libro>> GetAll()
    {
        try
        {
            IEnumerable<Libro> libros = await _unitOfWork.LibroRepository.GetAllAsync();
            return libros.Any() ? BuildResponse(libros.ToList(), "", HttpStatusCode.OK) :
                                  BuildResponse(libros.ToList(), "", HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            return new BaseMessage<Libro>
            {
                Message = $"[Exception]: {ex.Message}",
                StatusCode = HttpStatusCode.InternalServerError,
                ResponseElements = new()
            };
        }
    }

    public async Task<BaseMessage<Libro>> AddLibro(Libro libro)
    {
        var isValid = ValidateModel(libro);
        if (!string.IsNullOrEmpty(isValid))
        {
            return BuildResponse(null, isValid, HttpStatusCode.BadRequest);
        }

        try
        {
            await _unitOfWork.LibroRepository.AddAsync(libro);
            await _unitOfWork.SaveAsync();

            await SaveEstadisticaAsync(libro.Id);
        }
        catch (Exception ex)
        {
            return new BaseMessage<Libro>
            {
                Message = $"[Exception]: {ex.Message}",
                StatusCode = HttpStatusCode.InternalServerError,
                ResponseElements = new()
            };
        }

        return BuildResponse(new List<Libro> { libro }, "", HttpStatusCode.OK);
    }

    public async Task<BaseMessage<Libro>> UpdateLibro(Libro libro)
    {
        var isValid = ValidateModel(libro);
        if (!string.IsNullOrEmpty(isValid))
        {
            return BuildResponse(null, isValid, HttpStatusCode.BadRequest);
        }

        try
        {
            await _unitOfWork.LibroRepository.Update(libro);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            return new BaseMessage<Libro>
            {
                Message = $"[Exception]: {ex.Message}",
                StatusCode = HttpStatusCode.InternalServerError,
                ResponseElements = new()
            };
        }

        return BuildResponse(new List<Libro> { libro }, "", HttpStatusCode.OK);
    }

    public async Task<BaseMessage<Libro>> FindById(int id)
    {
        try
        {
            Libro? libro = await _unitOfWork.LibroRepository.FindAsync(id);
            return libro != null ? BuildResponse(new List<Libro> { libro }, "", HttpStatusCode.OK) :
                                   BuildResponse(new List<Libro>(), "", HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            return new BaseMessage<Libro>
            {
                Message = $"[Exception]: {ex.Message}",
                StatusCode = HttpStatusCode.InternalServerError,
                ResponseElements = new()
            };
        }
    }

    public async Task<BaseMessage<Libro>> GetAllFilter(RequestFilterLibroDTO requestFilterLibroDTO)
    {
        try
        {
            var libros = await _unitOfWork.LibroRepository.GetAllAsync(
                l => (string.IsNullOrEmpty(requestFilterLibroDTO.Nombre) || l.Titulo.Contains(requestFilterLibroDTO.Nombre)) &&
                     (requestFilterLibroDTO.AutorId == 0 || l.AutorId == requestFilterLibroDTO.AutorId) &&
                     (string.IsNullOrEmpty(requestFilterLibroDTO.Genero) || l.Genero == requestFilterLibroDTO.Genero) &&
                     (string.IsNullOrEmpty(requestFilterLibroDTO.Idioma) || l.Idioma == requestFilterLibroDTO.Idioma) &&
                     (string.IsNullOrEmpty(requestFilterLibroDTO.Formato) || l.Formato == requestFilterLibroDTO.Formato) &&
                     //(string.IsNullOrEmpty(requestFilterLibroDTO.AnioPublicacion) || l.AnioPublicacion == requestFilterLibroDTO.AnioPublicacion)
                     (!requestFilterLibroDTO.AnioPublicacion.HasValue || l.AnioPublicacion == requestFilterLibroDTO.AnioPublicacion.Value)
            );

            return libros.Any() ? BuildResponse(libros.ToList(), "", HttpStatusCode.OK) :
                                  BuildResponse(new List<Libro>(), "No se encontraron libros con los filtros especificados.", HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            return new BaseMessage<Libro>
            {
                Message = $"[Exception]: {ex.Message}",
                StatusCode = HttpStatusCode.InternalServerError,
                ResponseElements = new()
            };
        }
    }

    private string ValidateModel(Libro libro)
    {
        string message = string.Empty;

        if (string.IsNullOrEmpty(libro.Titulo))
        {
            message += "El t�tulo es requerido. ";
        }

        if (string.IsNullOrEmpty(libro.ISBN13) || !Regex.IsMatch(libro.ISBN13, "^\\d{10}|\\d{13}$"))
        {
            message += "El ISBN debe tener 10 o 13 caracteres num�ricos. ";
        }

        if (libro.AnioPublicacion < 1450 || libro.AnioPublicacion > 2026)
        {
            message += "El a�o de publicaci�n debe estar entre 1450 y 2026. ";
        }

        if (string.IsNullOrEmpty(libro.Editorial))
        {
            message += "La editorial es requerida. ";
        }

        return message;
    }

    private BaseMessage<Libro> BuildResponse(List<Libro> lista, string message = "", HttpStatusCode status = HttpStatusCode.OK)
    {
        return new BaseMessage<Libro>
        {
            Message = message,
            StatusCode = status,
            ResponseElements = lista
        };
    }

        private async Task SaveEstadisticaAsync(int idLibro){
        try{
            Estadistica estadistica = new (){
                AudioLibroId = null,
                AudioLibro = null,
                LibroId = idLibro,
                CountLeido = 0,
                CountDescargas = 0,
                CountEscuchado = 0
            };

            await _estadisticaService.AddEstadistica(estadistica);
        }catch(Exception){
            throw;
        }
    }

    private async Task DeleteEstadisticaAsync(int idLibro){
        try{
            await _estadisticaService.DeleteEstadistica(null,idLibro);
        }catch(Exception){
            throw;
        }
    }

}
