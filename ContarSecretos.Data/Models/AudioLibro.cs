using System.ComponentModel.DataAnnotations.Schema;

public class AudioLibro : BaseEntity<int>
{
    public string Titulo { get; set; }
    public string Genero { get; set; }
    public int NarradorId { get; set; }
    public string Duracion { get; set; }
    public int Tamanio { get; set; }
    public string Path { get; set; }
    [ForeignKey("AutorId")]
    public int AutorId { get; set; }
    
    public virtual Autor Autor{ get; set; }
}