using System;
using System.Collections.Generic;

namespace ApiVuelos.Models
{
    public partial class Compra
    {
        public Compra()
        {
            DetalleVuelos = new HashSet<DetalleVuelo>();
        }

        public int Id { get; set; }
        public int Usuario { get; set; }
        public string Asientos { get; set; } = null!;
        public decimal CostoTotal { get; set; }

        public virtual ICollection<DetalleVuelo> DetalleVuelos { get; set; }
    }
}
