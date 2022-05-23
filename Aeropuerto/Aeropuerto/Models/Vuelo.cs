using System;
using System.Collections.Generic;
using System.Text;

namespace Aeropuerto.Models
{
    public class Vuelo
    {
        public int? Id { get; set; }
        public string Aerolinea { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; } 
        public DateTime? FechaSalida { get; set; }
        public DateTime? FechaLlegada { get; set;}
        public int? AsientosDisponibles { get; set; }
        public decimal? PrecioVuelo { get; set; }
        public int? NumeroAsientos { get; set; }
        public int? IdVuelo { get; set; }
     }
}
