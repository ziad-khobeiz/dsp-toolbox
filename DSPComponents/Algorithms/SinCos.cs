using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class SinCos: Algorithm
    {
        public string type { get; set; }
        public float A { get; set; }
        public float PhaseShift { get; set; }
        public float AnalogFrequency { get; set; }
        public float SamplingFrequency { get; set; }
        public List<float> samples { get; set; }
        public override void Run()
        {
            // A * Cos(2pi * F * T + theta)
            if (AnalogFrequency * 2 > SamplingFrequency)
                return;

            float normalizedFrequency = AnalogFrequency / SamplingFrequency;
            int numberOfSamples = (int)SamplingFrequency;
            samples = new List<float>(numberOfSamples);
            for (int i = 0; i < numberOfSamples; i++)
            {
                double x = 2 * Math.PI * normalizedFrequency * i + PhaseShift;
                if (type == "sin")
                {
                    samples.Add((float)(A * Math.Sin(x)));
                }
                else
                {
                    samples.Add((float)(A * Math.Cos(x)));
                }
            }
        }
    }
}
