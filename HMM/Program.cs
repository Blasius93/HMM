using System;
using System.IO;

namespace HMM
{
    class Program
    {
        static readonly string StartupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.FullName;

        static void Main(string[] args)
        {
            double[,] A = {
                { 0.7, 0.3 },
                { 0.42, 0.58 }
            };

            double[,] B = {
                { 0.7, 0.25, 0.05 },
                { 0.3, 0.6, 0.1 }
            };

            //double[,] B = {
            //    { 0.8, 0.15, 0.05 },
            //    { 0.25, 0.65, 0.1 }
            //};

            double[] pi = {
                0.5, 0.5
            };

            string[] Q = { "Bessa", "Hossa" };
            int[] V = { 0, 1, 2 }; // Down, Up, Unchanged
            int[] observation;

            // Quarterly SP500
            observation = ReadFromFile("Observation_quarterly");
            SaveResult(Q, V, A, B, pi, observation, "AlgResults_quarterly");

            // Weekly SP500
            //observation = ReadFromFile("Observation_weekly");
            //SaveResult(Q, V, A, B, pi, observation, "AlgResults_weekly");

            // Other
            //observation = new int[] {};
            //SaveResult(Q, V, A, B, pi, observation, "AlgResults_other");
        }

        public static void SaveResult(string[] Q, int[] V, double[,] A, double[,] B, double[] pi,
            int[] observation, string fileName)
        {
            var N = Q.Length; // number of states in the model (Bessa, Hossa)
            var M = V.Length; // number of observation model (Down, Up, Unchanged)
            var T = observation.Length; // length of the observation sequence

            // Get results ForwardBackward
            string[] resultStateByState = ForwardBackward.GetResult(T, N, M, Q, V, A, B, pi, observation);
            string[] resultViterbi = Viterbi.GetResult(T, N, M, Q, V, A, B, pi, observation);

            SaveResults(resultStateByState, resultViterbi, fileName);
        }

        public static int[] ReadFromFile(string fileName)
        {
            var filePAth = $@"{StartupPath}\Observations\{fileName}.txt";
            string[] stringObservations = File.ReadAllLines(filePAth);

            int[] observations = Array.ConvertAll(stringObservations, int.Parse);

            return observations;
        }

        public static void SaveResults(string[] forwardBackwardAlgResults, string[] viterbiAlgResults, string fileName)
        {
            var separator = ";";

            using (StreamWriter outputFile = new StreamWriter($@"{StartupPath}\Results\{fileName}.xls"))
            {
                var headerLine = $"Forward Backward Algorithm Results{separator}Viterbi Algorithm Results";
                outputFile.WriteLine(headerLine);

                for (var i = 0; i < forwardBackwardAlgResults.Length; i++)
                {
                    var line = forwardBackwardAlgResults[i] + separator + viterbiAlgResults[i];
                    outputFile.WriteLine(line);
                }
            }
        }
    }
}
