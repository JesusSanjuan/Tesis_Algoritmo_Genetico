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
            do
            {
                Random rand = new Random();
                String entero = "";
                String flotante = "";
                for (int x = 0; x < 18; x++)
                {
                    int value = rand.Next(0, 2);
                    if (x < 9)
                    {
                        entero = entero + value;
                    }
                    else
                    {
                        flotante = flotante + value;
                    }
                }
                Console.Write("Numero completo en binario: {0}.{1} \n", entero, flotante);
                int enterooo = BinarioADecimal(entero);
                float partedecimal = BinarioADecimalFlotante(flotante);
                Console.Write("Numero en decimal {0}", concatenacion(enterooo.ToString(), partedecimal.ToString()));
                Console.Write("\n\n");
                Console.ReadKey();
            } while (true);
        }

        static int BinarioADecimal(String input)
        {
            char[] array = input.ToCharArray();
            Array.Reverse(array);
            int sum = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == '1')
                {
                    sum += (int)Math.Pow(2, i);
                }
            }
            return sum;
        }

        static float BinarioADecimalFlotante(String input)
        {
            float[] arrayBase = new float[] { 0.5F, 0.25F, 0.125F, 0.0625F, 0.03125F, 0.015625F, 0.0078125F, 0.00390625F, 0.001953125F, 0.0009765625F };
            char[] array = input.ToCharArray();
            //Array.Reverse(array);
            float sum = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == '1')
                {
                    sum = sum + arrayBase[i];
                }
            }
            return sum;
        }

        static float concatenacion(String input, String input2)
        {
            string parcial = input2.Substring(1, 8);
            String resultado = input + parcial;
            return Convert.ToSingle(resultado); ;
        }
    }
}
