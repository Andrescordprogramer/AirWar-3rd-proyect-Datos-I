using Microsoft.Maui.Controls;
using System;
using System.Timers; // Usando System.Timers.Timer
using Timer = System.Timers.Timer; // Alias para el Timer



namespace AirWar
{
    public class CronometroManager
    {
        private Timer cronometro; // Timer de System.Timers
        private int segundosRestantes; // Tiempo restante para el juego
        private Label cronometroLabel; // Para actualizar la interfaz
        private Action onTimeUp; // Acción para llamar cuando el tiempo se agote

        // Constructor que acepta el Label, el tiempo y la acción a ejecutar al finalizar
        public CronometroManager(Label label, int tiempo, Action onTimeUpAction)
        {
            cronometroLabel = label; // Asigna el Label
            segundosRestantes = tiempo; // Asigna el tiempo inicial
            onTimeUp = onTimeUpAction; // Asigna la acción a ejecutar al finalizar
            InicializarTimer(); // Inicializa el cronómetro
        }

        private void InicializarTimer()
        {
            cronometro = new Timer(1000); // Un segundo
            cronometro.Elapsed += Cronometro_Elapsed; // Asegúrate de que la firma sea correcta
            cronometro.Start();
        }

        private void Cronometro_Elapsed(object sender, ElapsedEventArgs e) // Aquí está la firma correcta
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (segundosRestantes > 0)
                {
                    segundosRestantes--;
                    cronometroLabel.Text = $"Tiempo restante: {segundosRestantes} segundos"; // Actualiza la interfaz
                }
                else
                {
                    cronometro.Stop();
                    cronometroLabel.Text = "¡Tiempo agotado!"; // Mensaje al acabar el tiempo
                    onTimeUp?.Invoke(); // Llama a la acción para mostrar "GAME OVER" y el botón "Salir"
                }
            });
        }
    }
}

