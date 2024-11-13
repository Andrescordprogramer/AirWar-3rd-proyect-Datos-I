using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;



namespace AirWar
{
    public class ZonaProhibidaManager
    {
        private readonly List<BoxView> zonasProhibidas;

        public ZonaProhibidaManager()
        {
            zonasProhibidas = new List<BoxView>(); // Inicializa las zonas prohibidas visualmente
            DefinirZonasProhibidas();
        }

        // Definir zonas prohibidas (por ejemplo, pueden ser áreas en forma de rectángulo)
        private void DefinirZonasProhibidas()
        {
            zonasProhibidas.Add(new BoxView
            {
                Color = Color.FromRgba(255, 0, 0, 0), // Rojo completamente transparente
                WidthRequest = 600,  // Aumentar el tamaño de la zona roja (anchura)
                HeightRequest = 500, // Aumentar el tamaño de la zona roja (altura)
            });

            zonasProhibidas.Add(new BoxView
            {
                Color = Color.FromRgba(0, 255, 0, 0), // Verde completamente transparente
                WidthRequest = 150,
                HeightRequest = 130,
            });

            // Agregar la nueva zona amarilla
            zonasProhibidas.Add(new BoxView
            {
                Color = Color.FromRgba(255, 255, 0, 0), // Amarillo completamente transparente
                WidthRequest = 60,
                HeightRequest = 420,
            });


            zonasProhibidas.Add(new BoxView
            {
                Color = Color.FromRgba(255, 0, 0, 0), // Rojo completamente transparente
                WidthRequest = 400,  // Aumentar el tamaño de la zona roja (anchura)
                HeightRequest = 300, // Aumentar el tamaño de la zona roja (altura)
            });

            zonasProhibidas.Add(new BoxView
            {
                Color = Color.FromRgba(255, 0, 0, 0), // Rojo completamente transparente
                WidthRequest = 200,  // Aumentar el tamaño de la zona roja (anchura)
                HeightRequest = 150, // Aumentar el tamaño de la zona roja (altura)
            });

            zonasProhibidas.Add(new BoxView
            {
                Color = Color.FromRgba(255, 0, 0, 0), // Rojo completamente transparente
                WidthRequest = 550,  // Aumentar el tamaño de la zona roja (anchura)
                HeightRequest = 200, // Aumentar el tamaño de la zona roja (altura)
            });

        }

        // Método para establecer las posiciones de las zonas prohibidas
        public void EstablecerPosicionesZonasProhibidas(AbsoluteLayout mapaLayout)
        {
            // Mover las zonas más a la derecha aumentando la posición X

            // Para la primera zona (roja), la nueva posición X es 750 y la Y se ajusta a 0
            AbsoluteLayout.SetLayoutBounds(zonasProhibidas[0], new Rect(800, 300, zonasProhibidas[0].WidthRequest, zonasProhibidas[0].HeightRequest));

            // Para la segunda zona (verde), la nueva posición X es 1800 (antes era 600)
            AbsoluteLayout.SetLayoutBounds(zonasProhibidas[1], new Rect(1820, 0, zonasProhibidas[1].WidthRequest, zonasProhibidas[1].HeightRequest));

            // Para la nueva zona amarilla, la posición X es 0 y Y es 700
            AbsoluteLayout.SetLayoutBounds(zonasProhibidas[2], new Rect(0, 520, zonasProhibidas[2].WidthRequest, zonasProhibidas[2].HeightRequest));

            // Para la segunda zona (verde), la nueva posición X es 1800 (antes era 600)
            AbsoluteLayout.SetLayoutBounds(zonasProhibidas[3], new Rect(800, 0, zonasProhibidas[3].WidthRequest, zonasProhibidas[3].HeightRequest));

            // Para la segunda zona (verde), la nueva posición X es 1800 (antes era 600)
            AbsoluteLayout.SetLayoutBounds(zonasProhibidas[4], new Rect(1200, 150, zonasProhibidas[4].WidthRequest, zonasProhibidas[4].HeightRequest));

            // Para la segunda zona (verde), la nueva posición X es 1800 (antes era 600)
            AbsoluteLayout.SetLayoutBounds(zonasProhibidas[5], new Rect(800, 800, zonasProhibidas[5].WidthRequest, zonasProhibidas[5].HeightRequest));

            // Agregar las zonas prohibidas al mapa
            foreach (var zona in zonasProhibidas)
            {
                mapaLayout.Children.Add(zona);
            }
        }

        // Método para verificar si una posición está dentro de una zona prohibida
        public bool EstaEnZonaProhibida(double x, double y)
        {
            foreach (var zona in zonasProhibidas)
            {
                var zonaBounds = AbsoluteLayout.GetLayoutBounds(zona);
                if (zonaBounds.Contains(x, y)) // Si está dentro de una zona prohibida, retornar true
                {
                    return true;
                }
            }
            return false; // Si no está dentro de ninguna zona prohibida, retornar false
        }
    }
}


