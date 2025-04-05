using System.Net;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class LibroService : ILibroService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEstadisticaService _estadisticaService;
    private readonly IFileService _fileService;

    public LibroService(IUnitOfWork unitOfWork,IEstadisticaService estadisticaService,IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _estadisticaService = estadisticaService;
        _fileService = fileService;
    }

    public async Task<BaseMessage<Libro>> GetAll()
    {
        try
        {                                                                               // se añade activo para habilitar o inabi libros
            IEnumerable<Libro> libros = await _unitOfWork.LibroRepository.GetAllAsync(l => (bool)l.Activo);
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

    public async Task<BaseMessage<Libro>> AddLibro(RequestLibroAddDTO libro)
    {
        var isValid = ValidateModel(libro);
        if (!string.IsNullOrEmpty(isValid))
        {
            return BuildResponse(null, isValid, HttpStatusCode.BadRequest);
        }


        Libro? libroSave = null;

        try
        {

           var autor = await _unitOfWork.AutorRepository.FindAsync(libro.AutorId);

            if(autor == null){
                return this.BuildResponse(null, "Autor no valido", HttpStatusCode.BadRequest);
            }

            var resultFile = _fileService.SaveFileLibroBase64(libro.Base64File);

            if(resultFile == null){
                return this.BuildResponse(null, "Archvio no procesado", HttpStatusCode.BadRequest);
            }

            libroSave = new(){
                Titulo = libro.Titulo,
                ISBN13 = libro.ISBN13,
                Editorial = libro.Editorial,
                AnioPublicacion = libro.AnioPublicacion, 
                Formato  = libro.Formato,
                Genero  = libro.Genero,
                Idioma  = libro.Idioma,
                Portada  = libro.Portada,
                Edicion = libro.Edicion,
                Activo    = libro.Activo,
                ContraPortada = libro.ContraPortada, 
                AutorId  = libro.AutorId,
                Path = resultFile.Path,
                Autor = autor, 
            };

            await _unitOfWork.LibroRepository.AddAsync(libroSave);
            await _unitOfWork.SaveAsync();

            //Crea el registro de estadistica
            await SaveEstadisticaAsync(libroSave.Id);
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

        return BuildResponse(new List<Libro> { libroSave }, "", HttpStatusCode.OK);
    }

    public async Task<BaseMessage<Libro>> UpdateLibro(RequestLibroAddDTO libro)
    {
        var isValid = ValidateModel(libro);
        if (!string.IsNullOrEmpty(isValid))
        {
            return BuildResponse(null, isValid, HttpStatusCode.BadRequest);
        }

        Libro? libroUpdate = null;

        try
        {
             var libroibroBD = await _unitOfWork.LibroRepository.FindAsync(x => x.Id == libro.Id);

            if(libroibroBD == null){
                return this.BuildResponse(null, "libro no encontrado", HttpStatusCode.BadRequest);
            }

            var autor = await _unitOfWork.AutorRepository.FindAsync(libro.AutorId);

            if(autor == null){
                return this.BuildResponse(null, "Autor no valido", HttpStatusCode.BadRequest);
            }

            var resultFile = _fileService.SaveFileLibroBase64(libro.Base64File);

            if(resultFile == null){
                return this.BuildResponse(null, "Archvio no procesado", HttpStatusCode.BadRequest);
            }

            var resultDeleteFile = _fileService.DeleteFile(libroibroBD.Path);

            if(!resultDeleteFile){
                return this.BuildResponse(null, "Error actualizando libro", HttpStatusCode.BadRequest);
            }

            libroUpdate = new(){
                Id = libro.Id,
                Titulo = libro.Titulo,
                ISBN13 = libro.ISBN13,
                Editorial = libro.Editorial,
                AnioPublicacion = libro.AnioPublicacion, 
                Formato  = libro.Formato,
                Genero  = libro.Genero,
                Idioma  = libro.Idioma,
                Portada  = libro.Portada,
                Edicion = libro.Edicion,
                Activo    = libro.Activo,
                ContraPortada = libro.ContraPortada, 
                AutorId  = libro.AutorId,
                Autor = autor, 
                Path = resultFile.Path,
            };

            await _unitOfWork.LibroRepository.Update(libroUpdate);
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

        return BuildResponse(new List<Libro> { libroUpdate }, "", HttpStatusCode.OK);
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

        //Elimina estadistica
        await DeleteEstadisticaAsync(id);

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
                l => (bool)l.Activo && (string.IsNullOrEmpty(requestFilterLibroDTO.Nombre) || l.Titulo.Contains(requestFilterLibroDTO.Nombre)) &&
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

    private string ValidateModel(RequestLibroAddDTO libro)
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
