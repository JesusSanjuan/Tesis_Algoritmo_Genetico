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
            String num1, num2;
            Console.WriteLine("Primer numero: ");
            num1 = Console.ReadLine();
            Console.WriteLine("Segundo numero: ");
            num2 = Console.ReadLine();
            Console.WriteLine("Resultado: {0}", CalcularMultiplicacion(num1, num2));
            //decimal x = 7922816251426433759354395033m;
            Console.ReadKey();
        }

        public static string CalcularMultiplicacion(string num1, string num2)
        {
            String original1 = num1, orignal2 = num2;
            num1 = num1.Replace(".", "");
            num2 = num2.Replace(".", "");

            int a1 = num1.Length;
            int a2 = num2.Length;
            char[,] matriz;
            char[] array1 = num1.ToCharArray();
            char[] array2 = num2.ToCharArray();
            char[] arra1;
            char[] arra2;
            char[] acarreo;
            char[] resultado;
            char[] resultadoEnviar;

            /*Contador punto*/
            int primer =-1, segundo=-1;
            Boolean primerB=false, segundoB=false;            
            for (int d = 0; d < original1.Length;d++)
            {
                if(original1[d]=='.' || primerB)
                {
                    primerB = true;
                    primer = primer + 1;
                }
            }
            for (int d2 = 0; d2 < orignal2.Length; d2++)
            {
                if (orignal2[d2] == '.' || segundoB)
                {
                    segundoB = true;
                    segundo = segundo + 1;
                }
            }
            /*Contador punto*/

            if (a1>a2)
            {
                matriz = new char[a2+1, a1 + a2];
                for(int x=0; x<a2; x++)
                {
                    for(int y=0;y< a1 + a2; y++)
                    {
                        matriz[x,y] = '0';
                    }
                }
                for (int i = array2.Length; 0 < i; i--)
                {
                    for (int j = array1.Length; 0 <i ; j--)
                    {

                    }
                }
            }
            else
            {
                matriz = new char[a2+1, a1 + a2];
            }

            
            return "";
        }
    }
}
