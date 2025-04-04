public class RequestLibroAddDTO{
    public int Id {get;set;}
    public string Titulo { get; set; }
    public string ISBN13 { get; set; }
    public string Editorial { get; set; }
    public int AnioPublicacion { get; set; }
    public string Formato { get; set; }
    public string Genero { get; set; }
    public string Idioma { get; set; }
    public string Portada { get; set; }
    public string Edicion { get; set; }
    public bool Activo { get; set; } = true;  // se añade el campo  para inactivar libro
    public string ContraPortada { get; set; }
    public int AutorId { get; set; }
    public string Base64File { get; set; }
}