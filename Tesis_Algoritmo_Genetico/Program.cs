using System;
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
            Random r= new Random();
            float numero;
            do
            {
                numero = (float)(r.Next(0, 101)) / 100;
                Console.WriteLine("Numero float aleatorio: "+numero);
                
                //Console.WriteLine("Press any key to exit.");
                Console.ReadKey();

            } while (true);
        }
    }
}
