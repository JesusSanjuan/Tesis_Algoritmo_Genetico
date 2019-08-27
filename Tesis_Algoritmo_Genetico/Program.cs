using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Tesis_Algoritmo_Genetico
{

    class Program
    {
        
        static void Main(string[] args)
        {
            StreamWriter outputFile = new StreamWriter("D:\\Archivo.txt");
            string x = "Holai";
            outputFile.WriteLine(x+"\t Hola");
            outputFile.Close();
        }

    }
}
