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
        System.Console.WriteLine("Se ha curado la carta {0} en {1}",B.Nombre,P);
        if(B.Vida > B.VidaTotal)
        {
            B.Vida = B.VidaTotal;
            System.Console.WriteLine("No puedes aumentar su vida mas del maximo");
        }
        System.Console.WriteLine("Vida: " + B.Vida);
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
        System.Console.WriteLine("{0} ataca a {1}",A.Nombre,B.Nombre);
        if(A.Ataque > B.Defensa)
        {
            System.Console.WriteLine("La vida de {0} bajo de {1} a ",B.Nombre,B.Vida);
            B.Vida -= (A.Ataque - B.Defensa);
            System.Console.WriteLine(B.Vida);
        }
        else
        {
            System.Console.WriteLine("La defensa de {0} es mayor que el ataque de {1}",B.Nombre,A.Nombre);
        }
        B.Vida = Math.Max(B.Vida,0);
        if(B.Vida==0)
        {
            System.Console.WriteLine("{0} ha muerto",B.Nombre);
            Console.WriteLine("has ganado {0}",B.Coste/2);
            Program.jugadorActual.Patrimonio += B.Coste/2;
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

        System.Console.WriteLine("{2} ha modificado el ataque de {0} en un {1}",B.Nombre,P,A.Nombre);
        System.Console.WriteLine("Ahora el ataque de {0} es de {1}",B.Nombre,B.Ataque);
        
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

        System.Console.WriteLine("{2} ha modificado el alcance de {0} en un {1}",B.Nombre,P,A.Nombre);
        System.Console.WriteLine("Ahora el alcance de {0} es de {1}",B.Nombre,B.Alcance);
        

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

        System.Console.WriteLine("{2} ha modificado la defensa de {0} en un {1}",B.Nombre,P,A.Nombre);
        System.Console.WriteLine("Ahora la defensa de {0} es de {1}",B.Nombre,B.Defensa);
        

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
        if(P < 0) jugador = Program.jugadorActual.ElegirJugador(2);
        else jugador = Program.jugadorActual.ElegirJugador(1);
        jugador.Patrimonio += P;
        System.Console.WriteLine("El patrimonio de {0} ha sido modificado en {1}",jugador.Nombre,P);
        System.Console.WriteLine("Ahora es de {0}",jugador.Patrimonio);
        Program.contexto.Guardar(jugador.Nombre+".Patrimonio",jugador.Patrimonio);
    }
}

public class Sacrificio : Acciones
{
    Carta A;
    List<Carta> Cementerio;
    Carta [,] Tablero;

public Sacrificio(Carta A,List<Carta> Cementerio,Carta [,] Tablero)
{
    this.A = A;
    this.Cementerio = Cementerio;
    this.Tablero = Tablero;
}
    public override void Ejecutar()
    {
    if(Program.Cementerio.Count()==0) 
    {
        System.Console.WriteLine("Cementerio vacio,no hay cartas que recuperar");
        return;
    }
    Carta card = Program.jugadorActual.GetCard(Program.Cementerio);
    Jugador jugador = Program.jugadorActual.ElegirJugador(1);
    card.Vida = card.VidaTotal;
    jugador.Mano.Add(card);
    Program.contexto.Guardar(jugador.Nombre+".Mano",jugador.Mano.Count());      
    System.Console.WriteLine("Ha recuperado la carta seleccionada");
    A.Vida = A.VidaTotal;
    Cementerio.Add(A);
    Tablero[A.Posx,A.Posy] = new Carta();
    jugador.CampCarts.Remove(A);
    Program.contexto.Guardar(jugador.Nombre+".Campo",jugador.CampCarts.Count());
    
    }
}

public class Rebote : Acciones
{
    Acciones accion;
    int cant;

    public Rebote(Acciones accion,int cant)
    {
        this.accion = accion;
        this.cant = cant;
    }

    public override void Ejecutar()
    {
        System.Console.WriteLine("Se va a ejecutar una accion {0} veces",cant);
        for(int i=0;i<cant;i++)
        {
            accion.Ejecutar();
        }
        
    }

    public class TimeAccion:Acciones
    {
        Acciones accion;
        int time;

        public TimeAccion(Acciones accion, int time)
        {
            this.accion = accion;
            this.time = time;
        }

        public override void Ejecutar()
        {
            System.Console.WriteLine("Se ha activado una accion de tiempo que se reactivara {0} veces",time);
            accion.Ejecutar();
            time--;
            Metodos.AccionDeTiempo(accion,time);
        }
    }
}
    
