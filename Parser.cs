using System;
using System.Collections.Generic;
namespace Proyecto;
public class Parser
{
    string Texto = "";
    string Titulo = "";
    public Carta miCarta = new Carta();
    List<string> instrucciones = new List<string>();

    public List<string> ConvertToList(ref int i)
    {
        List<string> ExpStr = new List<string>();
        i+=2;
        while(i<instrucciones.Count() && instrucciones[i]!="Vida" 
            && instrucciones[i]!="Ataque" && instrucciones[i]!="Defensa" 
            && instrucciones[i]!="Coste" && instrucciones[i]!="Alcance" 
            && instrucciones[i]!="Nombre" && instrucciones[i]!="Descripcion" 
            && instrucciones[i]!="Foto" && instrucciones[i]!="Poderes" 
            && instrucciones[i]!="Condiciones")
        {
            bool flag = true;
            foreach(var x in instrucciones[i])
            if(x == ' ')
            flag = false;

            if(instrucciones[i] == null || instrucciones[i] == " " || instrucciones[i] == "")
            flag = false;

            if(flag)
            ExpStr.Add(instrucciones[i]);
            i++;
        }
        return ExpStr;
    }

    public List<Expresion> Delimitar(List<string> ExpStr)
    {
        List<Expresion> result = new List<Expresion>();
        for(int j=0;j<ExpStr.Count();j++)
        {
            Expresion exp = new Constante(0);
            List<string> accion = new List<string>();
            while(j<ExpStr.Count() && ExpStr[j]!="|")
            {
                accion.Add(ExpStr[j]);
                j++;
            }
            for(int i=0;i<accion.Count();i++)
            {
               
                string aux = accion[i];
                if(aux =="Curar" || aux =="Potenciar" || aux =="Agilizar" || aux =="Defender" || aux =="Comerciar")
                {
                    List<string> list = new List<string>();
                    int cur = 0;

                    do
                    { 
                        i++;
                        list.Add(accion[i]);
                        if(accion[i]  == ")")
                        cur--;
                        if(accion[i]  == "(")
                        cur++;
                    }while(cur != 0);
                    Expresion EXP = Decodificar(list);
                    switch(aux )
                    {
                        case "Curar":
                        {                                
                            Curar Curacion = new Curar(miCarta,EXP.Evaluar());
                            exp = new And(exp,Curacion);
                        }break;
                        case "Potenciar":
                        {
                            Potenciar potencia = new Potenciar(miCarta,EXP.Evaluar());
                            exp = new And(exp,potencia);
                        }break;
                        case "Agilizar":
                        {
                            Agilizar Agilidad = new Agilizar(miCarta,EXP.Evaluar());
                            exp = new And(exp,Agilidad);
                        }break;
                        case "Defender":
                        {
                            Defender Defensa = new Defender(miCarta,EXP.Evaluar());
                            exp = new And(exp,Defensa);
                        }break;
                        case "Comerciar":
                        {
                            Comerciar Comercio = new Comerciar(EXP.Evaluar());
                            exp = new And(exp,Comercio);
                        }break;
                    }
                    
                }
                else if(aux =="Atacar")
                {
                    Atacar Ataque = new Atacar(miCarta);
                    exp = new And(exp,Ataque);
                }
                else
                {
                    Sacrificio Sacrif = new Sacrificio(miCarta,Program.Cementerio,Program.Tablero);
                    exp = new And(exp,Sacrif);
                }
            }    
            result.Add(exp);     
        }
        return result;
    }
    
    public Parser(string Titulo,string Texto)
    {
        this.Texto = Texto!;
        this.Titulo = Titulo!;
        string [] Instrucciones = Texto.Split();
        for(int i=0;i<Instrucciones.Length;i++)
        {
            if(Instrucciones[i] != " " && Instrucciones[i] != "  ")
            instrucciones.Add(Instrucciones[i]);
        }

        for(int i=0;i<instrucciones.Count();i++)
        {
            string aux = instrucciones[i];
            if(aux == "Vida" || aux == "Ataque" || aux == "Defensa" || aux == "Coste" || aux == "Alcance" )
            {
                List<string> ExpStr = ConvertToList(ref i);
                Expresion Exp = Decodificar(ExpStr);
                Asignacion Asig = new Asignacion(aux,Exp);
                int num = Exp.Evaluar();
                Asig.Evaluar();
                switch(aux)
                {
                    case "Vida" : 
                    miCarta.Vida = miCarta.VidaTotal = num;
                    Program.contexto.Guardar(miCarta.Nombre + ".Vida",miCarta.Vida);
                    break;
                    case "Ataque" : 
                    miCarta.Ataque = num;
                    Program.contexto.Guardar(miCarta.Nombre + ".Ataque",miCarta.Ataque);
                    break;
                    case "Defensa" : 
                    miCarta.Defensa = num;
                    Program.contexto.Guardar(miCarta.Nombre + ".Defensa",miCarta.Defensa);
                    break;
                    case "Coste" : 
                    miCarta.Coste = num;
                    Program.contexto.Guardar(miCarta.Nombre + ".Coste",miCarta.Coste);
                    break;
                    case "Alcance" : 
                    miCarta.Alcance = num;
                    Program.contexto.Guardar(miCarta.Nombre + ".Alcance",miCarta.Alcance);
                    break;
                }

            }
            else if(aux == "Nombre" || aux == "Descripcion" || aux == "Foto")
            {
                string sol = "";
                i+=2;
                while(i<instrucciones.Count() && instrucciones[i]!="Vida" 
                && instrucciones[i]!="Ataque" && instrucciones[i]!="Defensa" 
                && instrucciones[i]!="Coste" && instrucciones[i]!="Alcance" 
                && instrucciones[i]!="Nombre" && instrucciones[i]!="Descripcion" 
                && instrucciones[i]!="Foto" && instrucciones[i]!="Poderes" 
                && instrucciones[i]!="Condiciones")
                {
                    sol += instrucciones[i];
                    i++;
                    if(instrucciones.Count() - 1 != i)
                    sol += " ";
                }
                switch(aux)
                {
                    case "Nombre" : 
                    sol = sol.Substring(0,sol.Length-1);
                    miCarta.Nombre = sol;
                    Program.contexto.Guardar(miCarta.Nombre + ".Posx",-1);
                    Program.contexto.Guardar(miCarta.Nombre + ".Posy",-1);
                    break;
                    case "Descripcion" : 
                    miCarta.Descripcion = sol;
                    break;
                    case "Foto" : miCarta.Foto = sol;break;
                }
            }else
            {
                List<string> ExpStr = ConvertToList(ref i);
    
                if(aux == "Poderes")
                {
                    List<Expresion> lis = new List<Expresion>();
                    lis.Add(new Constante(0));
                    List<Expresion> Aux = Delimitar(ExpStr);
                    foreach(var x in Aux)
                    lis.Add(x);
                    miCarta.Poderes = lis;
                }
                else
                {
                    List<Expresion> lis = Transformar(ExpStr);
                    miCarta.Condiciones = lis;
                }
            }
            i--;
        }

    }

