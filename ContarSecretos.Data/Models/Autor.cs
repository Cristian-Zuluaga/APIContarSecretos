public class Autor : BaseEntity<int>
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Pseudonimos { get; set; }
    public DateOnly FechaNacimiento { get; set; }
    public string Pais { get; set; }
    public string Nacionalidad { get; set; }
    public bool EstaVivo { get; set; }
    public int FechaMuerte { get; set; }
    public string Idiomas { get; set; }
    public string Generos { get; set; }
    public string Biografia { get; set; }
    public string Galardones { get; set; }
    public bool EstaActivo { get; set; }

    public static implicit operator Autor(BaseMessage<Autor> v)
    {
        throw new NotImplementedException();
    }
}