using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;



namespace AirWar
{
    public class HangarManager
    {
        private readonly Random random;
        private readonly double AnchoMaximo;
        private readonly double AltoMaximo;
        private readonly AbsoluteLayout MapaLayout;
        private readonly List<Rect> posicionesHangares; // Lista privada de posiciones
        private readonly List<Rect> posicionesPortaaviones;
        private readonly Grafo grafo;
        private readonly ZonaProhibidaManager zonaProhibidaManager;

        public List<Rect> PosicionesHangares => posicionesHangares; // Propiedad pública para acceder a posicionesHangares
        public List<Rect> PosicionesPortaaviones => posicionesPortaaviones; // Propiedad pública para acceder a posicionesPortaaviones


        public HangarManager(AbsoluteLayout mapaLayout, double anchoMaximo, double altoMaximo, Grafo grafo)
        {
            random = new Random();
            MapaLayout = mapaLayout;
            AnchoMaximo = anchoMaximo;
            AltoMaximo = altoMaximo;
            posicionesHangares = new List<Rect>();
            this.grafo = grafo;
            zonaProhibidaManager = new ZonaProhibidaManager();

            zonaProhibidaManager.EstablecerPosicionesZonasProhibidas(MapaLayout);
        }

        // Crear hangares aleatorios
        public void CrearHangaresAleatorios()
        {
            for (int i = 0; i < 7; i++)
            {
                var hangar = new Image
                {
                    Source = "hangar.jpg",
                    WidthRequest = 50,
                    HeightRequest = 50
                };

                double posX, posY;
                bool colocadoCorrectamente;

                do
                {
                    posX = random.NextDouble() * (AnchoMaximo - hangar.WidthRequest);
                    posY = random.NextDouble() * (AltoMaximo - hangar.HeightRequest);

                    colocadoCorrectamente = true;

                    if (zonaProhibidaManager.EstaEnZonaProhibida(posX, posY))
                    {
                        colocadoCorrectamente = false;
                        continue;
                    }

                    foreach (var pos in posicionesHangares)
                    {
                        double distancia = CalcularDistancia(pos.X, pos.Y, posX, posY);
                        if (distancia < 100)
                        {
                            colocadoCorrectamente = false;
                            break;
                        }
                    }
                }
                while (!colocadoCorrectamente);

                Nodo nodo = new Nodo(i, posX, posY, Colors.Transparent, "hangar.jpg");
                grafo.AgregarNodo(nodo);

                var posicionHangar = new Rect(posX, posY, hangar.WidthRequest, hangar.HeightRequest);
                AbsoluteLayout.SetLayoutBounds(hangar, posicionHangar);
                AbsoluteLayout.SetLayoutFlags(hangar, AbsoluteLayoutFlags.None);
                MapaLayout.Children.Add(hangar);

                posicionesHangares.Add(posicionHangar); // Añadir la posición del hangar a la lista
            }
        }



        // Crear portaaviones aleatorios
        public void CrearPortaavionesAleatorios()
        {
            for (int i = 0; i < 5; i++) // Supongo que quieres 3 portaaviones
            {
                var portaaviones = new Image
                {
                    Source = "portaavion.jpg", // Asegúrate de que esta imagen exista en los recursos
                    WidthRequest = 40,
                    HeightRequest = 40
                };

                double posX, posY;
                bool colocadoCorrectamente;

                do
                {
                    posX = random.NextDouble() * (AnchoMaximo - portaaviones.WidthRequest);
                    posY = random.NextDouble() * (AltoMaximo - portaaviones.HeightRequest);

                    colocadoCorrectamente = true;

                    // Verificar si el portaaviones está dentro de una zona prohibida
                    if (!zonaProhibidaManager.EstaEnZonaProhibida(posX, posY)) // Ahora queremos que esté dentro de una zona prohibida
                    {
                        colocadoCorrectamente = false;
                        continue;
                    }

                    foreach (var pos in posicionesHangares)
                    {
                        double distancia = CalcularDistancia(pos.X, pos.Y, posX, posY);
                        if (distancia < 100) // Si está muy cerca de otro, no lo coloca
                        {
                            colocadoCorrectamente = false;
                            break;
                        }
                    }
                }
                while (!colocadoCorrectamente);

                var posicionPortaaviones = new Rect(posX, posY, portaaviones.WidthRequest, portaaviones.HeightRequest);
                AbsoluteLayout.SetLayoutBounds(portaaviones, posicionPortaaviones);
                AbsoluteLayout.SetLayoutFlags(portaaviones, AbsoluteLayoutFlags.None);
                MapaLayout.Children.Add(portaaviones);

                // Si también necesitas agregar la posición al grafo, hazlo aquí
                Nodo nodoPortaaviones = new Nodo(i, posX, posY, Colors.Transparent, "portaaviones.jpg");
                grafo.AgregarNodo(nodoPortaaviones);
            }
        }






        private double CalcularDistancia(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }

}



