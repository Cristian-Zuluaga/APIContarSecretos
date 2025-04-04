using System.Text.RegularExpressions;

public class FileService : IFileService
{
    public FileDTO? SaveFileAudioLibroBase64(string base64)
    {
        try{

            if (IsAudioBase64(base64, out string extension, out string pureBase64))
            {
                // Convertir Base64 a bytes
                byte[] fileBytes = Convert.FromBase64String(pureBase64);

                string saveDirectory = Path.Combine(AppContext.BaseDirectory, "Files");

                // Crear la carpeta si no existe
                if (!Directory.Exists(saveDirectory))
                    Directory.CreateDirectory(saveDirectory);

                // Generar un nombre de archivo único
                string fileName = $"Audio_{DateTime.Now:yyyyMMdd_HHmmss}.{extension}";
                string filePath = Path.Combine(saveDirectory, fileName);

                string pathBd = Path.Combine("Files", fileName);

                // Guardar el archivo
                File.WriteAllBytes(filePath, fileBytes);

                long fileSize = new FileInfo(filePath).Length;

                return new(){
                    Name = fileName,
                    Extension = extension,
                    Path = pathBd,
                    Size = fileSize
                };
            }
            else
            {
                return null;
            }  

        }catch(Exception ex){
            return null;
        }
    }

    string GetMimeType(string filePath)
    {
        var mimeTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { ".txt", "text/plain" },
            { ".pdf", "application/pdf" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            { ".mp3", "audio/mpeg" },
            { ".mp4", "video/mp4" },
            { ".json", "application/json" },
            { ".html", "text/html" },
            { ".csv", "text/csv" },
            { ".xml", "application/xml" },
            { ".zip", "application/zip" }
        };

        string extension = Path.GetExtension(filePath);
        return mimeTypes.ContainsKey(extension) ? mimeTypes[extension] : "application/octet-stream";
    }

    private static bool IsAudioBase64(string base64, out string extension, out string pureBase64)
    {
        var match = Regex.Match(base64, @"^data:(?<type>audio/[^;]+);base64,(?<data>.+)$");
        if (match.Success)
        {
            string mimeType = match.Groups["type"].Value;
            pureBase64 = match.Groups["data"].Value;

            extension = mimeType switch
            {
                "audio/mpeg" => "mp3",
                "audio/wav" => "wav",
                "audio/ogg" => "ogg",
                "audio/flac" => "flac",
                "audio/aac" => "aac",
                _ => "desconocido"
            };

            return extension != "desconocido";
        }

        pureBase64 = base64; // Si no tiene prefijo, se asume binario
        extension = "bin";
        return false;
    }
    
    private static bool IsDocumentBase64(string base64, out string extension, out string pureBase64)
    {
        var match = Regex.Match(base64, @"^data:(?<type>application/pdf);base64,(?<data>.+)$");
        if (match.Success)
        {
            string mimeType = match.Groups["type"].Value;
            pureBase64 = match.Groups["data"].Value;

            extension = mimeType switch
            {
               "application/pdf" => "pdf",
                _ => "desconocido"
            };

            return extension != "desconocido";
        }

        pureBase64 = base64; // Si no tiene prefijo, se asume binario
        extension = "bin";
        return false;
    }
    
    public FileDTO? SaveFileLibroBase64(string base64)
    {
         try{

            if (IsDocumentBase64(base64, out string extension, out string pureBase64))
            {
                // Convertir Base64 a bytes
                byte[] fileBytes = Convert.FromBase64String(pureBase64);

                string saveDirectory = Path.Combine(AppContext.BaseDirectory, "Files");

                // Crear la carpeta si no existe
                if (!Directory.Exists(saveDirectory))
                    Directory.CreateDirectory(saveDirectory);

                // Generar un nombre de archivo único
                string fileName = $"Libro_{DateTime.Now:yyyyMMdd_HHmmss}.{extension}";
                string filePath = Path.Combine(saveDirectory, fileName);

                string pathBd = Path.Combine("Files", fileName);

                // Guardar el archivo
                File.WriteAllBytes(filePath, fileBytes);

                long fileSize = new FileInfo(filePath).Length;

                return new(){
                    Name = fileName,
                    Extension = extension,
                    Path = pathBd,
                    Size = fileSize
                };
            }
            else
            {
                return null;
            }  

        }catch(Exception ex){
            return null;
        }
    
    }
    public string? GetFileByName(string name)
    {
        try{


            char separator = name.Contains('/') ? '/' : '\\'; // Detecta el separador usado
            
            int index = name.IndexOf(separator);
            string result = (index != -1) ? name.Substring(index + 1) : name;

            string path = Path.Combine(AppContext.BaseDirectory, "Files");
            string fileNameToFind = result;

            // Crear la carpeta si no existe
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string[] files = Directory.GetFiles(path, fileNameToFind, SearchOption.TopDirectoryOnly);

            if (files.Length > 0)
            {
                string filePath = files[0];
                byte[] fileBytes = File.ReadAllBytes(filePath);
                string base64String = Convert.ToBase64String(fileBytes);
                
                 // Obtener la extensión y el MIME type
                string extension = Path.GetExtension(filePath);
                string mimeType = this.GetMimeType(filePath);

                // Retornar el formato correcto
                return $"data:{mimeType};base64,{base64String}";
            }
            else
            {
                return null;
            }
        }catch(Exception ex){
            return null;
        }
    }

    public bool DeleteFile(string name)
    {
        try{


            char separator = name.Contains('/') ? '/' : '\\'; // Detecta el separador usado
            
            int index = name.IndexOf(separator);
            string result = (index != -1) ? name.Substring(index + 1) : name;

            string path = Path.Combine(AppContext.BaseDirectory, "Files");
            string fileNameToFind = result;

            // Crear la carpeta si no existe
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string[] files = Directory.GetFiles(path, fileNameToFind, SearchOption.TopDirectoryOnly);

            if (files.Length > 0)
            {
                string filePath = files[0]; // Tomar el primer archivo encontrado
                File.Delete(filePath); // Eliminar el archivo
                return true;
            }
            else
            {
                return false;
            }
        }catch(Exception ex){
            return false;
        } 
    }
}