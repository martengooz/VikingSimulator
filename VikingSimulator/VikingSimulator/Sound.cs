using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace VikingSimulator
{

    public class SoundEffectHelpers
    {
        Random randomSoundNumber = new Random();
        int soundNumber = 0;

        public SoundEffectHelpers() {}

        public void playSound(List<SoundEffect> soundEffects) {

            soundNumber = randomSoundNumber.Next(0, (soundEffects.Count() * 3));
            if (soundNumber < soundEffects.Count())
            {
                soundEffects[soundNumber].Play();
            }
            return;
        }
    }
}
