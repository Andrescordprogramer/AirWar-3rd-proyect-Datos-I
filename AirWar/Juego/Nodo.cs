using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;


public class Nodo
{
    public int Id { get; set; }
    public double PosX { get; set; }
    public double PosY { get; set; }
    public Color Color { get; set; }
    public Image NodoImagen { get; set; } // Imagen del nodo en lugar de un rectángulo

    public Nodo(int id, double posX, double posY, Color color, string imageSource)
    {
        Id = id;
        PosX = posX;
        PosY = posY;
        Color = color;

        // Crear la imagen del nodo
        NodoImagen = new Image
        {
            Source = "arista.png", // Ruta a la imagen de cada nodo
            WidthRequest = 9,
            HeightRequest = 9
        };

        // Establecer la posición de la imagen en el AbsoluteLayout
        AbsoluteLayout.SetLayoutBounds(NodoImagen, new Rect(PosX, PosY, NodoImagen.WidthRequest, NodoImagen.HeightRequest));
        AbsoluteLayout.SetLayoutFlags(NodoImagen, AbsoluteLayoutFlags.None);
    }
}





public class Ruta
{
    public Nodo NodoOrigen { get; set; }
    public Nodo NodoDestino { get; set; }
    public double Peso { get; set; } // Basado en la distancia y otros factores

    public Ruta(Nodo origen, Nodo destino, double peso)
    {
        NodoOrigen = origen;
        NodoDestino = destino;
        Peso = peso;
    }
}






public class Grafo
{
    private List<Nodo> nodos;
    private List<Ruta> rutas;
    private AbsoluteLayout mapaLayout;

    public Grafo(AbsoluteLayout layout)
    {
        nodos = new List<Nodo>();
        rutas = new List<Ruta>();
        mapaLayout = layout;
    }

    public void AgregarNodo(Nodo nodo)
    {
        nodos.Add(nodo);
        // Añadir la imagen del nodo al layout
        mapaLayout.Children.Add(nodo.NodoImagen);
    }

    public void AgregarRuta(Nodo origen, Nodo destino)
    {
        double distancia = CalcularDistancia(origen, destino);
        double peso = distancia; // Puedes agregar lógica adicional aquí
        rutas.Add(new Ruta(origen, destino, peso));
    }

    private double CalcularDistancia(Nodo a, Nodo b)
    {
        return Math.Sqrt(Math.Pow(b.PosX - a.PosX, 2) + Math.Pow(b.PosY - a.PosY, 2));
    }

    public List<Ruta> ObtenerRutas()
    {
        return rutas;
    }

    public List<Nodo> ObtenerNodos()
    {
        return nodos;
    }

    // Método para obtener el grafo completo
    public Grafo ObtenerGrafo()
    {
        return this;
    }
}



