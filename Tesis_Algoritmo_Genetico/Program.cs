using System;
using System.Diagnostics;

namespace Tesis_Algoritmo_Genetico
{

    class Program
    {
        static void Main(string[] args)
        {
            String inve, per, vss;
            decimal inversion, VS, VPN, ResultadoVPN, ResultadoTIR;
            int periodo;
            Console.WriteLine("Introduzca la inversion (P): ");
            inve = Console.ReadLine();
            Console.WriteLine("Introduzca el perdio (N) Meses: ");
            per = Console.ReadLine();
            Console.WriteLine("Introduzca el valor de salvamento: ");
            vss = Console.ReadLine();

            decimal[] FNE = new decimal[Convert.ToInt32(per)];
            /*FNE[0] = 1000;
            FNE[1] = 2000;
            FNE[2] = 1500;
            FNE[3] = 3000;
            /*Random rand = new Random();
            byte[] bytes = new byte[5];
            rand.NextBytes(bytes);*/

            for (int x = 0; x < Convert.ToInt32(per); x++)
            {
                /*Double numeroaleatorio = rand0.NextDouble() * 950000;
                numeroaleatorio = Math.Round(numeroaleatorio, 4);*/
                Console.WriteLine("\tIntroduzca el FNE {0}: ", x + 1);
                String temporal = Console.ReadLine();
                FNE[x] = Convert.ToDecimal(temporal);
                //FNE[x] = 15000;                
                //Console.Write("\n\tNumero aleatorio de FNE del mes {0}: {1}", x + 1, numeroaleatorio.ToString());
            }
            inversion = Convert.ToDecimal(inve);
            periodo = Convert.ToInt32(per);
            VS = Convert.ToDecimal(vss);


            Console.Write("\n\n\n\n\nCorre tiempo calculando\n\n\n\n\n");
            Stopwatch tiempo = Stopwatch.StartNew();
            VPN = aproximacioninicial(inversion, FNE, periodo);
            Console.WriteLine("\nAproximacion inicial es: {0}\n", VPN);
            ResultadoVPN = CalcularVPN(inversion, FNE, VS, VPN / 100, periodo);
            //Console.Write("\n\n RESULTADO DE VPN: {0} \n\n", ResultadoVPN.ToString("0,0.0000"));
            if (ResultadoVPN > 0)
            {
                ResultadoTIR = CalcularTIR(VPN / 100, 1, inversion, FNE, VS, periodo);
            }
            else
            {
                ResultadoTIR = CalcularTIR(VPN / 100, 2, inversion, FNE, VS, periodo);
            }
            ResultadoTIR = ResultadoTIR * 100;
            tiempo.Stop();
            Console.Write("\n\n RESULTADO DE TIR: {0} \n", ResultadoTIR.ToString());
            if (Stopwatch.IsHighResolution)
            {
                Console.WriteLine("Alta precisión");
            }
            else
            {
                Console.WriteLine("Baja precisión");
            }

            Console.WriteLine($"Tiempo: {tiempo.Elapsed.TotalSeconds} segundos");
            Console.WriteLine($"Precision: {(1.0 / Stopwatch.Frequency).ToString("E")} segundos");
            Console.ReadKey();
        }

        public static decimal CalcularTIR(decimal ValorTIR, int caso, decimal inversion, decimal[] FNE, decimal VdS, int n)
        {
            decimal TasaIncDec = 0.01M;
            decimal Resultado;
            Boolean MenosCero = false;
            decimal ValorTIRR = ValorTIR + TasaIncDec;
            switch (caso)
            {
                case 1:
                    do
                    {

                        Resultado = CalcularVPN(inversion, FNE, VdS, ValorTIRR, n);//Cabie aqui tenia hasta 10
                        Console.Write("1.---Ya es igual a 0? = {0} \n", Resultado);
                        if (MenosCero == true)
                        {
                            TasaIncDec = TasaIncDec / 2;
                            MenosCero = false;
                        }
                        if (Resultado > 0)
                        {
                            ValorTIRR = Math.Round(ValorTIRR + TasaIncDec, 10);
                        }
                        else
                        {
                            ValorTIRR = Math.Round(ValorTIRR - TasaIncDec, 10);
                            MenosCero = true;
                        }

                    } while (Math.Abs(Resultado) >= 0.01M);
                    break;
                case 2:
                    do
                    {
                        Resultado = CalcularVPN(inversion, FNE, VdS, ValorTIRR, n);
                        Console.Write("2.---Ya es igual a 0? = {0} \n", Resultado);
                        if (MenosCero == true)
                        {
                            TasaIncDec = TasaIncDec / 2;
                            MenosCero = false;
                        }
                        if (Resultado > 0)
                        {
                            ValorTIRR = ValorTIRR + TasaIncDec;
                            MenosCero = true;
                        }
                        else
                        {
                            ValorTIRR = ValorTIRR - TasaIncDec;

                        }
                    } while (Math.Abs(Resultado) >= 0.01M);
                    break;
            }
            return ValorTIRR;
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
                decimal valorinferior = (decimal)Math.Pow((double)DivTMAR, i);
                FNEAcumulado = FNEAcumulado + (FNE[i - 1] / valorinferior);
            }
            decimal valorinferiorF = (decimal)Math.Pow((double)DivTMAR, i);
            FNEAcumulado = FNEAcumulado + ((FNE[i - 1] + VS) / valorinferiorF);
            fVPN = FNEAcumulado - Inversion;

            /* }
             catch (OverflowException e)
             {
                 Console.WriteLine("A sobre pasado el tamaño del decimal Exception: {0} > {1}.", e, decimal.MaxValue);
             }*/
            return fVPN;
        }

        static decimal aproximacioninicial(decimal Inversion, decimal[] FNE, int Periodo)
        {
            decimal resultado, sumasuperior = 0, sumainferior = 0;
            for (int i = 0; i < FNE.Length; i++)
            {
                decimal t = (FNE[i] * (i + 1));
                sumasuperior = sumasuperior + t;
                sumainferior = sumainferior + FNE[i];
            }
            resultado = sumasuperior / sumainferior;

            decimal x0;
            x0 = (decimal)Math.Pow((double)(sumainferior / Inversion), (double)(1 / resultado));
            x0 = (x0 - 1) * 100;
            return x0;
        }
    }
}
