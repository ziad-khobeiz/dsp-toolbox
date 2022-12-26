using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        public enum WINDOW_FUNCTION
        {
            RECTANGULAR, HANNING, HAMMING, BLACKMAN
        }

        public float WindowMethod(int n, int N, string windowName)
        {
            if (windowName == "Rectangular")
            {
                return 1;
            }
            else if (windowName == "Hanning")
            {
                return 0.5f + 0.5f * (float)Math.Cos((2f * Math.PI * n) / N);
            }
            else if (windowName == "Hamming")
            {
                return 0.54f + 0.46f * (float)Math.Cos((2f * Math.PI * n) / N);
            }
            else if (windowName == "Blackman")
            {
                return 0.42f + 0.5f * (float)Math.Cos((2f * Math.PI * n) / (N - 1)) + 0.08f * (float)Math.Cos((4f * Math.PI * n) / (N - 1));
            }
            return 0;
        }

        public override void Run()
        {
            String windowName = "";
            float transitionWidth = 0;

            if (InputStopBandAttenuation <= 21)
            {
                windowName = "Rectangular";
                transitionWidth = 0.9f;
            }
            else if(InputStopBandAttenuation <= 44)
            {
                windowName = "Hanning";
                transitionWidth = 3.1f;
            }
            else if (InputStopBandAttenuation <= 53)
            {
                windowName = "Hamming";
                transitionWidth = 3.3f;
            }
            else if (InputStopBandAttenuation <= 74)
            {
                windowName = "Blackman";
                transitionWidth = 5.5f;
            }

            // deltaF = transition width / sampling frequency
            // detlaF = transition width (Normalized) / N
            // N = transition width (Normalized) / deltaF
            // N = transition width (Normalized) / transition width / sampling frequency
            float deltaF = InputTransitionBand / InputFS;
            int N = (int)Math.Ceiling(transitionWidth / deltaF);
            if (N % 2 == 0) N++;

            int start = -N / 2;
            OutputHn = new Signal(new List<float>(), new List<int>(), false);
            for (int i = 0; i < N; i++, start++)
            {
                OutputHn.SamplesIndices.Add(start);
            }

            float cutOffFrequencyNormalized = 0, cutOffFrequencyNormalized1 = 0, cutOffFrequencyNormalized2 = 0;
            if(InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                cutOffFrequencyNormalized1 = (float)(InputF1 - (InputTransitionBand / 2f)) / InputFS;
                cutOffFrequencyNormalized2 = (float)(InputF2 + (InputTransitionBand / 2f)) / InputFS;
            }

            else if(InputFilterType == FILTER_TYPES.BAND_STOP)
            {
                cutOffFrequencyNormalized1 = (float)(InputF1 + (InputTransitionBand / 2f)) / InputFS;
                cutOffFrequencyNormalized2 = (float)(InputF2 - (InputTransitionBand / 2f)) / InputFS;
            }

            else if(InputFilterType == FILTER_TYPES.LOW)
            {
                cutOffFrequencyNormalized = (float)(InputCutOffFrequency + (InputTransitionBand / 2f)) / InputFS;
            }

            else
            {
                cutOffFrequencyNormalized = (float)(InputCutOffFrequency - (InputTransitionBand / 2f)) / InputFS;
            }

            for(int i = 0; i < N; i++)
            {
                int n = Math.Abs(OutputHn.SamplesIndices[i]);
                float wn = WindowMethod(n, N, windowName), hn = 0;
                if(InputFilterType == FILTER_TYPES.LOW)
                {
                    
                    if(n == 0)
                    {
                        hn = 2 * cutOffFrequencyNormalized;
                    }
                    else
                    {
                        hn = (2 * cutOffFrequencyNormalized) * ((float)Math.Sin(2f * Math.PI * n * cutOffFrequencyNormalized)) / (2f * (float)Math.PI * n * cutOffFrequencyNormalized);
                    }
                }
                else if(InputFilterType == FILTER_TYPES.HIGH)
                {
                    if (n == 0)
                    {
                        hn = 1 - 2 * cutOffFrequencyNormalized;
                    }
                    else
                    {
                        hn = -2 * cutOffFrequencyNormalized * ((float)Math.Sin(2 * Math.PI * n * cutOffFrequencyNormalized)) / (2 * (float)Math.PI * n * cutOffFrequencyNormalized);
                    }
                }
                else if(InputFilterType == FILTER_TYPES.BAND_PASS)
                {
                    if (n == 0)
                    {
                        hn = 2 * (cutOffFrequencyNormalized2 - cutOffFrequencyNormalized1);
                    }
                    else
                    {
                        hn = 2 * cutOffFrequencyNormalized2 * ((float)Math.Sin(2 * Math.PI * n * cutOffFrequencyNormalized2)) / (2 * (float)Math.PI * n * cutOffFrequencyNormalized2);
                        hn -= (2 * cutOffFrequencyNormalized1 * ((float)Math.Sin(2 * Math.PI * n * cutOffFrequencyNormalized1)) / (2 * (float)Math.PI * n * cutOffFrequencyNormalized1));
                    }
                }
                else if(InputFilterType == FILTER_TYPES.BAND_STOP)
                {
                    if(n == 0)
                    {
                        hn = 1 - (2 * (cutOffFrequencyNormalized2 - cutOffFrequencyNormalized1));
                    }
                    else
                    {
                        float nw1 = 2f * (float)Math.PI * n * cutOffFrequencyNormalized1;
                        float nw2 = 2f * (float)Math.PI * n * cutOffFrequencyNormalized2;
                        hn = 2 * cutOffFrequencyNormalized1 * (float)Math.Sin(nw1) / nw1;
                        hn = hn - 2 * cutOffFrequencyNormalized2 * (float)Math.Sin(nw2) / nw2;
                    }
                }
                OutputHn.Samples.Add(wn * hn);
            }

            DirectConvolution directConvolution = new DirectConvolution();
            directConvolution.InputSignal1 = InputTimeDomainSignal;
            directConvolution.InputSignal2 = OutputHn;
            directConvolution.Run();
            OutputYn = directConvolution.OutputConvolvedSignal;
        }
    }
}
