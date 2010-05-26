/* --------------------------------------------------------------------------------------------------------
 * Author: Iñaki Ayucar (http://graphicdna.blogspot.com)
 * Date: 1/04/2009
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
using Microsoft.Xna.Framework.Audio;

namespace XNArkanoid.Components
{
    public class Level : DrawableGameComponent
    {
        public string PatternAssetName;
        public string FrameAssetName;
        private XNArkanoidGame mGame;
        private Texture2D mPattern, mFrame;
        public SoundEffectInstance mLevelInitializeCue;
        public System.Collections.Generic.List<Brick> mBricks;
        public System.Collections.Generic.List<Ball> mBalls;
        private System.Collections.Generic.List<Ball> mDeadBalls;
        private System.Collections.Generic.List<Prize> mPrizes;
        private System.Collections.Generic.List<Prize> mDeadPrizes;
        public delegate void EndLevelDelegate(Level pLevel);
        public static event EndLevelDelegate EndLevel;
        public delegate void AllBallsFallenDelegate(Level pLevel);
        public static event AllBallsFallenDelegate AllBallsFallen;


        /// <summary>
        /// 
        /// </summary>
        public Level(XNArkanoidGame pGame)
            : base(pGame)
        {
            mGame = pGame;
            this.mBricks = new List<Brick>();
            this.mBalls = new List<Ball>();
            this.mDeadBalls = new List<Ball>();
            this.mPrizes = new List<Prize>();
            this.mDeadPrizes = new List<Prize>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // Wait until init sound ends
            if (mLevelInitializeCue != null && mLevelInitializeCue.State == SoundState.Playing)
                return;

            // Update Bricks
            bool alldead = true;
            foreach (Brick brk in this.mBricks)
            {
                brk.Update(gameTime);
                if (brk.IsAlive)
                    alldead = false;
            }
            if (alldead && Level.EndLevel != null)
            {
                Level.EndLevel(this);
                return;
            }

            // Update Balls
            mDeadBalls.Clear();
            foreach (Ball ball in this.mBalls)
            {
                ball.Update(gameTime);
                if (ball.mBallState == eBallState.Dead)
                    mDeadBalls.Add(ball);
            }
            foreach (Ball ball in this.mDeadBalls)
                mBalls.Remove(ball);
            if (this.mBalls.Count == 0 && Level.AllBallsFallen != null)
            {
                Level.AllBallsFallen(this);
                return;
            }

            // Update Prizes
            mDeadPrizes.Clear();
            foreach (Prize p in this.mPrizes)
            {
                p.Update(gameTime);
                if (p.mPrizeDead)
                    mDeadPrizes.Add(p);
            }
            foreach (Prize p in mDeadPrizes)
                mPrizes.Remove(p);

            base.Update(gameTime);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pGameRectangle"></param>
        public void UpdateDrawingRectangles(Rectangle pGameRectangle)
        {
            foreach (Brick brk in this.mBricks)
                brk.UpdateDrawingRectangles(pGameRectangle);
        
       
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {

            // Wait until init sound ends
            if (mLevelInitializeCue != null && mLevelInitializeCue.State == SoundState.Playing)
            {
                // While level initializing draw a text with level number
                Vector2 pos = new Vector2(mGame.mGraphics.GraphicsDevice.Viewport.Width * 0.5f, mGame.mGraphics.GraphicsDevice.Viewport.Height * 0.5f);
                string text = "Level " + this.mGame.mLevels.IndexOf(this);
                Vector2 size = this.mGame.mSpriteFont.MeasureString(text);
                pos -= size * 0.5f;
                mGame.mSpriteBatch.DrawString(this.mGame.mSpriteFont, text, pos, Color.White);
                return;
            }

            // Draw game zone´s background and frame
            mGame.mSpriteBatch.Draw(this.mPattern, this.mGame.mFrameRectangle, Color.White);
            mGame.mSpriteBatch.Draw(this.mFrame, this.mGame.mFrameRectangle, Color.White);

            // Draw bricks balls and vaus
            foreach (Brick brk in this.mBricks)
                brk.Draw(gameTime);
            foreach (Ball bola in this.mBalls)
                bola.Draw(gameTime);
            foreach (Prize p in this.mPrizes)
                p.Draw(gameTime);

            base.Draw(gameTime);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadAllContent"></param>
        public new void LoadContent()
        {


            // The content manager loads each assert just once
            mPattern = mGame.Content.Load<Texture2D>(PatternAssetName);
            mFrame = mGame.Content.Load<Texture2D>(FrameAssetName);


            base.LoadContent();
        }
        /// <summary>
        /// Resets all game logic of the level to it´s initial state
        /// </summary>
        public void Reset()
        {
            mLevelInitializeCue = Sound.Sound.Play(eSounds.NewLevel);

            this.mBalls.Clear();
            Ball ball = new Ball(this.mGame);
            
            ball.mStickedToVausSecs = 2f;
            
            ball.mStickedToVausOffset = new Vector2(mGame.mVaus.Width - ball.mDiameterPixels, -ball.mRadiusPixels);
            this.mBalls.Add(ball);
        }

        #region Prize management
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPrizeType"></param>
        /// <param name="pPos"></param>
        public void AddPrize(ePrizes pPrizeType, Vector2 pPos)
        {
            Prize p = new Prize(this.mGame, pPrizeType, pPos);
            

            mPrizes.Add(p);
        }    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPrize"></param>
        public void ApplyPrize(Prize pPrize)
        {
            this.mGame.mVaus.mScore += 100;
            if (pPrize.mPrizeType == ePrizes.PlayerExtend)
                Sound.Sound.Play(eSounds.Hit4);
            else Sound.Sound.Play(eSounds.menu_back);


            foreach (Ball ball in mBalls)
                ball.ApplyPrize(pPrize);

            switch (pPrize.mPrizeType)
            {
                // Level Prizes
                case ePrizes.DisruptionIntoThreeBalls:
                    List<Ball> newballs = new List<Ball>();
                    newballs.AddRange(mBalls);
                    if (newballs.Count < 9)
                    {
                        foreach (Ball ball in mBalls)
                        {
                            if (ball.mBallState != eBallState.Normal)
                                continue;
                            Ball nb1 = new Ball(this.mGame);
                            Ball nb2 = new Ball(this.mGame);
                            nb1.mPosition = nb2.mPosition = ball.mPosition;
                            nb1.mDir = new Vector2(ball.mDir.X, ball.mDir.Y * -1);
                            nb2.mDir = new Vector2(ball.mDir.X * -1, ball.mDir.Y * -1);
                            newballs.Add(nb1);
                            newballs.Add(nb2);

                            // Do not allow more than 9 balls at a time
                            if (newballs.Count >= 9)
                                break;
                        }
                    }
                    mBalls = newballs;
                    break;
                case ePrizes.PlayerExtend:
                    mGame.mVaus.mLivesRemaining++;
                    break;
            }

            
        }
        #endregion
    }
}
