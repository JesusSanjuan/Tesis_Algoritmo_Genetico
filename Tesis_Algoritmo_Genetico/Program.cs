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
            num1 = num1.Replace(".", "");
            num2 = num2.Replace(".", "");










            int a1 = num1.Length;
            int a2 = num2.Length;
            char[] array1 = num1.ToCharArray();
            char[] array2 = num2.ToCharArray();
            char[] arra1;
            char[] arra2;
            char[] acarreo;
            char[] resultado;
            char[] resultadoEnviar;
            if (a1>a2)
            {
                arra1 = new char[num1.Length];
                arra2 = new char[num1.Length];
                acarreo = new char[num1.Length];
                resultado = new char[num1.Length+1];
                resultadoEnviar = new char[num1.Length + 2];
                for (int i = 0; i < arra1.Length; i++)
                {
                    arra1[i] = '0';
                    arra2[i] = '0';
                    acarreo[i] = '0';
                    resultado[i] = '0';
                    resultadoEnviar[i] = '0';
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
                resultadoEnviar = new char[num2.Length + 2];
                for (int i = 0; i < arra1.Length; i++)
                {
                    arra1[i] = '0';
                    arra2[i] = '0';
                    acarreo[i] = '0';
                    resultado[i] = '0';
                    resultadoEnviar[i] = '0';
                }
                
                int reducir = arra1.Length - 1;
                for (int i = array1.Length - 1; 0 <= i; i--)
                {
                    arra1[reducir--] = array1[i];
                }
                arra2 = num2.ToCharArray();
            }

            for(int x=arra1.Length;1<=x;x--)
            {
                int ar = int.Parse(acarreo[x - 1].ToString());
                int v1 = int.Parse(arra1[x - 1].ToString());
                int v2 = int.Parse(arra2[x - 1].ToString());
                int resultadocolumna = ar + v1 + v2;
                string myString = resultadocolumna.ToString();
                char[] myStringchar = myString.ToCharArray();
                if (x == 1)
                {
                    if (myString.Length == 2)
                    {
                        resultado[x - 1] = myStringchar[0];
                        resultado[x] = myStringchar[1];
                    }
                    else
                    {
                        resultado[x] = myStringchar[0];
                    }                        
                }
                else
                {                       
                    if (myString.Length == 2)
                    {
                        acarreo[x - 2] = myStringchar[0];
                        resultado[x] = myStringchar[1];
                    }
                    else
                    {
                        resultado[x] = myStringchar[0];
                    }
                }
            }
            int contador = 0;
            int incremento = 0;
            for (int x = resultadoEnviar.Length; 0 < x; x--)
            {                  
                if (2== contador)
                {
                    resultadoEnviar[x - 1] = '.';
                    incremento = 1;
                }
                else
                {
                    resultadoEnviar[x-1] = resultado[x - 2+incremento];
                }
                contador = contador + 1;
            }        
                
            string resultadoT = new string(resultadoEnviar);
            if (resultadoEnviar[0] == '0')
            {
                resultadoT = resultadoT.Substring(1, resultadoT.Length-1);
            }
            return resultadoT;
        }
    }
}