    public static List<Expresion> Transformar(List<string> l)
    {
        List<Expresion> result = new List<Expresion>();

        for(int i=0;i<l.Count();i++)
        {
            List<string> lista = new List<string>();
            Expresion exp;
            while(i<l.Count() && l[i]!="|")
            { 
                lista.Add(l[i]);
                i++;
            }
            
            exp = Decodificar(lista);
            result.Add(exp);
        }
        return result;
    }

    public static Expresion Decodificar(List<string> list)
    {
        //Reconocer los parentesis y resolver
        List<string> sol = new List<string>();
        for(int i=0;i<list.Count();i++)
        {
            if(list[i] == "(")
            {
                int cur = 1;
                List<string> aux = new List<string>();
                while(i < list.Count() && cur != 0)
                {
                    i++;
                    if(list[i] == "(")
                    cur++;
                    if(list[i] == ")")
                    cur--;
                    if(cur != 0)
                    aux.Add(list[i]);
                }

                Expresion exp = Decodificar(aux);
                int result = exp.Evaluar();
                
                sol.Add(result.ToString());
            }
            else
            {
                sol.Add(list[i]);
            }
        }
        return Resolver(sol); 
    }
    
    //NO parentesis
    public static Expresion Resolver(List<string> list)
    {
        //a+b*f-c/d + e*f
        //a+b*f-c/d
        if(list.Count() == 1)
        {
            string aux = list[0];
            if(('0'<= aux[0] && aux[0] <= '9') || aux[0] == '-')
            //es un numero
            return new Constante(int.Parse(aux));
            return new Variable(aux);
        }
        else
        {
            List<string> left = new List<string>();
            List<string> right = new List<string>();
            for(int i=0;i<list.Count();i++)
            left.Add(list[i]);

            for(int i=list.Count()-1;i>=0;i--)
            {
                left.RemoveAt(left.Count()-1);
                if(list[i] == "&&")
                    return new And(Decodificar(left),Decodificar(right));
                if(list[i] == "||")
                    return new Or(Decodificar(left),Decodificar(right));
                right.Add(list[i]);
            }

            left.RemoveRange(0,left.Count());
            right.RemoveRange(0,right.Count());

            for(int i=0;i<list.Count();i++)
            left.Add(list[i]);

            for(int i=list.Count()-1;i>=0;i--)
            {
                left.RemoveAt(left.Count()-1);
                if(list[i] == ">")
                    return new Mayor(Decodificar(left),Decodificar(right));
                if(list[i] == "<")
                    return new Menor(Decodificar(left),Decodificar(right));
                if(list[i] == "=")
                    return new Igual(Decodificar(left),Decodificar(right));
                right.Add(list[i]);
            }

            left.RemoveRange(0,left.Count());
            right.RemoveRange(0,right.Count());

            for(int i=0;i<list.Count();i++)
            left.Add(list[i]);

            for(int i=list.Count()-1;i>=0;i--)
            {
                left.RemoveAt(left.Count()-1);
                if(list[i] == "+")
                    return new Suma(Decodificar(left),Decodificar(right));
                
                if(list[i] == "-")
                    return  new Resta(Decodificar(left),Decodificar(right));
                right.Add(list[i]);
            }
            left.RemoveRange(0,left.Count());
            right.RemoveRange(0,right.Count());

            for(int i=0;i<list.Count();i++)
            left.Add(list[i]);

            for(int i=list.Count()-1;i>=0;i--)
            {
                left.RemoveAt(left.Count()-1);
                if(list[i] == "*")
                    return new Multiplicacion(Decodificar(left),Decodificar(right));
                if(list[i] == "/")
                    return new Division(Decodificar(left),Decodificar(right));
                right.Add(list[i]);
            }
            return new Constante(1);
        }   
    }
}



