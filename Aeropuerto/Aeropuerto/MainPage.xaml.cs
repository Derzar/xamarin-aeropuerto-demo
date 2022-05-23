using Aeropuerto.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Aeropuerto
{
    public partial class MainPage : ContentPage
    {
       // private string _url = "https://localhost:7168/api/Vuelos/ObtenerVuelos";
        public MainPage()
        {
            InitializeComponent();
        }
        private void btnEntrar_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new HomePage();
            Navigation.PushAsync(new HomePage());
        }
    }
}
