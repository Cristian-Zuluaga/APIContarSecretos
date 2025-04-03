using System.ComponentModel.DataAnnotations.Schema;

public class Estadistica : BaseEntity<int> 
{
    [ForeignKey("AudioLibroId")]
    public int? AudioLibroId { get; set; }
    [ForeignKey("LibroId")]
    public int? LibroId { get; set; }
    public int CountDescargas { get; set; }
    public int CountEscuchado { get; set; }
    public int CountLeido { get; set; }
    public virtual AudioLibro? AudioLibro{ get; set; } = new AudioLibro();
    public virtual Libro? Libro{ get; set; } = new Libro();
}