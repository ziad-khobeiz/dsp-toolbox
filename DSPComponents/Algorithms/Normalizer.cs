using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            float signal_min_value = float.PositiveInfinity;
            float signal_max_value = float.NegativeInfinity;
            foreach (float sample in InputSignal.Samples)
            {
                signal_max_value = Math.Max(signal_max_value, sample);
                signal_min_value = Math.Min(signal_min_value, sample);
            }
            OutputNormalizedSignal = new Signal(new List<float>(), false);
            foreach (float sample in InputSignal.Samples)
            {
                float normalized_sample = ((sample - signal_min_value) / (signal_max_value - signal_min_value)) * (InputMaxRange - InputMinRange) + InputMinRange;
                OutputNormalizedSignal.Samples.Add(normalized_sample);
            }
        }
    }
}
