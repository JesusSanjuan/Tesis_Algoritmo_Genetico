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
                byte[] bytes = new byte[5];
                rand.NextBytes(bytes);
                Double numeroaleatorio = rand.NextDouble() * 100;
                numeroaleatorio = Math.Round(numeroaleatorio, 2);
                int count = BitConverter.GetBytes(decimal.GetBits((decimal)numeroaleatorio)[3])[2];
                string str = numeroaleatorio.ToString();
                Console.Write("Numero alearotio en decimal {0:G} \n", numeroaleatorio);
                List<string> resultados = new List<string>();
                
                resultados =dividircadena(str,count+1);
                String primerconversion = convertirbinario(resultados[0]);
                String segundaconversion = convertirbinariodecimal(resultados[1]);
                String numerocompletobinario = primerconversion + "." + segundaconversion;
                Console.Write("Numero decimal en binario: {0:G} \n", numerocompletobinario);
                Console.ReadKey();
            } while (true);
        }

        static string convertirbinario(String cadena)
        {
            int Num = Convert.ToInt32(cadena);
            String cad = "";
            if (Num > 0)
            {               
                while (Num > 0)
                {
                    if (Num % 2 == 0)
                    {
                        cad = "0" + cad;
                    }
                    else
                    {
                        cad = "1" + cad;
                    }
                    Num = (int)(Num / 2);
                }
            }
            return cad;
        }

        static List<string> dividircadena(string cadena, int tamano)
        {
            String cad1="";
            String cad2="";
            int pos = 0;
            List<string> cadenas = new List<string>();
            for (int i = 0; i < cadena.Length - 1; i++)
            {
                if (cadena[i] =='.')
                {
                    pos =i;
                    break;
                }
            }
            cad1 = cadena.Substring(0, pos);
            cad2 = cadena.Substring(pos,tamano);
            cadenas.Add(cad1);
            cadenas.Add(cad2);
            return cadenas;
        }

        static string convertirbinariodecimal(string valor)
        {
            double valfloat = Convert.ToDouble(valor);
            valfloat = valfloat * 2;
            List<string> binariofraccion = new List<string>();
            double salida = 0;
            int contador = 0;
            do
            {
                List<string> resultados = new List<string>();
                int count = BitConverter.GetBytes(decimal.GetBits((decimal)valfloat)[3])[2];
                resultados =dividircadena(Convert.ToString(valfloat), count+1);
                binariofraccion.Add(resultados[0]);
                salida = Convert.ToDouble(resultados[1]);
                valfloat = Convert.ToDouble(resultados[1]) * 2;
                contador++;
                if(contador==8)
                {
                    break;
                }
            } while (salida != 0);
            return convertirstring(binariofraccion);
        }

        static string convertirstring(List<string> valor)
        {
            String palabra="";
            foreach (string contenido in valor)
            {
                palabra = palabra + contenido;
            }
            return palabra;
        }
    }
}
