namespace Proyecto;

public abstract class Acciones:Expresion
{
    public int Evaluar()
    {
        Ejecutar();
        return 1; 
    }
    public abstract void Ejecutar();
}
public class Curar : Acciones
{
    Carta A;
    Carta B = new Carta();
    int P;
    public Curar(Carta A,int P)
    {
        this.A = A;
        this.P = P;
    }
    public override void Ejecutar()
    {
        B = Program.jugadorActual.ElegirCarta(A,1);
        B.Vida += P;
        if(B.Vida > B.VidaTotal)
        B.Vida = B.VidaTotal;
        Program.contexto.Guardar(B.Nombre+".Vida",B.Vida);
    }
}

public class Atacar : Acciones
{
    Carta A;
    Carta B = new Carta();

    public Atacar(Carta A)
    {
        this.A = A;
    }
    public override void Ejecutar()
    {
        if(Metodos.Distancia(A.Posx,A.Posy,Program.jugadorContrario.CampCarts,A.Alcance).Count()!=0)
        {
        B = Program.jugadorActual.ElegirCarta(A,2);
        if(A.Ataque > B.Defensa)
        B.Vida -= (A.Ataque - B.Defensa);
        B.Vida = Math.Max(B.Vida,0);
        if(B.Vida==0)
        {
            Program.jugadorContrario.CampCarts.Remove(B);
            Program.Cementerio.Add(B);
            Program.Tablero[B.Posx,B.Posy] = new Carta();
            Program.contexto.Guardar(Program.jugadorContrario.Nombre + ".CampCarts",Program.jugadorContrario.CampCarts.Count());
        }
        Program.contexto.Guardar(B.Nombre+".Vida",B.Vida);
        }
        else System.Console.WriteLine("No puedes atacar ya que no hay enemigos cerca");
        return;
    }

}   

public class Potenciar : Acciones
{
    protected Carta A;
    protected int P;
    protected Carta B = new Carta();

    public Potenciar(Carta A,int P)
    {
        this.A = A;
        this.P = P;
    }



    public override void Ejecutar()
    {
        B = Program.jugadorActual.ElegirCarta(A,1);
        B.Ataque += P;
        Program.contexto.Guardar(B.Nombre+".Ataque",B.Ataque);
    }
}

public class Agilizar : Potenciar
{
    public Agilizar(Carta A,int P) : base(A,P)
    {

    }
    public override void Ejecutar()
    {
        B = Program.jugadorActual.ElegirCarta(A,1);
        B.Alcance += P;
        Program.contexto.Guardar(B.Nombre+".Alcance",B.Alcance);
    }
}


public class Defender : Potenciar
{
    public Defender(Carta A, int P):base(A,P)
    {

    }

    public override void Ejecutar()
    {
        B = Program.jugadorActual.ElegirCarta(A,1);
        B.Defensa += P;
        Program.contexto.Guardar(B.Nombre+".Defensa",B.Defensa);
    }
}

public class Comerciar : Acciones
{
    Jugador jugador = Program.jugadorActual;
    int P;
    public Comerciar(int P)
    {
        this.P = P;   
    }

    public override void Ejecutar()
    {
        System.Console.WriteLine("Elija el jugador \n 1 para jugador actual \n 2 para jugador contrario");
        int x = int.Parse(System.Console.ReadLine()!);
        if(x == 1)
        {
            jugador = Program.jugadorActual;
        }
        else
        {
            jugador = Program.jugadorContrario;
        }
        jugador.Patrimonio += P;
        Program.contexto.Guardar(jugador.Nombre+".Patrimonio",jugador.Patrimonio);
    }
}

public class Sacrificio : Acciones
{
    Carta A;
    List<Carta> Cementerio;
    Jugador jugador = Program.jugadorActual;
    Carta [,] Tablero;
    string nombre;

public Sacrificio(Carta A,List<Carta> Cementerio,Carta [,] Tablero)
{
    this.A = A;
    this.Cementerio = Cementerio;
    this.Tablero = Tablero;
    nombre = "";
}
    public override void Ejecutar()
    {
    System.Console.WriteLine("Que carta quiere recuperar");
    nombre = Console.ReadLine()!;
    System.Console.WriteLine("elija el jugador \n 1 para jugador actual \n para jugador contrario");
    int x = int.Parse(System.Console.ReadLine()!);
    if(x == 1)
    {
        jugador = Program.jugadorActual;
    }
    else
    {
        jugador = Program.jugadorContrario;
    }
        bool flag = false;
        foreach(Carta a in Cementerio)
        {
            if(a.Nombre==nombre)
            {
             jugador.Mano.Add(a);
             flag = true;
             Program.contexto.Guardar(jugador.Nombre+".Mano",jugador.Mano.Count());
             break;
            }
        }
        Cementerio.Add(A);
        Tablero[A.Posx,A.Posy] = new Carta();
        jugador.CampCarts.Remove(A);
        Program.contexto.Guardar(jugador.Nombre+".Campo",jugador.CampCarts.Count());
        if(!flag) System.Console.WriteLine("Carta no encontrada en el cementerio"); 
    }
}

public class RangoPoder : Acciones
{
    Acciones accion;
    int rango;
    Carta C;

    public RangoPoder(Acciones accion,int rango)
    {
        this.accion = accion;
        this.rango = rango;
    }

    public override void Ejecutar()
    {
        C = Program.jugadorActual.ElegirCarta();
        List<Carta> aux = new List<Carta>();
        foreach(Carta a in Program.jugadorActual.CampCarts)
        aux.Add(a);
        foreach(Carta a in Program.jugadorContrario.CampCarts)
        aux.Add(a);
        List<Carta> cartas = Metodos.Distancia(C.Posx,C.Posy,aux,rango);
        foreach(Carta x in cartas)
        {
            accion.Ejecutar();
        }
    }
}
    
