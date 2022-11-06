using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            int N = InputTimeDomainSignal.Samples.Count;
            OutputFreqDomainSignal = new Signal(new List<float>(N), false);
            OutputFreqDomainSignal.FrequenciesAmplitudes = new List<float>(N);
            OutputFreqDomainSignal.FrequenciesPhaseShifts = new List<float>(N);
            for (int k = 0; k < N; ++k)
            {
                Complex sum = new Complex(); 
                for (int n = 0; n < N; ++n)
                {
                    sum += InputTimeDomainSignal.Samples[n] * Complex.Exp(new Complex(0, -k * 2 * Math.PI * n / N));
                }
                OutputFreqDomainSignal.FrequenciesAmplitudes.Add((float)sum.Magnitude);
                OutputFreqDomainSignal.FrequenciesPhaseShifts.Add((float)sum.Phase);
            }
        }
    }
}
