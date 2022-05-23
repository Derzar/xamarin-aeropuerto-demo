namespace ApiVuelos.Models.Request
{
    public partial class Vuelo
    {
        public string? Aerolinea { get; set; }
        public string? Origen { get; set; }
        public string? Destino { get; set; }
        public DateTime? FechaSalida { get; set; }
        public DateTime? FechaLlegada { get; set; }
        public int? NumeroAsientos { get; set; }
        public decimal? PrecioVuelo { get; set; }
        public int? idVuelo { get; set; }
    }
}
