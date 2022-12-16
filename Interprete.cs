namespace Proyecto;
public class Contexto
{
    public Dictionary<string,int> contexto = new Dictionary<string, int>();
    public virtual void Guardar(string id, int valor)
    {
        if(contexto.ContainsKey(id))
        {
            contexto[id] = valor;
        }
        else
        {
            contexto.Add(id,valor);
        }
    }
    public int Obtener(string id)
    {
        return contexto[id];
    }

}

public class ContextoIf:Contexto
{
    public override void Guardar(string id, int valor)
    {
       if(contexto.ContainsKey(id))
       {
            contexto[id] = valor;
       } 
       else
       {
            if(Program.contexto.contexto.ContainsKey(id))
            {
                Program.contexto.contexto[id] = valor;
            }
            else
            {
                contexto.Add(id,valor);
            }
       }
    }
}

public class Asignacion: Expresion
{
    protected string a;
    protected Expresion b;
    
    public Asignacion(string a,Expresion b)
    {
        this.a = a;
        this.b = b;
    }

    public int Evaluar()
    {
        int resultado = b.Evaluar();
        if(If.Is_If() == false)
        Program.contexto.Guardar(a,resultado);
        else
        If.contextonuevo.Guardar(a,resultado);
        return 1;
    }
}

public class If:Expresion
{
    List<Expresion> Verdadero = new List<Expresion>();
    List<Expresion> Falso = new List<Expresion>();
    Expresion Condicion;

    public If(List<Expresion> a,List<Expresion> b, Expresion c)
    {
        this.Verdadero = a;
        this.Falso = b;
        this.Condicion = c; 
    }

    public static bool flag = false;
    public static ContextoIf contextonuevo = new ContextoIf();

    public static bool Is_If()
    {
        return flag;
    }

    public int Evaluar()
    {
        int resultado = Condicion.Evaluar();
        flag = true;
        contextonuevo = new ContextoIf();
        if(resultado == 0)
        {
            foreach(var Expresion in Falso)
            {
                Expresion.Evaluar();
            }
        }
        else
        {
            foreach(var Expresion in Verdadero)
            {
                Expresion.Evaluar();
            }
        }
        flag = false;
        return 1;
    }
}