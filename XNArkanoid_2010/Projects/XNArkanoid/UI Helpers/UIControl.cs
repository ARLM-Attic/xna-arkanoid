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
using Microsoft.Xna.Framework;

namespace XNArkanoid.UI
{
    public class UIControl : DrawableGameComponent
    {
        protected Vector2 mPos;
        public Vector2 Pos
        {
            get { return this.mPos; }
            set { this.mPos = value; }
        }
        protected Vector2 mSize = Vector2.Zero;
        public Vector2 Size
        {
            get { return this.mSize; }
            set
            {
                this.mSize = value;
            }
        }
        protected bool mFocus = false;
        public bool Focus
        {
            get { return mFocus; }
            set { mFocus = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public UIControl(Game pGame):base(pGame)
        {
        }
    }
}
