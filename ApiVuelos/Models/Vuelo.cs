using System;
using System.Collections.Generic;

namespace ApiVuelos.Models
{
    public partial class Vuelo
    {
        public Vuelo()
        {
            DetalleVuelos = new HashSet<DetalleVuelo>();
        }

        public int Id { get; set; }
        public string Aerolinea { get; set; } = null!;
        public string Origen { get; set; } = null!;
        public string Destino { get; set; } = null!;
        public DateTime FechaSalida { get; set; }
        public DateTime FechaLlegada { get; set; }
        public int NumeroAsientos { get; set; }
        public decimal PrecioVuelo { get; set; }
        public int AsientosDisponibles { get; set; }

        public virtual ICollection<DetalleVuelo> DetalleVuelos { get; set; }
    }
}
