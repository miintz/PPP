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
        private WaveForms Waves;

        Boolean Sine = true;
        Boolean Square = false;
        Boolean Saw = false;

        UInt32 SineCounter = 0;
        UInt32 SawCounter = 0;
        UInt32 SquareCounter = 0;

        PlanetData PlanetData { get; set; }

        Boolean lowhigh = true;

        public PlanetPlayer(PlanetData planet)
        {
            PlanetData = planet;
            PlanetData.Index = 0;

            Waves = new WaveForms();
        }

        public void PlayNote(Object state)
        {
            Console.WriteLine(PlanetData.Name + " @ " + PlanetData.getDate().Day + "/" + PlanetData.getDate().Month + "/" + PlanetData.getDate().Year + 
                " - RiseAngle: " + PlanetData.getRiseAzAngle() + "°" + 
                " - TransitionAngle: " + PlanetData.getTransitAltAngle() + "°" +
                " - SetAngle: " + PlanetData.getSetAzAngle() + "°");

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

            if (this.Sine)
            {
                Waves.AngleSineWave(PlanetData.getRiseAzAngle());
                if (SineCounter == 25)
                {
                    Sine = false;
                    Saw = true;
                    SineCounter = 0;
                }
                else
                    SineCounter++;
            }
            else if (this.Saw)
            {
                Waves.AngleSawWave(PlanetData.getTransitAltAngle());
                if (SawCounter == 25)
                {
                    Saw = false;
                    Square = true;
                    SawCounter = 0;
                }
                else
                    SawCounter++;
            }
            else if (this.Square)
            {
                Waves.AngleSquareWave(PlanetData.getSetAzAngle());
                if (SquareCounter == 25)
                {
                    Square = false;
                    Sine = true;
                    SquareCounter = 0;
                }
                else
                    SquareCounter++;
            }

            //metronome
            //simpleSound.Play();

            if (PlanetData.Index + 1 != PlanetData.Length)
                PlanetData.Index++;
            else
                PlanetData.Index = 0;
        }        
    }     
}
