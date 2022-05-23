namespace ApiVuelos.Models.xD
{
    public class Vuelos
    {
        public int Id { get; set; }
        public string Aerolinea { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaLlegada { get; set; }
        public int NumeroAsientos { get; set; }
        public decimal PrecioVuelo { get; set; }
        public int AsientosDisponibles { get; set; }
    }
}
