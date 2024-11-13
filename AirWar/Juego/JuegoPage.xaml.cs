using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AirWar.Server;


namespace AirWar
{
    public partial class JuegoPage : ContentPage
    {
        private TcpServer tcpServer;
        private CronometroManager cronometroManager;
        private double posicionBateria = 0.5;
        private double velocidadArma = 0.02;
        private readonly MapaManager mapaManager;

        private bool moviendoDerecha = true;
        private HangarManager hangarManager;
        private Rutas rutas;
        private const double AnchoMaximo = 1920;
        private const double AltoMaximo = 900;
        private Image arma;
        private List<Avion> aviones;
        private ArmaManager armaManager;
        private DisparoManager disparoManager;
        private ColisionManager colisionManager;

        public JuegoPage(TcpServer server)
        {
            tcpServer = server;
            InitializeComponent();

            var grafo = new Grafo(MapaLayout);

            

            // Crear el HangarManager para manejar tanto hangares como portaaviones
            hangarManager = new HangarManager(MapaLayout, AnchoMaximo, AltoMaximo, grafo);
            mapaManager = new MapaManager(MapaLayout, hangarManager);
            mapaManager.InicializarMapa();

            InicializarCronometro();

            // Crear los hangares aleatorios
            hangarManager.CrearHangaresAleatorios();

            // Crear los portaaviones aleatorios
            hangarManager.CrearPortaavionesAleatorios();  // Nuevo paso añadido

            Random random = new Random();
            aviones = new List<Avion>();
            for (int i = 0; i < 10; i++)
            {
                int hangarIndice = random.Next(grafo.ObtenerNodos().Count);
                Avion nuevoAvion = new Avion(grafo.ObtenerNodos(), hangarIndice);
                aviones.Add(nuevoAvion);
                MapaLayout.Children.Add(nuevoAvion.ImagenAvion);
            }

            foreach (var avion in aviones)
            {
                MoverAvionContinuamente(avion);
            }

            armaManager = new ArmaManager(MapaLayout, 0.01);
            StartMovingArma();

            disparoManager = new DisparoManager(MapaLayout, tcpServer, armaManager);

            colisionManager = new ColisionManager(MapaLayout, disparoManager.BalasConHitbox, aviones);
            _ = colisionManager.DetectarColisionesContinuamente(); // Inicia detección continua de colisiones
        }



        private async Task MoverAvionContinuamente(Avion avion)
        {
            while (true)
            {
                await avion.MoverAvion();
                await Task.Delay(50);
            }
        }

        private async void StartMovingArma()
        {
            await armaManager.MoverArmaContinuamente();
        }

        private void OnDispararPresionado(object sender, EventArgs e)
        {
            disparoManager.OnDispararPresionado(sender, e);
        }

        private async void OnDispararSoltado(object sender, EventArgs e)
        {
            disparoManager.OnDispararSoltado(sender, e);
        }

        private void InicializarCronometro()
        {
            cronometroManager = new CronometroManager(CronometroLabel, 180, MostrarGameOver);
        }

        private void MostrarGameOver()
        {
            GameOverContainer.IsVisible = true;
        }

        private async void OnSalirClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        public static void DispararRasp(int fuerzaDisparo)
        {
            return;
        }
    }
}







