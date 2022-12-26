﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }



        public Signal UpSampling(Signal signal)
        {
            List<float> newSignalSamples = new List<float>();
            for(int i = 0; i < signal.Samples.Count; i++)
            {
                newSignalSamples.Add(signal.Samples[i]);
                for(int j = 0; j < L - 1; j++)
                {
                    newSignalSamples.Add(0);
                }
            }
            return new Signal(newSignalSamples, signal.Periodic);
        }

        public Signal DownSampling(Signal signal)
        {
            List<float> newSignalSamples = new List<float>();
            for (int i = 0; i < signal.Samples.Count; i += M)
            {
                newSignalSamples.Add(signal.Samples[i]);
            }
            return new Signal(newSignalSamples, signal.Periodic);
        }

        public override void Run()
        {
            FIR lowPassFilter = new FIR();
            lowPassFilter.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
            lowPassFilter.InputFS = 8000;
            lowPassFilter.InputStopBandAttenuation = 50;
            lowPassFilter.InputCutOffFrequency = 1500;
            lowPassFilter.InputTransitionBand = 500;

            if(M != 0)
            {
                Signal newSignal = InputSignal;
                if (L != 0)
                {
                    newSignal = UpSampling(InputSignal);
                }
                lowPassFilter.InputTimeDomainSignal = newSignal;
                lowPassFilter.Run();
                OutputSignal = DownSampling(lowPassFilter.OutputYn);
            }
            else
            {
                if(L != 0)
                {
                    Signal newSignal = UpSampling(InputSignal);
                    lowPassFilter.InputTimeDomainSignal = newSignal;
                    lowPassFilter.Run();
                    OutputSignal = lowPassFilter.OutputYn;
                }
                else
                {
                    throw new Exception("L and M cannot be both 0");
                }
            }
        }
    }

}