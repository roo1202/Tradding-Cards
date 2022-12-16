using System;
using System.Collections.Generic;
namespace Proyecto;
class Program
{
    public static Jugador jugadorActual = new Jugador("");
    public static Jugador jugadorContrario = new Jugador("");
    public static Carta [,] Tablero = new Carta [10,10];
    public static List<Carta> Cementerio = new List<Carta>();
    
    public static Contexto contexto = new Contexto();
    public static int x;
    public static int y;
    public static int seleccion;
    public static List<int> disponibles = new List<int>(); 
    public static Carta Aux = new Carta();
    public static void Main(string []args)
    {
        //miMazo.CrearCarta();
        
        Jugador jugador1 = Jugador.Seleccionar_Jugador(1);
        Jugador jugador2 = Jugador.Seleccionar_Jugador(2);

        List<Carta> Mazo = miMazo.LeerMazo();
        
        //System.Console.WriteLine(Mazo.Count());
        //Mazo = miMazo.Barajear(Mazo);
        foreach(var c in contexto.contexto)
        {
            System.Console.WriteLine(c.Key);
        }
           
        Dictionary<Acciones,int> TimeActions = new Dictionary<Acciones,int>();

        int turno=-1;
        
        for(int i=0;i<10;i++)
        for(int j=0;j<10;j++)
        Tablero[i,j] = new Carta();

        for(int i=0;i<3;i++)
            {
            jugador1.Robar(Mazo,Mazo.Count()-1);
            jugador2.Robar(Mazo,Mazo.Count()-1);
            }

        bool flag = false;

        while(Metodos.Continua(jugador1) && Metodos.Continua(jugador2))
        {
            if(turno!=-1) System.Console.WriteLine("Cambio de jugador");
            else System.Console.WriteLine("Que comience el juego");
            turno++;
            if(turno%2 == 1) 
            {
                jugadorActual = jugador2;
                jugadorContrario = jugador1;
            }
            else 
            {
                jugadorActual = jugador1;
                jugadorContrario = jugador2;
            }
            contexto.Guardar(jugadorActual.Nombre + ".Patrimonio",jugadorActual.Patrimonio);
            contexto.Guardar(jugadorContrario.Nombre + ".Patrimonio",jugadorContrario.Patrimonio);
            contexto.Guardar(jugadorActual.Nombre + ".Mano",jugadorActual.Mano.Count());
            contexto.Guardar(jugadorContrario.Nombre + ".Mano",jugadorContrario.Mano.Count());
            contexto.Guardar(jugadorActual.Nombre + ".CampCarts",jugadorActual.CampCarts.Count());
            contexto.Guardar(jugadorContrario.Nombre + ".CampCarts",jugadorContrario.CampCarts.Count());
            
            jugadorActual.Robar(Mazo,Mazo.Count()-1);
            
            if(Mazo.Count() == 0 && flag)
            {
                System.Console.WriteLine("El juego Termino");
                if(jugadorActual.Patrimonio > jugadorContrario.Patrimonio)
                System.Console.WriteLine("Gano " + jugadorActual.Nombre);
                else if(jugadorActual.Patrimonio < jugadorContrario.Patrimonio)
                System.Console.WriteLine("Gano " + jugadorContrario.Nombre);
                else
                System.Console.WriteLine("Empate ");
                System.Console.WriteLine(1);
                break;
            }
            jugadorActual.Robar(Mazo,Mazo.Count()-1);

            Metodos.DoTimeActions(TimeActions);

            while(jugadorActual.LimCarts < jugadorActual.Mano.Count()){
                Cementerio.Add(jugadorActual.Mano[0]);
                jugadorActual.Mano.RemoveAt(0);
            }

            disponibles = jugadorActual.CartasDisponibles();
            if(disponibles.Count()!=0)
            {
            x = y = -1;

            jugadorActual.SeleccionarCarta();

            if(seleccion!= -1)
            {
            while(!Metodos.ValidarPosicion(x,y,Tablero,turno,true) || jugadorActual.Mano[seleccion].Condiciones[0].Evaluar() == 0)
            {
                jugadorActual.ElegirPosicion(true);
                
            }
            
            jugadorActual.Patrimonio -= jugadorActual.Mano[seleccion].Coste;
            contexto.Guardar(jugadorActual.Nombre+".Patrimonio",jugadorActual.Patrimonio);
            Tablero[x,y] = jugadorActual.Mano[seleccion];
            jugadorActual.CampCarts.Add(jugadorActual.Mano[seleccion]);
            jugadorActual.Mano.RemoveAt(seleccion);
            jugadorActual.CampCarts[jugadorActual.CampCarts.Count()-1].Posx = x;
            jugadorActual.CampCarts[jugadorActual.CampCarts.Count()-1].Posy = y;
            Aux = jugadorActual.CampCarts[jugadorActual.CampCarts.Count()-1];
            contexto.Guardar(jugadorActual.Nombre+".Campo",jugadorActual.CampCarts.Count());
            contexto.Guardar(jugadorActual.Nombre+".Mano",jugadorActual.Mano.Count());
            contexto.Guardar(Aux.Nombre+".Posx",Aux.Posx);
            contexto.Guardar(Aux.Nombre+".Posy",Aux.Posy);
            }
            }
            
            System.Console.WriteLine("Movamos ahora las cartas del campo");
            for(int i=0;i < jugadorActual.CampCarts.Count();i++)
            {
                Aux = jugadorActual.CampCarts[i];
                jugadorActual.Patrimonio -= 5/100*Aux.Coste;
                contexto.Guardar(jugadorActual.Nombre+".Patrimonio",jugadorActual.Patrimonio);
                System.Console.WriteLine("Mover la carta : {0}",Aux.Nombre);

                do{
                    jugadorActual.ElegirPosicion(false);
                    if(x==-1) break;
                }
                 while(!Metodos.ValidarPosicion(x,y,Tablero,turno,false)
                   || Metodos.DistanciaTablero(x,y,jugadorActual.CampCarts[i].Posx,jugadorActual.CampCarts[i].Posy)>jugadorActual.CampCarts[i].Alcance
                   );

                if(x!=-1)
                {
                    Tablero[x,y] = jugadorActual.CampCarts[i];
                    Tablero[jugadorActual.CampCarts[i].Posx,jugadorActual.CampCarts[i].Posy] = new Carta();
                    jugadorActual.CampCarts[i].Posx = x;
                    jugadorActual.CampCarts[i].Posy = y;
                    contexto.Guardar(Aux.Nombre+".Posx",Aux.Posx);
                    contexto.Guardar(Aux.Nombre+".Posy",Aux.Posy);
                }
            
                bool [] poderUsado = new bool [Aux.Poderes.Count()];
                seleccion = -1;
                do{
                    jugadorActual.ElegirPoder(Aux,poderUsado);
                    
                    if(seleccion == -1)
                    break;

                    if(seleccion >= Aux.Condiciones.Count() || seleccion <= 0 || poderUsado[seleccion])
                    continue; 
                    poderUsado[seleccion] = true;
                    if(Aux.Condiciones[seleccion].Evaluar() > 0)
                    Aux.Poderes[seleccion].Evaluar();
                    else System.Console.WriteLine("Poder no disponible");    

                }while(seleccion!=-1);
            }

        }
    }

}