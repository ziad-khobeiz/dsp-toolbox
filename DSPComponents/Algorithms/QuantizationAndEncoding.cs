using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {

            if (InputLevel <= 0)
                InputLevel = (1 << InputNumBits);

            else
                InputNumBits = getNumberOfBits(InputLevel - 1);

            float minSample = InputSignal.Samples.Min();
            float maxSample = InputSignal.Samples.Max();
            float delta = (maxSample - minSample) / InputLevel;
            int numberOfSamples = InputSignal.Samples.Count;

            OutputQuantizedSignal = new Signal(new List<float>(numberOfSamples), false);
            OutputIntervalIndices = new List<int>(numberOfSamples);
            OutputEncodedSignal = new List<string>(numberOfSamples);
            OutputSamplesError = new List<float>(numberOfSamples);


            foreach(float sample in InputSignal.Samples)
            {
                int index = (int)((sample - minSample) / delta);
                if (index == InputLevel) index = InputLevel - 1;

                OutputIntervalIndices.Add(index + 1);

                var encodedSample = toBinary(index, InputNumBits);
                OutputEncodedSignal.Add(encodedSample);

                OutputQuantizedSignal.Samples.Add(minSample + (index * delta) + (delta / 2));

                float error = OutputQuantizedSignal.Samples.Last() - sample;
                OutputSamplesError.Add(error);
            }
        }
        private String toBinary(int x, int numberOfBits)
        {
            String binaryNumber = "";

            for(int i = 0; i < numberOfBits; i++)
            {
                binaryNumber += (x % 2 == 1 ? '1' : '0');
                x /= 2;
            }
            char[] reversedBinaryNumberArray = binaryNumber.ToCharArray();
            Array.Reverse(reversedBinaryNumberArray);
            return new string(reversedBinaryNumberArray);
        }

        private int getNumberOfBits(int x)
        {
            int numberOfBits = 0;
            while(x > 0)
            {
                numberOfBits++;
                x /= 2;
            }
            return numberOfBits;
        }
    }
}
