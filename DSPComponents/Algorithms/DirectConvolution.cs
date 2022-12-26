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

            int MinH = InputSignal2.SamplesIndices.Min();
            int MinX = InputSignal1.SamplesIndices.Min();

            int MaxH = InputSignal2.SamplesIndices.Max();
            int MaxX = InputSignal1.SamplesIndices.Max();

            int MinIndicies = MinH + MinX;

            List<float> Y = new List<float>();
            List<int> Indices = new List<int>();

            for (int i = 0; i < XLength + HLength - 1; i++, MinIndicies++)
            {
                float Sum = 0;
                for(int j = MinX; j <= MaxX; j++)
                {
                    if (MinIndicies - j >= MinH && MinIndicies - j <= MaxH)
                    {
                        Sum += InputSignal1.Samples[j - MinX] * InputSignal2.Samples[(MinIndicies - j) - MinH];
                    }
                }
                Indices.Add(MinIndicies);
                Y.Add(Sum);
            }

            while (Y.Count > 0 && Math.Abs(Y.Last()) == 0)
            {
                Y.RemoveAt(Y.Count - 1);
                Indices.RemoveAt(Indices.Count - 1);
            }

            OutputConvolvedSignal = new Signal(Y, Indices, false);
        }

    }
}
