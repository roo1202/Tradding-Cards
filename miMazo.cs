using System;
using System.Collections.Generic;
namespace Proyecto;
public class miMazo
{
    public static Carta CrearCarta()
    {
        System.Console.WriteLine("Introduzca el nombre de la carta");
        string Titulo, Texto;
        Titulo = Console.ReadLine()!;
        Errores e = new Errores();
        System.Console.WriteLine("Introduzca las caracteristicas de la carta");
        e.Leer(Titulo);
        e.Revisar();
        System.Console.WriteLine("Termino la revision");;
        if(e.errores.Count() == 0)
        {
            System.Console.WriteLine("No hay errores");
            Texto = e.Texto;
            Parser p = new Parser(Titulo,Texto);
            Guardar(Titulo,Texto);
            return p.miCarta;
        }  
        else
        {
            foreach(var x in e.errores)
            System.Console.WriteLine(x);
            System.Console.WriteLine("Lo sentimos hay errores en la creacion");
            return new Carta();
        }
    }
    public static List<Carta> LeerMazo()
    {
        List<Carta> list = new List<Carta>();

        string []paths = Directory.GetFiles("./Content","*.*",SearchOption.AllDirectories);
        foreach(var x in paths)
        {
            string contenido = File.ReadAllText(x,System.Text.Encoding.UTF8);
            string Titulo = x.Substring(10);
            Parser aux = new Parser(Titulo,contenido);
            list.Add(aux.miCarta);
        }
        
        return list;
    }

    public static void Guardar(string Titulo,string texto)
    {
        File.WriteAllText("./Content/" +  Titulo,texto);
    }

    public static List<Carta> Barajear(List<Carta> mazo)
    {
        List<Carta> sol = new List<Carta>();
        Random r = new Random(Environment.TickCount);
        while(mazo.Count() > 0)
        {
            int pos = r.Next(0,mazo.Count()-1);
            sol.Add(mazo[pos]);
            mazo.RemoveAt(pos);
        }
        return sol;
    }
}