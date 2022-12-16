namespace Proyecto;

public class Carta
{
    public string Nombre;
    public int Vida;
    public int VidaTotal;
    public int Coste;
    public int Defensa;
    public int Ataque;
    public string Descripcion;
    public string Foto;
    public int Alcance;
    public int Posx;
    public int Posy;
    public List<Expresion> Poderes = new List<Expresion>();
    public List<Expresion> Condiciones = new List<Expresion>();
    public Carta()
    {
        Nombre = Descripcion = Foto = "*";
        //Vida = VidaTotal = Coste = Defensa = Ataque =Alcance = Posx = Posy = 0;
        
        Poderes = new List<Expresion>();
        Poderes.Add(new Constante(0));
        Condiciones = new List<Expresion>();
        Condiciones.Add(new Constante(0));
    }

    public Carta(string n,int v,int cos ,int def,int a,string d,string f,int al,List<Expresion> p,List<Expresion> c)
    {
        Nombre = n;
        Vida = VidaTotal = v;
        Coste =cos;
        Defensa = def;
        Ataque = a;
        Descripcion = d;
        Foto =f;
        Alcance = al;
        Poderes = p;
        Condiciones = c;
    }    
}