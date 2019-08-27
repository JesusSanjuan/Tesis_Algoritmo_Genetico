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
            Random rand = new Random();
            byte[] bytes = new byte[20];
            rand.NextBytes(bytes);
            decimal inicio = 500000;
            double lowerBound = (double)inicio - 20000;
            double upperBound = (double)inicio + 20000;

            decimal inversion = (decimal)(rand.NextDouble() * (upperBound - lowerBound) + lowerBound);//////////////////

            Random randNum = new Random();
            int per = randNum.Next(12, 600);////////////////////////////

            Random rand2 = new Random();
            byte[] bytes2 = new byte[20];
            rand2.NextBytes(bytes2);
            double lowerBound2 = (double)0;
            double upperBound2 = (double)20000;

            decimal VS = (decimal)(rand2.NextDouble() * (upperBound2 - lowerBound2) + lowerBound2);//////////////////


            decimal[] FNE = new decimal[per];
            for (int num = 0; num < per; num++)
            {
                Random rand3 = new Random();
                byte[] bytes3 = new byte[20];
                rand3.NextBytes(bytes3);
                decimal inicio3 = 5000;
                double lowerBound3 = (double)inicio3 - 2000;
                double upperBound3 = (double)inicio3 + 2000;
                FNE[num] = (decimal)(rand3.NextDouble() * (upperBound3 - lowerBound3) + lowerBound3);//////////////////
            }

            
            StreamWriter outputFile = new StreamWriter("D:\\Archivo.txt");
            string x = "Holai";
            outputFile.WriteLine(x+"\t Hola");
            outputFile.Close();
        }

    }
}
