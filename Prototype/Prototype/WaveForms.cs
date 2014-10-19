using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype
{
    class WaveForms
    {
        private WaveOut sineWaveOut;
        private WaveOut sawWaveOut;
        private WaveOut squareWaveOut;

        public void AngleSineWave(UInt32 Angle)
        {
            if (sineWaveOut == null)
            {
                var sineWaveProvider = new SineWaveProvider32();
                sineWaveProvider.SetWaveFormat(16000, 1); // 16kHz mono
                sineWaveProvider.Frequency = 500 + (Angle * 10);

                sineWaveProvider.Amplitude = 0.50f;
                sineWaveOut = new WaveOut();
                sineWaveOut.Init(sineWaveProvider);
                sineWaveOut.Play();
            }
            else
            {
                sineWaveOut.Stop();
                sineWaveOut.Dispose();
                sineWaveOut = null;
            }
        }

        public void AngleSawWave(UInt32 Angle)
        {
            if (sawWaveOut == null)
            {
                var sawWaveProvider = new SawWaveProvider32();
                sawWaveProvider.SetWaveFormat(10000, 1); // 16kHz mono
                sawWaveProvider.Frequency = 1500 + (Angle * 10);

                sawWaveProvider.Amplitude = 0.10f;
                sawWaveOut = new WaveOut();
                sawWaveOut.Init(sawWaveProvider);
                sawWaveOut.Play();
            }
            else
            {
                sawWaveOut.Stop();
                sawWaveOut.Dispose();
                sawWaveOut = null;
            }
        }

        public void AngleSquareWave(UInt32 Angle)
        {
            if (squareWaveOut == null)
            {
                var squareWaveProvider = new SquareWaveProvider32();
                squareWaveProvider.SetWaveFormat(10000, 1); // 16kHz mono
                squareWaveProvider.Frequency = 1500 + (Angle * 10);

                squareWaveProvider.Amplitude = 0.10f;
                squareWaveOut = new WaveOut();
                squareWaveOut.Init(squareWaveProvider);
                squareWaveOut.Play();
            }
            else
            {
                squareWaveOut.Stop();
                squareWaveOut.Dispose();
                squareWaveOut = null;
            }
        }
    }

    public class SawWaveProvider32 : WaveProvider32
    {
        int sample;

        public SawWaveProvider32()
        {
            Frequency = 1000;
            Amplitude = 0.25f; // let's not hurt our ears too much  
        }

        public float Frequency { get; set; }
        public float Amplitude { get; set; }

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            int sampleRate = WaveFormat.SampleRate;
            for (int n = 0; n < sampleCount; n++)
            {
                buffer[n + offset] = (float)(2 * ((sample * Frequency) / sampleRate - Math.Floor((sample * Frequency) / sampleRate + 0.5)));
                sample++;
                if (sample >= sampleRate) sample = 0;
            }
            return sampleCount;
        }
    }

    public class SineWaveProvider32 : WaveProvider32
    {
        int sample;

        public SineWaveProvider32()
        {
            Frequency = 1000;
            Amplitude = 0.25f; // let's not hurt our ears too much  
        }

        public float Frequency { get; set; }
        public float Amplitude { get; set; }

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            int sampleRate = WaveFormat.SampleRate;
            for (int n = 0; n < sampleCount; n++)
            {
                buffer[n + offset] = (float)(Amplitude * Math.Sin((2 * Math.PI * sample * Frequency) / sampleRate));
                sample++;
                if (sample >= sampleRate) sample = 0;
            }
            return sampleCount;
        }
    }

    public class SquareWaveProvider32 : WaveProvider32
    {
        int sample;

        public SquareWaveProvider32()
        {
            Frequency = 1000;
            Amplitude = 0.25f; // let's not hurt our ears too much  
        }

        public float Frequency { get; set; }
        public float Amplitude { get; set; }

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            int sampleRate = WaveFormat.SampleRate;
            int Channels = WaveFormat.Channels;
            double phaseAngle;
            phaseAngle = ((Math.PI * 2 * Frequency) / (sampleRate * Channels));

            for (int n = 0; n < sampleCount - 1; n++)
            {
                if (Math.Sign(Math.Sin(phaseAngle * n)) > 0)
                    buffer[n + offset] = Amplitude;
                else
                    buffer[n + offset] = -Amplitude;

                sample++;
                if (sample >= sampleRate) sample = 0;
            }
            return sampleCount;
        }
    }

    public abstract class WaveProvider32 : IWaveProvider
    {
        private WaveFormat waveFormat;

        public WaveProvider32()
            : this(44100, 1)
        {
        }

        public WaveProvider32(int sampleRate, int channels)
        {
            SetWaveFormat(sampleRate, channels);
        }

        public void SetWaveFormat(int sampleRate, int channels)
        {
            this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            WaveBuffer waveBuffer = new WaveBuffer(buffer);
            int samplesRequired = count / 4;
            int samplesRead = Read(waveBuffer.FloatBuffer, offset / 4, samplesRequired);
            return samplesRead * 4;
        }

        public abstract int Read(float[] buffer, int offset, int sampleCount);

        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }
    }   
}