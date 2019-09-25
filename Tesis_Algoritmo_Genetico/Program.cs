using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Tesis_Algoritmo_Genetico
{
    class Program
    {
        static void Main(string[] args)
        {
            String inve, per, vss;
            double inversion, VS;
            int periodo;
            Console.WriteLine("Introduzca la inversion (P):  ");
            inve = Console.ReadLine();
            Console.WriteLine("Introduzca el periodo (N) Meses: ");
            per = Console.ReadLine();
            Console.WriteLine("Introduzca el valor de salvamento: ");
            vss = Console.ReadLine();

            double[] FNE = new double[Convert.ToInt32(per)];

            for (int x = 0; x < Convert.ToInt32(per); x++)
            {                
                Console.WriteLine("\tIntroduzca el FNE {0}: ", x+1);
                String temporal= Console.ReadLine();
                FNE[x] = Convert.ToDouble(temporal);
            }

            inversion = Convert.ToDouble(inve);
            periodo = Convert.ToInt32(per);
            VS = Convert.ToDouble(vss);
             
            Console.WriteLine("\nIntroduzca la cantidad de la poblacion (Numeros pares):  ");
            String poblacionNumero;
            poblacionNumero = Console.ReadLine();       

            Console.WriteLine("\n\nAproximacion inicial..");

            Stopwatch tiempo = Stopwatch.StartNew();

            double aproxInicial= aproximacioninicial(inversion,FNE, periodo);
            Console.WriteLine("\nAproximacion inicial es: {0}\n", aproxInicial);
       
            Random random = new Random();
            double minimo = aproxInicial - 1000;
            double maximo = aproxInicial + 1000;              

            List<double> poblacion = new List<double>();

            while (poblacion.Count < Int32.Parse(poblacionNumero))
            {
                double numeroAleatorio = random.NextDouble() * (maximo - minimo) + minimo;
                if (!poblacion.Contains(numeroAleatorio))
                {
                    poblacion.Add(numeroAleatorio);
                }
            }

            int i = 1;
            double porcentajeconvergencia = 0;
            double porcentaje = (((double)100) / Int32.Parse(poblacionNumero));
            List<double> ResultadosFX;
            Random random2 = new Random();
            do
            {                
                ResultadosFX = fx(inversion, FNE, VS, poblacion, periodo);

                List<int> torneo1 = posTorneo( 0, poblacion.Count / 2);
                List<int> torneo2 = posTorneo( poblacion.Count / 2, poblacion.Count);
                List<double> padre = Seleccion(torneo1, torneo2, ResultadosFX, poblacion);

                List<int> cruce1 = posTorneo( 0, padre.Count / 2);
                List<int> cruce2 = posTorneo( padre.Count / 2, padre.Count);

                List<double> hijos_Generados = new List<double>();

                double probCruz = random2.NextDouble();

                if (probCruz < 0.9)
                {
                    hijos_Generados = CruceTotal(padre, cruce1, cruce2, i);
                }
                else
                {
                    hijos_Generados = padre;
                }

                poblacion.Clear();
                poblacion = padre.Concat(hijos_Generados).ToList();
                poblacion = DesordenarLista(poblacion);

                var agrupacion = poblacion.GroupBy(x => x).Select(g => g.Count()).ToList();
                agrupacion = agrupacion.OrderByDescending(o => o).ToList();
                var agrupacion2 = agrupacion.GroupBy(x => x).Select(g => new { Text = g.Key, Count = g.Count() }).ToList();
                var agrupacion3 = agrupacion.GroupBy(x => x).Select(g => g.Key).ToList();
                var valormax = agrupacion3.Max();

                foreach (var el in agrupacion2)
                {
                    if(el.Text==valormax && el.Count == 1)
                    {
                        double valor = Convert.ToDouble(el.Text);                        
                        porcentajeconvergencia = porcentaje * valor;   
                    }
                    break;
                }
                Console.WriteLine("\t****************************\n");
                Console.WriteLine("\tGeneracion: {0} , Convergencia del: {1}\n", i, porcentajeconvergencia);
                i = i + 1;              
            } while (porcentajeconvergencia < (double)99.5);
            tiempo.Stop();



            var resultTIR = poblacion.GroupBy(x => x).Select(g => new { Text = g.Key, Count = g.Count() }).ToList();
            resultTIR = resultTIR.OrderByDescending(o => o.Count).ToList();

            var resultTMR = ResultadosFX.GroupBy(x => x).Select(g => new { Text = g.Key, Count = g.Count() }).ToList();
            resultTMR = resultTMR.OrderByDescending(o => o.Count).ToList();
            Console.WriteLine("____________________________\n");
           
            Console.WriteLine("\n\t\t RESULTADO TMAR: {0}", resultTMR[0].Text);
            Console.WriteLine("\n\t\t RESULTADO TIR: {0}", resultTIR[0].Text);
            Console.WriteLine("\n\nEL ALGORITMO TERMINO LA BUSQUEDA\n\n");   
            
            Console.WriteLine($"Tiempo: {tiempo.Elapsed.TotalSeconds} segundos");
            Console.ReadKey();
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
            x0 = Math.Pow(t1, t2);
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
                    List<double> reorganizacionPos = tem.Where(x => x >= 0).ToList();
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
                        List<double> reorganizacionPos = tem.Where(x => x >= 0).ToList();
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

        static List<double> CruceTotal(List<double> padre, List<int> cruce1, List<int> cruce2, int iteracion)
        {
            Random random = new Random();
            List<double> poblacionnueva1a = Cruce(cruce1, cruce2, padre);
            List<double> poblacionnueva1 = mutacion(poblacionnueva1a, iteracion,random);
            cruce1 = DesordenarLista(cruce1);
            cruce2 = DesordenarLista(cruce2);
            List<double> poblacionnueva2a = Cruce(cruce1, cruce2, padre);
            List<double> poblacionnueva2 = mutacion(poblacionnueva2a, iteracion,random);
            return poblacionnueva1.Concat(poblacionnueva2).ToList();
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

        static List<double> mutacion(List<double> poblacion1, int iteracion, Random random)
        {            
            for (int i = 0; i < poblacion1.Count; i++)
            {
               // double numeroAleatorio = random.NextDouble();
                double longitud = Convert.ToString(poblacion1[i]).Length-1;
                double p = Math.Pow(2 + (((longitud - 2) / (70 - 1)) * iteracion), -1);
               // Console.WriteLine("\n\t\t\t\t\tMUTACION {0}", numeroAleatorio);
                if (p > .1)//AQUI PERMITE LA MUTACION lgo invertido
                {
                    double mediageometrica = poblacion1.Sum() / poblacion1.Count;
                    double desviasion =desviasionstandar(poblacion1, mediageometrica);
                    double z = (poblacion1[i] - mediageometrica) / desviasion;
                    poblacion1[i] = poblacion1[i] + z;
                    Console.WriteLine("\n\t\t\t\tMUTACION");
                }
                else
                {
                    poblacion1[i] = poblacion1[i];
                }
            }
            return poblacion1;
        }

        static double desviasionstandar(List<double> poblacion1, double mediageometrica)
        {            
            double sumatoria = 0;
            for (int i = 0; i < poblacion1.Count; i++)
            {
                sumatoria = sumatoria + Math.Pow((poblacion1[0] - mediageometrica), 2);
            }
            return Math.Sqrt(sumatoria / (poblacion1.Count - 1));
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