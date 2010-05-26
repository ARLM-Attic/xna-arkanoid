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
using Microsoft.Xna.Framework.Graphics;
using XNArkanoid.UI;

namespace XNArkanoid.UIGame
{
    public class GUIContinueScreen : UIDialog
    {
        protected SpriteBatch mSpriteBatch = null;
        private SpriteFont mSpriteFont = null;
        private float mCounter = 10;
        private int mLastIntCounter = 10;
        public float Counter
        {
            get { return mCounter; }
            set
            {
                mCounter = value;
                mLastIntCounter = (int)mCounter;
            }
        }


        public event EventHandler CounterReachedZero = null;
        public event EventHandler PlayerContinues = null;

        /// <summary>
        /// 
        /// </summary>
        public GUIContinueScreen(Game pGame)
            : base(pGame)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (XNArkanoidGame.mGameState != eGameState.DialogContinue)
                return;

            // Check for player pressing A
            if (Input.XInputHelper.GamePads[PlayerIndex.One].APressed && PlayerContinues != null)
                PlayerContinues(this, EventArgs.Empty);

            // Refresh counter
            this.mCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Beep if counter changed
            if ((int)this.mCounter != mLastIntCounter)
                Sound.Sound.Play(eSounds.menu_select2);
            mLastIntCounter = (int)mCounter;

            // Check for counter reaching 0
            if (mCounter <= 0 && CounterReachedZero != null)
            {
                Sound.Sound.Play(eSounds.menu_advance);
                CounterReachedZero(this, EventArgs.Empty);
            }

            base.Update(gameTime);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (XNArkanoidGame.mGameState != eGameState.DialogContinue)
                return;

            string text = "Continue?  " + (int)mCounter;
            Vector2 size = mSpriteFont.MeasureString(text);
            Vector2 pos = Vector2.Zero;
            pos.X = ((float)mGame.mGraphics.GraphicsDevice.Viewport.Width * 0.5f) - (size.X * 0.5f);
            pos.Y = ((float)mGame.mGraphics.GraphicsDevice.Viewport.Height * 0.5f) - (size.Y * 0.5f);

            mSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            mSpriteBatch.DrawString(mSpriteFont, text, pos, Color.White);
            mSpriteBatch.End();

            // Don´t call base.Draw because there´s no control in mControls to render
        }
        /// <summary>
        /// Create all controls that will be displayed in the dialog and load all the content related
        /// </summary>
        /// <param name="loadAllContent"></param>
        protected override void LoadContent()
        {
            // Create spritebatch
            this.mSpriteBatch = new SpriteBatch(mGame.mGraphics.GraphicsDevice);

            this.mSpriteFont = mGame.Content.Load<SpriteFont>(@"Content\SpriteFont1");

            base.LoadContent();
        }

    }
}
