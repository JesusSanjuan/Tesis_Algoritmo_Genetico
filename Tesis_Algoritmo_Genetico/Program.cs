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
            StreamWriter outputFile = new StreamWriter("D:\\Archivo.txt");
            outputFile.WriteLine("Tiempo algoritmo secuencial\t\t\t\t\t\tTiempo algoritmo Genetico\t\t\t\t\t\tAG TIR\t\t\t\t\t\tPorcentaje Convergencia\t\t\t\t\t\tPrecicion a 0\t\t\t\t\t\tInversion\t\t\t\t\t\tPeriodo\t\t\t\t\t\tValor de Salvamento");
            int i = 1;
            do {

                Random rand = new Random();
                byte[] bytes = new byte[40];
                rand.NextBytes(bytes);
                double inicio = 50000000;
                double lowerBound = (double)inicio - 40000000;
                double upperBound = (double)inicio + 49999999;

                double inversion = (double)(rand.NextDouble() * (upperBound - lowerBound) + lowerBound);
                inversion = Math.Truncate(inversion * 10000) / 10000;//////////////////
                Random randNum = new Random();
                int periodo = randNum.Next(12, 600);////////////////////////////

                Random rand2 = new Random();
                byte[] bytes2 = new byte[40];
                rand2.NextBytes(bytes2);
                double lowerBound2 = (double)0;
                double upperBound2 = (double)9999999;

                double VS = rand2.NextDouble() * (upperBound2 - lowerBound2) + lowerBound2;//////////////////
                VS = Math.Truncate(VS * 100) / 100;

                double[] FNE = new double[periodo];

                Random rand3 = new Random();
                byte[] bytes3 = new byte[40];
                rand3.NextBytes(bytes3);
                for (int num = 0; num < periodo; num++)
                {
                    double inicio3 = 50000000;
                    double lowerBound3 = (double)inicio3 - 40000000;
                    double upperBound3 = (double)inicio3 + 49999999;
                    FNE[num] = rand3.NextDouble() * (upperBound3 - lowerBound3) + lowerBound3; //////////////////
                }
                
                Stopwatch tiempo = Stopwatch.StartNew();
                //double ResultadoTIR = iterativo(inversion, FNE, VS, periodo);
                tiempo.Stop();
                String Ztiempo1 = tiempo.Elapsed.TotalSeconds.ToString();

                
                List<string> Resultados2 = genetico(inversion, FNE, VS, periodo);
                
                Console.WriteLine("Fin de la prueba {0} con el algoritmo genetico\n",i);
                
                outputFile.WriteLine(Ztiempo1 + " Seg\t\t\t\t\t\t\t\t\t\t\t" + Resultados2[0].ToString() + " Seg\t\t\t\t\t\t\t" + Resultados2[1].ToString()+" %\t\t\t\t\t" + Resultados2[2].ToString() + " %\t\t\t\t\t\t\t\t" + Resultados2[3].ToString()+ " fx\t\t\t\t" + Resultados2[4].ToString() + " iteraciones\t\t\t\t$ " + inversion.ToString() + "\t\t\t\t\t" + periodo.ToString() + " meses\t\t\t\t\t" + VS.ToString()+ " vs\n");
                
                i = i + 1;
            }while(i <= (int)10);
            Console.WriteLine("\tCONCLUYERON TODAS LAS PRUEBAS\n");
            outputFile.Close();
            Console.ReadKey();
        }

        public static double iterativo(double inversion,double[] FNE, double VS, int periodo )
        {
            double  ResultadoVPN, ResultadoTIR, VPN;
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

        public static List<string> genetico(double inversion, double[] FNE, double VS, int periodo)
        {
            Stopwatch tiempo2 = Stopwatch.StartNew();
            double aproxInicial = aproximacioninicial(inversion, FNE, periodo);
            int poblacionNumero = 1000;

            Random rand2 = new Random();
            byte[] bytes2 = new byte[20];
            rand2.NextBytes(bytes2);
            double lowerBound = (double)aproxInicial - 5000;
            double upperBound = (double)aproxInicial + 5000;

            List<double> poblacion = new List<double>();

            while (poblacion.Count < poblacionNumero)
            {
                double numeroAleatorio = (double)(rand2.NextDouble() * (upperBound - lowerBound) + lowerBound);
                if (!poblacion.Contains(numeroAleatorio))
                {
                    poblacion.Add(numeroAleatorio);
                }
            }
            int i = 1;
            double porcentajeconvergencia = 0;
            double porcentaje = (((double)100) / poblacionNumero);
            List<double> ResultadosFX;
            do
            {
                ResultadosFX = fx(inversion, FNE, VS, poblacion, periodo);

                List<int> torneo1 = posTorneo(0, poblacion.Count / 2);
                List<int> torneo2 = posTorneo(poblacion.Count / 2, poblacion.Count);
                List<double> padre = Seleccion(torneo1, torneo2, ResultadosFX, poblacion);

                List<int> cruce1 = posTorneo(0, padre.Count / 2);
                List<int> cruce2 = posTorneo(padre.Count / 2, padre.Count);

                List<double> poblacionnueva1 = Cruce(cruce1, cruce2, padre);
                cruce1 = DesordenarLista(cruce1);
                cruce2 = DesordenarLista(cruce2);
                List<double> poblacionnueva2 = Cruce(cruce1, cruce2, padre);
                List<double> hijos_Generados = poblacionnueva1.Concat(poblacionnueva2).ToList();
                poblacion.Clear();
                poblacion = padre.Concat(hijos_Generados).ToList();

                if (cruce1.Count() != cruce2.Count())
                {
                    int numeroAleatorio = new Random().Next(0, padre.Count / 2);
                    int numeroAleatorio2 = new Random().Next(padre.Count / 2, padre.Count);
                    List<double> hijoimpar = CruceImpar(padre[numeroAleatorio], padre[numeroAleatorio2]);
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
                        double valor = Convert.ToDouble(el.Text);
                        porcentajeconvergencia = porcentaje * valor;

                        //Console.WriteLine("\t****************************\n");
                        //Console.WriteLine("\tGeneracion: {0} , Convergencia del: {1}\n", i, porcentajeconvergencia);
                       // break;
                    }
                }
                i = i + 1;
            } while (porcentajeconvergencia <(double)98);
            tiempo2.Stop();
            String Ztiempo2 = tiempo2.Elapsed.TotalSeconds.ToString();
            List<string> resultados=new List<string>();
            resultados.Add(Ztiempo2);
            resultados.Add(poblacion[0].ToString());
            resultados.Add(porcentajeconvergencia.ToString());
            resultados.Add(ResultadosFX[0].ToString());
            resultados.Add(i.ToString()); ;
            return resultados;
        }

        public static double CalcularTIR(double ValorTIR, int caso, double inversion, double[] FNE, double VdS, int n)
        {
            double TasaIncDec = 0.01;
            double Resultado;
            Boolean MenosCero = false;
            double ValorTIRR = ValorTIR + TasaIncDec;
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

                    } while (Math.Abs(Resultado) >= 0.01);
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
                    } while (Math.Abs(Resultado) >= 0.01);
                    break;
            }
            return ValorTIRR;
        }
        
        static double aproximacioninicial(double Inversion, double[] FNE, int Periodo)
        {
            double resultado, sumasuperior = 0, sumainferior = 0;
            for (int i = 0; i < FNE.Length; i++)
            {
                double t = (FNE[i] * (i + 1));
                sumasuperior = sumasuperior + t;
                sumainferior = sumainferior + FNE[i];
            }
            resultado = sumasuperior / sumainferior;

            double x0;
            double t1 = sumainferior / Inversion;
            double t2 = 1 / resultado;
            x0 = Math.Pow(t1,t2);
            x0 = (x0 - 1) * 100;
            return x0;
        }

        static List<double> fx(double Inversion, double[] FNE, double VS, List<double> poblacion, int Periodo)
        {
            List<double> ResultadosFX = new List<double>();
            double valorFX = 0;
            foreach (double contenido in poblacion)
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

        static List<double> Seleccion(List<int> p1, List<int> p2, List<double> ResultadosFX, List<double> poblacion)
        {
            List<double> padre = new List<double>();
            List<double> padrefx = new List<double>();

            for (int i = 0; i < p1.Count; i++)
            {
                double fx1 = ResultadosFX[p1[i]];
                double fx2 = ResultadosFX[p2[i]];

                double pob1 = poblacion[p1[i]];
                double pob2 = poblacion[p2[i]];

                List<double> tem = new List<double>();
                tem.Add(fx1);
                tem.Add(fx2);


                double ganador = 0;
                int indexganadorPob = 0;

                if (fx1 >= 0 && fx2 >= 0)
                {
                    List<double> reorganizacionPos = tem.Where(x => x > 0).ToList();
                    ganador = reorganizacionPos.Min(x => x);
                    indexganadorPob = ResultadosFX.FindIndex(x => x == ganador);
                    padrefx.Add(ganador);
                    padre.Add(poblacion[indexganadorPob]);
                }
                else if (fx1 < -0 && fx2 < -0)
                {
                    List<double> reorganizacionNega = tem.Where(x => x < 0).ToList();
                    ganador = reorganizacionNega.Max(x => x);
                    indexganadorPob = ResultadosFX.FindIndex(x => x == ganador);
                    padrefx.Add(ganador);
                    padre.Add(poblacion[indexganadorPob]);
                }
                else
                {
                    double fx1p, fx2p;
                    fx1p = Math.Abs(fx1);
                    fx2p = Math.Abs(fx2);

                    tem.Clear();

                    tem.Add(fx1p);
                    tem.Add(fx2p);

                    if (fx1 < -0)
                    {

                        List<double> reorganizacionPos = tem.Where(x => x > 0).ToList();
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
                        List<double> reorganizacionPos = tem.Where(x => x > 0).ToList();
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

        static List<double> Cruce(List<int> cruce1, List<int> cruce2, List<double> padre)
        {
            List<double> hijos = new List<double>();
            for (int i = 0; i < cruce1.Count; i++)
            {
                double padre1 = padre[cruce1[i]];
                double padre2 = padre[cruce2[i]];

                double media = (padre1 + padre2) / 2;
                //double media_geometrica = (double)Math.Sqrt((Math.Pow((double)padre1,2) * (Math.Pow((double)padre2,2))));
                //double media_geometrica = (double)Math.Sqrt((double)(padre1*padre2));
                hijos.Add(media);
            }
            return hijos;
        }

        static List<double> CruceImpar(double padre1, double padre2)
        {
            List<double> hijos = new List<double>();
            double media = (padre1 + padre2) / 2;
            hijos.Add(media);
            return hijos;
        }

        public static double CalcularVPN(double Inversion, double[] FNE, double VS, double TMAR, int Periodo)
        {
            double FNEAcumulado = 0, fVPN = 0;
            int i = 0;
            double DivTMAR = 1 + TMAR;
            for (i = 1; i < Periodo; i++)
            {
                double valorinferior = Math.Pow(DivTMAR, i);               
                FNEAcumulado = FNEAcumulado + (FNE[i - 1] / valorinferior);
            }
            double valorinferiorF = Math.Pow(DivTMAR, i);
            FNEAcumulado = FNEAcumulado + ((FNE[i - 1] + VS) / valorinferiorF);
            fVPN = FNEAcumulado - Inversion;
            return fVPN;
        }
    }
}
