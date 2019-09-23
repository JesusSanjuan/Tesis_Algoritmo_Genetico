using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace Tesis_Algoritmo_Genetico
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Lectura de informacion de datos de VPN*/
            String inve, per, vss;
            double inversion, VS;
            int periodo;
            Console.WriteLine("Introduzca la inversion (P):  ");
            inve = Console.ReadLine();
            Console.WriteLine("Introduzca el periodo (N) Meses: ");
            per = Console.ReadLine();
            Console.WriteLine("Introduzca el valor de salvamento: ");
            vss = Console.ReadLine();
            //Console.WriteLine("Introduzca el VPN posible: ");
            //vpn = Console.ReadLine();

            double[] FNE = new double[Convert.ToInt32(per)];
            /*Random rand0 = new Random();
            byte[] bytes = new byte[5];
            rand0.NextBytes(bytes);*/

            //FNE[0] = 1000;
            //FNE[1] = 2000;
            //FNE[2] = 1500;
            //FNE[3] = 3000;
            //FNE[4] = 400;
            //FNE[5] = 200;

            for (int x = 0; x < Convert.ToInt32(per); x++)
            {
                /*Double numeroaleatorio = rand0.NextDouble() * 950000;
                numeroaleatorio = Math.Round(numeroaleatorio, 4);*/
                Console.WriteLine("\tIntroduzca el FNE {0}: ", x+1);
                String temporal= Console.ReadLine();
                FNE[x] = Convert.ToDouble(temporal);
                //FNE[x] = 15000;                
                //Console.Write("\n\tNumero aleatorio de FNE del mes {0}: {1}", x + 1, numeroaleatorio.ToString());
            }

            inversion = Convert.ToDouble(inve);
            periodo = Convert.ToInt32(per);
            VS = Convert.ToDouble(vss);
            //VPN = Convert.Todouble(vpn);
            /*Lectura de informacion de datos de VPN*/
             
            Console.WriteLine("\nIntroduzca la cantidad de la poblacion (Numeros pares):  ");
            String poblacionNumero;
            poblacionNumero = Console.ReadLine();

            /*Console.WriteLine("Introduzca la convergencia:  ");
            String convergencia;
            convergencia = Console.ReadLine();*/          

            Console.WriteLine("\n\nBuscando aproximacion inicial..");

            Stopwatch tiempo = Stopwatch.StartNew();

            double aproxInicial= aproximacioninicial(inversion,FNE, periodo);
            Console.WriteLine("\nAproximacion inicial es: {0}\n", aproxInicial);
            Console.WriteLine("Generandos numeros aleatorios -100 y +100 entorno a la aproximacion incial\n");

       
            Random random = new Random();
            double minimo = aproxInicial - 1500;
            double maximo = aproxInicial + 1500;              

            List<double> poblacion = new List<double>();

            while (poblacion.Count < Int32.Parse(poblacionNumero))
            {
                double numeroAleatorio = random.NextDouble() * (maximo - minimo) + minimo;
                if (!poblacion.Contains(numeroAleatorio))
                {
                    poblacion.Add(numeroAleatorio);
                }
            }
           /* Console.WriteLine("____________________________\n");                     
            Console.WriteLine("Imprimiendo Poblacion inicial:");
             foreach (float contenido in poblacion)
             {
                 Console.WriteLine("{0}", contenido.ToString());
             }
            Console.WriteLine("____________________________\n");
            Console.WriteLine("Evaluando, Buscando convergencia del 95%");
            Console.WriteLine("____________________________\n\n\n");*/
            //Console.ReadKey();
            int i = 1;
            double porcentajeconvergencia = 0;
            double porcentaje = (((double)100) / Int32.Parse(poblacionNumero));
            List<double> ResultadosFX;
            do
            {                
                ResultadosFX = fx(inversion, FNE, VS, poblacion, periodo);

                List<int> torneo1 = posTorneo( 0, poblacion.Count / 2);
                List<int> torneo2 = posTorneo( poblacion.Count / 2, poblacion.Count);
                List<double> padre = Seleccion(torneo1, torneo2, ResultadosFX, poblacion);

                List<int> cruce1 = posTorneo( 0, padre.Count / 2);
                List<int> cruce2 = posTorneo( padre.Count / 2, padre.Count);               
                
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

                //Agrupamos la lista
                //var agrupacion = poblacion.GroupBy(x => x).Select(g => new { Text = g.Key, Count = g.Count() }).ToList();      
                //var agrupacioncont = poblacion.GroupBy(x => x).Select(g => new { Count = g.Count() }).ToList();
                
                List<double> contandovalores = new List<double>();
                List<double> contandovalores2 = new List<double>();
                /*foreach (var valor in poblacion)
                {
                    contandovalores.Add(valor);
                    contandovalores2 .Add( poblacion.Where(x => x == valor).Count());
                    
                }*/



                var agrupacion = poblacion.GroupBy(x => x).Select(g => g.Count()).ToList();
                agrupacion =agrupacion.OrderByDescending(o => o).ToList();                
                var agrupacion2= agrupacion.GroupBy(x => x).Select(g => new { Text = g.Key, Count = g.Count() }).ToList();
                var agrupacion3 = agrupacion.GroupBy(x => x).Select(g =>  g.Key).ToList();
                var valormax = agrupacion3.Max();
               
                foreach (var el in agrupacion2)
                {
                    if(el.Text==valormax && el.Count == 1)
                    {
                        double valor = Convert.ToDouble(el.Text);                        
                        porcentajeconvergencia = porcentaje * valor;
                        
                        Console.WriteLine("\t****************************\n");
                        Console.WriteLine("\tGeneracion: {0} , Convergencia del: {1}\n", i, porcentajeconvergencia);
                        break;
                    }
                }
                i = i + 1;              
            } while (porcentajeconvergencia < (double)95);
            tiempo.Stop();



            var resultTIR = poblacion.GroupBy(x => x).Select(g => new { Text = g.Key, Count = g.Count() }).ToList();
            resultTIR = resultTIR.OrderByDescending(o => o.Count).ToList();

            var resultTMR = ResultadosFX.GroupBy(x => x).Select(g => new { Text = g.Key, Count = g.Count() }).ToList();
            resultTMR = resultTMR.OrderByDescending(o => o.Count).ToList();
            Console.WriteLine("____________________________\n");
            Console.WriteLine("Imprimiendo la Poblacion de la generacion {0} final: ", i);
            foreach (double contenido in poblacion)
            {
                Console.WriteLine("\t{0}", contenido.ToString());
            }
            Console.WriteLine("____________________________\n");
            Console.WriteLine("Imprimiendo la ResultadoFX de la generacion {0} final: ", i);
            foreach (double contenido in ResultadosFX)
            {
                Console.WriteLine("\t{0}", contenido.ToString());
            }
            Console.WriteLine("\n\t\t RESULTADO TMAR: {0}", resultTMR[0].Text);
            Console.WriteLine("\n\t\t RESULTADO TIR: {0}", resultTIR[0].Text);
            Console.WriteLine("\n\nEL ALGORITMO A TERMINADO LA BUSQUEDA\n\n");   
            
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
