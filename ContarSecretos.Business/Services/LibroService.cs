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

    public LibroService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseMessage<Libro>> GetAll()
    {
        try
        {                                                                               // se añade activo para habilitar o inabi libros
            IEnumerable<Libro> libros = await _unitOfWork.LibroRepository.GetAllAsync(l => l.Activo);
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
        ////download libro **begin**

    public async Task<BaseMessage<byte[]>> DownloadLibro(int id)
{
    try
    {
        var libro = await _unitOfWork.LibroRepository.FindAsync(id);

        if (libro == null)
        {
            return new BaseMessage<byte[]>
            {
                Message = "Libro no encontrado.",
                StatusCode = HttpStatusCode.NotFound,
                ResponseElements = new List<byte[]>()
            };
        }
            
    // nombre buscando el  campo en la base de datos para almacenar el nombre del archivo PDF
    //string rutaArchivo = Path.Combine("Libros", libros.NombreArchivoPDF); 

        //   nombre con el ID del libro
        string rutaArchivo = Path.Combine("Libros", $"Libro_{id}.pdf");

        if (!File.Exists(rutaArchivo))
        {
            return new BaseMessage<byte[]>
            {
                Message = "El archivo no existe.",
                StatusCode = HttpStatusCode.NotFound,
                ResponseElements = new List<byte[]>()
            };
        }

        byte[] archivoBytes = await File.ReadAllBytesAsync(rutaArchivo);

        return new BaseMessage<byte[]>
        {
            StatusCode = HttpStatusCode.OK,
            ResponseElements = new List<byte[]> { archivoBytes }
        };
    }
    catch (Exception ex)
    {
        return new BaseMessage<byte[]>
        {
            Message = $"[Exception]: {ex.Message}",
            StatusCode = HttpStatusCode.InternalServerError,
            ResponseElements = new List<byte[]>()
        };
    }
}


////download libro **end**

//desactivar libri ***begin***

    public async Task<BaseMessage<Libro>> DesactivarLibro(int id)
{
    try
    {
        var libro = await _unitOfWork.LibroRepository.FindAsync(id);
        
        if (libro == null)
        {
            return new BaseMessage<Libro>
            {
                Message = "Libro no encontrado.",
                StatusCode = HttpStatusCode.NotFound,
                ResponseElements = new List<Libro>()
            };
        }

        libro.Activo = false;  // libro inactivo
        await _unitOfWork.LibroRepository.Update(libro);
        await _unitOfWork.SaveAsync();

        return new BaseMessage<Libro>
        {
            StatusCode = HttpStatusCode.OK,
            Message = "Libro desactivado correctamente.",
            ResponseElements = new List<Libro> { libro }
        };
    }
    catch (Exception ex)
    {
        return new BaseMessage<Libro>
        {
            Message = $"[Exception]: {ex.Message}",
            StatusCode = HttpStatusCode.InternalServerError,
            ResponseElements = new List<Libro>()
        };
    }
}
/// desactivar libro **end**

/// activar libro **begin**

public async Task<BaseMessage<Libro>> ActivarLibro(int id)
{
    try
    {
        var libro = await _unitOfWork.LibroRepository.FindAsync(id);
        
        if (libro == null)
        {
            return new BaseMessage<Libro>
            {
                Message = "Libro no encontrado.",
                StatusCode = HttpStatusCode.NotFound,
                ResponseElements = new List<Libro>()
            };
        }

        libro.Activo = true;  // Reactivar libro
        await _unitOfWork.LibroRepository.Update(libro);
        await _unitOfWork.SaveAsync();

        return new BaseMessage<Libro>
        {
            StatusCode = HttpStatusCode.OK,
            Message = "Libro activado correctamente.",
            ResponseElements = new List<Libro> { libro }
        };
    }
    catch (Exception ex)
    {
        return new BaseMessage<Libro>
        {
            Message = $"[Exception]: {ex.Message}",
            StatusCode = HttpStatusCode.InternalServerError,
            ResponseElements = new List<Libro>()
        };
    }
}
/// activar libro **end**
///eliminar libro
public async Task<BaseMessage<Libro>> EliminarLibro(int id)
{
    try
    {
        var libro = await _unitOfWork.LibroRepository.FindAsync(id);

        if (libro == null)
        {
            return new BaseMessage<Libro>
            {
                Message = "Libro no encontrado.",
                StatusCode = HttpStatusCode.NotFound,
                ResponseElements = new()
            };
        }

        await _unitOfWork.LibroRepository.Delete(libro);
        await _unitOfWork.SaveAsync();

        return new BaseMessage<Libro>
        {
            Message = "Libro eliminado exitosamente.",
            StatusCode = HttpStatusCode.OK,
            ResponseElements = new() 
        };
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
//eliminar libro end

    public async Task<BaseMessage<Libro>> GetAllFilter(RequestFilterLibroDTO requestFilterLibroDTO)
    {
        try
        {
            var libros = await _unitOfWork.LibroRepository.GetAllAsync(
                l => l.Activo && (string.IsNullOrEmpty(requestFilterLibroDTO.Nombre) || l.Titulo.Contains(requestFilterLibroDTO.Nombre)) &&
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
}
