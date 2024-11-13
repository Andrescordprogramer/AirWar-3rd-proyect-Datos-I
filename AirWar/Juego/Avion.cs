using System;
using System.Collections.Generic;
using System.Threading.Tasks; // Necesario para Task.Delay
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;

namespace AirWar
{
    public class Avion
    {
        public double PosX { get; set; }
        public double PosY { get; set; }
        public Image ImagenAvion { get; set; } // La imagen que representa el avión
        public Hitbox Hitbox { get; private set; } // Añadir la propiedad Hitbox

        private List<Nodo> hangares; // Lista de hangares a los que se moverá el avión
        private Nodo hangarDestino; // Hangar al que el avión se moverá
        private double velocidad = 10; // Velocidad inicial (puedes ajustar este valor)
        private bool esperando; // Flag para indicar si el avión está esperando

        // Modificado para aceptar un índice de hangar, de modo que los aviones empiecen en diferentes hangares
        public Avion(List<Nodo> hangares, int hangarIndice)
        {
            this.hangares = hangares;
            this.PosX = hangares[hangarIndice].PosX; // Empieza en el hangar especificado
            this.PosY = hangares[hangarIndice].PosY;
            this.ImagenAvion = new Image
            {
                Source = "avion.png", // Imagen del avión en la carpeta de recursos
                WidthRequest = 30,
                HeightRequest = 30
            };
            this.Hitbox = new Hitbox(PosX, PosY, ImagenAvion.WidthRequest, ImagenAvion.HeightRequest);
            EstablecerDestinoAleatorio(); // Establecer un destino aleatorio
        }

        // Método para mover el avión a lo largo de los hangares
        public async Task MoverAvion()
        {
            if (esperando) return; // Si está esperando, no hacer nada

            double distanciaX = hangarDestino.PosX - PosX;
            double distanciaY = hangarDestino.PosY - PosY;
            double distanciaTotal = Math.Sqrt(distanciaX * distanciaX + distanciaY * distanciaY);

            if (distanciaTotal < velocidad)
            {
                esperando = true;
                Random random = new Random();
                int tiempoEspera = random.Next(3000, 6000); // Entre 3 y 6 segundos
                await Task.Delay(tiempoEspera);  // Espera asincrónica

                EstablecerDestinoAleatorio(); // Establecer un nuevo destino aleatorio
                esperando = false;
            }
            else
            {
                // Calcular el paso para mover el avión hacia el destino
                double pasoX = (distanciaX / distanciaTotal) * velocidad;
                double pasoY = (distanciaY / distanciaTotal) * velocidad;

                PosX += pasoX;
                PosY += pasoY;

                // Actualizar la posición de la UI de manera segura
                await ActualizarPosicionUIAsync();
            }
        }

        private async Task ActualizarPosicionUIAsync()
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                AbsoluteLayout.SetLayoutBounds(ImagenAvion, new Rect(PosX, PosY, ImagenAvion.WidthRequest, ImagenAvion.HeightRequest));
                Hitbox.Actualizar(PosX, PosY); // Actualizar la hitbox cuando el avión se mueve
            });
        }

        private void EstablecerDestinoAleatorio()
        {
            Random random = new Random();
            Nodo nuevoDestino;
            do
            {
                nuevoDestino = hangares[random.Next(hangares.Count)];
            } while (nuevoDestino.PosX == PosX && nuevoDestino.PosY == PosY); // Evitar que el avión elija el mismo punto

            hangarDestino = nuevoDestino;
        }

        public void CambiarVelocidad(double nuevaVelocidad)
        {
            velocidad = nuevaVelocidad;
        }
    }
}


