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
            Console.WriteLine("Seundo numero: ");
            num2 = Console.ReadLine();
            CalcularSuma(num1, num2);
            Console.ReadKey();
        }

        public static string CalcularSuma(string num1, string num2)
        {
            int a1 = num1.Length;
            int a2 = num2.Length;
            char[] array1 = num1.ToCharArray();
            char[] array2 = num2.ToCharArray();
            char[] arra1;
            char[] arra2;
            char[] acarreo;
            char[] resultado;
            if (a1>a2)
            {
                arra1 = new char[num1.Length];
                arra2 = new char[num1.Length];
                acarreo = new char[num1.Length];
                resultado = new char[num1.Length+1];
                for (int i = 0; i < arra1.Length; i++)
                {
                    arra1[i] = '0';
                    arra2[i] = '0';
                    acarreo[i] = '0';
                    resultado[i] = '0';
                }

                arra1 = num1.ToCharArray();
                int reducir = arra1.Length - 1;
                for (int i = array2.Length - 1; 0 <= i; i--)
                {
                    arra2[reducir--] = array2[i];
                }
            }
            else
            {
                arra1 = new char[num2.Length];
                arra2 = new char[num2.Length];
                acarreo = new char[num2.Length];
                resultado = new char[num2.Length + 1];
                for (int i = 0; i < arra1.Length; i++)
                {
                    arra1[i] = '0';
                    arra2[i] = '0';
                    acarreo[i] = '0';
                    resultado[i] = '0';
                }
                
                int reducir = arra1.Length - 1;
                for (int i = array1.Length - 1; 0 <= i; i--)
                {
                    arra1[reducir--] = array1[i];
                }
                arra2 = num2.ToCharArray();
            }

            for(int x=arra1.Length-1;0<=x;x--)
            {
                int ar = int.Parse(acarreo[x].ToString());
                int v1 = int.Parse(arra1[x].ToString());
                int v2 = int.Parse(arra2[x].ToString());

                int resultadocolumna = ar + v1 + v2;
               
            }
            return "";
        }
    }
}
