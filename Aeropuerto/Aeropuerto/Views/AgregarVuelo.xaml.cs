using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http.Headers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aeropuerto.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AgregarVuelo : ContentPage
	{
        private string _url = "http://10.0.2.2:7168/";
        bool actualizar = false;
        Models.Vuelo vuelo = new Models.Vuelo();
        public AgregarVuelo (bool actualizar = false, Models.Vuelo vuelo = null)
		{
            this.actualizar = actualizar;
            this.vuelo = vuelo;
			InitializeComponent();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (actualizar)
            {
                btnGuardar.IsVisible = false;
                btnGuardar.IsEnabled = false;
                TituloAgregar.IsVisible = false;
                btnActualizar.IsEnabled = true;
                btnActualizar.IsVisible = true;
                TituloActualizar.IsVisible = true;
                CargarDatos();
            }
        }

        private void CargarDatos()
        {
            Aerolinea.Text = vuelo.Aerolinea;
            Origen.Text = vuelo.Origen;
            Destino.Text = vuelo.Destino;
            FechaSalida.Date = vuelo.FechaSalida.Value;
            var horaSalida = new TimeSpan(vuelo.FechaSalida.Value.Hour, vuelo.FechaSalida.Value.Minute, vuelo.FechaSalida.Value.Second);
            HoraSalida.Time = horaSalida;
            FechaLlegada.Date = vuelo.FechaLlegada.Value;
            var horaLlegada = new TimeSpan(vuelo.FechaLlegada.Value.Hour, vuelo.FechaLlegada.Value.Minute, vuelo.FechaLlegada.Value.Second);
            HoraLlegada.Time = horaLlegada;
            PrecioVuelo.Text = Convert.ToString(vuelo.PrecioVuelo);
            NumeroAsientos.Text = Convert.ToString(vuelo.NumeroAsientos);
        }

        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
			var respuesta = await GuardarVueloAsync();
            if (respuesta)
            {
                await DisplayAlert("Guardado", "Se ha guardado con éxito", "Ok");
                await this.Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error", "Ha ocurrido un error al guardar", "Ok");
                await this.Navigation.PopModalAsync();
            }
        }

        private async Task<bool> GuardarVueloAsync()
        {   
            bool respuesta = false;

            string horaSalida = FechaSalida.Date.ToString("MM/dd/yyyy") + " " + HoraSalida.Time.ToString();
            string horaLlegada = FechaLlegada.Date.ToString("MM/dd/yyyy") + " " + HoraLlegada.Time.ToString();

            Models.Vuelo vuelo = new Models.Vuelo();
            vuelo.Aerolinea = Aerolinea.Text;
            vuelo.Origen = Origen.Text;
            vuelo.Destino = Destino.Text;
            vuelo.FechaSalida = Convert.ToDateTime(horaSalida);
            vuelo.FechaLlegada = Convert.ToDateTime(horaLlegada);
            vuelo.NumeroAsientos = Convert.ToInt32(NumeroAsientos.Text);
            vuelo.PrecioVuelo = Convert.ToDecimal(PrecioVuelo.Text);
            
            
            var json = JsonConvert.SerializeObject(vuelo);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(_url + "api/Vuelos/InsertarVuelos", stringContent);

            if (response.IsSuccessStatusCode)
            {
                respuesta = true;
            }

            return respuesta;
        }

        private async void btnCancelar_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PopModalAsync();
        }

        private async void btnActualizar_Clicked(object sender, EventArgs e)
        {
            var respuesta = await ActualizarVuelosAsync();
            if (respuesta)
            {
                await DisplayAlert("Actualizado", "Se ha actualizado con éxito", "Ok");
                await this.Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error", "Ha ocurrido un error al guardar", "Ok");
                await this.Navigation.PopModalAsync();
            }
        }

        private async Task<bool> ActualizarVuelosAsync()
        {
            bool respuesta = false;

            string horaSalida = FechaSalida.Date.ToString("MM/dd/yyyy") + " " + HoraSalida.Time.ToString();
            string horaLlegada = FechaLlegada.Date.ToString("MM/dd/yyyy") + " " + HoraLlegada.Time.ToString();

            Models.Vuelo vuelos = new Models.Vuelo();
            vuelos.Aerolinea = Aerolinea.Text;
            vuelos.Origen = Origen.Text;
            vuelos.Destino = Destino.Text;
            vuelos.FechaSalida = Convert.ToDateTime(horaSalida);
            vuelos.FechaLlegada = Convert.ToDateTime(horaLlegada);
            vuelos.NumeroAsientos = Convert.ToInt32(NumeroAsientos.Text);
            vuelos.PrecioVuelo = Convert.ToDecimal(PrecioVuelo.Text);
            vuelos.IdVuelo = vuelo.Id;
            var json = JsonConvert.SerializeObject(vuelos);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+

            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.PutAsync(_url + "api/Vuelos/ActualizarVuelos",stringContent);

            if (response.IsSuccessStatusCode)
            {
                respuesta = true;
            }

            return respuesta;
        }
    }
}