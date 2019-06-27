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
            double inversion, periodo, VS, VPN;
            Console.WriteLine("Introduzca la inversion (P): ");
            inve = Console.ReadLine();
            Console.WriteLine("Introduzca el perdio (N): ");
            per= Console.ReadLine();
            Console.WriteLine("Introduzca la valor de salvamento: ");
            vss = Console.ReadLine();
            Console.WriteLine("Introduzca el VPN posible: ");
            vpn = Console.ReadLine();

            double[] FNE = new double[Convert.ToInt32(per)];
            Console.WriteLine("Introduzca los FNE: ");
            for(int x=0; x<Convert.ToInt32(per);x++)
            {
                Console.WriteLine("\tIntroduzca el FNE {0}: ", x+1);
                String temporal= Console.ReadLine();
                FNE[x] = Convert.ToDouble(temporal);
            }
            inversion = Convert.ToDouble(inve);
            periodo = Convert.ToDouble(per);
            VS = Convert.ToDouble(vss);
            VPN = Convert.ToDouble(vpn);
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
                for (i = 1; i < Periodo; i++)
                {
                    decimal valorinferior = (decimal)Math.Pow((double)DivTMAR, i);
                    FNEAcumulado = FNEAcumulado + FNE[i - 1] / valorinferior;
                }
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
