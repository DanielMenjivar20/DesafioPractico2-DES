using System.ComponentModel.DataAnnotations;

namespace LibrosAPI.Models;

public class Libro
{
    public int Id { get; set; }

    [Required]
    public string Titulo { get; set; } = string.Empty;

    public string? Autor { get; set; }

    public int AnioPublicacion { get; set; }
}