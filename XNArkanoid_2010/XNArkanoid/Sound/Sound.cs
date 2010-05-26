/* --------------------------------------------------------------------------------------------------------
 * Author: Iñaki Ayucar (http://graphicdna.blogspot.com)
 * Date: 1/05/2010
 * 
 * This software is distributed "for free" for any non-commercial usage. The software is provided “as-is.” 
 * You bear the risk of using it. The author give no express warranties, guarantees or conditions.
 * If you use this software in another project, you agree to mention the author and source.
 ---------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Audio;


namespace XNArkanoid.Sound
{

    /// <summary>
    /// Abstracts away the sounds for a simple interface using the Sounds enum
    /// </summary>
    public static class Sound
    {
        private static Dictionary<eSounds, Microsoft.Xna.Framework.Audio.SoundEffect> mSoundEffects = new Dictionary<eSounds, SoundEffect>();


        /// <summary>
        /// Plays a sound
        /// </summary>
        /// <param name="sound">Which sound to play</param>
        /// <returns>XACT cue to be used if you want to stop this particular looped sound. Can NOT be ignored.  If the cue returned goes out of scope, the sound stops!!</returns>
        public static SoundEffectInstance Play(eSounds sound)
        {
            SoundEffectInstance returnValue = mSoundEffects[sound].CreateInstance();
            returnValue.Play();
            return returnValue;
        }

        /// <summary>
        /// Plays a sound
        /// </summary>
        /// <param name="sound">Which sound to play</param>
        /// <returns>Nothing!  This cue will play through to completion and then free itself.</returns>
        public static void PlayCue(eSounds sound)
        {
            mSoundEffects[sound].Play();
        }
      
        /// <summary>
        /// Starts up the sound code
        /// </summary>
        public static void Initialize()
        {
            mSoundEffects.Add(eSounds.CoinUp, State.Game.Content.Load<SoundEffect>(@"Content\Sounds\CoinUp"));

            mSoundEffects.Add(eSounds.Death, State.Game.Content.Load<SoundEffect>(@"Content\Sounds\Death"));
            mSoundEffects.Add(eSounds.GameOver, State.Game.Content.Load<SoundEffect>(@"Content\Sounds\GameOver"));
            mSoundEffects.Add(eSounds.Hit1, State.Game.Content.Load<SoundEffect>(@"Content\Sounds\Hit1"));
            mSoundEffects.Add(eSounds.Hit2, State.Game.Content.Load<SoundEffect>(@"Content\Sounds\Hit2"));
            mSoundEffects.Add(eSounds.Hit3, State.Game.Content.Load<SoundEffect>(@"Content\Sounds\Hit3"));
            mSoundEffects.Add(eSounds.Hit4, State.Game.Content.Load<SoundEffect>(@"Content\Sounds\Hit4"));
            mSoundEffects.Add(eSounds.Intro, State.Game.Content.Load<SoundEffect>(@"Content\Sounds\Intro"));
            mSoundEffects.Add(eSounds.menu_advance, State.Game.Content.Load<SoundEffect>(@"Content\Sounds\menu_advance"));
            mSoundEffects.Add(eSounds.menu_back, State.Game.Content.Load<SoundEffect>(@"Content\Sounds\menu_back"));
            mSoundEffects.Add(eSounds.menu_badselect, State.Game.Content.Load<SoundEffect>(@"Content\Sounds\menu_bad_select"));
            mSoundEffects.Add(eSounds.menu_scroll, State.Game.Content.Load<SoundEffect>(@"Content\Sounds\menu_scroll"));
            mSoundEffects.Add(eSounds.menu_select, State.Game.Content.Load<SoundEffect>(@"Content\Sounds\menu_select"));
            mSoundEffects.Add(eSounds.menu_select_1, State.Game.Content.Load<SoundEffect>(@"Content\Sounds\menu_select_1"));
            mSoundEffects.Add(eSounds.menu_select_4, State.Game.Content.Load<SoundEffect>(@"Content\Sounds\menu_select_4"));
            mSoundEffects.Add(eSounds.menu_select2, State.Game.Content.Load<SoundEffect>(@"Content\Sounds\menu_select2"));
            mSoundEffects.Add(eSounds.menu_select3, State.Game.Content.Load<SoundEffect>(@"Content\Sounds\menu_select3"));
            mSoundEffects.Add(eSounds.NewLevel, State.Game.Content.Load<SoundEffect>(@"Content\Sounds\NewLevel"));
        }

        /// <summary>
        /// Shuts down the sound code tidily
        /// </summary>
        public static void Dispose()
        {
            foreach (SoundEffect eff in mSoundEffects.Values)
                eff.Dispose();

        }
    }
}
