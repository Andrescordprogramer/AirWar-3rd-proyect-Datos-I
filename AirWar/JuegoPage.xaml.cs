using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System;

namespace AirWar
{
    public partial class JuegoPage : ContentPage
    {
        private double posicionBateria = 0;
        private Random random = new Random();
        private const double AnchoMaximo = 1920; // Ancho del área permitida
        private const double AltoMaximo = 400;   // Alto del área permitida
        private Image arma; // Representación del arma usando una imagen

        public JuegoPage()
        {
            InitializeComponent();
            InicializarMapa(); // Inicializa el mapa con hangares y portaaviones
            InicializarArma();  // Inicializa el arma (batería antiaérea)
        }

        // Método para inicializar el mapa
        private void InicializarMapa()
        {
            // Añadimos el mapa de fondo (imagen)
            var mapa = new Image
            {
                Source = "mapa_mundo.png",
                Aspect = Aspect.Fill // Ajusta la imagen para llenar el layout
            };
            AbsoluteLayout.SetLayoutBounds(mapa, new Rect(0, 0, 1, 1)); // Ocupa toda la pantalla
            AbsoluteLayout.SetLayoutFlags(mapa, AbsoluteLayoutFlags.All);
            MapaLayout.Children.Add(mapa);

            // Agregamos hangares de forma aleatoria dentro del rango permitido
            for (int i = 0; i < 5; i++) // Ejemplo de 5 hangares
            {
                AgregarHangar();
            }

            // Agregamos la línea debajo del mapa donde se moverá el arma
            var linea = new BoxView
            {
                Color = Colors.Black,
                HeightRequest = 5 // Altura de la línea
            };
            AbsoluteLayout.SetLayoutBounds(linea, new Rect(0, 1, 1, 5)); // Posiciona la línea en la parte inferior
            AbsoluteLayout.SetLayoutFlags(linea, AbsoluteLayoutFlags.All);
            MapaLayout.Children.Add(linea);
        }



        // Método para agregar hangares aleatoriamente en el rango de 1920x800
        private void AgregarHangar()
        {
            // Crear la imagen del hangar
            var hangar = new Image
            {
                Source = "hangar.jpg", // carpeta de recursos
                WidthRequest = 50,
                HeightRequest = 50
            };

            // Posición aleatoria en el rango permitido (1920x800)
            double posX = random.NextDouble() * AnchoMaximo / 1920; // Escalado entre 0 y 1 para el ancho
            double posY = random.NextDouble() * AltoMaximo / 800;   // Escalado entre 0 y 1 para el alto

            // Añadir hangar al mapa dentro del rango
            AbsoluteLayout.SetLayoutBounds(hangar, new Rect(posX, posY, 50, 50)); // Posiciones aleatorias dentro del rango
            AbsoluteLayout.SetLayoutFlags(hangar, AbsoluteLayoutFlags.PositionProportional);
            MapaLayout.Children.Add(hangar);
        }




        // Método para inicializar el arma que se moverá debajo de la línea usando una imagen
        private void InicializarArma()
        {
            arma = new Image
            {
                Source = "arma.png", // Imagen del arma
                WidthRequest = 100,  // Ajusta el tamaño según sea necesario
                HeightRequest = 50
            };
            // Posiciona el arma sobre la línea
            AbsoluteLayout.SetLayoutBounds(arma, new Rect(0.5, 0.95, 100, 50)); // Coordenadas iniciales
            AbsoluteLayout.SetLayoutFlags(arma, AbsoluteLayoutFlags.PositionProportional);
            MapaLayout.Children.Add(arma);
        }





        // Mover la batería hacia la izquierda
        private void OnMoverIzquierdaClicked(object sender, EventArgs e)
        {
            posicionBateria = Math.Max(posicionBateria - 0.05, 0); // Mueve la batería 5% a la izquierda
            ActualizarPosicionArma();
        }





        // Mover la batería hacia la derecha
        private void OnMoverDerechaClicked(object sender, EventArgs e)
        {
            posicionBateria = Math.Min(posicionBateria + 0.05, 1); // Mueve la batería 5% a la derecha
            ActualizarPosicionArma();
        }





        // Actualiza la posición del arma en la interfaz
        private void ActualizarPosicionArma()
        {
            AbsoluteLayout.SetLayoutBounds(arma, new Rect(posicionBateria, 0.95, 100, 50)); // Actualiza la posición del arma
        }






        // Lógica para disparar
        private void OnDispararClicked(object sender, EventArgs e)
        {
            // Implementa la lógica para disparar desde la batería
            // Animación o movimiento de balas en el mapa
        }
    }
}


