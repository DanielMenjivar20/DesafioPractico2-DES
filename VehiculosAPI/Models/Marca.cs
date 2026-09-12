using System.Text.Json.Serialization;

namespace VehiculosAPI.Models
{
    public class Marca
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Modelo> Modelos { get; set; } = new List<Modelo>();
    }
}