namespace Proyecto;
public class Errores
{
    public List<string> Tokens = new List<string>();
    public Errores()
    {
        Tokens.Add("Vida");
        Tokens.Add("Defensa");
        Tokens.Add("Ataque");
        Tokens.Add("Alcance");
        Tokens.Add("Poderes");
        Tokens.Add("Condiciones");
        Tokens.Add("Nombre");
        Tokens.Add("Coste");
        Tokens.Add("Descripcion");
        Tokens.Add("Foto");
    }
    public string Texto = "";
    public List<string> Asignaciones = new List<string>();
    public List<string> errores = new List<string>();
    public string Titulo = "";

    int n = 0, m = 0;
    
    public void Leer(string titulo)
    {
        Titulo = titulo;
        while(true)
        {
            string s = System.Console.ReadLine()!;
            if(s == "end")
            break;
            Texto += s + " ";
            Asignaciones.Add(s);
        }
    }
    public void Revisar()
    {
        Dictionary<string,bool> Dic = new Dictionary<string, bool>();
        for(int i=0;i<Asignaciones.Count();i++)
        {
            string Campo = "";
            string valor = "";
            string aux = Asignaciones[i];
            for(int j=0;j<aux.Length;j++)
            {
                if(aux[j] == ' ')
                {
                    while(aux[j] == ' ')
                        j++;
                    if(aux[j] == '=')
                    {
                        j++;
                        while(aux[j] == ' ')
                        j++;

                        for(;j<aux.Length;j++)
                        valor += aux[j];
                    }
                    else
                    {
                        errores.Add("Se esperaba un = [Linea " + (i+1) + "]"); 
                        break;
                    }
                }
                if(j < aux.Length)
                Campo += aux[j];
            }
            bool flag = false;
            foreach(var x in Tokens)
            if(x == Campo)
            flag = true;
            if(!flag)
            {
                errores.Add("El campo " + Campo + " no existe  [Linea : " + (i+1) + "]");
                continue; 
            }
            else
            {
                if(!Dic.ContainsKey(Campo))
                Dic.Add(Campo,true);
                else
                {
                    errores.Add("Ha repetido la declaracion del campo " + Campo + " [Linea : " + i+1 + "]");
                    continue;
                }
            }

            if(Campo == "Ataque" || Campo == "Defensa" || Campo == "Alcance" || Campo == "Vida" || Campo == "Coste")
            {
                CheckExpresion(Dividir(valor),i+1);
            }
            else if(Campo == "Poderes")
            {
                CheckAcciones(Dividir(valor),i+1);
            }
            else if(Campo == "Condiciones")
            {
                CheckCondiciones(Dividir(valor),i+1);
            }
        }
        if(n != m - 1)
        {
            errores.Add("No hay la misma cantidad de condiciones que de poderes");
        }
    }

    public string[] Dividir(string s)
    {
        string aux = "";
        for(int i=0;i<s.Length;i++)
        {
            if(i + 1 < s.Length && s[i] == s[i+1] && s[i] == ' ')
            continue;
            aux += s[i];
        }
        return aux.Split();
    }
    public bool IsOperation(string x)
    {
        if(x == "+" || x == "-" || x == "*" || x == "/" || x == "<" || x == ">" || x == "=")
        return true;
        return false;
    }

    public bool IsNumber(string s)
    {
        if(s == " " || s.Length == 0)
        return false;
        foreach(var x in s)
        if('0'>x || x>'9')
        return false;
        return true;
    }

    public bool ContainsLetter(string s)
    {
        foreach(var x in s)
       if(('a' <= x && x <= 'z') || ('A' <= x && x <= 'Z'))
        return true;
        return false;
    }

    public bool ValidateStr(string s)
    {
        int nonumber = 0;
        foreach(var x in s)
        {
            if(x == ' ')
            return false;
            if('0' > x || x > '9')
            nonumber++;    
        }
        if(nonumber > 1)
        return false;
        if(nonumber == 1 && s[0] != '-' && s.Length > 1)
        return false;
        return true;
    }

