using System;
using System.Collections.Generic;
namespace Proyecto;

public interface Expresion
{   
    int Evaluar();
}
public abstract class Binario : Expresion
{
    protected Expresion expresion1;
    protected Expresion expresion2;
    public Binario(Expresion expresion1,Expresion expresion2)
    {
        this.expresion1 = expresion1;
        this.expresion2 = expresion2;
    }

    public int Evaluar()
    {
        int resultado1 = expresion1.Evaluar();
        int resultado2 = expresion2.Evaluar();
        return this.Evaluar(resultado1,resultado2);
    }
    protected abstract int Evaluar(int resultado1,int resultado2);
}

public class Suma:Binario
{
    public Suma(Expresion expresion1, Expresion expresion2):base(expresion1,expresion2)
    {

    }
    protected override int Evaluar(int resultado1,int resultado2)
    {
        return resultado1 + resultado2;
    }
}

public class Resta:Binario
{
    public Resta(Expresion expresion1, Expresion expresion2):base(expresion1,expresion2)
    {

    }
    protected override int Evaluar(int resultado1,int resultado2)
    {
        return resultado1 - resultado2;
    }
}

public class Division:Binario
{
    public Division(Expresion expresion1, Expresion expresion2):base(expresion1,expresion2)
    {

    }
    protected override int Evaluar(int resultado1,int resultado2)
    {
        return resultado1 / resultado2;
    }
}

public class Multiplicacion:Binario
{
    public Multiplicacion(Expresion expresion1, Expresion expresion2):base(expresion1,expresion2)
    {

    }
    protected override int Evaluar(int resultado1,int resultado2)
    {
        return resultado1 * resultado2;
    }
}

public class Menor:Binario
{
    public Menor(Expresion expresion1, Expresion expresion2):base(expresion1,expresion2)
    {

    }
    protected override int Evaluar(int resultado1,int resultado2)
    {
        if(resultado1 < resultado2)
            return 1;
        return 0;
    }
}

public class Igual:Binario
{
    public Igual(Expresion expresion1, Expresion expresion2):base(expresion1,expresion2)
    {

    }
    protected override int Evaluar(int resultado1,int resultado2)
    {
        if(resultado1 == resultado2)
            return 1;
        return 0;
    }
}

public class Mayor:Binario
{
    public Mayor(Expresion expresion1, Expresion expresion2):base(expresion1,expresion2)
    {

    }
    protected override int Evaluar(int resultado1,int resultado2)
    {
        if(resultado1 > resultado2)
            return 1;
        return 0;
    }
}

public class And:Binario
{
    public And(Expresion expresion1, Expresion expresion2):base(expresion1,expresion2)
    {
        
    }
    protected override int Evaluar(int resultado1, int resultado2)
    {
        if(resultado1!=0 && resultado2!=0)
        return 1;
        return 0;
    }
}

public class Or:Binario
{
    public Or(Expresion expresion1, Expresion expresion2):base(expresion1,expresion2)
    {
        
    }
    protected override int Evaluar(int resultado1, int resultado2)
    {
        if(resultado1!=0 || resultado2!=0)
        return 1;
        return 0;
    }
}

public abstract class Unario : Expresion
{
    protected Expresion expresion;
    public Unario(Expresion expresion)
    {
        this.expresion = expresion;
    }
    protected abstract int Evaluar(int resultado);
    public int Evaluar()
    {
        int resultado = expresion.Evaluar();
        return this.Evaluar(resultado);
    }
}

public class Negacion : Unario
{
    public Negacion(Expresion expresion):base(expresion)
    {

    }
    protected override int Evaluar(int resultado)
    {
        if(resultado == 1)
        return 0;
        return 1;
    }
}
public class Constante:Expresion
{
    int expresion;
    public Constante(int expresion)
    {
        this.expresion = expresion;
    }
    public int Evaluar()
    {
        return expresion;
    }
}

public class Variable:Expresion
{   
    string expresion;
    public Variable(string expresion)
    {
        this.expresion = expresion;
    }
    public int Evaluar()
    {
        switch(expresion)
        {
            case "miPatrimonio" : expresion = Program.jugadorActual.Nombre + ".Patrimonio";
            break;
            case "suPatrimonio" : expresion = Program.jugadorContrario.Nombre + ".Patrimonio";
            break;
            case "miMano" : expresion = Program.jugadorActual.Nombre + ".Mano";
            break;
            case "suMano" : expresion = Program.jugadorContrario.Nombre + ".Mano";
            break;
            case "miCampo" : expresion = Program.jugadorActual.Nombre + ".CampCarts";
            break;
            case "suCampo" : expresion = Program.jugadorContrario.Nombre + ".CampCarts";
            break;
        }
        return Program.contexto.Obtener(expresion);
    }
}