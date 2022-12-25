using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            int length = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;

            while(InputSignal1.Samples.Count < length) InputSignal1.Samples.Add(0);
            while (InputSignal2.Samples.Count < length) InputSignal2.Samples.Add(0);
            
            DiscreteFourierTransform dftSignal1 = new DiscreteFourierTransform();
            dftSignal1.InputTimeDomainSignal = InputSignal1;
            dftSignal1.Run();

            DiscreteFourierTransform dftSignal2 = new DiscreteFourierTransform();
            dftSignal2.InputTimeDomainSignal = InputSignal2;
            dftSignal2.Run();

            Signal output = new Signal(false, new List<float>(), new List<float>(), new List<float>());
            for (int i = 0; i < length; i++)
            {
                Complex Signal1 = Complex.FromPolarCoordinates(dftSignal1.OutputFreqDomainSignal.FrequenciesAmplitudes[i], dftSignal1.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]);

                Complex Signal2 = Complex.FromPolarCoordinates(dftSignal2.OutputFreqDomainSignal.FrequenciesAmplitudes[i], dftSignal2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]);

                Complex Signal1Signal2Mul = Complex.Multiply(Signal1, Signal2);

                output.FrequenciesPhaseShifts.Add((float)Signal1Signal2Mul.Phase);
                output.FrequenciesAmplitudes.Add((float)Signal1Signal2Mul.Magnitude);
            }

            InverseDiscreteFourierTransform Idft = new InverseDiscreteFourierTransform();
            Idft.InputFreqDomainSignal = output;
            Idft.Run();
            
            while(Idft.OutputTimeDomainSignal.Samples.Count > 0 && Idft.OutputTimeDomainSignal.Samples.Last() == 0)
            {
                Idft.OutputTimeDomainSignal.Samples.RemoveAt(Idft.OutputTimeDomainSignal.Samples.Count - 1);
            }
            OutputConvolvedSignal = Idft.OutputTimeDomainSignal;

        }
    }
}
