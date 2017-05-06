using System;

namespace HMM
{
    public class Viterbi
    {
        // Algorithm from https://en.wikipedia.org/wiki/Viterbi_algorithm
        public static string[] GetResult(int T, int N, int M, string[] Q, int[] V, double[,] A, double[,] B,
            double[] pi, int[] observation)
        {
            double[,] T1 = new double[N, T];
            double[,] T2 = new double[N, T];

            for (var i = 0; i <= N - 1; i++)
            {
                T1[i, 0] = pi[i] * B[i, observation[0]];
                T2[i, 0] = 0;
            }

            for (var i = 1; i <= T - 1; i++)
            {
                for (var j = 0; j <= N - 1; j++)
                {
                    var maxValueWithIndex = GetMaxValueWithIndex(T1, A, N, i - 1, j);
                    T1[j, i] = B[j, observation[i]] * maxValueWithIndex.Item1;
                    T2[j, i] = maxValueWithIndex.Item2;
                }
            }

            var Z = new int[T];
            var X = new string[T];

            Z[T - 1] = GetIndexOfMaxValue(T1, T - 1, N);
            X[T - 1] = Q[Z[T - 1]];

            for (var i = T - 1; i >= 1; i--)
            {
                Z[i - 1] = (int)T2[Z[i], i];
                X[i - 1] = Q[Z[i - 1]];
            }

            return X;
        }

        private static Tuple<double, int> GetMaxValueWithIndex(double[,] T, double[,] A, int N, int tRow, int aRow)
        {
            var max = -1.0;
            var index = -1;

            for (var k = 0; k <= N - 1; k++)
            {
                var currentValue = T[k, tRow] * A[k, aRow];
                if (currentValue >= max)
                {
                    max = currentValue;
                    index = k;
                }
            }

            return new Tuple<double, int>(max, index);
        }

        private static int GetIndexOfMaxValue(double[,] T, int tRow, int N)
        {
            var max = -1.0;
            var index = -1;

            for (var k = 0; k <= N - 1; k++)
            {
                var currentValue = T[k, tRow];
                if (currentValue > max)
                {
                    max = currentValue;
                    index = k;
                }
            }

            return index;
        }
    }
}
