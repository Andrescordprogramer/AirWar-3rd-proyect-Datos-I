using Microsoft.Maui.Controls;
using System.Net.Sockets;
using AirWar.Server;

namespace AirWar
{
    public partial class MainPage : ContentPage
    {
        public TcpServer Server;

        public MainPage()
        {
            InitializeComponent();
            ConnectToServer();
        }

        // Evento para iniciar el juego
        private void OnIniciarJuegoClicked(object sender, EventArgs e)
        {
            // Crear una instancia de TcpServer y pasarle la instancia de JuegoPage
            var juegoPage = new JuegoPage(Server);
            Server = new TcpServer(juegoPage);

            // Aquí navegamos a la página de juego o mapa
            Navigation.PushAsync(juegoPage); // Redirige a la página del juego
        }

        // Evento para salir del juego
        private void OnSalirClicked(object sender, EventArgs e)
        {
            // Cerramos la aplicación
            System.Environment.Exit(0);
        }

        // Conectar al servidor en la Raspberry Pi Pico W
        private async void ConnectToServer()
        {
            string ipAddress = "192.168.0.140"; // IP de la Raspberry Pi Pico W
            int port = 12345; // Puerto de la Raspberry Pi
            Server = new TcpServer(null); // Inicializar Server con un valor por defecto
            await Server.ConectarServer_Raspberry(ipAddress, port);
        }
    }
}
