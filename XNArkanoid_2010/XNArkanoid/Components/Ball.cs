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
    public class Ball : DrawableGameComponent
    {
        private static Texture2D mTexture;
        public const float cSpeedNormal = 320f;
        public const float cSpeedFast = 380f;
        public const float cSpeedSlow = 220f;
        public const float cIncSpeedOnHit = 10f;

        private XNArkanoidGame mGame;
      
        private float mDyingTime;
        private Vector4 mDyingColor;
        private Rectangle mDrawingRectangle;
        private float mMultVelocidad = 1f;
        private float mSpeed;
        public float mDiameterPixels;
        public float mRadiusPixels;
     
        public eBallState mBallState = eBallState.Normal;
        public Vector2 mStickedToVausOffset = Vector2.Zero;
        public float mStickedToVausSecs = 0;      

        public Vector2 mPosition;     
        public Vector2 mDir;      

        /// <summary>
        /// Balls are created dynamically inside a game or when the game is reset, not when 
        /// game starts. Thats why I pass the texture and spriteBatch as a parameter in the constructor
        /// instead of assigning them in the LoadGraphicsContent, which will be called just once at game start
        /// </summary>
        public Ball(XNArkanoidGame pGame):base(pGame)
        {
            this.mGame = pGame;

            this.mDiameterPixels = 16;
            this.mRadiusPixels = 8;

            this.mDir = new Vector2(1f, -1f);
            this.mDir.Normalize();
            this.mSpeed = cSpeedNormal;

            this.mBallState = eBallState.Normal;

            //XNArkanoid.mKeyboard.KeyDown += new global::XNArkanoid.Input.KeyboardInput.KeyDelegate(mKeyboard_KeyDown);
        }
        /// <summary>
        /// 
        /// </summary>
        public static void LoadTextures(XNArkanoidGame pGame)
        {
            mTexture = pGame.Content.Load<Texture2D>(@"Content\Textures\Balls\default_ball");
        }     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if(Input.XInputHelper.GamePads[PlayerIndex.One].APressed)
                this.mStickedToVausSecs = 0f;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            switch (mBallState)
            {
                case eBallState.Normal:
                    if (mStickedToVausSecs > 0)
                    {
                        this.mPosition = mGame.mVaus.mPosition + this.mStickedToVausOffset;
                        mStickedToVausSecs -= dt;
                    }
                    else
                    {
                        mStickedToVausSecs = 0f;
                        this.CheckCollisions(dt);

                        // Refresh ball position
                        this.mPosition.X = (float)this.mPosition.X + (this.mDir.X * this.mSpeed * this.mMultVelocidad * dt);
                        this.mPosition.Y = (float)this.mPosition.Y + (this.mDir.Y * this.mSpeed * this.mMultVelocidad * dt);

                        // Check fallen ball
                        if (this.mPosition.Y > (mGame.mGameRectangle.Y + mGame.mGameRectangle.Height - mRadiusPixels))
                        {
                            Sound.Sound.Play(eSounds.Death);
                            this.mBallState = eBallState.Dying;
                            this.mDyingColor = new Vector4(1f, 1f, 1f, 1f);
                            this.mDyingTime = 2;
                        }
                    }

                    // Balls will be drawn with Pos in the center of the sprite, instead of top left
                    // corner. Just to make calcs easier
                    mDrawingRectangle = new Rectangle((int)(this.mPosition.X - mRadiusPixels), (int)(this.mPosition.Y - mRadiusPixels), (int)this.mDiameterPixels, (int)this.mDiameterPixels);
                    break;
                case eBallState.Dying:
                    this.mDyingTime -= dt;
                    this.mDyingColor.W -= dt;
                    if (this.mDyingTime <= 0)
                        this.mBallState = eBallState.Dead;
                    break;
            }
            
            base.Update(gameTime);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {

            switch (mBallState)
            {
                case eBallState.Normal:
                    mGame.mSpriteBatch.Draw(mTexture, mDrawingRectangle, Color.White);
                    break;
                case eBallState.Dying:
                    mGame.mSpriteBatch.Draw(mTexture, mDrawingRectangle, new Color(mDyingColor));
                    break;
            }
            base.Draw(gameTime);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPrize"></param>
        public void ApplyPrize(Prize pPrize)
        {
            switch (pPrize.mPrizeType)
            {
                case ePrizes.SpeedDown:
                    this.mSpeed = cSpeedSlow;
                    break;
            }
        }

        #region Collisions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        private void CheckCollisions(float dt)
        {
            // Check collisions with walls
            if (this.mPosition.X > (mGame.mGameRectangle.X + mGame.mGameRectangle.Width - this.mRadiusPixels))
            {
                this.mDir.X = -1;
                this.mSpeed = Math.Min(cSpeedFast, mSpeed + cIncSpeedOnHit);
            }
            if (this.mPosition.X < mGame.mGameRectangle.X + mRadiusPixels)
            {
                this.mDir.X = 1;
                this.mSpeed = Math.Min(cSpeedFast, mSpeed + cIncSpeedOnHit);
            }
            if (this.mPosition.Y < mGame.mGameRectangle.Y + mRadiusPixels)
            {
                this.mDir.Y = 1;
                this.mSpeed = Math.Min(cSpeedFast, mSpeed + cIncSpeedOnHit);
            }

            // Check Collisions with vaus
            Vector2 coll, dir;
            if (this.Intersects(mGame.mVaus.mDrawingRectangle, out coll, out dir))
            {
                Sound.Sound.Play(eSounds.Hit1);
                this.mDir.X = this.mPosition.X - (mGame.mVaus.mPosition.X + mGame.mVaus.HalfWidth);
                this.mDir.X /= (mGame.mVaus.HalfWidth * 0.5f);
                this.mDir.Y = -1;
                this.mDir.Normalize();

                // Each time the ball touches the vaus, speed is reset
                this.mSpeed = cSpeedNormal;
            }

            // Check collisions with bricks
            foreach (Brick brk in mGame.mCurrentLevel.mBricks)
            {
                if (!brk.IsAlive)
                    continue;

                if (this.Intersects(brk.mDrawingRectangle, out coll, out dir))
                {
                    if (brk.RemainingHits > 1)
                        Sound.Sound.Play(eSounds.Hit3);
                    else Sound.Sound.Play(eSounds.Hit2);


                    if (dir == Vector2.Zero)
                        this.mDir = new Vector2(this.mDir.X * -1, this.mDir.Y * -1);
                    else
                    {
                        if (dir.X != 0)
                        {
                            if (Math.Sign(dir.X) != Math.Sign(this.mDir.X))
                                this.mDir.X *= -1;
                        }
                        if (dir.Y != 0)
                        {
                            if (Math.Sign(dir.Y) != Math.Sign(this.mDir.Y))
                                this.mDir.Y *= -1;
                        }
                    }

                    brk.HitByBall();

                    this.mSpeed = Math.Min(cSpeedFast, mSpeed + cIncSpeedOnHit);

                    // Do not allow two brick hits in the same step
                    break;
                }

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        private bool Intersects(Rectangle pRect, out Vector2 pCollPoint, out Vector2 pCollDir)
        {
            float Rad2 = this.mRadiusPixels * this.mRadiusPixels;
            
            pCollDir = pCollPoint = Vector2.Zero;

            // Translate coordinates, placing Ball´s pos at the origin. 
            float maxx = (pRect.X + pRect.Width);
            float maxy = (pRect.Y + pRect.Height);
            float minx2, maxx2, miny2, maxy2;
            maxx2 = maxx - this.mPosition.X;
            maxy2 = maxy - this.mPosition.Y;
            minx2 = pRect.X - this.mPosition.X;
            miny2 = pRect.Y - this.mPosition.Y;

            // R to left of circle center 
            if (maxx2 < 0)
            {
                if (maxy2 < 0)
                {
                    // R in lower left corner -> el punto más cercano es el inferior derecho
                    if ((maxx2 * maxx2 + maxy2 * maxy2) < Rad2)
                    {
                        pCollPoint = new Vector2(maxx, maxy);
                        pCollDir = new Vector2(1, 0);
                        return true;
                    }
                    else return false;
                }
                else if (miny2 > 0)
                {
                    // R in upper left corner 
                    if ((maxx2 * maxx2 + miny2 * miny2) < Rad2)
                    {
                        pCollPoint = new Vector2(maxx, pRect.Y);
                        pCollDir = new Vector2(1, 0);
                        return true;
                    }
                    else return false;
                }
                else
                {
                    // R due West of circle 
                    if (Math.Abs(maxx2) < mRadiusPixels)
                    {
                        pCollPoint = new Vector2(maxx, mPosition.Y);
                        pCollDir = new Vector2(1, 0);
                        return true;
                    }
                    else return false;
                }
            }
            // R to right of circle center 7
            else if (minx2 > 0)
            {
                if (maxy2 < 0) 	/* R in lower right corner */
                {
                    if ((minx2 * minx2 + maxy2 * maxy2) < Rad2)
                    {
                        pCollPoint = new Vector2(pRect.X, maxy);
                        pCollDir = new Vector2(-1, 0);
                        return true;
                    }
                    else return false;
                }
                else if (miny2 > 0)  	/* R in upper right corner */
                {
                    if ((minx2 * minx2 + miny2 * miny2) < Rad2)
                    {
                        pCollPoint = new Vector2(pRect.X, pRect.Y);
                        pCollDir = new Vector2(-1, 0);
                        return true;
                    }
                    else return false;

                }
                else 				/* R due East of circle */
                {
                    if (minx2 < mRadiusPixels)
                    {
                        pCollPoint = new Vector2(pRect.X, mPosition.Y);
                        pCollDir = new Vector2(-1, 0);
                        return true;
                    }
                    else return false;

                }
            }
            // R on circle vertical centerline
            else
            {
                if (maxy2 < 0) 	/* R due South of circle */
                {
                    if (Math.Abs(maxy2) < mRadiusPixels)
                    {
                        pCollPoint = new Vector2(mPosition.X, maxy);
                        pCollDir = new Vector2(0, 1);
                        return true;
                    }
                    else return false;

                }
                else if (miny2 > 0)  	/* R due North of circle */
                {
                    if (miny2 < mRadiusPixels)
                    {
                        pCollPoint = new Vector2(mPosition.X, pRect.Y);
                        pCollDir = new Vector2(0, -1);
                        return true;
                    }
                    else return false;

                }
                else 				/* R contains circle centerpoint */
                {
                    pCollPoint = new Vector2(mPosition.X, mPosition.Y);
                    pCollDir = new Vector2(0, 1);
                    return (true);
                }
            }
        }
        #endregion

    }
}
