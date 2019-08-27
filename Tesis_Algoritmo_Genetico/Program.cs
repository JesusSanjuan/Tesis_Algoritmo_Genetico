using System;
using System.IO;
using System.Diagnostics;
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
            Random rand = new Random();
            byte[] bytes = new byte[20];
            rand.NextBytes(bytes);
            decimal inicio = 500000;
            double lowerBound = (double)inicio - 20000;
            double upperBound = (double)inicio + 20000;

            decimal inversion = (decimal)(rand.NextDouble() * (upperBound - lowerBound) + lowerBound);//////////////////

            Random randNum = new Random();
            int periodo = randNum.Next(12, 600);////////////////////////////

            Random rand2 = new Random();
            byte[] bytes2 = new byte[20];
            rand2.NextBytes(bytes2);
            double lowerBound2 = (double)0;
            double upperBound2 = (double)20000;

            decimal VS = (decimal)(rand2.NextDouble() * (upperBound2 - lowerBound2) + lowerBound2);//////////////////


            decimal[] FNE = new decimal[periodo];

            Random rand3 = new Random();
            byte[] bytes3 = new byte[20];
            rand3.NextBytes(bytes3);
            for (int num = 0; num < periodo; num++)
            {                
                decimal inicio3 = 5000;
                double lowerBound3 = (double)inicio3 - 2000;
                double upperBound3 = (double)inicio3 + 2000;
                FNE[num] = (decimal)(rand3.NextDouble() * (upperBound3 - lowerBound3) + lowerBound3);//////////////////
            }


            StreamWriter outputFile = new StreamWriter("D:\\Archivo.txt");

            Stopwatch tiempo = Stopwatch.StartNew();
            decimal ResultadoTIR = iterativo(inversion, FNE, VS, periodo);
            tiempo.Stop();
            Console.WriteLine($"Tiempo: {tiempo.Elapsed.TotalSeconds} segundos");

            Stopwatch tiempo2 = Stopwatch.StartNew();
            decimal ResultadoTIR2 = genetico(inversion, FNE, VS, periodo);
            tiempo2.Stop();
            Console.WriteLine($"Tiempo: {tiempo2.Elapsed.TotalSeconds} segundos");
            
            outputFile.WriteLine("\t");
            outputFile.Close();
        }

        public static decimal iterativo(decimal inversion,decimal[] FNE, decimal VS, int periodo )
        {
            decimal  ResultadoVPN, ResultadoTIR, VPN;
            VPN = aproximacioninicial(inversion, FNE, periodo);
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
            return ResultadoTIR;
        }

        public static decimal genetico(decimal inversion, decimal[] FNE, decimal VS, int periodo)
        {
            decimal aproxInicial = aproximacioninicial(inversion, FNE, periodo);
            int poblacionNumero = 240;

            Random rand2 = new Random();
            byte[] bytes2 = new byte[5];
            rand2.NextBytes(bytes2);
            double lowerBound = (double)aproxInicial - 100;
            double upperBound = (double)aproxInicial + 100;

            List<decimal> poblacion = new List<decimal>();

            while (poblacion.Count < poblacionNumero)
            {
                decimal numeroAleatorio = (decimal)(rand2.NextDouble() * (upperBound - lowerBound) + lowerBound);
                if (!poblacion.Contains(numeroAleatorio))
                {
                    poblacion.Add(numeroAleatorio);
                }
            }
            int i = 1;
            decimal porcentajeconvergencia = 0;
            decimal porcentaje = Convert.ToDecimal(100) / Convert.ToDecimal(poblacionNumero);
            List<decimal> ResultadosFX;
            do
            {
                ResultadosFX = fx(inversion, FNE, VS, poblacion, periodo);

                List<int> torneo1 = posTorneo(0, poblacion.Count / 2);
                List<int> torneo2 = posTorneo(poblacion.Count / 2, poblacion.Count);
                List<decimal> padre = Seleccion(torneo1, torneo2, ResultadosFX, poblacion);

                List<int> cruce1 = posTorneo(0, padre.Count / 2);
                List<int> cruce2 = posTorneo(padre.Count / 2, padre.Count);

                List<decimal> poblacionnueva1 = Cruce(cruce1, cruce2, padre);
                cruce1 = DesordenarLista(cruce1);
                cruce2 = DesordenarLista(cruce2);
                List<decimal> poblacionnueva2 = Cruce(cruce1, cruce2, padre);
                List<decimal> hijos_Generados = poblacionnueva1.Concat(poblacionnueva2).ToList();
                poblacion.Clear();
                poblacion = padre.Concat(hijos_Generados).ToList();

                if (cruce1.Count() != cruce2.Count())
                {
                    int numeroAleatorio = new Random().Next(0, padre.Count / 2);
                    int numeroAleatorio2 = new Random().Next(padre.Count / 2, padre.Count);
                    List<decimal> hijoimpar = CruceImpar(padre[numeroAleatorio], padre[numeroAleatorio2]);
                    poblacion = poblacion.Concat(hijoimpar).ToList();
                }

                var agrupacion = poblacion.GroupBy(x => x).Select(g => g.Count()).ToList();
                var agrupacion2 = agrupacion.GroupBy(x => x).Select(g => new { Text = g.Key, Count = g.Count() }).ToList();
                var agrupacion3 = agrupacion.GroupBy(x => x).Select(g => g.Key).ToList();
                var valormax = agrupacion3.Max();

                foreach (var el in agrupacion2)
                {
                    if (el.Count == 1 && el.Text == valormax)
                    {
                        decimal valor = Convert.ToDecimal(el.Text);
                        porcentajeconvergencia = porcentaje * valor;

                        Console.WriteLine("\t****************************\n");
                        Console.WriteLine("\tGeneracion: {0} , Convergencia del: {1}\n", i, porcentajeconvergencia);
                        break;
                    }
                }
                i = i + 1;
            } while (Decimal.ToInt32(porcentajeconvergencia) < Decimal.ToInt32(98));

            return poblacion[0];
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

        static List<int> posTorneo(int inicio, int tamañopoblacion)
        {
            var torneo = new List<int>();
            var resultorneo = new List<int>();
            for (int i = inicio; i < tamañopoblacion; i++)
            {
                torneo.Add(i);
            }
            resultorneo = DesordenarLista(torneo);
            return resultorneo;
        }

        public static List<T> DesordenarLista<T>(List<T> input)
        {
            List<T> arr = input;
            List<T> arrDes = new List<T>();

            Random randNum = new Random();
            while (arr.Count > 0)
            {
                int val = randNum.Next(0, arr.Count - 1);
                arrDes.Add(arr[val]);
                arr.RemoveAt(val);
            }
            return arrDes;
        }

        static List<decimal> Seleccion(List<int> p1, List<int> p2, List<decimal> ResultadosFX, List<decimal> poblacion)
        {
            List<decimal> padre = new List<decimal>();
            List<decimal> padrefx = new List<decimal>();

            for (int i = 0; i < p1.Count; i++)
            {
                decimal fx1 = ResultadosFX[p1[i]];
                decimal fx2 = ResultadosFX[p2[i]];

                decimal pob1 = poblacion[p1[i]];
                decimal pob2 = poblacion[p2[i]];

                List<decimal> tem = new List<decimal>();
                tem.Add(fx1);
                tem.Add(fx2);


                decimal ganador = 0;
                int indexganadorPob = 0;

                if (fx1 >= 0 && fx2 >= 0)
                {
                    List<decimal> reorganizacionPos = tem.Where(x => x > 0).ToList();
                    ganador = reorganizacionPos.Min(x => x);
                    indexganadorPob = ResultadosFX.FindIndex(x => x == ganador);
                    padrefx.Add(ganador);
                    padre.Add(poblacion[indexganadorPob]);
                }
                else if (fx1 < -0 && fx2 < -0)
                {
                    List<decimal> reorganizacionNega = tem.Where(x => x < 0).ToList();
                    ganador = reorganizacionNega.Max(x => x);
                    indexganadorPob = ResultadosFX.FindIndex(x => x == ganador);
                    padrefx.Add(ganador);
                    padre.Add(poblacion[indexganadorPob]);
                }
                else
                {
                    decimal fx1p, fx2p;
                    fx1p = Math.Abs(fx1);
                    fx2p = Math.Abs(fx2);

                    tem.Clear();

                    tem.Add(fx1p);
                    tem.Add(fx2p);

                    if (fx1 < -0)
                    {

                        List<decimal> reorganizacionPos = tem.Where(x => x > 0).ToList();
                        ganador = reorganizacionPos.Min(x => x);
                        if (fx1p == ganador)
                        {
                            indexganadorPob = ResultadosFX.FindIndex(x => x == fx1);
                            padrefx.Add(fx1);
                            padre.Add(poblacion[indexganadorPob]);
                        }
                        else
                        {
                            indexganadorPob = ResultadosFX.FindIndex(x => x == fx2p);
                            padrefx.Add(fx2);
                            padre.Add(poblacion[indexganadorPob]);
                        }

                    }
                    else
                    {
                        List<decimal> reorganizacionPos = tem.Where(x => x > 0).ToList();
                        ganador = reorganizacionPos.Min(x => x);
                        if (fx2p == ganador)
                        {
                            indexganadorPob = ResultadosFX.FindIndex(x => x == fx2);
                            padrefx.Add(fx2);
                            padre.Add(poblacion[indexganadorPob]);
                        }
                        else
                        {
                            indexganadorPob = ResultadosFX.FindIndex(x => x == fx1p);
                            padrefx.Add(fx1);
                            padre.Add(poblacion[indexganadorPob]);
                        }
                    }
                }
            }
            return padre;
        }

        static List<decimal> Cruce(List<int> cruce1, List<int> cruce2, List<decimal> padre)
        {
            List<decimal> hijos = new List<decimal>();
            for (int i = 0; i < cruce1.Count; i++)
            {
                decimal padre1 = padre[cruce1[i]];
                decimal padre2 = padre[cruce2[i]];

                decimal media = (padre1 + padre2) / 2;
                //decimal media_geometrica = (decimal)Math.Sqrt((Math.Pow((double)padre1,2) * (Math.Pow((double)padre2,2))));
                //decimal media_geometrica = (decimal)Math.Sqrt((double)(padre1*padre2));
                hijos.Add(media);
            }
            return hijos;
        }

        static List<decimal> CruceImpar(decimal padre1, decimal padre2)
        {
            List<decimal> hijos = new List<decimal>();
            decimal media = (padre1 + padre2) / 2;
            hijos.Add(media);
            return hijos;
        }

        public static decimal CalcularVPN(decimal Inversion, decimal[] FNE, decimal VS, decimal TMAR, int Periodo)
        {
            decimal FNEAcumulado = 0, fVPN = 0;
            int i = 0;
            decimal DivTMAR = 1 + TMAR;
            for (i = 1; i < Periodo; i++)
            {
                decimal valorinferior = (decimal)Math.Pow((double)DivTMAR, i);
                FNEAcumulado = FNEAcumulado + (FNE[i - 1] / valorinferior);
            }
            decimal valorinferiorF = (decimal)Math.Pow((double)DivTMAR, i);
            FNEAcumulado = FNEAcumulado + ((FNE[i - 1] + VS) / valorinferiorF);
            fVPN = FNEAcumulado - Inversion;
            return fVPN;
        }
    }




}
}
