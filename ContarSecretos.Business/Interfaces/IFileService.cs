public interface IFileService
{
    FileDTO? SaveFileAudioLibroBase64(string base64);
    FileDTO? SaveFileLibroBase64(string base64);
    string? GetFileByName(string name);
    bool DeleteFile(string name);
}