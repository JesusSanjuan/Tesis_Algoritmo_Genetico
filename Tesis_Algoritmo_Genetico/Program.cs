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
            String inve, per, vss, vpn;
            decimal inversion, VS, VPN, ResultadoVPN, ResultadoTIR;
            int periodo;
            Console.WriteLine("Introduzca la inversion (P): ");
            inve = Console.ReadLine();
            Console.WriteLine("Introduzca el periodo (N) Meses: ");
            per = Console.ReadLine();
            Console.WriteLine("Introduzca la valor de salvamento: ");
            vss = Console.ReadLine();
            Console.WriteLine("Introduzca el VPN posible: ");
            vpn = Console.ReadLine();

            decimal[] FNE = new decimal[Convert.ToInt32(per)];
            Random rand0 = new Random();
            byte[] bytes = new byte[5];
            rand0.NextBytes(bytes);

            for (int x = 0; x < Convert.ToInt32(per); x++)
            {
                /*Double numeroaleatorio = rand0.NextDouble() * 950000;
                numeroaleatorio = Math.Round(numeroaleatorio, 4);
                // Console.WriteLine("\tIntroduzca el FNE {0}: ", x+1);
                //  String temporal= Console.ReadLine();
                FNE[x] = Convert.ToDecimal(numeroaleatorio);*/
                FNE[x] = 15000;
                //Console.Write("\n\tNumero aleatorio de FNE del mes {0}: {1}", x + 1, numeroaleatorio.ToString());
            }
            inversion = Convert.ToDecimal(inve);
            periodo = Convert.ToInt32(per);
            VS = Convert.ToDecimal(vss);
            VPN = Convert.ToDecimal(vpn);
            /*Lectura de informacion de datos de VPN*/

            Console.WriteLine("Introduzca la cantidad de la problacion:  ");
            String poblacionNumero;
            poblacionNumero = Console.ReadLine();
            List<decimal> poblacion = new List<decimal>();
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
                decimal resultado = concatenacion(enterooo.ToString(), partedecimal.ToString());
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

            Console.WriteLine("Evaluando");
            Evaluacion(inversion, FNE, VS, poblacion, periodo);
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

        static int Evaluacion(decimal Inversion, decimal[] FNE, decimal VS, List<decimal> poblacion, int Periodo)
        {
            List<decimal> Resultado = new List<decimal>();
            foreach (decimal contenido in poblacion)
            {
                //Console.WriteLine("{0}", contenido.ToString());
                Resultado.Add(CalcularVPN(Inversion, FNE, VS, contenido / 100, Periodo));
            }

            return 0;
        }

        public static decimal CalcularVPN(decimal Inversion, decimal[] FNE, decimal VS, decimal TMAR, int Periodo)
        {
            //VAN
            decimal FNEAcumulado = 0, fVPN = 0;
            int i = 0;
            /* try
             {*/
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

            /* }
             catch (OverflowException e)
             {
                 Console.WriteLine("A sobre pasado el tamaño del decimal Exception: {0} > {1}.", e, decimal.MaxValue);
             }*/
            return fVPN;
        }

    }
}
