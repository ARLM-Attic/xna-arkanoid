/* --------------------------------------------------------------------------------------------------------
 * Author: Iñaki Ayucar (http://graphicdna.blogspot.com)
 * Date: 1/05/2010
 * 
 * This software is distributed "for free" for any non-commercial usage. The software is provided “as-is.” 
 * You bear the risk of using it. The author give no express warranties, guarantees or conditions.
 * If you use this software in another project, you agree to mention the author and source.
 ---------------------------------------------------------------------------------------------------------*/
#region Dependencies
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion
namespace XNArkanoid.Input
{
    /// <summary>
    /// Provides a wrapper around the gamepads to allow single button presses to be detected
    /// </summary>
    public static class XInputHelper
    {
        /// <summary>
        /// Current pressed state of the gamepads
        /// </summary>
        private static GamePads gamePads = new GamePads();

        #region Properties
        public static GamePads GamePads
        {
            get
            {
                return gamePads;
            }
        }
        #endregion

        /// <summary>
        /// Update the state so presses can be detected - this should be called once per frame
        /// </summary>
        public static void Update(Game game, KeyboardState keyState)
        {
            gamePads.Update(game, keyState);
        }
    }
}