    public bool CheckCondiciones(string []s,int I)
    {
        bool flag = true;
        for(int i=0;i<s.Length;i++)
        {
            string aux = "";
            while(i < s.Length && s[i] != "|")
            aux += s[i++] + " ";
            m++;
            
            if(!CheckExpresion(Dividir(aux),I))
            flag = false;
        }
        if(!flag)
        errores.Add("Hay un error en las condiciones [Linea " + I + "]");
        return flag;
    }
    public bool CheckExpresion(string []s,int I)
    {
        bool ErrorDeCaracter = false;
        bool ErrorDeParentesis = false;
        bool ErrorDeOperadores = false;
        bool ErrorDeNumero = false;

        bool EnOperador = false;
        bool EnNumero = false;
        int cur = 0;

        for(int i=0;i<s.Length;i++)
        {
            string aux = s[i];
           
            if(ContainsLetter(aux))
            {
                if( aux != "miPatrimonio" &&
                aux != "suPatrimonio" && aux != "miMano" && aux != "suMano" && aux != "miCampo" &&
                aux != "suCampo" && aux != Titulo + ".Vida" && aux != Titulo + ".Ataque" && aux != Titulo + ".Defensa" 
                && aux != Titulo + ".Coste" && aux != Titulo + ".Alcance") 
                    ErrorDeCaracter = true;
            }
            else
            {
                
            if(!ValidateStr(aux))
            {
                errores.Add("Hay un error de escritura [Linea " + I + "]");
            }

            }
            if(aux == "(")
            {
                cur++;
            }
            if(aux == ")")
            {
                cur--;   
            }
            
            if(cur < 0)
            ErrorDeParentesis = true;
            
            if(IsOperation(aux))
            {
                if(EnOperador)
                {
                    ErrorDeOperadores = true;
                }
                else
                {
                    EnOperador = true;
                    EnNumero = false;
                }
            }
            else if(IsNumber(aux))
            {
                if(EnNumero)
                {
                    ErrorDeNumero = true;
                }
                else
                {
                    EnNumero = true;
                    EnOperador = false;
                }
            }
        }
        if(cur > 0)
        ErrorDeParentesis = true;

        if(ErrorDeCaracter)
        errores.Add("No se esperaba una letra o no se encuentra el dato utilizado [Linea : " + I + "]");
        if(ErrorDeNumero)
        errores.Add("Se esperaba algun operando [Linea : " + I + "]");
        if(ErrorDeParentesis)
        errores.Add("Se esperaba algun () [Linea : " + I + "]");
        if(ErrorDeOperadores || EnOperador)
        errores.Add("Se esperaba algun numero entre operandos [Linea : " + I + "]");

        if(ErrorDeCaracter || ErrorDeNumero || ErrorDeOperadores || ErrorDeParentesis)
        return false;
        return true;
    }

    public bool CheckAcciones(string []s,int I)
    {
        List<string> acciones = new List<string>();
        acciones.Add("Atacar");
        acciones.Add("Defender");
        acciones.Add("Agilizar");
        acciones.Add("Curar");
        acciones.Add("Sacrificio");
        acciones.Add("Comerciar");
        acciones.Add("Potenciar");
        acciones.Add("Durante");
        acciones.Add("Repetir");

        bool ans = false;
        for(int i=0;i<s.Length;i++)
        {
            string aux = s[i];
            if(aux == "")
            continue;
            if(aux == "|")
            n++;
            if(aux == "|" || aux == " ")
            continue;
            bool flag = false;
            foreach(var x in acciones)
            if(x == aux)
            flag = true;

            if(!flag)
            {
                ans = true;
                errores.Add("No existe el poder " + aux + "[Linea : " + I + "]");
                continue;
            }

            if(aux == "Atacar")
            continue;

            if(aux == "Sacrificio")
                continue;

            int cur = 0;
            string exp = "";
            i++;
            for(;i<s.Length;i++)
            {
                exp+=s[i] + " ";
                if(s[i] == "(")
                cur++;
                if(s[i] == ")")
                cur--;

                if(cur == 0)
                break;
            }

            if(cur != 0)
            {
                ans = true;
                errores.Add("No se completo el balanceo de parentesis [Linea " + I + " ]");
            }

            if(exp.Length == 0)
            {
                ans = true;
                errores.Add("Se esperaba una expresion en la accion " + aux  + " [Linea : " + I + "]");
                continue;
            }

            if(aux != "Durante" && aux != "Repetir")
            {
                CheckExpresion(Dividir(exp),I);
            }
            else
            {
                string primeraexp = "";
                string segundaexp = "";
                bool FLAG = false;
                int comas = 0;
                for(int j=1;j<exp.Length-2;j++)
                {
                    if(exp[j] == ',')
                    {
                        comas++;
                        FLAG = true;
                        continue;
                    }
                    if(FLAG)
                    segundaexp+=exp[j];
                    else
                    primeraexp+=exp[j];
                }
                if(comas != 1)
                {
                    errores.Add("Hay una cantidad de argumentos diferentes de 1 [Linea : " + I + "]");
                }
                else
                {
                    CheckAcciones(Dividir(primeraexp),I);
                    CheckExpresion(Dividir(segundaexp),I);
                }
            }
        }
        return !ans;
    }
}