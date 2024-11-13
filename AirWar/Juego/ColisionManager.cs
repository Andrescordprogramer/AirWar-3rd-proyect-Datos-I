using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirWar
{
    public class ColisionManager
    {
        private readonly AbsoluteLayout mapaLayout;
        private readonly List<Tuple<Image, Hitbox>> balasConHitbox;
        private readonly List<Avion> aviones;

        public ColisionManager(AbsoluteLayout layout, List<Tuple<Image, Hitbox>> balas, List<Avion> aviones)
        {
            mapaLayout = layout;
            balasConHitbox = balas;
            this.aviones = aviones;
        }

        public async Task DetectarColisionesContinuamente()
        {
            while (true)
            {
                DetectarColisiones();
                await Task.Delay(50);
            }
        }

        private void DetectarColisiones()
        {
            var balasAEliminar = new List<Tuple<Image, Hitbox>>();
            var avionesAEliminar = new List<Avion>();

            foreach (var balaTuple in balasConHitbox)
            {
                var bala = balaTuple.Item1;
                var hitboxBala = balaTuple.Item2;

                foreach (var avion in aviones)
                {
                    if (hitboxBala.IntersectsWith(avion.Hitbox))
                    {
                        // Lógica para manejar la colisión
                        mapaLayout.Children.Remove(bala);
                        mapaLayout.Children.Remove(avion.ImagenAvion);
                        balasAEliminar.Add(balaTuple);
                        avionesAEliminar.Add(avion);
                        break;
                    }
                }
            }

            foreach (var bala in balasAEliminar)
            {
                balasConHitbox.Remove(bala);
            }

            foreach (var avion in avionesAEliminar)
            {
                aviones.Remove(avion);
            }
        }
    }
}







