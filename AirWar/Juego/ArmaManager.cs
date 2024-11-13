using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System;
using System.Threading.Tasks;



namespace AirWar
{
    public class ArmaManager
    {
        private Image arma;
        private AbsoluteLayout MapaLayout;
        private double posicionBateria;
        private bool moviendoDerecha;
        private double velocidadArma;

        public ArmaManager(AbsoluteLayout mapaLayout, double velocidadInicial)
        {
            MapaLayout = mapaLayout;
            velocidadArma = velocidadInicial;
            posicionBateria = 0.5;  // Inicializamos la posición
            InicializarArma();
        }

        public void InicializarArma()
        {
            arma = new Image
            {
                Source = "arma.png",
                WidthRequest = 400,
                HeightRequest = 100
            };
            AbsoluteLayout.SetLayoutBounds(arma, new Rect(posicionBateria, 0.95, 100, 50));
            AbsoluteLayout.SetLayoutFlags(arma, AbsoluteLayoutFlags.PositionProportional);
            MapaLayout.Children.Add(arma);
        }

        public async Task MoverArmaContinuamente()
        {
            while (true)
            {
                // Cambia la dirección si llega a los límites
                if (posicionBateria >= 1)
                {
                    moviendoDerecha = false;
                }
                else if (posicionBateria <= 0)
                {
                    moviendoDerecha = true;
                }

                // Ajusta la posición en función de la dirección
                if (moviendoDerecha)
                {
                    posicionBateria = Math.Min(posicionBateria + velocidadArma, 1);
                }
                else
                {
                    posicionBateria = Math.Max(posicionBateria - velocidadArma, 0);
                }

                // Actualiza la posición en la interfaz de usuario en el hilo principal
                Device.BeginInvokeOnMainThread(() =>
                {
                    ActualizarPosicionArma();
                });

                await Task.Delay(50); // Controla la velocidad del ciclo
            }
        }


        private void ActualizarPosicionArma()
        {
            AbsoluteLayout.SetLayoutBounds(arma, new Rect(posicionBateria, 0.95, 100, 50));
        }

        public double ObtenerPosicionBateria()
        {
            return posicionBateria;
        }
    }
}
