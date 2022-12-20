using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {
            List<int> ShiftedIndices = new List<int>();
            List<float> Samples = new List<float>();

            if (InputSignal.Periodic == true)
                ShiftingValue *= -1;
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                ShiftedIndices.Add(InputSignal.SamplesIndices[i] - ShiftingValue);
                Samples.Add(InputSignal.Samples[i]);
            }

            OutputShiftedSignal = new Signal(Samples, ShiftedIndices, InputSignal.Periodic);
        }
    }
}
