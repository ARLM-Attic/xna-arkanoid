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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNA = Microsoft.Xna.Framework;

namespace XNArkanoid.UI
{
    /// <summary>
    /// This class holds all the SpriteBatch funciontality but adding the texture to be rendered
    /// and a Texture AlignMode to render it.
    /// </summary>
    public partial class UIButton : UIControl
    {
        public SpriteBatch mSpriteBatch = null;
                
        protected Texture2D mTexture = null;
        public Texture2D Texture
        {
            get { return this.mTexture; }
            set
            {
                this.mTexture = value;
                this.RefreshAlign();
            }
        }
        /// <summary>
        /// Hide the normal Size Property in order to refresh the Alignment every time is changed
        /// </summary>
        public new Vector2 Size
        {
            get { return mSize; }
            set
            {
                mSize = value;
                this.RefreshAlign();
            }
        }
        protected eAlignMode mAlignMode = eAlignMode.TopCenter;
        public eAlignMode AlignMode
        {
            get { return this.mAlignMode; }
            set
            {
                this.mAlignMode = value;
                this.RefreshAlign();
            }
        }
        protected Vector2 mAlignOffset = Vector2.Zero;
        protected Color mColorTint = Color.White;
        public Color ColorTint
        {
            get { return mColorTint; }
            set { mColorTint = value; }
        }

        protected Color mFocusColor = Color.SteelBlue;
        public Color FocusColor
        {
            get { return mFocusColor; }
            set { mFocusColor = value; }
        }
        private float mFocusAnimationSpeed = 1f;
        public float FocusAnimationSpeed
        {
            get { return mFocusAnimationSpeed; }
            set { mFocusAnimationSpeed = value; }
        }
        private Color mFocusAnimationCurrentColor = Color.White;
        private int mFocusAnimationCurrentDir = 1;

        /// <summary>
        /// A single spriteBatch is used for the full UI
        /// </summary>
        /// <param name="game"></param>
        public UIButton(Game game, SpriteBatch pSpriteBatch): base(game)
        {
            this.mSpriteBatch = pSpriteBatch;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (this.mFocus)
            {
                Vector3 focus = mFocusColor.ToVector3();
                Vector3 cur = mFocusAnimationCurrentColor.ToVector3();
                float inc = (float)gameTime.ElapsedGameTime.TotalSeconds * mFocusAnimationCurrentDir * mFocusAnimationSpeed;
                cur = new Vector3(cur.X + inc, cur.Y + inc, cur.Z + inc);
                cur.X = Math.Min(focus.X, cur.X);
                cur.Y = Math.Min(focus.Y, cur.Y);
                cur.Z = Math.Min(focus.Z, cur.Z);
                cur.X = Math.Max(0, cur.X);
                cur.Y = Math.Max(0, cur.Y);
                cur.Z = Math.Max(0, cur.Z);
                this.mFocusAnimationCurrentColor = new Color(cur);


                if (mFocusAnimationCurrentColor == mFocusColor || mFocusAnimationCurrentColor == Color.Black)
                    mFocusAnimationCurrentDir *= -1;
            }
            else mFocusAnimationCurrentColor = mColorTint;

            base.Update(gameTime);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (this.mTexture == null)
                return;

            
            Vector2 pos = this.mPos + this.mAlignOffset;
            mSpriteBatch.Draw(this.mTexture, new Rectangle((int)pos.X, (int)pos.Y, (int)this.mSize.X, (int)this.mSize.Y), mFocusAnimationCurrentColor);
            base.Draw(gameTime);

        }      
        /// <summary>
        /// 
        /// </summary>
        protected void RefreshAlign()
        {
            Vector2 halfTex = new Vector2((float)this.mSize.X * 0.5f, (float)this.mSize.Y * 0.5f);
            switch (this.mAlignMode)
            {
                case eAlignMode.Center:
                    mAlignOffset = new Vector2(-halfTex.X, -halfTex.Y);
                    break;
                case eAlignMode.TopLeft:
                    mAlignOffset = Vector2.Zero;
                    break;
                case eAlignMode.TopRight:
                    mAlignOffset = new Vector2(-this.mSize.X, 0f);
                    break;
                case eAlignMode.BottomLeft:
                    mAlignOffset = new Vector2(0, -this.mSize.Y);
                    break;
                case eAlignMode.BottomRight:
                    mAlignOffset = new Vector2(-this.mSize.X, -this.mSize.Y);
                    break;
                case eAlignMode.TopCenter:
                    mAlignOffset = new Vector2(-(this.mSize.X * 0.5f), 0f);
                    break;
                case eAlignMode.BottomCenter:
                    mAlignOffset = new Vector2(-(this.mSize.X * 0.5f), -this.mSize.Y);
                    break;
            }
        }
    }
}


