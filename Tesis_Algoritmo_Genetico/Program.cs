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
            char[] acarreo;
            char[] resultadoEnviar;
            string s="";
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

            if (a1 >= a2)
            {

                matriz = new char[a2 + 2, a1 + a2+1];
                for (int x = 0; x <= a2+1; x++)
                {
                    for (int y = 0; y < a1 + a2+1; y++)
                    {
                        matriz[x, y] = '0';
                    }
                }


                int renglon = 1;
                int escalera = 0;
                for (int i = array2.Length - 1; 0 <= i; i--)
                {
                    int ancho = a1 + a2;
                    acarreo = new char[a1];
                    for (int x = 0; x < acarreo.Length; x++)
                    {
                        acarreo[x] = '0';
                    }
                    int numero1 = int.Parse(array2[i].ToString());
                    for (int j = array1.Length - 1; 0 <= j; j--)
                    {
                        int numero2 = int.Parse(array1[j].ToString());
                        int sumacarreo = Int32.Parse(acarreo[j].ToString());
                        int mult = (numero1 * numero2) + sumacarreo;
                        string con1 = mult.ToString();
                        char[] myStringchar = con1.ToCharArray();
                        if (con1.Length == 2)
                        {
                            if (j == 0)
                            {
                                matriz[renglon, ancho - escalera - 1] = myStringchar[0];
                                matriz[renglon, ancho - escalera] = myStringchar[1];
                            }
                            else
                            {
                                acarreo[j - 1] = myStringchar[0];
                                matriz[renglon, ancho - escalera] = myStringchar[1];
                            }
                        }
                        else
                        {
                            matriz[renglon, ancho - escalera] = myStringchar[0];
                        }
                        ancho = ancho - 1;
                    }
                    escalera = escalera + 1;
                    renglon = renglon + 1;
                }
                
                for (int j = a1 + a2 ; 0 <= j; j--)
                {
                    int i;
                    int acumuladorsuma = 0;
                    for (i = 0; i <= a2; i++)
                    {
                        int numero1 = int.Parse(matriz[i, j].ToString());
                        acumuladorsuma = acumuladorsuma + numero1;
                    }
                    string suma = acumuladorsuma.ToString();
                    char[] charsuma = suma.ToCharArray();
                    if (suma.Length == 2)
                    {
                        int temp = j;
                        matriz[i, j] = charsuma[1];
                        matriz[0, --temp] = charsuma[0];
                    }
                    else
                    {
                        matriz[i, j] = charsuma[0];
                    }
                }
                if (matriz[a2 + 1, 0] == '0')
                {
                    resultadoEnviar = new char[a1 + a2 + 1];
                }
                else
                {
                    resultadoEnviar = new char[a1 + a2 + 2];
                }
                int punto = primer + segundo;
                int contador = 0;
               Boolean xx=false;
                for(int a=a1+ a2; 0<a;a--)
                {                    
                    if(contador==punto)
                    {
                        int tem = a;
                        resultadoEnviar[tem] = '.';
                        a++;
                        xx = true;
                    }
                    else
                    {
                        
                        if (xx==false)
                        {
                            int tem = a;
                            resultadoEnviar[tem] = matriz[a2 + 1, a];
                        }
                        else
                        {
                            int tem = a;
                            resultadoEnviar[--tem] = matriz[a2 + 1, a];
                        }
                                               
                    }
                    contador++;
                }
                s = new string(resultadoEnviar);
                if (resultadoEnviar[0] == '0')
                {
                    s = s.Substring(1, s.Length - 1);
                }
                
                

            }
            /*else
            {
                matriz = new char[a2 + 1, a1 + a2];
            }*/

            
            return s;
        }
    }
}
