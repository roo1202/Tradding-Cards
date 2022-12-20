using System;
using System.Collections.Generic;
namespace Proyecto;
public class Jugador 
{
    public string Nombre {get;set;}
    public int Patrimonio {get;set;}
    public int LimCarts {get;set;}
    public List<Carta> Mano = new List<Carta>();
    public List<Carta> CampCarts = new List<Carta>();

    public Jugador(string Nombre)
    {
        this.Nombre = Nombre;
        this.Patrimonio = 500;
        this.LimCarts = 6;
    }
    
    public void Robar (List<Carta> Robando,int PosCarta)
    {
        Mano.Add(Robando[PosCarta]);
        Robando.RemoveAt(PosCarta);
        Program.contexto.Guardar(Program.jugadorActual.Nombre+".Mano",Program.jugadorActual.Mano.Count());
        Program.contexto.Guardar(Program.jugadorActual.Nombre+".Mano",Program.jugadorActual.Mano.Count());
    }

    public List<int> CartasDisponibles()
    {
        List<int> disponibles = new List<int>();
        //System.Console.WriteLine("Cartas disponibles :");
        for(int i=0;i<Mano.Count();i++)
        {   
            if(Mano[i].Condiciones[0].Evaluar() == 0)
            continue;
            //System.Console.WriteLine(Mano[i].Nombre + " " + i);   
            disponibles.Add(i);
        
        }

        return disponibles;
    }

    public static Jugador Seleccionar_Jugador()
    {
        System.Console.WriteLine("Seleccione el jugador,1 para humano,2 para virtual");
        int n = int.Parse(Console.ReadLine()!);
        if(n==1)
        {
            System.Console.WriteLine("Introduzca su nombre");
            string name = Console.ReadLine()!;
            return new Jugador(name);
        }
        else 
        {
        string [] jugadores = {"Robert_Barathyon","Daenerys","Cersei","Joffrey","Thom","Jon_Snow"};
        return new JugadorVirtual(jugadores[Metodos.GetRandom(0,jugadores.Length-1)]);
        }
    }

    public virtual void SeleccionarCarta()
    {
        System.Console.WriteLine("seleccione la carta,en caso contrario introduzca -1");
        int seleccion;
        seleccion = int.Parse(Console.ReadLine()!);
        if(seleccion != -1)
        {
            Program.seleccion = seleccion;
        }

    }

    public virtual void ElegirPosicion(bool flag,int turno)
    {
        System.Console.WriteLine("Seleccione la posicion");
        Program.x = int.Parse(Console.ReadLine()!);
        if(Program.x==-1) return;
        Program.y = int.Parse(Console.ReadLine()!);
    }

    public virtual void ElegirPoder(Carta C,bool [] usados)
    {
        System.Console.WriteLine("Elige uno de los siguientes poderes disponibles; o -1 para saltar");
        for(int x=1;x<C.Condiciones.Count();x++)
        {
            if(C.Condiciones[x].Evaluar() > 0 && usados[x]==false)
            System.Console.WriteLine(x);
        }
        Program.seleccion = int.Parse(Console.ReadLine()!);
    }

    public static bool Exist(int posx, int posy)
    {
        bool flag = false;
        foreach(var v in Program.jugadorActual.CampCarts)
        if(v.Posx == posx && posy == v.Posy)
        flag = true;
        foreach(var v in Program.jugadorContrario.CampCarts)
        if(v.Posx == posx && posy == v.Posy)
        flag = true;
        return flag;
    }
    public virtual Carta ElegirCarta(Carta A,int tipo)
    {
        int PX,PY;
        Carta Devuelta = new Carta();
        do{
            System.Console.WriteLine("Elija una carta");
            PX = int.Parse(Console.ReadLine()!);
            PY = int.Parse(Console.ReadLine()!);
        }while(Metodos.DistanciaTablero(PX,PY,A.Posx,A.Posy)>A.Alcance || !Exist(PX,PY));
        Devuelta = Program.Tablero[PX,PY];
        return Devuelta;
    }

}

public class JugadorVirtual : Jugador
{
    public JugadorVirtual(string Nombre) : base(Nombre)
    {
        this.Nombre= Nombre;
        System.Console.WriteLine("Te enfrentaras con " + Nombre);
        Patrimonio = 5000;
        LimCarts = 6;
    }

    public override void SeleccionarCarta()
    {
        int indice = 0;
        int ataque = 0;
        for(int i =0;i<Program.disponibles.Count();i++)
        {
            if(ataque < Program.jugadorActual.Mano[Program.disponibles[i]].Ataque) 
            {
                ataque = Program.jugadorActual.Mano[Program.disponibles[i]].Ataque;
                indice = Program.disponibles[i];
            }
        }
        Program.seleccion = indice;
    }

    public override void ElegirPosicion(bool flag,int turno)
    {
        //System.Console.WriteLine("llegue");
        if(flag)
        {
            if(turno%2==1)
            {
            Program.y = Metodos.GetRandom(0,9);
            Program.x = Metodos.GetRandom(8,9);
            }
            else
            {
                Program.y = Metodos.GetRandom(0,9);
                Program.x = Metodos.GetRandom(0,1);
            }
        }
        else
        {
            if(turno%2==1)
            {
                Program.y = Program.Aux.Posy;
                Program.x = Math.Max(Program.Aux.Posx - Program.Aux.Alcance,0);
            }
            else
            {
                System.Console.WriteLine(Program.Aux.Posx);
                System.Console.WriteLine(Program.Aux.Posy);
                Program.y = Program.Aux.Posy;
                Program.x = Math.Min(Program.Aux.Posx + Program.Aux.Alcance,9); 
            }
            
        }
    }

    public override Carta ElegirCarta(Carta A,int tipo)
    {
        Carta Devuelta = new Carta();
        int vida= 1000000;
        int indice = 0;
        List<Carta> cercanas = new List<Carta>();
        if(tipo == 1) cercanas = Metodos.Distancia(A.Posx,A.Posy,Program.jugadorActual.CampCarts,A.Alcance);
        else cercanas = Metodos.Distancia(A.Posx,A.Posy,Program.jugadorContrario.CampCarts,A.Alcance);
        for(int i=0;i<cercanas.Count();i++)
        {
            if(vida > cercanas[i].Vida)
            {
                vida = cercanas[i].Vida;
                indice = i;
            }
        }
        Devuelta = cercanas[indice];
        return Devuelta;
    }
    public override void ElegirPoder(Carta C,bool [] usados)
    {
        List<int> disponibles = new List<int>();
        for(int x=1;x<C.Condiciones.Count();x++)
        {
            if(C.Condiciones[x].Evaluar() > 0 && usados[x]==false)
            {
            Program.seleccion = x;
            return;
            }
        }
        Program.seleccion = -1;
    }
}