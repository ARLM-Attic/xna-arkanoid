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
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace XNArkanoid.Components
{
    /// <summary>
    /// Class taken from XNA Game Studio Express documentation
    /// It is prepared to draw animated textures with animation frames inside a single texture.
    /// All frames must be the same size and must be arranged horizontally inside the bigger texture
    /// The total texture width divided by the number of frames passed, gives the individual frame width
    /// </summary>
    public class AnimatedTexture
    {
        private int mFrameCount;
        private Texture2D mTexture;
        private float mTimePerFrame;
        private int mFrame;
        private float mTotalElapsed;
        private bool mPaused;
        public float mRotation, mScale, mDepth;
        public Vector2 mOrigin;
        private int mFrameWidth;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Origin"></param>
        /// <param name="Rotation"></param>
        /// <param name="Scale"></param>
        /// <param name="Depth"></param>
        public AnimatedTexture(Vector2 Origin, float Rotation, float Scale, float Depth)
        {
            this.mOrigin = Origin;
            this.mRotation = Rotation;
            this.mScale = Scale;
            this.mDepth = Depth;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="content"></param>
        /// <param name="asset"></param>
        /// <param name="FrameCount"></param>
        /// <param name="FramesPerSec"></param>
        public void Load(GraphicsDevice device, ContentManager content, string asset, int FrameCount, int FramesPerSec)
        {
            mFrameCount = FrameCount;
            mTexture = content.Load<Texture2D>(asset);
            mTimePerFrame = (float)1 / FramesPerSec;
            mFrame = 0;
            mTotalElapsed = 0;
            mPaused = false;
            mFrameWidth = mTexture.Width / mFrameCount;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elapsed"></param>
        public void Update(float elapsed)
        {
            if (mPaused)
                return;
            mTotalElapsed += elapsed;
            if (mTotalElapsed > mTimePerFrame)
            {
                mFrame++;
                // Keep the Frame between 0 and the total frames, minus one.
                mFrame = mFrame % mFrameCount;
                mTotalElapsed -= mTimePerFrame;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Batch"></param>
        /// <param name="screenpos"></param>
        public void Draw(SpriteBatch Batch, Vector2 screenpos)
        {
            DrawFrame(Batch, mFrame, screenpos);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Batch"></param>
        /// <param name="Frame"></param>
        /// <param name="screenpos"></param>
        public void DrawFrame(SpriteBatch Batch, int Frame, Vector2 screenpos)
        {
            //Rectangle sourcerect = new Rectangle(FrameWidth * Frame, 0,
            //    FrameWidth, mTexture.Height);
            //Batch.Draw(mTexture, screenpos, sourcerect, Color.White,
            //    mRotation, mOrigin, mScale, SpriteEffects.None, mDepth);
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsPaused
        {
            get { return mPaused; }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            mFrame = 0;
            mTotalElapsed = 0f;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            Pause();
            Reset();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Play()
        {
            mPaused = false;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Pause()
        {
            mPaused = true;
        }

    }
}
