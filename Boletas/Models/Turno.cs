using System.ComponentModel.DataAnnotations.Schema;

namespace Boletas.Models
{
    [Table("Turno")]
    public class Turno
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = null!;
    }
}