using System;
using System.Collections.Generic;
namespace Proyecto;
class Program
{
    public static Jugador jugadorActual = new Jugador("");
    public static Jugador jugadorContrario = new Jugador("");
    public static Carta [,] Tablero = new Carta [10,10];
    public static List<Carta> Cementerio = new List<Carta>();
    public static Dictionary<Acciones,int> TimeActions = new Dictionary<Acciones,int>();
    public static Contexto contexto = new Contexto();
    public static int x;
    public static int y;
    public static int seleccion;
    public static List<int> disponibles = new List<int>(); 
    public static Carta Aux = new Carta();
    public static void Main(string []args)
    {
        //miMazo.CrearCarta();
        //return;
        
        Jugador jugador1 = Jugador.Seleccionar_Jugador();
        Jugador jugador2 = Jugador.Seleccionar_Jugador();

        List<Carta> Mazo = miMazo.LeerMazo();
        
        //System.Console.WriteLine(Mazo.Count());
        //Mazo = miMazo.Barajear(Mazo);
           
        int turno=-1;
        
        for(int i=0;i<10;i++)
        for(int j=0;j<10;j++)
        Tablero[i,j] = new Carta();

        for(int i=0;i<3;i++)
            {
            jugador1.Robar(Mazo,Metodos.GetRandom(0,Mazo.Count()-1));
            jugador2.Robar(Mazo,Metodos.GetRandom(0,Mazo.Count()-1));
            }

        
        while(Metodos.Continua(jugador1) && Metodos.Continua(jugador2))
        {
            Metodos.LeerTablero(Tablero);
            if(turno!=-1)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("Cambio de jugador a " + jugadorActual.Nombre);
            }
            else System.Console.WriteLine("Que comience el juego, comienza " + jugador2.Nombre);
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
            
            if(Mazo.Count() == 0)
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
            System.Console.WriteLine("Tu patrimonio es de : " + jugadorActual.Patrimonio);
            jugadorActual.Robar(Mazo,Metodos.GetRandom(0,Mazo.Count()-1));

            if(TimeActions.Count() > 0)
            {
                System.Console.WriteLine("Ejecutandose acciones de tiempo");
                Metodos.DoTimeActions();
            }

            while(jugadorActual.LimCarts < jugadorActual.Mano.Count()){
                Cementerio.Add(jugadorActual.Mano[0]);
                jugadorActual.Mano.RemoveAt(0);
            }

            disponibles = jugadorActual.CartasDisponibles();
            if(disponibles.Count()!=0)
            {
                foreach(var c in disponibles)
                {
                    System.Console.WriteLine("Carta : " + c);
                    jugadorActual.Mano[c].LeerCarta();
                }
            x = y = -1;

            jugadorActual.SeleccionarCarta();

            if(seleccion!= -1)
            {
                
            while(!Metodos.ValidarPosicion(x,y,Tablero,turno,true) || jugadorActual.Mano[seleccion].Condiciones[0].Evaluar() == 0)
            {
                jugadorActual.ElegirPosicion(true,turno);
                
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
            System.Console.WriteLine();
            System.Console.WriteLine("Movamos ahora las cartas del campo");
            for(int i=0;i < jugadorActual.CampCarts.Count();i++)
            {
                Aux = jugadorActual.CampCarts[i];
                System.Console.WriteLine("Tiene que pagar {0} por {1}",(double)(5/100*Aux.Coste),Aux.Nombre);
                jugadorActual.Patrimonio -= (int)(double)(5/100*Aux.Coste);
                System.Console.WriteLine("Patrimonio actual {0}",jugadorActual.Patrimonio);
                contexto.Guardar(jugadorActual.Nombre+".Patrimonio",jugadorActual.Patrimonio);
                System.Console.WriteLine("Moviendo la carta : ");
                Aux.LeerCarta();
                        break;
                    }
                    jugadorActual.ElegirPosicion(false,turno);
                    //System.Console.WriteLine(Metodos.ValidarPosicion(x,y,Tablero,turno,false));
                    if(x==-1) break;
                }
                 while(!Metodos.ValidarPosicion(x,y,Tablero,turno,false)
                   || Metodos.DistanciaTablero(x,y,Aux.Posx,Aux.Posy)>Aux.Alcance
                   );

                if(x!=-1)
                {
                    Tablero[x,y] = Aux;
                    Tablero[Aux.Posx,Aux.Posy] = new Carta();
                    Aux.Posx = x;
                    Aux.Posy = y;
                    System.Console.WriteLine(jugadorActual.CampCarts[i].Posx + " " + jugadorActual.CampCarts[i].Posy);
                    contexto.Guardar(Aux.Nombre+".Posx",Aux.Posx);
                    contexto.Guardar(Aux.Nombre+".Posy",Aux.Posy);
                }
            
                bool [] poderUsado = new bool [Aux.Poderes.Count()];
                seleccion = -1;
                do{
                    System.Console.WriteLine(Aux.Descripcion);
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