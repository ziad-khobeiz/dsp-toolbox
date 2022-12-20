using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
 
        public override void Run()
        {
            List<float> answer = new List<float>();
            float sum = 0;
            int totalIterations = InputSignal.Samples.Count - InputWindowSize + 1;

            for (int i = 0; i < totalIterations; i++)
            {
                sum = 0;
                for (int j = i; j < i + InputWindowSize; j++)
                {
                    sum += InputSignal.Samples[j];
                }
                answer.Add(sum / InputWindowSize);
            }

            OutputAverageSignal = new Signal(answer, false);
        }
    }
}
