using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using NAudio;
using NAudio.Wave;

namespace Prototype
{
    class Program
    {
        static void Main(string[] args)
        {            
            Console.WriteLine("Welcome to the Planetary Perceptualization Program ALPHA 0.01 by MANOS & MAARTEN");
            Console.WriteLine("");
            Console.WriteLine("Enter planet/satellite name (e.g. Jupiter):");
            
            String name = Console.ReadLine();

            String file = name + ".txt";

            Console.WriteLine("");
            Console.WriteLine("You have selected: " + name);            

            String[] lines = File.ReadAllLines(file);
            PlanetData Planet = new PlanetData(name, lines.Length);

            for (int o = 0; o < lines.Length; o++ )
            {
                String line = lines[o];
                String[] words = line.Split(new char[] { ',' });                             
                                                 
                Planet.setDate(Convert.ToDateTime(words[0]));

                Planet.setRiseAzDate(Convert.ToDateTime(words[1]));
                Planet.setRizeAzAngle(Convert.ToUInt32(words[2]));

                Planet.setTransitAltDate(Convert.ToDateTime(words[3]));
                Planet.setTransitAltAngle(Convert.ToUInt32(words[4]));

                Planet.setSetAzDate(Convert.ToDateTime(words[5]));
                Planet.setSetAzAngle(Convert.ToUInt32(words[6]));

                Planet.Index = o;
            }            

            PlanetPlayer player = new PlanetPlayer(Planet);
            TimerCallback tcb = player.PlayNote;

            Console.WriteLine("Loaded data, press any key to continue...");
            Console.ReadKey();

            Timer t = new Timer(tcb, 0, 75, 75);

            Console.ReadKey();

            t.Dispose(); //dispose to smooth out escape
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