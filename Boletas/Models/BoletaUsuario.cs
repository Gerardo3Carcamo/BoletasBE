using System.ComponentModel.DataAnnotations.Schema;
namespace Boletas.Models
{
    [Table("BoletaUsuario")]
    public class BoletaUsuario
    {
        public int Id { get; set; }
        public int IdBoleta { get; set; }
        public string Nomina { get; set; } = null!;
        public string NombreUsuario { get; set; } = null!;
        public string Direccion { get; set; } = null!;
    }
}