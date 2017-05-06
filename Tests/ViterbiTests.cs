using HMM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ViterbiTests
    {
        // Expected result from sanjose-hmm.pdf
        [TestMethod]
        public void HotColdTest()
        {
            double[,] A = {
                { 0.7, 0.3 },
                { 0.4, 0.6 }
            };

            double[,] B = {
                { 0.1, 0.4, 0.5 },
                { 0.7, 0.2, 0.1 }
            };

            double[] pi = {
                0.6, 0.4
            };

            int[] observation = { 0, 1, 0, 2 };

            string[] Q = { "H", "C" };
            int[] V = { 0, 1, 2 }; // small, medium, large

            var T = observation.Length; // length of the observation sequence
            var N = Q.Length; // number of states in the model (H, C)
            var M = V.Length; // number of observation model (Small, Medium, Large)

            var expectedResult = new string[] { "C", "C", "C", "H" };
            var resultFromViterbi = Viterbi.GetResult(T, N, M, Q, V, A, B, pi, observation);

            CollectionAssert.AreEqual(expectedResult, resultFromViterbi);
        }

        // Expected result from https://en.wikipedia.org/wiki/Viterbi_algorithm
        [TestMethod]
        public void HealthyFeverTest()
        {
            double[,] A = {
                { 0.7, 0.3 },
                { 0.4, 0.6 }
            };

            double[,] B = {
                { 0.5, 0.4, 0.1 },
                { 0.1, 0.3, 0.6 }
            };

            double[] pi = {
                0.6, 0.4
            };

            int[] observation = { 0, 1, 2 };

            string[] Q = { "Healthy", "Fever" };
            int[] V = { 0, 1, 2 };

            var T = observation.Length; // length of the observation sequence
            var N = Q.Length; // number of states in the model (H, C)
            var M = V.Length; // number of observation model (Small, Medium, Large)
            
            var expectedResult = new string[] { "Healthy", "Healthy", "Fever"};
            var resultFromViterbi = Viterbi.GetResult(T, N, M, Q, V, A, B, pi, observation);

            CollectionAssert.AreEqual(expectedResult, resultFromViterbi);
        }
    }
}
