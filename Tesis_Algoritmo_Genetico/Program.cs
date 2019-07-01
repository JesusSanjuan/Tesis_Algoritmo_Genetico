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
            String inve, per, vss,vpn;
            decimal inversion, VS, VPN, ResultadoVPN, ResultadoTIR;
            int periodo;
            Console.WriteLine("Introduzca la inversion (P): ");
            inve = Console.ReadLine();
            Console.WriteLine("Introduzca el perdio (N) Meses: ");
            per= Console.ReadLine();
            Console.WriteLine("Introduzca la valor de salvamento: ");
            vss = Console.ReadLine();
            Console.WriteLine("Introduzca el VPN posible: ");
            vpn = Console.ReadLine();

            decimal[] FNE = new decimal[Convert.ToInt32(per)];
            Random rand = new Random();
            byte[] bytes = new byte[5];
            rand.NextBytes(bytes);
            
            for (int x=0; x<Convert.ToInt32(per);x++)
            {
                Double numeroaleatorio = rand.NextDouble() * 2000000;
                numeroaleatorio = Math.Round(numeroaleatorio, 4);
                // Console.WriteLine("\tIntroduzca el FNE {0}: ", x+1);
                //  String temporal= Console.ReadLine();
                FNE[x] = Convert.ToDecimal(numeroaleatorio);
                Console.Write("\n\tNumero aleatorio de FNE del mes {0}: {1}", x+1,numeroaleatorio.ToString());
            }
            inversion = Convert.ToDecimal(inve);
            periodo = Convert.ToInt32(per);
            VS = Convert.ToDecimal(vss);
            VPN = Convert.ToDecimal(vpn);

            Console.ReadKey();
            DateTime tiempo1 = DateTime.Now;
            ResultadoVPN = CalcularVPN(inversion, FNE, VS, VPN / 100, periodo);
            Console.Write("\n\n RESULTADO DE VPN: {0}", ResultadoVPN.ToString("0,0.0000")); 
            if (ResultadoVPN>0)
            {
                ResultadoTIR=CalcularTIR(VPN/100, 1, inversion, FNE, VS, periodo);
            }
            else
            {
                ResultadoTIR=CalcularTIR(VPN/100, 2, inversion, FNE, VS, periodo);
            }
            ResultadoTIR = ResultadoTIR * 100;
            Console.Write("\n\n RESULTADO DE TIR: {0} ", ResultadoTIR.ToString("0,0.0000"));
            DateTime tiempo2 = DateTime.Now;
            TimeSpan total = new TimeSpan(tiempo2.Ticks - tiempo1.Ticks);
            Console.Write("\n\n\n\n TIEMPO TOTAL DE EJECUCION: {0} ", total.ToString());            
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
            decimal FNEAcumulado = 0, fVPN = 0;
            int i = 0;
            try
            {
                decimal DivTMAR = 1M + TMAR;
                Console.WriteLine("\n\n\n\nCalculo de VPN\n");
                for (i = 1; i < Periodo; i++)
                {
                    decimal valorinferior = (decimal)Math.Pow((double)DivTMAR, i);
                    FNEAcumulado = FNEAcumulado + FNE[i - 1] / valorinferior;
                    Console.WriteLine("Pos: {0} Valor: {1}",i,FNEAcumulado);
                }
                Console.WriteLine("\n\n\n\n\n");
                decimal valorinferiorF = (decimal)Math.Pow((double)DivTMAR, i);
                FNEAcumulado = FNEAcumulado + ((FNE[i - 1] + VS) / valorinferiorF);
                fVPN = FNEAcumulado - Inversion;
            }
            catch (OverflowException e)
            {
                Console.WriteLine("Exception: {0} > {1}.", e, decimal.MaxValue);
            }
            return fVPN;
        }
    }
}
