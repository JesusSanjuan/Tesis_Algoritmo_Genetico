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
            /*Lectura de informacion de datos de VPN*/
            String inve, per, vss;
            decimal inversion, VS;
            int periodo;
            Console.WriteLine("Introduzca la inversion (P):  ");
            inve = Console.ReadLine();
            Console.WriteLine("Introduzca el periodo (N) Meses: ");
            per = Console.ReadLine();
            Console.WriteLine("Introduzca la valor de salvamento: ");
            vss = Console.ReadLine();
            //Console.WriteLine("Introduzca el VPN posible: ");
            //vpn = Console.ReadLine();

            decimal[] FNE = new decimal[Convert.ToInt32(per)];
            Random rand0 = new Random();
            byte[] bytes = new byte[5];
            rand0.NextBytes(bytes);

            FNE[0] = 15000;
            FNE[1] = 15000;
            FNE[2] = 15000;
            FNE[3] = 15000;
            FNE[4] = 15000;
            //FNE[5] = 200;

            /*for (int x = 0; x < Convert.ToInt32(per); x++)
            {
                /*Double numeroaleatorio = rand0.NextDouble() * 950000;
                numeroaleatorio = Math.Round(numeroaleatorio, 4);
                // Console.WriteLine("\tIntroduzca el FNE {0}: ", x+1);
                //  String temporal= Console.ReadLine();
                FNE[x] = Convert.ToDecimal(numeroaleatorio);*/
                //FNE[x] = 15000;
                
                //Console.Write("\n\tNumero aleatorio de FNE del mes {0}: {1}", x + 1, numeroaleatorio.ToString());
          //  }*/

            inversion = Convert.ToDecimal(inve);
            periodo = Convert.ToInt32(per);
            VS = Convert.ToDecimal(vss);
            //VPN = Convert.ToDecimal(vpn);
            /*Lectura de informacion de datos de VPN*/

            Console.WriteLine("Introduzca la cantidad de la problacion (Numeros pares):  ");
            String poblacionNumero;
            poblacionNumero = Console.ReadLine();

            /*Console.WriteLine("Introduzca las generaciones a realizar:  ");
            String generaciones;
            generaciones = Console.ReadLine();*/
            
            Console.WriteLine("Introduzca la convergencia:  ");
            String convergencia;
            convergencia = Console.ReadLine();

            Console.WriteLine("\n\nBuscando aproximacion inicial..");
            decimal aproxInicial= aproximacioninicial(inversion,FNE, periodo);
            Console.WriteLine("\nAproximacion inicial es: {0}\n", aproxInicial);
            Console.WriteLine("____________________________\n");
            Console.WriteLine("Generandos numeros aleatorios -3 y +1 entorno a la aproximacion incial\n");

            // Generate and display 5 random byte (integer) values.
            Random rand2 = new Random();            
            byte[] bytes2 = new byte[5];
            rand2.NextBytes(bytes2);
            double lowerBound = (double) aproxInicial- 5;
            double upperBound = (double )aproxInicial+ 5;      

            List<decimal> poblacion = new List<decimal>();
            //List<int> poblacionPunto = new List<int>();

            while (poblacion.Count < Int32.Parse(poblacionNumero))
            {
                decimal numeroAleatorio = (decimal)(rand2.NextDouble() * (upperBound - lowerBound) + lowerBound);
                string numeroAleatorioString = numeroAleatorio.ToString();
                int count = BitConverter.GetBytes(decimal.GetBits(numeroAleatorio)[3])[2];
                if (!poblacion.Contains(numeroAleatorio))
                {
                    poblacion.Add(numeroAleatorio);
                    //poblacionPunto.Add((numeroAleatorioString.Length-1)- count);
                }
            }
            /*Console.WriteLine("____________________________\n");                     
            Console.WriteLine("Imprimiendo Poblacion en decimal:");
             foreach (float contenido in poblacion)
             {
                 Console.WriteLine("{0}", contenido.ToString());
             }*/
            Console.WriteLine("____________________________\n");
            Console.WriteLine("Evaluando");
            decimal convergengiaiteracion = 0;
            decimal SumatorioaFxAnterior = 0;
            int i = 0;
            do
            {
                if(i==60)
                {

                }
                List<decimal> ResultadosFX = fx(inversion, FNE, VS, poblacion, periodo);
                decimal SumatorioaFx = fxSumatoria(ResultadosFX);

                convergengiaiteracion = SumatorioaFxAnterior / SumatorioaFx;

                List<int> torneo1 = posTorneo( 0, poblacion.Count / 2);
                List<int> torneo2 = posTorneo( poblacion.Count / 2, poblacion.Count);
                List<decimal> padre = Seleccion(torneo1, torneo2, ResultadosFX, poblacion);

                List<int> cruce1 = posTorneo( 0, padre.Count / 2);
                List<int> cruce2 = posTorneo( padre.Count / 2, padre.Count);

                List<int> crucetotal = cruce1.Concat(cruce2).ToList();
                int pos = Impar(crucetotal, padre.Count);

                if (pos != -1)
                {
                    int numeroAleatorio;
                    do
                    {
                        numeroAleatorio= new Random().Next(0, padre.Count);
                    } while (numeroAleatorio == pos);
                    List<decimal> hijoimpar = CruceImpar(padre[pos], padre[numeroAleatorio]);
                    List<decimal> poblacionnueva1 = Cruce(cruce1, cruce2, padre);
                    cruce2.Reverse();
                    List<decimal> poblacionnueva2 = Cruce(cruce1, cruce2, padre);
                    List<decimal> hijos_Generados = poblacionnueva1.Concat(poblacionnueva2).ToList();
                    poblacion.Clear();
                    poblacion = padre.Concat(hijos_Generados).ToList();
                    poblacion = poblacion.Concat(hijoimpar).ToList();
                }
                else
                {
                    List<decimal> poblacionnueva1 = Cruce(cruce1, cruce2, padre);
                    cruce2.Reverse();
                    List<decimal> poblacionnueva2 = Cruce(cruce1, cruce2, padre);
                    List<decimal> hijos_Generados = poblacionnueva1.Concat(poblacionnueva2).ToList();
                    poblacion.Clear();
                    poblacion = padre.Concat(hijos_Generados).ToList();
                    
                }
                SumatorioaFxAnterior = SumatorioaFx;
                i = i + 1;
            } while (Convert.ToDecimal(convergencia) >= convergengiaiteracion);
            //Console.WriteLine("\nIteraciones Totales: {0}\n", i);
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

        static decimal fxSumatoria(List<decimal> ResultadosFx)
        {
            decimal SumatoriaFx = 0;
            foreach (decimal contenido in ResultadosFx)
            {
                SumatoriaFx = SumatoriaFx + contenido;
            }
            return SumatoriaFx;
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

        static int Impar(List<int> crucetotal, int padre)
        {
            int valorfaltante = -1; ;
            for(int i=0; i< padre; i++)
            {
                int pos = crucetotal.FindIndex(x => x == i);
                if(pos==-1)
                {
                    valorfaltante = i;
                    break;
                }
            }
            return valorfaltante;
        }

        public static decimal CalcularVPN(decimal Inversion, decimal[] FNE, decimal VS, decimal TMAR, int Periodo)
        {
            decimal FNEAcumulado = 0, fVPN = 0;
            int i = 0;
            decimal DivTMAR = 1M + TMAR;
            for (i = 1; i < Periodo; i++)
            {
                decimal valorinferior = Decimal.Round((decimal)Math.Pow((double)DivTMAR, i), 10);
                string str = valorinferior.ToString();
                valorinferior = Convert.ToDecimal(str);
                FNEAcumulado = Decimal.Round(FNEAcumulado + FNE[i - 1] / valorinferior, 10);
                string str2 = FNEAcumulado.ToString();
                FNEAcumulado = Convert.ToDecimal(str2);
            }
            decimal valorinferiorF = Decimal.Round((decimal)Math.Pow((double)DivTMAR, i), 10);
            string str3 = valorinferiorF.ToString();
            valorinferiorF = Convert.ToDecimal(str3);
            FNEAcumulado = Decimal.Round(FNEAcumulado + ((FNE[i - 1] + VS) / valorinferiorF), 10);
            string str4 = FNEAcumulado.ToString();
            FNEAcumulado = Convert.ToDecimal(str4);
            fVPN = FNEAcumulado - Inversion;
            return fVPN;
        }        
    }
}
