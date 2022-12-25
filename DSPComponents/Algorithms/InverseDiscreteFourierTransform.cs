using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            int N = InputFreqDomainSignal.FrequenciesAmplitudes.Count;
            OutputTimeDomainSignal = new Signal(new List<float>(N), false);
            for (int n = 0; n < N; ++n)
            {
                Complex sum = new Complex();
                for (int k = 0; k < N; ++k)
                {
                    Complex x_k = Complex.FromPolarCoordinates(InputFreqDomainSignal.FrequenciesAmplitudes[k], InputFreqDomainSignal.FrequenciesPhaseShifts[k]);
                    sum += x_k * Complex.Exp(new Complex(0, k * 2 * Math.PI * n / N));
                }
                OutputTimeDomainSignal.Samples.Add((float)((1.0 / N) * sum.Real));
            }
        }
    }
}
