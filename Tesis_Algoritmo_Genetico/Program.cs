using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Tesis_Algoritmo_Genetico
{
   
    class Program
    {        
        static void Main(string[] args)
        {
            StreamWriter outputFile = new StreamWriter("D:\\Archivo.txt");
            outputFile.WriteLine("Num Prueba\t\t\tTiempo algoritmo Genetico\t\tAG TIR\t\t\t\t\tAG Aprox Inicial\t\tPorcentaje Convergencia\t\tPrecision a 0\t\t\t\t\tPeriodo\t\t\t\tGeneraciones del AG\t\t\t\t ¿Inversion\t\t\tValor de Salvamento");
            Console.WriteLine("Introduza la cantidad de pruebas a realizar:  ");
            int i = Convert.ToInt32(Console.ReadLine());
            int cont = 1;
            Random random = new Random();
            Random randNum = new Random();
            Random random2 = new Random();
            Random random3 = new Random();
            Console.WriteLine("\nEjecutando pruebas\n");
            Stopwatch tiempoT = Stopwatch.StartNew();
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
                outputFile.WriteLine("Prueba "+cont.ToString()+"\t\t\t\t" + Resultados2[0] + " Seg\t\t\t" + Resultados2[1]+" %\t\t\t" + Resultados2[2] + "%\t\t\t" + Resultados2[3] + " %\t\t\t\t\t" + Resultados2[4]+ " fx\t\t\t\t\t" + periodo.ToString() + "meses\t\t\t$ " + Resultados2[5]  + " Generaciones\t\t\t" + inversion.ToString() + "\t\t" + VS.ToString()+ " vs\n");
                
                cont = cont + 1;
            }while(cont <= i);
            tiempoT.Stop();
            String tiemTotal = tiempoT.Elapsed.TotalSeconds.ToString();
            String tiemTotal2 = tiempoT.Elapsed.TotalMinutes.ToString();
            Console.WriteLine("\tCONCLUYERON TODAS LAS PRUEBAS EN {0} SEGUNDOS  \n", tiemTotal);
            outputFile.WriteLine("\tCONCLUYERON TODAS LAS PRUEBAS EN {0} SEGUNDOS  \n", tiemTotal);
            outputFile.WriteLine("\tCONCLUYERON TODAS LAS PRUEBAS EN {0} MINUTOS  \n", tiemTotal2);
            outputFile.Close();
            Thread.Sleep(1000);
            Console.WriteLine("\t ABRIENDO REPORTE\n");
            AbrirArchivo("D:\\Archivo.txt");
            Thread.Sleep(1000);
            Console.WriteLine("\t Cerrando\n");
            Thread.Sleep(1000);
        }

        public static List<string> genetico(double inversion, double[] FNE, double VS, int periodo)
        {
            Stopwatch tiempo2 = Stopwatch.StartNew();
            double aproxInicial = aproximacioninicial(inversion, FNE, periodo);
            int poblacionNumero = 1000;

            Random random = new Random();
            double minimo = aproxInicial - 1000;
            double maximo = aproxInicial + 1000;

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
            Random random2 = new Random();
            do
            {
                ResultadosFX = fx(inversion, FNE, VS, poblacion, periodo);

                List<int> torneo1 = posTorneo(0, poblacion.Count / 2);
                List<int> torneo2 = posTorneo(poblacion.Count / 2, poblacion.Count);
                List<double> padre = Seleccion(torneo1, torneo2, ResultadosFX, poblacion);

                List<int> cruce1 = posTorneo(0, padre.Count / 2);
                List<int> cruce2 = posTorneo(padre.Count / 2, padre.Count);
                int c1 = cruce1.Count();
                int c2 = cruce2.Count();
                List<double> hijos_Generados = new List<double>();


                double probCruz = random2.NextDouble();

                if (probCruz < 0.8)
                {
                    hijos_Generados = CruceTotal(padre, cruce1, cruce2);
                }
                else
                {
                    /* if (c1 != c2)LO COMENTADO ES PARA MANEJAR POBLACIONES IMPARES
                     {                        
                         padre.RemoveAt(random.Next(0, padre.Count));
                         hijos_Generados = padre;LO COMENTADO ES PARA MANEJAR POBLACIONES IMPARES
                     }
                     elseLO COMENTADO ES PARA MANEJAR POBLACIONES IMPARES
                     {*/
                    hijos_Generados = padre;
                    //}LO COMENTADO ES PARA MANEJAR POBLACIONES IMPARES
                }

                poblacion.Clear();
                poblacion = padre.Concat(hijos_Generados).ToList();
                poblacion= DesordenarLista(poblacion);

                /*if (c1 != c2)///LO COMENTADO ES PARA MANEJAR POBLACIONES IMPARES
                {
                    int numeroAleatorio = new Random().Next(0, padre.Count / 2);
                    int numeroAleatorio2 = new Random().Next(padre.Count / 2, padre.Count);
                    List<double> hijoimpar = CruceImpar(padre[numeroAleatorio], padre[numeroAleatorio2]);
                    poblacion = poblacion.Concat(hijoimpar).ToList();LO COMENTADO ES PARA MANEJAR POBLACIONES IMPARES
                }*/

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
                    }
                    break;
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
            resultados.Add(aproxInicial.ToString());
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

        static List<double> CruceTotal(List<double> padre, List<int> cruce1, List<int> cruce2)
        {
            List<double> poblacionnueva1 = Cruce(cruce1, cruce2, padre);
            cruce1 = DesordenarLista(cruce1);
            cruce2 = DesordenarLista(cruce2);
            List<double> poblacionnueva2 = Cruce(cruce1, cruce2, padre);
           return poblacionnueva1.Concat(poblacionnueva2).ToList();
        }

        static List<double> Cruce(List<int> cruce1, List<int> cruce2, List<double> padre)
        {
            List<double> hijos = new List<double>();
            Random random = new Random();
            for (int i = 0; i < cruce1.Count; i++)
            {
                double padre1 = padre[cruce1[i]];
                double padre2 = padre[cruce2[i]];

                double media = (padre1 + padre2) / 2;
                //double media_geometrica = (double)Math.Sqrt((Math.Pow((double)padre1,2) * (Math.Pow((double)padre2,2))));
                //double media_geometrica = (double)Math.Sqrt((double)(padre1*padre2));
                //media = mutacion(media, random);
                hijos.Add(media);
            }
            return hijos;
        }

        static double mutacion(double hijo, Random random)
        {            
            double mutacion = random.NextDouble();
            if (mutacion < 0.06)
            {
               // int mutApli = random.Next(1, 1000);
                return hijo+ Convert.ToDouble(mutacion);
            }
            else
            {
                return hijo;
            }            
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

        private static void AbrirArchivo(string Path)
        {
            Process P = new Process();
            try
            {
                P.StartInfo.FileName = Path;
                P.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                P.Start();
                //Espera el proceso para que lo termine y continuar
                P.WaitForExit();
                //Liberar
                P.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " No se puede abrir el documento " + Path, "Error");
            }
        }
    }
}
