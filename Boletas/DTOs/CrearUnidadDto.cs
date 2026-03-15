namespace Boletas.DTOs
{
    public class CrearUnidadDto
    {
        public int Marca { get; set; }
        public string Modelo { get; set; } = null!;
        public int Anio { get; set; }
        public string NumeroUnidad { get; set; } = null!;
    }
}
