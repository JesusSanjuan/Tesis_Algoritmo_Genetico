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
            outputFile.WriteLine("Prueba\t\t\t\t\tTiempo algoritmo Genetico\t\t\t\t\t\tAG TIR\t\t\t\t\t\tPorcentaje Convergencia\t\t\t\t\t\tprecision a 0\t\t\t\t\t\tIteraciones AG\t\t\t\t\t\tInversion\t\t\t\t\t\tPeriodo\t\t\t\t\t\tValor de Salvamento");
            Console.WriteLine("Introduza la cantidad de pruebas a realizar:  ");
            int i = Convert.ToInt32(Console.ReadLine());
            int cont = 1;
            Random random = new Random();
            Random randNum = new Random();
            Random random2 = new Random();
            Random random3 = new Random();
            do {
                /**************************************************************************/
                
                double minimo = 1000000000000;
                double maximo = 9999999999999;
                double inversion = random.NextDouble() * (maximo - minimo) + minimo;
                /**************************************************************************/
                
                int periodo = randNum.Next(12, 601);
                /**************************************************************************/
                
                double minimo2 = 0;
                double maximo2 = 9999999999;
                double VS = random2.NextDouble() * (maximo2 - minimo2) + minimo2;

                double[] FNE = new double[periodo];    
                
                double minimo3 = -1000000000000;
                double maximo3 = 9999999999999;
                for (int num = 0; num < periodo; num++)
                {                   
                    FNE[num] = random3.NextDouble() * (maximo3 - minimo3) + minimo3; 
                }
                /**************************************************************************/
                List<string> Resultados2 = genetico(inversion, FNE, VS, periodo);                
                Console.WriteLine("Fin de la prueba {0} de {1}, con el algoritmo genetico\n",cont,i);                
                outputFile.WriteLine("Prueba numero "+cont.ToString()+"\t\t\t\t" + Resultados2[0].ToString() + " Seg\t\t\t\t\t\t\t" + Resultados2[1].ToString()+" %\t\t\t\t\t" + Resultados2[2].ToString() + " %\t\t\t\t\t\t\t\t" + Resultados2[3].ToString()+ " fx\t\t\t\t" + Resultados2[4].ToString() + " iteraciones\t\t\t\t\t$ " + inversion.ToString() + "\t\t\t\t\t\t" + periodo.ToString() + " meses\t\t\t\t\t" + VS.ToString()+ " vs\n");
                
                cont = cont + 1;
            }while(cont <= i);

            Console.WriteLine("\tCONCLUYERON TODAS LAS PRUEBAS\n");
            outputFile.Close();
            Console.ReadKey();
        }

        public static List<string> genetico(double inversion, double[] FNE, double VS, int periodo)
        {
            Stopwatch tiempo2 = Stopwatch.StartNew();
            double aproxInicial = aproximacioninicial(inversion, FNE, periodo);
            int poblacionNumero = 1000;

            Random random = new Random();
            double minimo = aproxInicial - 500;
            double maximo = aproxInicial + 500;

            List<double> poblacion = new List<double>();

            while (poblacion.Count < poblacionNumero)
            {
                double numeroAleatorio = random.NextDouble() * (maximo - minimo) + minimo;
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
                agrupacion = agrupacion.OrderByDescending(o => o).ToList();
                var agrupacion2 = agrupacion.GroupBy(x => x).Select(g => new { Text = g.Key, Count = g.Count() }).ToList();
                var agrupacion3 = agrupacion.GroupBy(x => x).Select(g => g.Key).ToList();
                var valormax = agrupacion3.Max();

                foreach (var el in agrupacion2)
                {
                    if (el.Text == valormax && el.Count == 1 )
                    {
                        double valor = Convert.ToDouble(el.Text);
                        porcentajeconvergencia = porcentaje * valor;

                        //Console.WriteLine("\t****************************\n");
                        //Console.WriteLine("\tGeneracion: {0} , Convergencia del: {1}\n", i, porcentajeconvergencia);
                       // break;
                    }
                }
                i = i + 1;
            } while (porcentajeconvergencia <(double)99);
            tiempo2.Stop();
            var resultTIR = poblacion.GroupBy(x => x).Select(g => new { Text = g.Key, Count = g.Count() }).ToList();
            resultTIR = resultTIR.OrderByDescending(o => o.Count).ToList();

            var resultTMR = ResultadosFX.GroupBy(x => x).Select(g => new { Text = g.Key, Count = g.Count() }).ToList();
            resultTMR = resultTMR.OrderByDescending(o => o.Count).ToList();

            String Ztiempo2 = tiempo2.Elapsed.TotalSeconds.ToString();
            List<string> resultados = new List<string>();
            resultados.Add(Ztiempo2);
            resultados.Add(resultTIR[0].Text.ToString());
            resultados.Add(porcentajeconvergencia.ToString());
            resultados.Add(resultTMR[0].Text.ToString());
            resultados.Add(i.ToString());
            return resultados;
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
                if (valorinferior == 0 && DivTMAR > 0)
                {
                    valorinferior = Double.MaxValue;
                }
                if (valorinferior == 0 && DivTMAR < 0)
                {
                    valorinferior = Double.MinValue;
                }                
                FNEAcumulado = FNEAcumulado + (FNE[i - 1] / valorinferior);
            }
            double valorinferiorF = Math.Pow(DivTMAR, i);
            if (valorinferiorF == 0 && DivTMAR > 0)
            {
                valorinferiorF = Double.MaxValue;
            }
            if (valorinferiorF == 0 && DivTMAR < 0)
            {
                valorinferiorF = Double.MinValue;
            }
            FNEAcumulado = FNEAcumulado + ((FNE[i - 1] + VS) / valorinferiorF);
            fVPN = FNEAcumulado - Inversion;
            return fVPN;
        }
    }  
}
