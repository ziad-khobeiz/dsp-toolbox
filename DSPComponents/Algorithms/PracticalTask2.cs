﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            Signal InputSignal = LoadSignal(SignalPath);
            Signal LastOutput;
            FIR fir = new FIR();
            fir.InputTimeDomainSignal = InputSignal;
            fir.InputFS = Fs;
            fir.InputF1 = miniF;
            fir.InputF2 = maxF;
            fir.InputFilterType = FILTER_TYPES.BAND_PASS;
            fir.InputStopBandAttenuation = 50;
            fir.InputTransitionBand = 500;

            fir.Run();
            LastOutput = fir.OutputYn;
            SaveSignalTimeDomain(LastOutput, "/Filtering.ds");

            if(newFs >= 2 * maxF)
            {
                Sampling sampling = new Sampling();
                sampling.InputSignal = LastOutput;
                sampling.L = L;
                sampling.M = M;
                Fs = newFs;

                sampling.Run();
                LastOutput = sampling.OutputSignal;
                SaveSignalTimeDomain(LastOutput, "/Sampling.ds");
            }

            DC_Component dC_Component = new DC_Component();
            dC_Component.InputSignal = LastOutput;

            dC_Component.Run();
            LastOutput = dC_Component.OutputSignal;
            SaveSignalTimeDomain(LastOutput, "/DC.ds");

            Normalizer normalizer = new Normalizer();
            normalizer.InputSignal = LastOutput;
            normalizer.InputMinRange = -1;
            normalizer.InputMaxRange = 1;

            normalizer.Run();
            LastOutput = normalizer.OutputNormalizedSignal;
            SaveSignalTimeDomain(LastOutput, "/Normalizing.ds");

            DiscreteFourierTransform discreteFourierTransform = new DiscreteFourierTransform();
            discreteFourierTransform.InputTimeDomainSignal = LastOutput;

            discreteFourierTransform.Run();
            LastOutput = discreteFourierTransform.OutputFreqDomainSignal;
            OutputFreqDomainSignal = LastOutput;
            SaveSignalFreqDomain(LastOutput, "/DFT.ds");
        }

        private static void SaveSignalFreqDomain(Signal signal, string filepath)
        {
            using (StreamWriter sw = new StreamWriter(filepath))
            {
                sw.WriteLine(1);
                sw.WriteLine(0);
                sw.WriteLine(signal.FrequenciesAmplitudes.Count);
                for (int i = 0; i < signal.FrequenciesAmplitudes.Count; ++i)
                {
                    sw.Write(Math.Round(signal.Frequencies[i], 1));
                    sw.Write(' ');
                    sw.Write(signal.FrequenciesAmplitudes[i]);
                    sw.Write(' ');
                    sw.WriteLine(signal.FrequenciesPhaseShifts[i]);
                }
            }
        }

        private static void SaveSignalTimeDomain(Signal signal, string filepath)
        {
            using (StreamWriter sw = new StreamWriter(filepath))
            {
                sw.WriteLine(0);
                sw.WriteLine(0);
                sw.WriteLine(signal.Samples.Count);
                for (int i = 0; i < signal.Samples.Count; ++i)
                {
                    sw.Write(signal.SamplesIndices[i]);
                    sw.Write(' ');
                    sw.WriteLine(signal.Samples[i]);
                }
            }
        }

        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}
