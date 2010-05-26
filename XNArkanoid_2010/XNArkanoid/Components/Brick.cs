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
using XNArkanoid;

namespace XNArkanoid.Components
{
    public class Brick : DrawableGameComponent
    {
        public Vector2 Position;
        public int Score;
        public int Hits = 1;
        public ePrizes Prize = ePrizes.None;
        public Vector2 Size;
        public Color Color;
        private static List<Texture2D> mBrickTextures = new List<Texture2D>();
        private XNArkanoidGame mGame = null;
        public Rectangle mDrawingRectangle;
        private Rectangle mShadowDrawingRectangle;

        #region Props
        public bool IsAlive
        {
            get
            {
                if (Hits == 0)
                    return false;
                else return true;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Brick(XNArkanoidGame pGame) : base(pGame)
        {
            mGame = pGame;
        }
        /// <summary>
        /// 
        /// </summary>
        public static void LoadTextures(XNArkanoidGame pGame)
        {
            mBrickTextures.Clear();
            mBrickTextures.Add(pGame.Content.Load<Texture2D>("Content\\Textures\\Bricks\\BrickNormal"));
            mBrickTextures.Add(pGame.Content.Load<Texture2D>("Content\\Textures\\Bricks\\BrickStone"));
            mBrickTextures.Add(pGame.Content.Load<Texture2D>("Content\\Textures\\Bricks\\BrickShadow"));
        }    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {

         
            base.Update(gameTime);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pGameRectangle"></param>
        public void UpdateDrawingRectangles(Rectangle pGameRectangle)
        {
            this.mDrawingRectangle = new Rectangle((int)Position.X + pGameRectangle.X, (int)Position.Y + pGameRectangle.Y, (int)Size.X, (int)Size.Y);
            this.mShadowDrawingRectangle = new Rectangle((int)Position.X + 15 + pGameRectangle.X, (int)Position.Y + 15 + pGameRectangle.Y, (int)Size.X, (int)Size.Y);
  
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {

            if (!this.IsAlive)
                return;

            // Draw Shadow First
            mGame.mSpriteBatch.Draw(mBrickTextures[2], mShadowDrawingRectangle, Color.White);

            // Normal Draw
            if (this.Hits > 1)
                mGame.mSpriteBatch.Draw(mBrickTextures[1], mDrawingRectangle, Color);
            else mGame.mSpriteBatch.Draw(mBrickTextures[0], mDrawingRectangle, Color); 

            base.Draw(gameTime);
        }    
        /// <summary>
        /// 
        /// </summary>
        public void HitByBall()
        {
            if(this.Hits > 0)
                this.Hits--;

            if (this.Hits == 0 && this.Prize != ePrizes.None)
                mGame.mCurrentLevel.AddPrize(this.Prize, new Vector2(mDrawingRectangle.Location.X, mDrawingRectangle.Location.Y));

            mGame.mVaus.mScore += this.Score;
        }
     
    }
}
