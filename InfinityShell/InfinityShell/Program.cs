using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;





namespace InfinityShell
{

    public class Infinity
    {
        public Infinity()
        {

        }
        private string no(string txt, string a, string b)
        {
            int one = txt.IndexOf(a) + a.Length;
            int two = txt.IndexOf(b);
            return txt.Substring(one, two - one);
        }
        public string errors = "";
        List<string> importedlibs = new List<string>();
        List<string> vars = new List<string>();
        List<string> funs = new List<string>();
        private void setpre(string lib)
        {
            importedlibs.Add(lib);
        }

        private string between(string txt, string a, string b)
        {
            
            string type = "none";
            string wat = "";
            string acsess = "";
            string ac = "";
            string val = "";

            /* Owen...the root cause is this function.  The following lines assume that a 
             * variable is actuallly set ahead of time before it splits the "print()" command.
             * If there is no var assigned then the following lines never execute and the
             * fout variable never gets set with anything.   
             */
            


            foreach (string var in vars)
            {

                string finalbetween = no(txt, a , b);
                acsess = finalbetween;
                int na = var.IndexOf("name<") + "name<".Length;
                int ne = var.IndexOf(">");
                string name = var.Substring(na, ne - na);
                ac = name;
                int va = var.IndexOf("value:") + "value:".Length;
                int ve = var.IndexOf(";");
                string value = var.Substring(va, ve - va);
                val = value;


                if (acsess.Equals(ac))
                {
                    wat = val;
                    return wat;
                }
                    
               
            }
           
            return acsess;
        }
        public string compile(string code)
        {
            setpre("ui");
            string fout = @"///////program_started\\\\\\\\" + "\n";
            string[] bif = {"print",
                    "_import","str","copytext","copyimage"};
            string[] libs = { "ui" };
            string[] keywords = { "var", "class", "def" };

            

            foreach (string f in keywords)
            {
               if (code.Contains(f))
                {
                    if (f.Equals("var"))
                    {

                        vars.Add("name<" + no(code, "var", "=").Replace(" ", "") + ">" + "value:" + no(code, "=", ";").Replace("'", "").Replace("\"", "") + ";");


                    }
                    if (f.Equals("def"))
                    {
                        funs.Add("name<" + no(code, "def", "(").Replace(" ", "") + ">" + "code:" + no(code, "{", "}")+";");

                    }

                }
            }
            foreach(string fun in funs)
            {
                if (no(fun, "name<", ">") == fun)
                {
                    this.compile(no(fun,"code:",";"));
                }
            }



            foreach (string f in bif)
            {

                if (code.Contains(f))
                {
                    if (f.Equals("print"))
                    {
                        if (vars.Count == 0)
                        {
                            
                        }
                        else
                        {
                            fout += between(code, "(", ")");
                        }
                       
                    }
                    else if (f.Equals("copytext"))
                    {
                        Clipboard.SetText(between(code, "(", ")"));
                    }
                    else if (f.Equals("copyimage"))
                    {
                        Clipboard.SetImage(System.Drawing.Image.FromFile(between(code, "(", ")")));
                    }
                    else if (f.Equals("_import"))
                    {
                        foreach (string lib in libs)
                        {
                            if (lib.Equals(between(code, "import", ";").Replace(" ", "")))
                            {
                                importedlibs.Add(lib);
                            }
                        }

                    }
                }
            }
            return fout;
        }
        class Program
        {
            [STAThread]
            static void Main(string[] args)
            {
                while (true)
                {
                    InfinityShell.Infinity infinity = new InfinityShell.Infinity();
                    Console.Write(">Shell>");
                    Console.Write(infinity.compile(Console.ReadLine()));
                    Console.ReadLine();
                }
            }
        }
    }
}
