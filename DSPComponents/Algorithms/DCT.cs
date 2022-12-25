using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            OutputSignal = new Signal(new List<float>(), false);
            int length = InputSignal.Samples.Count;
            for (int k = 0; k < length; k++)
            {
                float sum = 0;
                for (int n = 0; n < length; n++)
                {
                    sum += InputSignal.Samples[n] * (float)Math.Cos(Math.PI / (4.0f * InputSignal.Samples.Count) * (2 * n - 1) * (2 * k - 1));
                }
                sum = (float)Math.Sqrt(2.0f / InputSignal.Samples.Count) * sum;
                OutputSignal.Samples.Add(sum);
            }
        }
    }
}
