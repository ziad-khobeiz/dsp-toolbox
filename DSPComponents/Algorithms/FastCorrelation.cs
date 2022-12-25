using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            List<float> NewInputSignal2 = new List<float>();
            int length = InputSignal1.Samples.Count;
            for (int i = 0; i < length; i++)
            {
                if (InputSignal2 == null) NewInputSignal2.Add(InputSignal1.Samples[i]);
                else NewInputSignal2.Add(InputSignal2.Samples[i]);
            }
            for (int i = 0; i < length; i++)
            {
                NewInputSignal2.Add(0);
            }
            float sum1 = 0, sum2 = 0;
            for (int i = 0; i < length; i++)
            {
                sum1 += InputSignal1.Samples[i] * InputSignal1.Samples[i];
                sum2 += NewInputSignal2[i] * NewInputSignal2[i];
            }
            float sumMul = sum1 * sum2;
            sumMul = (float)Math.Sqrt(sumMul);
            sumMul /= length;

            List<float> NonNormalized = new List<float>();
            List<float> Normalized = new List<float>();
            for (int j = 0; j < length; j++)
            {
                float sum = 0;
                for (int n = 0; n < length; n++)
                {
                    sum += InputSignal1.Samples[n] * NewInputSignal2[n + j];
                }
                sum /= length;
                NonNormalized.Add(sum);
                Normalized.Add(sum / sumMul);
                NewInputSignal2[length + j] = NewInputSignal2[j];
            }
            OutputNonNormalizedCorrelation = new List<float>(NonNormalized);
            OutputNormalizedCorrelation = new List<float>(Normalized);
        }
    }
}