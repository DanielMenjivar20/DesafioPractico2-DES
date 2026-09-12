using System.ComponentModel.DataAnnotations;

namespace PersonasAPI.Models
{
    public class Persona
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El DUI es obligatorio.")]
        [RegularExpression(@"^\d{8}-\d{1}$", ErrorMessage = "El formato del DUI debe ser 00000000-0.")]
        public string Dui { get; set; }

        public DateTime FechaNacimiento { get; set; }
    }
}