using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;




namespace AirWar
{
    public class MapaManager
    {
        private readonly AbsoluteLayout mapaLayout;
        private readonly HangarManager hangarManager;
        private Rutas rutas;

        public MapaManager(AbsoluteLayout mapaLayout, HangarManager hangarManager)
        {
            this.mapaLayout = mapaLayout;
            this.hangarManager = hangarManager;
        }

        public void InicializarMapa()
        {
            // Añade el mapa de fondo
            var mapa = new Image
            {
                Source = "mapa_mundo.png",
                Aspect = Aspect.Fill
            };
            AbsoluteLayout.SetLayoutBounds(mapa, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(mapa, AbsoluteLayoutFlags.All);
            mapaLayout.Children.Add(mapa);

            // Crear hangares aleatorios
            hangarManager.CrearHangaresAleatorios();

            // Obtener las posiciones de los hangares y crear el objeto Rutas con estas posiciones
            var posiciones = hangarManager.PosicionesHangares;
            rutas = new Rutas(posiciones);

            // Visualizar rutas en el mapa
            VisualizarRutas();

            // Línea para el movimiento del arma
            var linea = new BoxView
            {
                Color = Colors.Black,
                HeightRequest = 5
            };
            AbsoluteLayout.SetLayoutBounds(linea, new Rect(0, 1, 1, 5));
            AbsoluteLayout.SetLayoutFlags(linea, AbsoluteLayoutFlags.All);
            mapaLayout.Children.Add(linea);
        }


        private void VisualizarRutas()
        {
            var adyacencias = rutas.ObtenerAdyacencias();

            foreach (var origen in adyacencias.Keys)
            {
                var coordenadaOrigen = rutas.ObtenerCoordenada(origen);

                foreach (var destino in adyacencias[origen])
                {
                    var coordenadaDestino = rutas.ObtenerCoordenada(destino);

                    // Crear una línea que representa la ruta entre los puntos
                    var linea = new BoxView
                    {
                        Color = Colors.Red,
                        WidthRequest = 2
                    };

                    // Establecer las coordenadas de la línea
                    AbsoluteLayout.SetLayoutBounds(linea, new Rect(coordenadaOrigen.X, coordenadaOrigen.Y, coordenadaDestino.X - coordenadaOrigen.X, coordenadaDestino.Y - coordenadaOrigen.Y));
                    AbsoluteLayout.SetLayoutFlags(linea, AbsoluteLayoutFlags.PositionProportional);
                    mapaLayout.Children.Add(linea);
                }
            }
        }
    }
}
