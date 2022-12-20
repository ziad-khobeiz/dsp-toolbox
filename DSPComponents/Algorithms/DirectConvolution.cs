using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            int XLength = InputSignal1.Samples.Count;
            int HLength = InputSignal2.Samples.Count;

            int MinIndicies = InputSignal1.SamplesIndices.Min() + InputSignal2.SamplesIndices.Min();

            List<float> Y = new List<float>();
            List<int> Indices = new List<int>();

            for (int i = 0; i < XLength + HLength - 1; i++, MinIndicies++)
            {
                float Sum = 0;
                for(int j = 0; j < HLength; j++)
                {
                    if (i - j >= 0 && i - j < XLength)
                    {
                        Sum += InputSignal1.Samples[i - j] * InputSignal2.Samples[j];
                    }
                }
                if (Sum == 0 && i == XLength + HLength - 2) continue;

                Indices.Add(MinIndicies);
                Y.Add(Sum);
            }

            OutputConvolvedSignal = new Signal(Y, Indices, false);
        }

    }
}
