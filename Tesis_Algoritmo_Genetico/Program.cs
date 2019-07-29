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
            Console.WriteLine("Introduzca la cantidad de la problacion:  ");
            String poblacionNumero;
            poblacionNumero = Console.ReadLine();
            List<float> poblacion = new List<float>();
            List<String> poblacionBinariaV = new List<String>();
            List<String> poblacionBinaria = new List<String>();
            Random rand = new Random();
            for (int p = 0; p < Int32.Parse(poblacionNumero); p++)
            {
                String entero = "";
                String flotante = "";
                for (int x = 0; x < 20; x++)
                {
                    int value = rand.Next(0, 2);
                    if (x < 8)
                    {
                        entero = entero + value;
                    }
                    else
                    {
                        flotante = flotante + value;
                    }
                }
                poblacionBinaria.Add(entero + flotante);
                poblacionBinariaV.Add(entero + "." + flotante);
                int enterooo = BinarioADecimal(entero);
                float partedecimal = BinarioADecimalFlotante(flotante);
                float resultado = concatenacion(enterooo.ToString(), partedecimal.ToString());
                poblacion.Add(resultado);
            }
            Console.WriteLine("Imprimiendo Poblacion binaria:");
            foreach (string contenido in poblacionBinaria)
            {
                Console.WriteLine("{0}", contenido);
            }
            Console.WriteLine("____________________________\n");
            Console.WriteLine("Imprimiendo Poblacion binaria con punto:");
            foreach (string contenido in poblacionBinariaV)
            {
                Console.WriteLine("{0}", contenido);
            }
            Console.WriteLine("____________________________\n");            
            Console.WriteLine("Imprimiendo Poblacion en decimal:");
             foreach (float contenido in poblacion)
             {
                 Console.WriteLine("{0}", contenido.ToString());
             }
            Console.WriteLine("____________________________\n");
            Console.ReadKey();
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
            float[] arrayBase = new float[] { 0.5F, 0.25F, 0.125F, 0.0625F, 0.03125F, 0.015625F, 0.0078125F, 0.00390625F, 0.001953125F, 0.0009765625F, 0.00048828125F,0.000244140625F, 0.0001220703125F, 0.00006103515625F, 0.000030517578125F};
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
            char[] array = input2.ToCharArray();
            String parcial = "";
            int pos = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == '.')
                {
                    pos = i;
                }
            }
            for (int j = pos; j < array.Length; j++)
            {
                parcial = parcial + array[j];
            }
            String resultado = input + parcial;
            return Convert.ToSingle(resultado); ;
        }
    }
}
