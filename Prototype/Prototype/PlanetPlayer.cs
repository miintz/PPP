using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Prototype
{
    class PlanetPlayer
    {
        private WaveOut waveOut;

        PlanetData PlanetData { get; set; }

        Boolean lowhigh = true;

        public PlanetPlayer(PlanetData planet)
        {
            PlanetData = planet;
            PlanetData.Index = 0;
        }

        public void PlayNote(Object state)
        {
            Console.WriteLine("MARS @ " + PlanetData.getDate().ToString());

            SoundPlayer simpleSound;
            if (lowhigh)
            {
                simpleSound = new SoundPlayer(@"high.wav");
                lowhigh = false;
            }
            else
            {
                simpleSound = new SoundPlayer(@"low.wav");
                lowhigh = true;
            }

            AngleSineWave(PlanetData.getRizeAzAngle());

            //metronome
            //simpleSound.Play();

            if (PlanetData.Index + 1 != PlanetData.Length)
                PlanetData.Index++;
            else
                PlanetData.Index = 0;
        }

        private void AngleSineWave(UInt32 Angle)
        {
            if (waveOut == null)
            {
                var sineWaveProvider = new SineWaveProvider32();
                sineWaveProvider.SetWaveFormat(16000, 1); // 16kHz mono
                sineWaveProvider.Frequency = 500 + (Angle * 10);

                sineWaveProvider.Amplitude = 0.25f;
                waveOut = new WaveOut();
                waveOut.Init(sineWaveProvider);
                waveOut.Play();
            }
            else
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
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
}
