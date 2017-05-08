using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMM
{
    public class ForwardBackward
    {
        // Algorithm from sanjose-hmm.pdf
        public static string[] GetResult(int T, int N, int M, string[] Q, int[] V, double[,] A, double[,] B, double[] pi, int[] observation)
        {
            // Alpha-pass

            //Compute a0(i)
            double c0 = 0;
            //var resultA = new List<List<double>>();
            double[,] resultA = new double[T, N];
            for (var i = 0; i <= (N - 1); i++)
            {
                resultA[0, i] = pi[i] * B[i, observation[0]];
                c0 = c0 + resultA[0, i];
            }

            //scale the a0(i)
            var divide = c0;
            c0 = 1 / divide;
            for (var i = 0; i <= (N - 1); i++)
            {
                resultA[0, i] = c0 * resultA[0, i];
            }

            // Compute at(i)
            var ct = new double[T];
            ct[0] = c0;
            for (var t = 1; t <= T - 1; t++)
            {
                ct[t] = 0;
                for (var i = 0; i <= N - 1; i++)
                {
                    resultA[t, i] = 0;
                    for (var j = 0; j <= N - 1; j++)
                    {
                        resultA[t, i] = resultA[t, i] + resultA[t - 1, j] * A[j, i];
                    }

                    resultA[t, i] = resultA[t, i] * B[i, observation[t]];
                    ct[t] = ct[t] + resultA[t, i];
                }

                //scale at(i)
                ct[t] = 1 / ct[t];
                for (var i = 0; i <= N - 1; i++)
                {
                    resultA[t, i] = ct[t] * resultA[t, i];
                }
            }

            // beta-pass
            var resultB = new double[T, N];

            // Let b(T-1)(i) scaled by c(t-1)
            for (var i = 0; i <= N - 1; i++)
            {
                resultB[T - 1, i] = ct[T - 1];
            }
            for (var t = T - 2; t >= 0; t--)
            {
                for (var i = 0; i <= N - 1; i++)
                {
                    resultB[t, i] = 0;
                    for (var j = 0; j <= N - 1; j++)
                    {
                        resultB[t, i] = resultB[t, i] + A[i, j] * B[j, observation[t + 1]] * resultB[t + 1, j];
                    }
                    // scale Bi(t) with same scale factor as At(i)
                    resultB[t, i] = ct[t] * resultB[t, i];
                }
            }

            // Compute yt(i,j) and yt(i)
            //var y = new List<List<List<double>>>();
            double[,,] y = new double[T, N, N];
            //var resultY = new List<List<double>>();
            var resultY = new double[T, N];
            for (var t = 0; t <= T - 2; t++)
            {
                double denom = 0;
                for (var i = 0; i <= N - 1; i++)
                {
                    for (var j = 0; j <= N - 1; j++)
                    {
                        denom = denom + resultA[t, i] * A[i, j] * B[j, observation[t + 1]] * resultB[t + 1, j];
                    }
                }
                for (var i = 0; i <= N - 1; i++)
                {
                    resultY[t, i] = 0;
                    for (var j = 0; j <= N - 1; j++)
                    {
                        y[t, i, j] = (resultA[t, i] * A[i, j] * B[j, observation[t + 1]] * resultB[t + 1, j]) / denom;
                        resultY[t, i] = resultY[t, i] + y[t, i, j];
                    }
                }
            }

            // Potrzebne?
            //double denom2 = 0;
            //for (var i = 0; i <= N - 1; i++)
            //{
            //    denom2 = denom2 + resultA[T-1, i] * resultB[T - 1, i];
            //}

            //for (var i = 0; i <= N - 1; i++)
            //{
            //    resultY[T-1,i] = ( resultA[T-1, i] * resultB[T-1, i] )/denom2;
            //}

            // To co wyżej to:
            for (var i = 0; i <= N - 1; i++)
            {
                resultY[T - 1, i] = resultA[T - 1, i];
            }

            var resultStateByState = new string[T];
            for (var t = 0; t <= T - 1; t++)
            {
                double tempMax = 0;
                for (var i = 0; i <= N - 1; i++)
                {
                    if (resultY[t, i] > tempMax)
                    {
                        tempMax = resultY[t, i];
                        resultStateByState[t] = Q[i];
                    }

                }
            }

            return resultStateByState;
        }
    }
}
