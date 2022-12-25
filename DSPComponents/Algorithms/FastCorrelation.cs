using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
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
            int length = InputSignal1.Samples.Count;
            if (InputSignal2 == null) InputSignal2 = new Signal(InputSignal1.Samples, false); 
           

            DiscreteFourierTransform dftSignal1 = new DiscreteFourierTransform();
            dftSignal1.InputTimeDomainSignal = InputSignal1;
            dftSignal1.Run();

            DiscreteFourierTransform dftSignal2 = new DiscreteFourierTransform();
            dftSignal2.InputTimeDomainSignal = InputSignal2;
            dftSignal2.Run();

            Signal output = new Signal(false, new List<float>(), new List<float>(), new List<float>());
            for (int i = 0; i < length; i++)
            {
                Complex Signal1 = Complex.FromPolarCoordinates(dftSignal1.OutputFreqDomainSignal.FrequenciesAmplitudes[i], -1 * dftSignal1.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]);

                Complex Signal2 = Complex.FromPolarCoordinates(dftSignal2.OutputFreqDomainSignal.FrequenciesAmplitudes[i], dftSignal2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]);

                Complex Signal1Signal2Mul = Complex.Multiply(Signal1, Signal2);
                output.Frequencies.Add(0);
                output.FrequenciesPhaseShifts.Add((float)Signal1Signal2Mul.Phase);
                output.FrequenciesAmplitudes.Add((float)Signal1Signal2Mul.Magnitude);
            }

            InverseDiscreteFourierTransform Idft = new InverseDiscreteFourierTransform();
            Idft.InputFreqDomainSignal = output;
            Idft.Run();

            float sum1 = 0, sum2 = 0;
            for (int i = 0; i < length; i++)
            {
                sum1 += InputSignal1.Samples[i] * InputSignal1.Samples[i];
                sum2 += InputSignal2.Samples[i] * InputSignal2.Samples[i];
            }
            float sumMul = sum1 * sum2;
            sumMul = (float)Math.Sqrt(sumMul);
            sumMul /= length;

            List<float> NonNormalized = new List<float>();
            List<float> Normalized = new List<float>();
            for (int i = 0; i < length; i++)
            {
                float value = Idft.OutputTimeDomainSignal.Samples[i] / length;
                NonNormalized.Add(value);
                Normalized.Add(value / sumMul);
            }
            OutputNonNormalizedCorrelation = new List<float>(NonNormalized);
            OutputNormalizedCorrelation = new List<float>(Normalized);
        }
    }
}