using System.ComponentModel.DataAnnotations.Schema;

namespace Boletas.Models
{
    [Table("Marca")]
    public class Marca
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Imagen { get; set; }
    }
}
