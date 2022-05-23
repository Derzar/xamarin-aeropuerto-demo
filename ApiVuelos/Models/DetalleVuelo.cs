using System;
using System.Collections.Generic;

namespace ApiVuelos.Models
{
    public partial class DetalleVuelo
    {
        public int Id { get; set; }
        public string Asiento { get; set; } = null!;
        public decimal PrecioAsiento { get; set; }
        public bool AsientoDisponible { get; set; }
        public int IdVuelo { get; set; }
        public int? IdCompra { get; set; }

        public virtual Compra? IdCompraNavigation { get; set; }
        public virtual Vuelo IdVueloNavigation { get; set; } = null!;
    }
}
