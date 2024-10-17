using Microsoft.Maui.Controls;

namespace AirWar
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // Evento para iniciar el juego
        private void OnIniciarJuegoClicked(object sender, EventArgs e)
        {
            // Aquí navegamos a la página de juego o mapa
            Navigation.PushAsync(new JuegoPage()); // Redirige a la página del juego (debes crearla)
        }

        // Evento para salir del juego
        private void OnSalirClicked(object sender, EventArgs e)
        {
            // Cerramos la aplicación
            System.Environment.Exit(0);
        }
    }
}
