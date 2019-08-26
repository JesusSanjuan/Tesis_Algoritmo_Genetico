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
            decimal inversion, VS;
            int periodo;
            Console.WriteLine("Introduzca la inversion (P):  ");
            inve = Console.ReadLine();
            Console.WriteLine("Introduzca el periodo (N) Meses: ");
            per = Console.ReadLine();
            Console.WriteLine("Introduzca el valor de salvamento: ");
            vss = Console.ReadLine();
            //Console.WriteLine("Introduzca el VPN posible: ");
            //vpn = Console.ReadLine();

            decimal[] FNE = new decimal[Convert.ToInt32(per)];
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
                FNE[x] = Convert.ToDecimal(temporal);
                //FNE[x] = 15000;                
                //Console.Write("\n\tNumero aleatorio de FNE del mes {0}: {1}", x + 1, numeroaleatorio.ToString());
            }

            inversion = Convert.ToDecimal(inve);
            periodo = Convert.ToInt32(per);
            VS = Convert.ToDecimal(vss);
            //VPN = Convert.ToDecimal(vpn);
            /*Lectura de informacion de datos de VPN*/

            Console.WriteLine("\nIntroduzca la cantidad de la poblacion (Numeros pares):  ");
            String poblacionNumero;
            poblacionNumero = Console.ReadLine();

            /*Console.WriteLine("Introduzca la convergencia:  ");
            String convergencia;
            convergencia = Console.ReadLine();*/          

            Console.WriteLine("\n\nBuscando aproximacion inicial..");

            Stopwatch tiempo = Stopwatch.StartNew();

            decimal aproxInicial= aproximacioninicial(inversion,FNE, periodo);
            Console.WriteLine("\nAproximacion inicial es: {0}\n", aproxInicial);
            Console.WriteLine("Generandos numeros aleatorios -100 y +100 entorno a la aproximacion incial\n");

            // Generate and display 5 random byte (integer) values.
            Random rand2 = new Random();            
            byte[] bytes2 = new byte[5];
            rand2.NextBytes(bytes2);
            double lowerBound = (double) aproxInicial- 100;
            double upperBound = (double )aproxInicial+ 100;      

            List<decimal> poblacion = new List<decimal>();

            while (poblacion.Count < Int32.Parse(poblacionNumero))
            {
                decimal numeroAleatorio = (decimal)(rand2.NextDouble() * (upperBound - lowerBound) + lowerBound);
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
            decimal porcentajeconvergencia = 0;
            decimal porcentaje = Convert.ToDecimal(100) / Convert.ToDecimal(poblacionNumero);
            List<decimal> ResultadosFX;
            do
            {                
                ResultadosFX = fx(inversion, FNE, VS, poblacion, periodo);

                List<int> torneo1 = posTorneo( 0, poblacion.Count / 2);
                List<int> torneo2 = posTorneo( poblacion.Count / 2, poblacion.Count);
                List<decimal> padre = Seleccion(torneo1, torneo2, ResultadosFX, poblacion);

                List<int> cruce1 = posTorneo( 0, padre.Count / 2);
                List<int> cruce2 = posTorneo( padre.Count / 2, padre.Count);               
                
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

                //Agrupamos la lista
                //var agrupacion = poblacion.GroupBy(x => x).Select(g => new { Text = g.Key, Count = g.Count() }).ToList();      
                //var agrupacioncont = poblacion.GroupBy(x => x).Select(g => new { Count = g.Count() }).ToList();
                var agrupacion = poblacion.GroupBy(x => x).Select(g =>  g.Count() ).ToList();
                var agrupacion2= agrupacion.GroupBy(x => x).Select(g => new { Text = g.Key, Count = g.Count() }).ToList();
                var agrupacion3 = agrupacion.GroupBy(x => x).Select(g =>  g.Key).ToList();
                var valormax = agrupacion3.Max();
               
                foreach (var el in agrupacion2)
                {
                    if(el.Count ==1 && el.Text==valormax)
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
            tiempo.Stop();
            Console.WriteLine("____________________________\n");
            /*Console.WriteLine("Imprimiendo la Poblacion de la generacion {0} final: ", i);
            foreach (decimal contenido in poblacion)
            {
                Console.WriteLine("\t{0}", contenido.ToString());
            }
            Console.WriteLine("____________________________\n");
            Console.WriteLine("Imprimiendo la ResultadoFX de la generacion {0} final: ", i);
            foreach (decimal contenido in ResultadosFX)
            {
                Console.WriteLine("\t{0}", contenido.ToString());
            }*/
            Console.WriteLine("\n\t\t RESULTADO TMAR: {0}", ResultadosFX[0]);
            Console.WriteLine("\n\t\t RESULTADO TIR: {0}", poblacion[0]);
            Console.WriteLine("\n\nEL ALGORITMO A TERMINADO LA BUSQUEDA\n\n");            
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
        
        static List<int> posTorneo( int inicio, int tamañopoblacion)
        {
            var torneo = new List<int>();
            var resultorneo = new List<int>();
            for (int i=inicio;i<tamañopoblacion; i++)
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

        static List<decimal> Seleccion(List<int> p1, List<int> p2, List<decimal> ResultadosFX, List<decimal>poblacion)
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

                    if (fx1<-0)
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

        static List<decimal> Cruce(List<int> cruce1, List<int> cruce2,List<decimal> padre)
        {
            List<decimal> hijos= new List<decimal>();
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
