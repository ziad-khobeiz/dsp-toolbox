using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            List<int> ShiftedIndices = new List<int>();
            List<float> Samples = new List<float>();
            for (int i = InputSignal.Samples.Count - 1; i >= 0; i--)
            {
                ShiftedIndices.Add(-InputSignal.SamplesIndices[i]);
                Samples.Add(InputSignal.Samples[i]);
            }
            InputSignal.Periodic = !InputSignal.Periodic;
            OutputFoldedSignal = new Signal(Samples, ShiftedIndices, InputSignal.Periodic);

        }
    }
}
