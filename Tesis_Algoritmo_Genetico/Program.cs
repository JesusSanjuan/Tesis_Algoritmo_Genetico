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
            Console.WriteLine("Introduzca la inversion (P):  ");
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

            /*for (int x = 0; x < Convert.ToInt32(per); x++)
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

            Console.WriteLine("Introduzca la cantidad de la problacion (Numeros pares):  ");
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
            List<int> poblacionPunto = new List<int>();

            while (poblacion.Count < Int32.Parse(poblacionNumero))
            {
                decimal numeroAleatorio = (decimal)(rand2.NextDouble() * (upperBound - lowerBound) + lowerBound);
                string numeroAleatorioString = numeroAleatorio.ToString();
                int count = BitConverter.GetBytes(decimal.GetBits(numeroAleatorio)[3])[2];
                if (!poblacion.Contains(numeroAleatorio))
                {
                    poblacion.Add(numeroAleatorio);
                    poblacionPunto.Add((numeroAleatorioString.Length-1)- count);
                }
            }
            Console.WriteLine("____________________________\n");                     
            Console.WriteLine("Imprimiendo Poblacion en decimal:");
             foreach (float contenido in poblacion)
             {
                 Console.WriteLine("{0}", contenido.ToString());
             }
            Console.WriteLine("____________________________\n");

            List<decimal> ResultadosFX= fx(inversion, FNE, VS, poblacion, periodo);
            decimal SumatorioaFx = fxSumatoria(ResultadosFX);
            List<int> torneo1 = posTorneo(poblacion.Count,0, poblacion.Count/2);
            List<int> torneo2 = posTorneo(poblacion.Count,poblacion.Count/2, poblacion.Count);
            Console.WriteLine("Evaluando");
            //Evaluacion(inversion, FNE, VS, poblacion, periodo);
            Console.ReadKey();

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

        static List<decimal> fx(decimal Inversion, decimal[] FNE, decimal VS, List<decimal> poblacion, int Periodo)
        {
            List<decimal> ResultadosFX = new List<decimal>();
            decimal valorFX = 0;
            foreach (decimal contenido in poblacion)
            {
                valorFX = CalcularVPN(Inversion, FNE, VS, contenido / 100, Periodo);
                ResultadosFX.Add(valorFX);
            }
            return ResultadosFX;
        }

        static decimal fxSumatoria(List<decimal> ResultadosFx)
        {
            decimal SumatoriaFx = 0;
            foreach (decimal contenido in ResultadosFx)
            {
                SumatoriaFx = SumatoriaFx + contenido;
            }
            return SumatoriaFx;
        }

        static List<int> posTorneo(int poblacionT,int inicio, int tamañopoblacion)
        {
            var torneo = new List<int>();
            while (torneo.Count < (poblacionT/2))
            {
                int numeroAleatorio = new Random().Next(inicio,tamañopoblacion);
                if (!torneo.Contains(numeroAleatorio))
                {
                    torneo.Add(numeroAleatorio);
                }
            }
            return torneo;
        }
            static int Evaluacion(decimal Inversion, decimal[] FNE, decimal VS, List<decimal> poblacion, int Periodo)
            {
            List<decimal> ResultadosFX = new List<decimal>();
            decimal SumatorioaFx=0;
            decimal valorFX=0;
            foreach (decimal contenido in poblacion)
            {
                valorFX = CalcularVPN(Inversion, FNE, VS, contenido / 100, Periodo);                
                ResultadosFX.Add(valorFX);
                SumatorioaFx = SumatorioaFx + valorFX;
            }

            List<decimal> reorganizacionPos = ResultadosFX.Where(x => x > 0).ToList();
            decimal valorPositivo = reorganizacionPos.Min(x => x);
            int indexPositivo = reorganizacionPos.FindIndex(x => x == valorPositivo);

            List<decimal> reorganizacionNega = ResultadosFX.Where(x => x < 0).ToList();
            decimal valorNegativo= reorganizacionNega.Max(x => x);
            int indexNegativo = reorganizacionNega.FindIndex(x => x == valorNegativo);


           /* var torneo1 = new List<int>();
            while (torneo1.Count < (poblacion.Count/2))
            {
                int numeroAleatorio = new Random().Next(0, (poblacion.Count / 2));
                if (!torneo1.Contains(numeroAleatorio))
                {
                    torneo1.Add(numeroAleatorio);
                }
            }

            var torneo2 = new List<int>();
            while (torneo2.Count < (poblacion.Count / 2))
            {
                int numeroAleatorio = new Random().Next((poblacion.Count / 2), poblacion.Count);
                if (!torneo2.Contains(numeroAleatorio))
                {
                    torneo2.Add(numeroAleatorio);
                }
            }*/
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
    }
}
