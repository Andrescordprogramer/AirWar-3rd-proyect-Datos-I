using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;
using AirWar.Server;


namespace AirWar
{
    public partial class JuegoPage : ContentPage
    {
        private TcpServer tcpServer;
        private Timer cronometro = new Timer(); // Cronómetro para el juego
        private int segundosRestantes = 180; // Tiempo restante para el juego
        private double posicionBateria = 0.5; // Posición inicial centrada
        private double velocidadArma = 0.01;  // Velocidad del arma
        private bool moviendoDerecha = true;  // Dirección del movimiento
        private Random random = new Random();
        private const double AnchoMaximo = 1920; // Ancho del área permitida
        private const double AltoMaximo = 400;   // Alto del área permitida
        private Stopwatch presionadoCronometro = new Stopwatch(); // Para medir el tiempo que se presiona el botón
        private Image arma; // Representación del arma usando una imagen
        private Image bala;

        public JuegoPage(TcpServer server)
        {
            tcpServer = server;
            InitializeComponent();
            InicializarMapa(); // Inicializa el mapa con hangares y portaaviones
            InicializarArma();  // Inicializa el arma (batería antiaérea)
            InicializarTimer(); // Inicializa el cronómetro

            // Inicia el movimiento automático del arma
            _ = MoverArmaContinuamente();
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

        // Método para inicializar el arma
        private void InicializarArma()
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

        // Mover el arma continuamente de izquierda a derecha
        private async Task MoverArmaContinuamente()
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

                // Actualiza la posición en la interfaz
                ActualizarPosicionArma();
                await Task.Delay(50); // Controla la velocidad del ciclo
            }
        }

        // Actualiza la posición del arma
        private void ActualizarPosicionArma()
        {
            AbsoluteLayout.SetLayoutBounds(arma, new Rect(posicionBateria, 0.95, 100, 50));
        }

        // Método para disparar la bala con una fuerza específica
        public static void DispararRasp(int fuerzaDisparo)
        {
            return;
        }

        // Detectar cuándo se empieza a presionar el botón de disparo
        private void OnDispararPresionado(object sender, EventArgs e)
        {
            presionadoCronometro.Restart(); // Reinicia el cronómetro
        }

        // Detectar cuándo se suelta el botón de disparo
        private async void OnDispararSoltado(object sender, EventArgs e)
        {
            presionadoCronometro.Stop();
            int tiempoPresionado = (int)presionadoCronometro.ElapsedMilliseconds; // Tiempo en milisegundos  
            Disparar(tiempoPresionado);
            if (tcpServer != null)
            {
                await tcpServer.EnviarMensaje("DisparoMau");
            }
            else
            {
                // Manejar el caso en que tcpServer es null
                Console.WriteLine("tcpServer no está inicializado.");
            }
        }

        // Método para disparar la bala con una velocidad basada en el tiempo presionado
        private void Disparar(int tiempoPresionado)
        {
            // Crear la bala
            var bala = new Image
            {
                Source = "bala.png",
                WidthRequest = 40,
                HeightRequest = 40
            };

            // Captura la posición X de la batería en el momento del disparo
            double posicionXBala = posicionBateria;

            // Posiciona la bala en el centro del arma
            AbsoluteLayout.SetLayoutBounds(bala, new Rect(posicionXBala, 0.9, 20, 20));
            AbsoluteLayout.SetLayoutFlags(bala, AbsoluteLayoutFlags.PositionProportional);
            MapaLayout.Children.Add(bala);

            // Convierte el tiempo presionado de milisegundos a segundos
            double tiempoPresionadoSegundos = tiempoPresionado / 1000.0;

            // Calcula la velocidad de la bala basada en el tiempo presionado (mayor tiempo, mayor velocidad)
            double velocidadBala = 0.005 + (0.005 * tiempoPresionadoSegundos); // Ajusta la fórmula según sea necesario

            // Mueve la bala hacia arriba
            _ = MoverBala(bala, velocidadBala, posicionXBala);
        }


        // Mover la bala hacia arriba y eliminarla cuando salga de la pantalla
        private async Task MoverBala(Image bala, double velocidad, double posicionXBala)
        {
            double posicionY = 0.9;

            while (posicionY > -0.1) // La bala se elimina cuando sale de la pantalla
            {
                posicionY -= velocidad;
                AbsoluteLayout.SetLayoutBounds(bala, new Rect(posicionXBala, posicionY, 20, 20));
                await Task.Delay(50); // Controla la velocidad del ciclo
            }

            // Elimina la bala una vez que sale de la pantalla
            MapaLayout.Children.Remove(bala);
        }

        private void InicializarTimer()
        {
            cronometro = new Timer(1000); // Intervalo de 1 segundo
            cronometro.Elapsed += OnTimedEvent;
            cronometro.Start();
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            segundosRestantes--;

            Dispatcher.Dispatch(() =>
            {
                CronometroLabel.Text = TimeSpan.FromSeconds(segundosRestantes).ToString(@"mm\:ss");

                if (segundosRestantes <= 0)
                {
                    //segundosRestantes.Stop();
                    //Navigation.PushAsync(new FinalPage()); // aquí se redirige a la página final
                }
            });
        }
    }
}


