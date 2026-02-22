using Boletas.Models;

namespace Boletas.DTOs
{
    public class BoletaDto
    {
        public Boleta Boletas { get; set; }
        public List<Usuario> Usuarios { get; set; }
    }
}
