using System.ComponentModel.DataAnnotations.Schema;

namespace Boletas.Models
{
    [Table("Unidad")]
    public class Unidad
    {
        public int Id { get; set; }
        public string NumeroUnidad { get; set; } = null!;
        public string Marca { get; set; } = null!;
        public string Modelo { get; set; } = null!;
    }
}