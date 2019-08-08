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
            /*Lectura de informacion de datos de VPN*/
            String inve, per, vss;
            decimal inversion, VS;
            int periodo;
            Console.WriteLine("Introduzca la inversion (P): ");
            inve = Console.ReadLine();
            Console.WriteLine("Introduzca el periodo (N) Meses: ");
            per = Console.ReadLine();
            Console.WriteLine("Introduzca la valor de salvamento: ");
            vss = Console.ReadLine();
            //Console.WriteLine("Introduzca el VPN posible: ");
            //vpn = Console.ReadLine();

            decimal[] FNE = new decimal[Convert.ToInt32(per)];
            Random rand0 = new Random();
            byte[] bytes = new byte[5];
            rand0.NextBytes(bytes);

            FNE[0] = 400;
            FNE[1] = 500;
            FNE[2] = 650;
            FNE[3] = 350;
            FNE[4] = 400;
            FNE[5] = 200;

         /*   for (int x = 0; x < Convert.ToInt32(per); x++)
            {
                /*Double numeroaleatorio = rand0.NextDouble() * 950000;
                numeroaleatorio = Math.Round(numeroaleatorio, 4);
                // Console.WriteLine("\tIntroduzca el FNE {0}: ", x+1);
                //  String temporal= Console.ReadLine();
                FNE[x] = Convert.ToDecimal(numeroaleatorio);*/
                //FNE[x] = 15000;
                
                //Console.Write("\n\tNumero aleatorio de FNE del mes {0}: {1}", x + 1, numeroaleatorio.ToString());
          //  }*/

            inversion = Convert.ToDecimal(inve);
            periodo = Convert.ToInt32(per);
            VS = Convert.ToDecimal(vss);
            //VPN = Convert.ToDecimal(vpn);
            /*Lectura de informacion de datos de VPN*/

            Console.WriteLine("Introduzca la cantidad de la problacion:  ");
            String poblacionNumero;
            poblacionNumero = Console.ReadLine();

            Console.WriteLine("\n\nBuscando aproximacion inicial..");
            decimal aproxInicial= aproximacioninicial(inversion,FNE, periodo);
            Console.WriteLine("\nAproximacion inicial es: {0}\n", aproxInicial);
            Console.WriteLine("____________________________\n");
            Console.WriteLine("Generandos numeros aleatorios -3 y +1 entorno a la aproximacion incial\n");

            // Generate and display 5 random byte (integer) values.
            Random rand2 = new Random();            
            byte[] bytes2 = new byte[5];
            rand2.NextBytes(bytes2);
            double lowerBound = (double) aproxInicial- 3;
            double upperBound = (double )aproxInicial+ 1;      

            List<decimal> poblacion = new List<decimal>();
            List<String> poblacionBinaria = new List<String>();

            for (int ctr = 0; ctr < Int32.Parse(poblacionNumero); ctr++)
            {
                decimal numerogenerado = (decimal)(rand2.NextDouble() * (upperBound - lowerBound) + lowerBound);
                Console.Write("Numero aletorio generado: {0,8:N6}\n", numerogenerado);
                int count = BitConverter.GetBytes(decimal.GetBits(numerogenerado)[3])[2];
                string str = numerogenerado.ToString();
                List<string> resultados = new List<string>();
                resultados = dividircadena(str, count + 1);

                String primerconversion = enteroabinario(resultados[0]);
                decimal conprimerconversion= Convert.ToDecimal(primerconversion);
                string fmt = "000000";
                primerconversion = conprimerconversion.ToString(fmt);
                String segundaconversion = DecimalBinarioaBinario(resultados[1]);
                String numerocompletobinario = primerconversion + "." + segundaconversion;
                poblacionBinaria.Add(numerocompletobinario);

                String entero = numerocompletobinario.Substring(0, numerocompletobinario.IndexOf("."));
                String flotante = numerocompletobinario.Substring(numerocompletobinario.IndexOf(".") + 1, numerocompletobinario.Length - numerocompletobinario.IndexOf(".") - 1);
                
                int enterooo = BinarioAentero(entero);
                float partedecimal = BinarioADecimalFlotante(flotante);
                decimal resultado = concatenacion(enterooo.ToString(), partedecimal.ToString());
                poblacion.Add(resultado);                
            }
            Console.WriteLine("____________________________\n");

            Console.WriteLine("Imprimiendo Poblacion binaria con punto:");
            foreach (string contenido in poblacionBinaria)
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

            Console.WriteLine("Evaluando");
            Evaluacion(inversion, FNE, VS, poblacion, periodo);
            Console.ReadKey();

        }

        static int BinarioAentero(String input)
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

        static string enteroabinario(String cadena)
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

       static string DecimalBinarioaBinario(string valor)
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
                resultados = dividircadena(Convert.ToString(valfloat), count + 1);
                binariofraccion.Add(resultados[0]);
                salida = Convert.ToDouble(resultados[1]);
                valfloat = Convert.ToDouble(resultados[1]) * 2;
                contador++;
                if (contador == 12)
                {
                    break;
                }
            } while (salida != 0);
            return convertirstring(binariofraccion);
        }


        static decimal concatenacion(String input, String input2)
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
            return Convert.ToDecimal(resultado); ;
        }

        static decimal aproximacioninicial(decimal Inversion, decimal[] FNE, int Periodo)
        {
            decimal resultado, sumasuperior=0, sumainferior=0;
            for(int i=0;i<FNE.Length;i++)
            {
                decimal t = (FNE[i] * (i + 1));
                sumasuperior = sumasuperior + t;
                sumainferior = sumainferior + FNE[i];
            }
            resultado = sumasuperior / sumainferior;

            decimal x0;
            x0 =(decimal) Math.Pow((double)(sumainferior/Inversion ), (double)(1/resultado));
            x0 = (x0 - 1)*100;
            return x0;
        }

        static int Evaluacion(decimal Inversion, decimal[] FNE, decimal VS, List<decimal> poblacion, int Periodo)
        {
            List<decimal> Resultado = new List<decimal>();
            List<decimal> ProbabilidadSeleccion = new List<decimal>();
            decimal SumatorioaFx=0;
            decimal valorSumatoria=0;
            foreach (decimal contenido in poblacion)
            {
                valorSumatoria = CalcularVPN(Inversion, FNE, VS, contenido / 100, Periodo);
                if(valorSumatoria>0)
                {
                    SumatorioaFx = SumatorioaFx + valorSumatoria;
                    
                }
                Resultado.Add(valorSumatoria);
            }
            foreach (decimal contenido in Resultado)
            {
                ProbabilidadSeleccion.Add((contenido/SumatorioaFx)*100);
            }
            return 0;
        }

        public static decimal CalcularVPN(decimal Inversion, decimal[] FNE, decimal VS, decimal TMAR, int Periodo)
        {
            decimal FNEAcumulado = 0, fVPN = 0;
            int i = 0;
            decimal DivTMAR = 1M + TMAR;
            for (i = 1; i < Periodo; i++)
            {
                decimal valorinferior = Decimal.Round((decimal)Math.Pow((double)DivTMAR, i), 10);
                string str = valorinferior.ToString();
                valorinferior = Convert.ToDecimal(str);
                FNEAcumulado = Decimal.Round(FNEAcumulado + FNE[i - 1] / valorinferior, 10);
                string str2 = FNEAcumulado.ToString();
                FNEAcumulado = Convert.ToDecimal(str2);
            }
            decimal valorinferiorF = Decimal.Round((decimal)Math.Pow((double)DivTMAR, i), 10);
            string str3 = valorinferiorF.ToString();
            valorinferiorF = Convert.ToDecimal(str3);
            FNEAcumulado = Decimal.Round(FNEAcumulado + ((FNE[i - 1] + VS) / valorinferiorF), 10);
            string str4 = FNEAcumulado.ToString();
            FNEAcumulado = Convert.ToDecimal(str4);
            fVPN = FNEAcumulado - Inversion;
            return fVPN;
        }
        static List<string> dividircadena(string cadena, int tamano)
        {
            String cad1 = "";
            String cad2 = "";
            int pos = 0;
            List<string> cadenas = new List<string>();
            for (int i = 0; i < cadena.Length - 1; i++)
            {
                if (cadena[i] == '.')
                {
                    pos = i;
                    break;
                }
            }
            cad1 = cadena.Substring(0, pos);
            cad2 = cadena.Substring(pos, tamano);
            cadenas.Add(cad1);
            cadenas.Add(cad2);
            return cadenas;
        }

        static string convertirstring(List<string> valor)
        {
            String palabra = "";
            foreach (string contenido in valor)
            {
                palabra = palabra + contenido;
            }
            return palabra;
        }
    }
}
