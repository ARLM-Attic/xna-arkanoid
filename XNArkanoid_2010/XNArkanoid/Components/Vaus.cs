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
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace XNArkanoid.Components
{
    public class Vaus : DrawableGameComponent
    {
        public static Texture2D mTextureLaser;
        public static Texture2D mTextureBigger;
        public static Texture2D mTextureShot;
        public static Texture2D mTexture;

        #region Constants
        public const float cSpeedNormal = 12f;
        public const float cSpeedFast = 20f;
        public const float cSpeedSlow = 8f;
        public const float cWidthNormal = 70f;
        public const float cWidthBig = 100f;
        public const float cWidthSmall = 40f;
        public const float cShotSpeed = 800f;
        public const int cHeight = 24;
        #endregion

        private XNArkanoidGame mGame = null;

        public int mLivesRemaining = 0;
        public int mScore;

        private int mShotSize = 12;
        private int mShotHalfSize = 6;
        private eVausState mVausState = eVausState.Normal;
        public eVausState VausState
        {
            get { return mVausState; }
            set
            {
                mVausState = value;
                if (mVausState == eVausState.BiggerVaus)
                    this.Width = cWidthBig * XNArkanoidGame.mScreenWidthMultiplier;
                else this.Width = cWidthNormal * XNArkanoidGame.mScreenWidthMultiplier;
            }
        }      
        public Rectangle mDrawingRectangle;     
        private float mWidth = 0f;
        private float mHalfWidth = 0f;
        public float Width
        {
            get { return mWidth; }
            set
            {
                mWidth = value;
                mHalfWidth = mWidth * 0.5f;
            }
        }
        public float HalfWidth
        {
            get { return mHalfWidth; }
        }
        public Vector2 mPosition;     
        private float mSpeed;
        

        private List<Vector2> mShots = null;
        private double mLastShotTime;

        /// <summary>
        /// 
        /// </summary>
        public Vaus(XNArkanoidGame pGame):base(pGame)
        {
            this.mGame = pGame;
            this.mPosition.X = 100f;
            this.mSpeed = cSpeedNormal;
            this.Width = cWidthNormal;

            this.mShots = new List<Vector2>();
        }
        /// <summary>
        /// 
        /// </summary>
        public static void LoadTextures(XNArkanoidGame pGame)
        {
            mTexture = pGame.Content.Load<Texture2D>(@"Content\Textures\Vaus\vaus_normal");
            mTextureLaser = pGame.Content.Load<Texture2D>(@"Content\Textures\Vaus\vaus_laser");
            mTextureBigger = pGame.Content.Load<Texture2D>(@"Content\Textures\Vaus\vaus_bigger");
            mTextureShot = pGame.Content.Load<Texture2D>(@"Content\Textures\Vaus\vaus_shot");

        }       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            float thumb = 0f;
            float newpos = this.mPosition.X;

            // Check Mouse
            Microsoft.Xna.Framework.Input.MouseState st = Mouse.GetState();
            if (st.LeftButton == ButtonState.Pressed)
            {
                newpos = st.X;
            }


            // Check Input
            if ((thumb = Input.XInputHelper.GamePads[PlayerIndex.One].ThumbStickLeftX) != 0f)
            {
                newpos += this.mSpeed * thumb;
            }
            else if ((thumb = Input.XInputHelper.GamePads[PlayerIndex.One].ThumbStickRightX) != 0f)
            {
                newpos -= this.mSpeed * thumb;
            }
            else if (Input.XInputHelper.GamePads[PlayerIndex.One].LeftPressedContinous)
            {
                newpos -= this.mSpeed;
            }
            else if (Input.XInputHelper.GamePads[PlayerIndex.One].RightPressedContinous)
            {
                newpos += this.mSpeed;
            }



            // Apply frame limits
            if (newpos < mGame.mGameRectangle.X)
                newpos = mGame.mGameRectangle.X;
            int limMax = mGame.mGameRectangle.X + mGame.mGameRectangle.Width - (int)this.mWidth;
            if (newpos > limMax)
                newpos = limMax;
            mPosition.X = newpos;

            // Refresh drawingRect
            mDrawingRectangle = new Rectangle((int)mPosition.X, (int)mPosition.Y, (int)mWidth, (int)((float)cHeight * XNArkanoidGame.mScreenHeightMultiplier));

            // Update Shots
            this.UpdateShots(gameTime);

            base.Update(gameTime);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {

            switch(this.mVausState)
            {
                case eVausState.BiggerVaus:
                    mGame.mSpriteBatch.Draw(mTextureBigger, mDrawingRectangle, Color.White);
                    break;
                case eVausState.Laser:
                    mGame.mSpriteBatch.Draw(mTextureLaser, mDrawingRectangle, Color.White);
                    break;
                default:
                    mGame.mSpriteBatch.Draw(mTexture, mDrawingRectangle, Color.White);
                    break;
            }

            foreach (Vector2 shot in mShots)
                mGame.mSpriteBatch.Draw(mTextureShot, new Rectangle((int)shot.X, (int)shot.Y, mShotSize, mShotSize), Color.White);


            base.Draw(gameTime);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateShots(GameTime gameTime)
        {
            if (Input.XInputHelper.GamePads[PlayerIndex.One].APressed && mVausState == eVausState.Laser && mShots.Count < 4)
            {
                // Do not allow to fire if last ball is dying
                if (mGame.mCurrentLevel.mBalls.Count > 0 && (mGame.mCurrentLevel.mBalls.Count > 1 || mGame.mCurrentLevel.mBalls[0].mBallState == eBallState.Normal))
                {
                    double t = gameTime.TotalGameTime.TotalSeconds - mLastShotTime;
                    // Do not allow more than 10 shots per sec
                    if (t > 0.1)
                    {
                        this.mShots.Add(new Vector2(this.mPosition.X + this.mHalfWidth - mShotHalfSize, this.mPosition.Y));
                        Sound.Sound.Play(eSounds.menu_select);
                        mLastShotTime = gameTime.TotalGameTime.TotalSeconds;
                    }
                }
            }
            List<int> toremove = new List<int>();
            for (int i = 0; i < mShots.Count; i++)
            {
                mShots[i] = new Vector2(mShots[i].X, mShots[i].Y - cShotSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                if (mShots[i].Y < 0)
                {
                    toremove.Add(i);
                    continue;
                }

                foreach (Brick brk in mGame.mCurrentLevel.mBricks)
                {
                    if (!brk.IsAlive)
                        continue;

                    if (brk.mDrawingRectangle.Contains((int)mShots[i].X, (int)mShots[i].Y))
                    {
                        Sound.Sound.Play(eSounds.menu_select_1);
                        brk.HitByBall();
                        toremove.Add(i);
                    }
                }
            }
            foreach (int idx in toremove)
                mShots.Remove(mShots[idx]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPrize"></param>
        public void ApplyPrize(Prize pPrize)
        {
            switch (pPrize.mPrizeType)
            {
                case ePrizes.ExpandVaus:
                    this.VausState = eVausState.BiggerVaus;
                    break;
                case ePrizes.Laser:
                    this.VausState = eVausState.Laser;
                    break;
                case ePrizes.CatchBall:
                    this.VausState = eVausState.Catcher;
                    break;
                default:
                    this.VausState = eVausState.Normal;
                    break;
            }
        }
    }
}
