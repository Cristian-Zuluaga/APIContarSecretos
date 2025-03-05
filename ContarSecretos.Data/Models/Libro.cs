using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Libro : BaseEntity<int>
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "El ISBN es obligatorio.")]
    [RegularExpression(@"^\d{13}$", ErrorMessage = "El ISBN debe tener 13 dígitos.")]
    public string ISBN13 { get; set; }

    [Required(ErrorMessage = "La editorial es obligatoria.")]
    public string Editorial { get; set; }

    [Range(1450, 2026, ErrorMessage = "El año de publicación debe estar entre 1450 y 2026.")]
    public int AnioPublicacion { get; set; }

    [Required(ErrorMessage = "El formato es obligatorio.")]
    public string Formato { get; set; }

    [Required(ErrorMessage = "El género es obligatorio.")]
    public string Genero { get; set; }

    [Required(ErrorMessage = "El idioma es obligatorio.")]
    [RegularExpression(@"^[a-z]{2}(-[A-Z]{2})?$", ErrorMessage = "El idioma debe estar en formato de código oficial, ej: es, en-US.")]
    public string Idioma { get; set; }

    public string Portada { get; set; }

    public string Edicion { get; set; }

    [MaxLength(500, ErrorMessage = "La contraportada no puede tener más de 500 palabras.")]
    public string ContraPortada { get; set; }

    [ForeignKey("AutorId")]
    public int AutorId { get; set; }

    public virtual Autor Autor { get; set; }
}