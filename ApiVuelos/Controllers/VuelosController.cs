using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ApiVuelos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VuelosController : Controller
    {
          /// <summary>
          /// Obtener Vuelos (MVuelo)
          /// </summary>
          /// <returns></returns>
        [HttpGet("ObtenerVuelos")]
        public ActionResult ObtenerVuelos()
        {
            using (Models.DataVuelosContext db = new Models.DataVuelosContext())
            {
                var listaVuelos = db.Vuelos
                                    //.Where(s => s.NumeroAsientos > 0)
                                    .ToList();

                return Ok(listaVuelos);
            }
        }

        /// <summary>
        /// Agregar Vuelos (MVuelo)
        /// </summary>
        /// <param name="vuelo"></param>
        /// <returns></returns>
        [HttpPost("InsertarVuelos")]
        public ActionResult InsertarVuelos([FromBody] Models.Request.Vuelo vuelo)
        {
            using (Models.DataVuelosContext db = new Models.DataVuelosContext())
            {
                Models.Vuelo oVuelo = new Models.Vuelo();
                oVuelo.Aerolinea = vuelo.Aerolinea;
                oVuelo.Origen = vuelo.Origen;
                oVuelo.Destino = vuelo.Destino;
                oVuelo.FechaSalida = vuelo.FechaSalida.Value;
                oVuelo.FechaLlegada = vuelo.FechaLlegada.Value;
                oVuelo.NumeroAsientos = vuelo.NumeroAsientos.Value;
                oVuelo.PrecioVuelo = vuelo.PrecioVuelo.Value;
                oVuelo.AsientosDisponibles = vuelo.NumeroAsientos.Value;
                //db.SaveChanges();

                var costoAsiento = vuelo.PrecioVuelo.Value / vuelo.NumeroAsientos.Value;

                var claveVuelo = vuelo.Aerolinea.Substring(0, 3) + "-" + oVuelo.Origen.Substring(0,3) + "-" + oVuelo.Destino.Substring(0,3);

                for (int i = 1; i <= vuelo.NumeroAsientos; i++)
                {
                    Models.DetalleVuelo detalleVuelo = new Models.DetalleVuelo();
                    //detalleVuelo.IdVuelo = oVuelo.Id;
                    detalleVuelo.Asiento = claveVuelo + "-" + Convert.ToString(i);
                    detalleVuelo.PrecioAsiento = costoAsiento;
                    detalleVuelo.AsientoDisponible = true;
                    oVuelo.DetalleVuelos.Add(detalleVuelo);
                }

                db.Vuelos.Add(oVuelo);
                db.SaveChanges();


            }

            return Ok();
        }

        /// <summary>
        /// Actualizar Vuelos (MVuelo)
        /// </summary>
        /// <param name="vuelo"></param>
        /// <returns></returns>
        [HttpPut("ActualizarVuelos")]
        public ActionResult ActualizarVuelos([FromBody] Models.Request.Vuelo vuelo)
        {
            if (vuelo.idVuelo != null && vuelo != null && vuelo.idVuelo != 0)
            {

                using (Models.DataVuelosContext db = new Models.DataVuelosContext())
                {
                    var actualizarVuelo = db.Vuelos
                                             .Where(v => v.Id == vuelo.idVuelo)
                                             .FirstOrDefault();
                    if (actualizarVuelo != null)
                    {

                        var asientos = actualizarVuelo.NumeroAsientos;
                        var asientosDisponibles = actualizarVuelo.AsientosDisponibles;

                        actualizarVuelo.Aerolinea = vuelo.Aerolinea ?? actualizarVuelo.Aerolinea;   
                        actualizarVuelo.Origen = vuelo.Origen ?? actualizarVuelo.Origen;
                        actualizarVuelo.Destino = vuelo.Destino ?? actualizarVuelo.Destino;
                        actualizarVuelo.FechaSalida = vuelo.FechaSalida ?? actualizarVuelo.FechaSalida;
                        actualizarVuelo.FechaLlegada = vuelo.FechaLlegada ?? actualizarVuelo.FechaLlegada;
                        actualizarVuelo.NumeroAsientos = vuelo.NumeroAsientos ?? actualizarVuelo.NumeroAsientos;
                        actualizarVuelo.PrecioVuelo = vuelo.PrecioVuelo ?? actualizarVuelo.PrecioVuelo;
                        actualizarVuelo.AsientosDisponibles = vuelo.NumeroAsientos ?? actualizarVuelo.NumeroAsientos;


                        if (((vuelo.NumeroAsientos != 0 && vuelo.NumeroAsientos != null) ||
                            (actualizarVuelo.PrecioVuelo != 0 && vuelo.PrecioVuelo != null)) &&
                            asientosDisponibles == asientos)
                        {

                            var datosEliminados = db.DetalleVuelos
                                .Where(v => v.IdVuelo == vuelo.idVuelo)
                                .ToList();

                            foreach (var item in datosEliminados)
                            {
                                db.DetalleVuelos.Remove(item);
                            }

                            actualizarVuelo.DetalleVuelos = new List<Models.DetalleVuelo>();

                            var claveVuelo = actualizarVuelo.Aerolinea.Substring(0, 3) + "-" + actualizarVuelo.Origen.Substring(0, 3) + "-" + actualizarVuelo.Destino.Substring(0, 3);
                            var costoAsiento = actualizarVuelo.PrecioVuelo / actualizarVuelo.NumeroAsientos;

                            for (int i = 1; i <= actualizarVuelo.NumeroAsientos; i++)
                            {
                                Models.DetalleVuelo detalleVuelo = new Models.DetalleVuelo();
                                detalleVuelo.Asiento = claveVuelo + "-" + Convert.ToString(i);
                                detalleVuelo.PrecioAsiento = costoAsiento;
                                detalleVuelo.AsientoDisponible = true;
                                //detalleVuelo.IdVuelo = actualizarVuelo.Id;
                                actualizarVuelo.DetalleVuelos.Add(detalleVuelo);
                            }

                            db.Vuelos.Update(actualizarVuelo);

                        }
                        else if (vuelo.Aerolinea != null)
                        {
                            var detalleVuelo = db.DetalleVuelos
                                                .Where(v => v.IdVuelo == vuelo.idVuelo)
                                                .ToList();

                            foreach (var item in detalleVuelo)
                            {
                                var length = item.Asiento.Length - 3;
                                item.Asiento = vuelo.Aerolinea.Substring(0, 3) + item.Asiento.Substring(3, length);
                                db.DetalleVuelos.Update(item);
                            }
                        }
                        db.SaveChanges();
                    }
                    return Ok();
                }
            }
            else { return BadRequest(); }
        }

        /// <summary>
        /// Borrar Vuelo (MVuelo)
        /// </summary>
        /// <param name="idVuelo"></param>
        /// <returns></returns>
        [HttpDelete("BorrarVuelos")]
        public ActionResult BorrarVuelos(int idVuelo)
        {
            using (Models.DataVuelosContext db = new Models.DataVuelosContext())
            {
                var datosEliminados = db.Vuelos
                                .Where(v => v.Id == idVuelo)
                                .Include(v => v.DetalleVuelos)
                                .FirstOrDefault();

                foreach (var item in datosEliminados.DetalleVuelos)
                {
                    db.DetalleVuelos.Remove(item);
                }

                db.Vuelos.Remove(datosEliminados);

                db.SaveChanges();

                return Ok();
            }
        }


        /// <summary>
        /// Obtener Vuelos Disponibles(MCompra)
        /// </summary>
        /// <returns></returns>
        [HttpGet("ObtenerVuelosDisponibles")]
        public ActionResult ObtenerVuelosDisponibles()
        {
            using (Models.DataVuelosContext db = new Models.DataVuelosContext())
            {
                var listaVuelos = db.Vuelos
                                    .Where(s => s.NumeroAsientos > 0)
                                    .ToList();

                return Ok(listaVuelos);
            }
        }

        /// <summary>
        /// Agregar Compra y Actualizar DetalleVuelo (MCompra)
        /// </summary>
        /// <param name="compra"></param>
        /// <returns></returns>
        [HttpPost("AgregarCompra")]
        public ActionResult AgregarCompra([FromBody] Models.Request.Compra compra)
        {
            using (Models.DataVuelosContext db = new Models.DataVuelosContext())
            {
                var noAsientos = compra.NumeroAsientos.Value;

                var listaVuelos = db.DetalleVuelos
                            .OrderByDescending(c => c.Id)
                            .Where(c => c.AsientoDisponible == true)
                            .Where(c => c.IdVuelo == compra.idVuelo)
                            .Take(noAsientos)
                            .ToList();


                if (listaVuelos.Count() == noAsientos)
                {
                    var listAsientos = new List<string>();
                    decimal costoTotal = 0;

                    foreach (var item in listaVuelos)
                    {
                        listAsientos.Add(item.Asiento);
                        costoTotal = costoTotal + item.PrecioAsiento;
                        item.AsientoDisponible = false;
                    }


                    Models.Compra oCompra = new Models.Compra();
                    oCompra.Usuario = compra.Usuario.Value;
                    oCompra.Asientos = string.Join(", ", listAsientos.ToArray());
                    oCompra.CostoTotal = costoTotal;
                    oCompra.DetalleVuelos = listaVuelos;

                    db.Compras.Update(oCompra);

                    db.SaveChanges();

                    return Ok();
                }
                else { return BadRequest(); }


            }
        }


        /// <summary>
        /// Obtener Información Vuelos Usuario
        /// </summary>
        /// <param name="Usuario"></param>
        /// <returns></returns>
        [HttpGet("ObtenerVuelosUsuario")]
        public ActionResult ObtenerVuelosUsuario(int Usuario)
        {
            using (Models.DataVuelosContext db = new Models.DataVuelosContext())
            {
                var listaCompras = db.Compras
                                    .Where(s => s.Usuario == Usuario)
                                     .Include(s => s.DetalleVuelos)
                                    .ToList();

                var json = JsonConvert.SerializeObject(listaCompras, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });

                return Ok(json);
            }
        }

        [HttpDelete("CancelarCompra")]
        public ActionResult CancelarCompra(int idCompra)
        {
            using (Models.DataVuelosContext db = new Models.DataVuelosContext())
            {
                var comprasEliminadas = db.Compras
                             .Where(v => v.Id == idCompra)
                             .Include(v => v.DetalleVuelos)
                             .FirstOrDefault();

                foreach (var item in comprasEliminadas.DetalleVuelos)
                {
                    item.IdCompra = null;
                    item.AsientoDisponible = true;
                    db.DetalleVuelos.Update(item);
                }

                db.Compras.Remove(comprasEliminadas);

                db.SaveChanges();

                return Ok();
            }
        }
    }
}
