public class RequestAudioLibroDTO{
     public int Id {get;set;}
    public string Titulo { get; set; }
    public string Genero { get; set; }
    public int NarradorId { get; set; }
    public string Duracion { get; set; }
    public int AutorId { get; set; }
    public string Base64File { get; set; }
}