using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirWar
{
    public class Hitbox
    {
        public double X { get; set; }
        public double Y { get;set; }
        public double Ancho { get; set; }
        public double Alto { get;set; }

        public Hitbox(double x, double y, double ancho, double alto)
        {
            X = x;
            Y = y;
            Ancho = ancho;
            Alto = alto;
        }

        public void Actualizar(double x, double y)
        {
            X = x;
            Y = y;
        }

        public bool IntersectsWith(Hitbox otra)
        {
            return !(X + Ancho < otra.X || otra.X + otra.Ancho < X || Y + Alto < otra.Y || otra.Y + otra.Alto < Y);
        }

        public void ActualizarY(double y)
        {
            Y = y;
        }
    }
}


