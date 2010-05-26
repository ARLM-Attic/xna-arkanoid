/* --------------------------------------------------------------------------------------------------------
 * Author: Iñaki Ayucar (http://graphicdna.blogspot.com)
 * Date: 1/05/2010
 * 
 * This software is distributed "for free" for any non-commercial usage. The software is provided “as-is.” 
 * You bear the risk of using it. The author give no express warranties, guarantees or conditions.
 * If you use this software in another project, you agree to mention the author and source.
 ---------------------------------------------------------------------------------------------------------*/
#region Dependencies
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace XNArkanoid.Input
{
    /// <summary>
    /// Easy access to a collection of gamepads
    /// </summary>
    public class GamePads
    {
        private GamePadHelper[] gamePads = new GamePadHelper[] 
        { 
            new GamePadHelper(PlayerIndex.One),
            new GamePadHelper(PlayerIndex.Two),
            new GamePadHelper(PlayerIndex.Three),
            new GamePadHelper(PlayerIndex.Four)
        };

        /// <summary>
        /// Returns the correct gamepad for a player
        /// </summary>
        /// <param name="player">Which player.</param>
        /// <returns></returns>
        public GamePadHelper this[PlayerIndex player]
        {
            get
            {
                return gamePads[(int)player];
            }
        }

        /// <summary>
        /// Updates the state of all gamepads so the XXXpressed functions will work. This method should be called once per frame
        /// </summary>
        public void Update(Game game, KeyboardState keyState)
        {
            foreach (GamePadHelper gamepad in gamePads)
            {
                gamepad.Update(game, keyState);
            }
        }
    }
}
