using Aeropuerto.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aeropuerto.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Vuelos : ContentPage
    {
        private string _url = "http://10.0.2.2:7168/";
        public Vuelos()
        {
            InitializeComponent();
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var respuesta =  await CargarVuelosAsync();
            if (!respuesta)
            {
                await DisplayAlert("Error", "Ha ocurrido un error al obtener vuelos", "Ok");
            }
        }

        private async Task<bool> CargarVuelosAsync()
        {
            bool respuesta = false;

            var vuelos = new List<Vuelo>();
            HttpClient client = new HttpClient();
            HttpResponseMessage reponse = await client.GetAsync(_url + "api/Vuelos/ObtenerVuelos");
            if (reponse.IsSuccessStatusCode)
            {
                string resp = await reponse.Content.ReadAsStringAsync();
                vuelos = JsonConvert.DeserializeObject<List<Vuelo>>(resp);
                listaVuelos.ItemsSource = vuelos;
                if (vuelos.Count == 0)
                    listaVuelos.IsVisible = false;
                else
                    listaVuelos.IsVisible = true;
                respuesta = true;
            }
            return respuesta;
        }

        private void btnAgregarVuelo_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushModalAsync(new AgregarVuelo());
        }

        private async void SwipeActualizarVuelo_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            var vuelo = item.BindingContext as Models.Vuelo;
            await this.Navigation.PushModalAsync(new AgregarVuelo(true, vuelo));
        }

        private async void SwipeBorrarVuelo_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            var vuelo = item.BindingContext as Models.Vuelo;
            var respuesta = await BorrarVueloAsync(vuelo.Id.Value);
            if (respuesta)
            {
                await DisplayAlert("Borrado", "Se ha borrado el vuelo con éxito", "Ok");
                await CargarVuelosAsync();
            }
            else
            {
                await DisplayAlert("Error", "Ha ocurrido un error al borrar", "Ok");
            }
        }

        private async Task<bool> BorrarVueloAsync(int IdVuelo)
        {
            bool respuesta = false;
            HttpClient client = new HttpClient();
            HttpResponseMessage reponse = await client.DeleteAsync(_url + "api/Vuelos/BorrarVuelos?idVuelo=" + IdVuelo);
            if (reponse.IsSuccessStatusCode)
            {
                respuesta = true;
            }
            return respuesta;
        }
    }
}