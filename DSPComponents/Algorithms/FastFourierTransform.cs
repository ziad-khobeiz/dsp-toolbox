using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public int InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            int N = InputTimeDomainSignal.Samples.Count;
            OutputFreqDomainSignal = new Signal(new List<float>(N), false);
            OutputFreqDomainSignal.FrequenciesAmplitudes = new List<float>(N);
            OutputFreqDomainSignal.FrequenciesPhaseShifts = new List<float>(N);
            OutputFreqDomainSignal.Frequencies = new List<float>(N);
            float phi = (float)(2 * Math.PI * InputSamplingFrequency / N);
            for (int k = 0; k < N; ++k)
            {
                Complex sum = new Complex();
                for (int n = 0; n < N; ++n)
                {
                    sum += InputTimeDomainSignal.Samples[n] * Complex.Exp(new Complex(0, -k * 2 * Math.PI * n / N));
                }
                OutputFreqDomainSignal.Frequencies.Add(phi * k);
                OutputFreqDomainSignal.FrequenciesAmplitudes.Add((float)sum.Magnitude);
                OutputFreqDomainSignal.FrequenciesPhaseShifts.Add((float)sum.Phase);
            }
        }
    }
}
