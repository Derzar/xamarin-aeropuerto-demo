namespace ApiVuelos.Models.xD
{
    public class DetalleVuelo
    {
        public int Id { get; set; }
        public string Asiento { get; set; }
        public decimal PrecioAsiento { get; set; }
        public bool AsientoDisponible { get; set; }
        public int idVuelo { get; set; }
        public int? idCompra { get; set; }
    }
}
