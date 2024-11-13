using System;
using System.Collections.Generic;
using Microsoft.Maui.Graphics; // Asegúrate de que este espacio de nombres es correcto para Rect



namespace AirWar
{
    public class Ruta
    {
        public int Destino { get; set; }
        public double Peso { get; set; }

        public Ruta(int destino, double peso)
        {
            Destino = destino;
            Peso = peso;
        }
    }

    public class Rutas
    {
        private readonly List<List<Ruta>> grafo;
        private readonly List<Rect> posicionesHangares;

        public Rutas(List<Rect> posicionesHangares)
        {
            this.posicionesHangares = posicionesHangares;
            grafo = new List<List<Ruta>>();

            // Inicializar el grafo con listas de adyacencia vacías
            for (int i = 0; i < posicionesHangares.Count; i++)
            {
                grafo.Add(new List<Ruta>());
            }

            GenerarRutasAleatorias();
        }

        private double CalcularDistancia(Rect origen, Rect destino)
        {
            double deltaX = origen.X - destino.X;
            double deltaY = origen.Y - destino.Y;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        private void GenerarRutasAleatorias()
        {
            Random random = new Random();

            for (int i = 0; i < posicionesHangares.Count; i++)
            {
                for (int j = i + 1; j < posicionesHangares.Count; j++)
                {
                    double distancia = CalcularDistancia(posicionesHangares[i], posicionesHangares[j]);
                    double peso = distancia; // Peso basado en la distancia

                    // Agregar ruta de i a j
                    grafo[i].Add(new Ruta(j, peso));

                    // Agregar ruta de j a i
                    grafo[j].Add(new Ruta(i, peso));
                }
            }
        }

        public List<Ruta> ObtenerRutasDesde(int hangar)
        {
            if (hangar < 0 || hangar >= grafo.Count)
                throw new ArgumentOutOfRangeException(nameof(hangar), "Índice de hangar fuera de rango");

            return grafo[hangar];
        }

        public Dictionary<int, List<int>> ObtenerAdyacencias()
        {
            var adyacencias = new Dictionary<int, List<int>>();

            for (int i = 0; i < grafo.Count; i++)
            {
                var adyacente = new List<int>();
                foreach (var ruta in grafo[i])
                {
                    adyacente.Add(ruta.Destino);
                }
                adyacencias[i] = adyacente;
            }

            return adyacencias;
        }

        public Rect ObtenerCoordenada(int hangar)
        {
            if (hangar < 0 || hangar >= posicionesHangares.Count)
                throw new ArgumentOutOfRangeException(nameof(hangar), "Índice de hangar fuera de rango");

            return posicionesHangares[hangar];
        }

        public void ImprimirGrafo()
        {
            for (int i = 0; i < grafo.Count; i++)
            {
                Console.Write($"Hangar {i}: ");
                foreach (var ruta in grafo[i])
                {
                    Console.Write($"-> Hangar {ruta.Destino} (Peso: {ruta.Peso:F2}) ");
                }
                Console.WriteLine();
            }
        }
    }
}


