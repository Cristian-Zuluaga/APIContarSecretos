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
    public FileDTO? SaveFileLibroBase64(string base64)
    {
        throw new NotImplementedException();
    }
    public string? GetFileByName(string name)
    {
        throw new NotImplementedException();
    }
}