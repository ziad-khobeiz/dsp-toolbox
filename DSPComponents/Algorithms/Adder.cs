using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            int maximum_samples_count = 0;
            foreach (Signal signal in InputSignals)
            {
                maximum_samples_count = Math.Max(maximum_samples_count, signal.Samples.Count);
            }
            List<float> samples = new List<float>(maximum_samples_count);
            for (int i = 0; i < maximum_samples_count; ++i)
            {
                samples.Add(0);
            }
            foreach (Signal signal in InputSignals)
            {
                for (int i = 0; i < signal.Samples.Count; ++i)
                {
                    samples[i] += signal.Samples[i];
                }
            }
            OutputSignal = new Signal(samples, false);
        }
    }
}