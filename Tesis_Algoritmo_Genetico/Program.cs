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
            


           // do
           // {
                Random rand = new Random();
                // Generate and display 5 random byte (integer) values.
                byte[] bytes = new byte[5];
                rand.NextBytes(bytes);
                Console.WriteLine("Five Doubles between 0 and 5.");
                for (int ctr = 0; ctr <= 4; ctr++)
                    Console.Write("{0,8:N3}", rand.NextDouble() * 1000);
                Console.ReadKey();

           // } while (true);
        }
    }
}
