using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics.Text;


//server
namespace AirWar.Server
{
    public partial class TcpServer: ContentPage
    {
        TcpClient client;
        NetworkStream stream;
        private JuegoPage JuegoPage;

        public TcpServer(JuegoPage juegoPage)
        {
            JuegoPage = juegoPage;
        }


        // Conectar al servidor en la Raspberry Pi Pico W
        public async Task ConectarServer_Raspberry(string ipAddres, int port)
        {
            client = new TcpClient();
            await client.ConnectAsync(ipAddres, port); // Conectar al servidor
            stream = client.GetStream();
            await Task.Run(RecibirMensaje); // Iniciar la tarea de recepción
        }


        // Método para enviar mensajes (público)
        public async Task EnviarMensaje(string mensaje)
        {
            if (stream != null && stream.CanWrite)
            {
                byte[] mensajeEnBytes = Encoding.UTF8.GetBytes(mensaje);
                await stream.WriteAsync(mensajeEnBytes, 0, mensajeEnBytes.Length);
            }
        }


        // Método para enviar mensajes (público)
        private async void RecibirMensaje()
        {
            byte[] mensajeEnBytes = new byte[1024];
            while (true)
            {
                int bytesLeidos = await stream.ReadAsync(mensajeEnBytes, 0, mensajeEnBytes.Length);
                string mensajeDecodificado = Encoding.ASCII.GetString(mensajeEnBytes, 0, bytesLeidos);
                ProcesarMensaje(mensajeDecodificado);// Procesar el mensaje recibido
            }
        }


        // Procesar el mensaje recibido
        public void ProcesarMensaje(string mensaje)
        {
            int fuerzaDisparo = int.Parse(mensaje);
            if (fuerzaDisparo >= 0 && fuerzaDisparo <= 100)
            {
                JuegoPage.DispararRasp(fuerzaDisparo);
            }
        }
    }
}