/* --------------------------------------------------------------------------------------------------------
 * Author: Iñaki Ayucar (http://graphicdna.blogspot.com)
 * Date: 1/05/2010
 * 
 * This software is distributed "for free" for any non-commercial usage. The software is provided “as-is.” 
 * You bear the risk of using it. The author give no express warranties, guarantees or conditions.
 * If you use this software in another project, you agree to mention the author and source.
 ---------------------------------------------------------------------------------------------------------*/
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNArkanoid.Descs
{
    /// <summary>
    /// Reference on Serialization:
    /// http://blogs.msdn.com/shawnhar/archive/2008/08/12/everything-you-ever-wanted-to-know-about-intermediateserializer.aspx
    /// </summary>
    public class BrickDesc 
    {
        public enum ePrizes
        {
            None,
            BreakToNextLevel,
            CatchBall,
            DisruptionIntoThreeBalls,
            ExpandVaus,
            Laser,
            PlayerExtend,
            SpeedDown,
        }

        public Vector2 Position;
        public int Score;
        public int Hits = 1;
        public ePrizes Prize = ePrizes.None;
        public Vector2 Size;
        public Color Color;
        
    }
}
