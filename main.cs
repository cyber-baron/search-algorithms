using System;
using System.Diagnostics;

namespace PFindStringCS
{
    class Program
    {
        static void Main(string[] args)
        {
            string strS1 = "aaaaaaaaaaaaaaaaaaaaaaaaaaab";
            string strS2 = "aaaaaaab";
                                

            Console.WriteLine("Simple algorithm");
            Console.WriteLine($"Index where pattern found: {SimpleSearch(strS1, strS2)} \n");

            Console.WriteLine($"Rabin-Karp algorithm");
            Console.WriteLine($"Index where pattern found: {RKSearch(strS1, strS2)} \n");

            Console.WriteLine($"Knut-Moris-Prat algorithm");
            Console.WriteLine($"Index where pattern found: {KMPSearch(strS1, strS2)} \n");


            Console.ReadLine();
        }

        //==============================================================
        // алгоритм КМП
        static public int KMPSearch(string strS1, string strS2)
        {
            Stopwatch timerKMP = new Stopwatch();
            timerKMP.Start();

            int M = strS2.Length;
            int N = strS1.Length;
            int res = 0;

            int[] arrPrefix = new int[M]; // create arrPrefix[] that will hold the longest prefix suffix values for pattern
            int j = 0; // index for srtS2[]
            int i = 0; // index for strS1[]

            GetPrefix(strS2, M, arrPrefix); // Preprocess the pattern (calculate arrPrefix[] array)

            while (i < N) 
            {
                if (strS2[j] == strS1[i]) 
                {
                    j++;
                    i++;
                }
                if (j == M) 
                {
                    res = i - j;
                    j = arrPrefix[j - 1];

                    timerKMP.Stop();
                    var elapsedTime1 = timerKMP.Elapsed;
                    Console.WriteLine($"Time: {elapsedTime1}");
                    //Console.WriteLine("\nlength txt " + N + "\n");

                    return res;
                }
                else if (i < N && strS2[j] != strS1[i]) 
                {
                    if (j != 0)
                        j = arrPrefix[j - 1];
                    else
                        i = i + 1;
                }
            }
            timerKMP.Stop();
            var elapsedTime2 = timerKMP.Elapsed;
            Console.WriteLine($"Time: {elapsedTime2}");

            return -1;
        }

        //==============================================================
        // Префикс-функция для алгоритма КМП
        static public int[] GetPrefix(string strS2, int M, int[] arrPrefix)
        {
            // length of the previous longest prefix suffix
            int len = 0;
            int i = 1;

            arrPrefix[0] = 0; // arrPrefix[0] is always 0

            // the loop calculates arrPrefix[i] for i = 1 to M-1
            while (i < M)
            {
                if (strS2[i] == strS2[len])
                {
                    len++;
                    arrPrefix[i] = len;
                    i++;
                }
                else // (pat[i] != pat[len])
                {
                    if (len != 0)
                        len = arrPrefix[len - 1];
                    else 
                    {
                        arrPrefix[i] = len;
                        i++;
                    }
                }
            }

            return arrPrefix;
        }

        //==============================================================
        // алгоритм Рабина-Карпа
        static public int RKSearch(string strS1, string strS2)
        {
            Stopwatch timerRK = new Stopwatch();
            timerRK.Start();

            int q = 3571; //15487469;// Math.Pow(2, 64) - 1;  big Mersenne prime
            int d = 256; // size of alphabet

            int i, j;
            int N = strS1.Length;
            int M = strS2.Length;

            int h = 1;
            int p = 0; //hash value for text
            int t = 0; //hash value for pattern

            for (i = 0; i < M - 1; i++)
                h = (h * d) % q;

            for (i = 0; i < M; i++)
            {
                p = (d * p + strS2[i]) % q;
                t = (d * t + strS1[i]) % q;
            }

            for (i = 0; i <= N - M; i++)
            {
                if (p == t)
                {
                    for (j = 0; j < M; j++)
                    {
                        if (strS1[i + j] != strS2[j])
                            break;
                    }

                    if (j == M)
                    {
                        timerRK.Stop();
                        var elapsedTime1 = timerRK.Elapsed;
                        Console.WriteLine($"Time: {elapsedTime1}");

                        return i;
                    }
                }
            
                if (i < N - M)
                {
                    t = (d * (t - strS1[i] * h) + strS1[i + M]) % q;

                    if (t < 0)
                        t = (t + q);
                }
            }
            timerRK.Stop();
            var elapsedTime2 = timerRK.Elapsed;
            Console.WriteLine($"Time: {elapsedTime2}");

            return -1;
        }

        //===================================================================
        // алгоритм наивного поиска
        static public int SimpleSearch(string strHaystack, string strNeedle)
        {
            Stopwatch timerSimp = new Stopwatch();
            timerSimp.Start();

            int intHLen = strHaystack.Length;
            int intNLen = strNeedle.Length;
            bool blnSuccess = false;

            for (int intC1 = 0; intC1 <= intHLen - intNLen; intC1++)
            {
                int intC2= 0;
                while (strNeedle[intC2] == strHaystack[intC1+intC2])
                {
                    if (intC2 == intNLen - 1)
                    { 
                        blnSuccess = true;
                        break;
                    }
                    intC2++;
                }
                if (blnSuccess)
                {
                    timerSimp.Stop();
                    var elapsedTime1 = timerSimp.Elapsed;
                    Console.WriteLine($"Time: {elapsedTime1}");

                    return (intC1);
                }
            }
            timerSimp.Stop();
            var elapsedTime2 = timerSimp.Elapsed;
            Console.WriteLine($"Time: {elapsedTime2}");

            return -1;
        }

       
    }
}