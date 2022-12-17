using System;
using System.Collections.Generic;
namespace Proyecto;
public class Metodos
{

    public static void Mover(Carta [,] Tablero,Carta A,int newX,int newY)
    {
        Tablero[newX,newY] = A;
        Tablero[A.Posx,A.Posy] = new Carta();
        A.Posx = newX;
        A.Posy = newY;
        Program.contexto.Guardar(A.Nombre+".Posx",A.Posx);
        Program.contexto.Guardar(A.Nombre+".Posy",A.Posy);
    }

    public static bool ValidarPosicion (int x,int y,Carta [,] Tablero,int t,bool flag)
    {
        if(x < 0 || x > 9 || y < 0 || y > 9 || Tablero[x,y].Nombre != "*")return false;
        if(flag && ((t%2==0 && x>1) || (t%2==1 && x<8))) return false; 
        return true;
    }

    public static bool Continua(Jugador A)
    {
        if(A.Patrimonio > 0)
        return true;
        if(Program.jugadorActual.Patrimonio > Program.jugadorContrario.Patrimonio)
        System.Console.WriteLine("El juego ha terminado, ha ganado : \n" + 
        Program.jugadorActual.Nombre);
        else System.Console.WriteLine("El juego ha terminado, ha ganado : \n" + 
        Program.jugadorContrario.Nombre);
        return false;
    }

     public static List<Carta> Distancia(int x,int y,List<Carta> list,int radio)
    {
        List<Carta> cartas = new List<Carta>();
        for(int i=0;i < list.Count();i++)
        {
            int dist = DistanciaTablero(list[i].Posx,list[i].Posy,x,y);
            if(dist<=radio)
            cartas.Add(list[i]); 
        }
        return cartas;
    }

    public static void AccionDeRango(int x,int y,List<Carta>list,int radio, Acciones accion)
    {
        List<Carta> enZona = new List<Carta>();

        enZona = Distancia(x,y,list,radio);
        {
            foreach(var A in enZona)
            {
                accion.Ejecutar();
            }
        }
    }

    public static void AccionDeTiempo(Acciones accion,int time,Dictionary<Acciones,int> TimeActions)
    {
        TimeActions.Add(accion,time);
    }

    public static void DoTimeActions(Dictionary<Acciones,int> TimeActions)
    {
        foreach(var action in TimeActions)
        {
            action.Key.Ejecutar();
            if(action.Value==1)
            TimeActions.Remove(action.Key);
            else
            TimeActions[action.Key] --;
        }
    }

    public static int DistanciaTablero(int x,int y,int newx,int newy)
    {
        return Math.Abs(newx - x) + Math.Abs(newy - y);
    }

    public static Carta MasCercano(int x,int y,List<Carta> list)
    {
        List<Carta> existe = new List<Carta>();
        for(int i=0;i<=18;i++)
        {
            existe = Distancia(x,y,list,i);
            if(existe.Count()!=0) return existe[0];
        }
        return new Carta();
    }

    public static int GetRandom(int inicio,int fin)
    {
        Random num = new Random();
        int n = num.Next(inicio,fin);
        return n;
    }

    public static void LeerTablero(Carta [,] tablero)
    {
        for(int z=0;z<tablero.GetLength(0);z++)
        {
            System.Console.Write(z + "  ");
        }
        for(int i=0;i<tablero.GetLength(0);i++)
        {
            System.Console.WriteLine(i + " ");
            for(int j=0;j<tablero.GetLength(1);j++)
            {
                if(tablero[i,j].Nombre!="*") System.Console.Write("|+|");
                else System.Console.Write("|_|");
            }
            System.Console.WriteLine();
        }
    }

}