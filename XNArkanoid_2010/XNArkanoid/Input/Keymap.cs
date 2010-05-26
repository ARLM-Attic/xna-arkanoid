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
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
#endregion

namespace XNArkanoid.Input
{
    class Keymap
    {

        private Dictionary<GamePadKeys, List<Keys>> bindings;
        // currently a hack so I can bind the same keyboard key to multiple gamepad keys



        public Keymap()
        {
            bindings = new Dictionary<GamePadKeys, List<Keys>>();
        }


        public void Add(GamePadKeys gk, Keys k)
        {

            List<Keys> keyboardkey = new List<Keys>();
            keyboardkey.Add(k);


            if (bindings.ContainsKey(gk))
            {

                bindings.Remove(gk);
                bindings.Add(gk, keyboardkey);

            }
            else
            {
                bindings.Add(gk, keyboardkey);
            }

        }


        public Keys Get(GamePadKeys gk)
        {
            return bindings[gk][0];
        }

    }
}
