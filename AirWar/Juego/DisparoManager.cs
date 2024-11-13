using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AirWar
{
    public class DisparoManager
    {
        private readonly AbsoluteLayout mapaLayout;
        private readonly AirWar.Server.TcpServer tcpServer;
        private readonly ArmaManager armaManager;
        private readonly Stopwatch presionadoCronometro = new Stopwatch();

        // Lista para almacenar las balas activas con sus hitboxes
        public List<Tuple<Image, Hitbox>> BalasConHitbox { get; private set; } = new List<Tuple<Image, Hitbox>>();

        public DisparoManager(AbsoluteLayout layout, AirWar.Server.TcpServer server, ArmaManager armaMgr)
        {
            mapaLayout = layout;
            tcpServer = server;
            armaManager = armaMgr;
        }

        public void OnDispararPresionado(object sender, EventArgs e)
        {
            presionadoCronometro.Restart();
        }

        public async void OnDispararSoltado(object sender, EventArgs e)
        {
            presionadoCronometro.Stop();
            int tiempoPresionado = (int)presionadoCronometro.ElapsedMilliseconds;
            Disparar(tiempoPresionado);
            if (tcpServer != null)
            {
                await tcpServer.EnviarMensaje("DisparoMau");
            }
            else
            {
                Console.WriteLine("tcpServer no está inicializado.");
            }
        }

        private void Disparar(int tiempoPresionado)
        {
            var bala = new Image
            {
                Source = "bala.png",
                WidthRequest = 40,
                HeightRequest = 40
            };

            double posicionXBala = armaManager.ObtenerPosicionBateria();

            AbsoluteLayout.SetLayoutBounds(bala, new Rect(posicionXBala, 0.9, 20, 20));
            AbsoluteLayout.SetLayoutFlags(bala, AbsoluteLayoutFlags.PositionProportional);
            mapaLayout.Children.Add(bala);

            // Crear la hitbox para la bala
            var balaHitbox = new Hitbox(posicionXBala * mapaLayout.Width, 0.9 * mapaLayout.Height, 20, 20);
            BalasConHitbox.Add(new Tuple<Image, Hitbox>(bala, balaHitbox));

            double tiempoPresionadoSegundos = tiempoPresionado / 1000.0;
            double velocidadBala = 0.01 + (0.005 * tiempoPresionadoSegundos);

            _ = MoverBala(bala, balaHitbox, velocidadBala, posicionXBala);
        }

        private async Task MoverBala(Image bala, Hitbox balaHitbox, double velocidad, double posicionXBala)
        {
            double posicionY = 0.9;
            while (posicionY > 0)
            {
                posicionY -= velocidad;
                AbsoluteLayout.SetLayoutBounds(bala, new Rect(posicionXBala, posicionY, 20, 20));

                // Actualizar la posición de la hitbox de la bala
                balaHitbox.Y = posicionY * mapaLayout.Height;

                // Verificar colisiones con los aviones aquí (implementación futura)

                await Task.Delay(30);
            }
            mapaLayout.Children.Remove(bala);
            BalasConHitbox.Remove(new Tuple<Image, Hitbox>(bala, balaHitbox));
        }
    }
}


