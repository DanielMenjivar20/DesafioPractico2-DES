using System.Text.Json.Serialization;

namespace VehiculosAPI.Models
{
    public class Modelo
    {
        public int Id { get; set; }
        public int MarcaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public Marca Marca { get; set; } = null!;

        [JsonIgnore]
        public ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
    }
}