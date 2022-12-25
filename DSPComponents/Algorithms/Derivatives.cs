using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            // Shifter Right
            Shifter ShiftRight = new Shifter();
            ShiftRight.InputSignal = InputSignal;
            ShiftRight.ShiftingValue = (InputSignal.Periodic ? 1 : -1);
            ShiftRight.Run();

            // Shifter Left
            Shifter ShiftLeft = new Shifter();
            ShiftLeft.InputSignal = InputSignal;
            ShiftLeft.ShiftingValue = (InputSignal.Periodic ? -1 : 1);
            ShiftLeft.Run();

            List<float> FirstDerivativeValues = new List<float>();
            List<float> SecondDerivativeValues = new List<float>();

            for (int i = 0; i < InputSignal.Samples.Count - 1; i++)
            {
                float derivativeValue = InputSignal.Samples[i + 1] - ShiftRight.OutputShiftedSignal.Samples[i];
                FirstDerivativeValues.Add(derivativeValue);
            }
            FirstDerivative = new Signal(FirstDerivativeValues, false);

            for (int i = 1; i < InputSignal.Samples.Count; i++)
            {
                float derivativeValue = ShiftLeft.OutputShiftedSignal.Samples[i] - InputSignal.Samples[i - 1] - FirstDerivative.Samples[i - 1];
                SecondDerivativeValues.Add(derivativeValue);
            }
            SecondDerivative = new Signal(SecondDerivativeValues, false);
        }
    }
}
