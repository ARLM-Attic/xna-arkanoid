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
    public class Prize : DrawableGameComponent
    {
        
        public static Dictionary<ePrizes, Texture2D> mPrizeTextures = new Dictionary<ePrizes, Texture2D>();
        public const float cSpeedNormal = 320f;
        private XNArkanoidGame mGame = null;
        public ePrizes mPrizeType = ePrizes.None;
        private Vector2 mPosition;
        private Rectangle mDrawingRectangle;       
        public bool mPrizeDead = false;

        /// <summary>
        /// 
        /// </summary>
        public Prize(XNArkanoidGame pGame, ePrizes pPrizeType, Vector2 pPos)
            : base(pGame)
        {
            mGame = pGame;
            mPrizeType = pPrizeType;
            mPosition = pPos;
        }
       
        /// <summary>
        /// 
        /// </summary>
        public static void LoadTextures(XNArkanoidGame pGame)
        {
            mPrizeTextures.Clear();
            mPrizeTextures.Add(ePrizes.BreakToNextLevel, pGame.Content.Load<Texture2D>("Content\\Textures\\Prizes\\PrizeB"));
            mPrizeTextures.Add(ePrizes.CatchBall, pGame.Content.Load<Texture2D>("Content\\Textures\\Prizes\\PrizeC"));
            mPrizeTextures.Add(ePrizes.DisruptionIntoThreeBalls, pGame.Content.Load<Texture2D>("Content\\Textures\\Prizes\\PrizeD"));
            mPrizeTextures.Add(ePrizes.ExpandVaus, pGame.Content.Load<Texture2D>("Content\\Textures\\Prizes\\PrizeE"));
            mPrizeTextures.Add(ePrizes.Laser, pGame.Content.Load<Texture2D>("Content\\Textures\\Prizes\\PrizeL"));
            mPrizeTextures.Add(ePrizes.PlayerExtend, pGame.Content.Load<Texture2D>("Content\\Textures\\Prizes\\PrizeP"));
            mPrizeTextures.Add(ePrizes.SpeedDown, pGame.Content.Load<Texture2D>("Content\\Textures\\Prizes\\PrizeS"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            this.mPosition.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * cSpeedNormal;

            this.mDrawingRectangle = new Rectangle((int)this.mPosition.X, (int)this.mPosition.Y, 40, 20);

            // Check vaus - prize collisions
            this.mPrizeDead = false;
            if (this.mDrawingRectangle.Intersects(mGame.mVaus.mDrawingRectangle))
            {
                mGame.mCurrentLevel.ApplyPrize(this);
                mGame.mVaus.ApplyPrize(this);
                this.mPrizeDead = true;
            }

            base.Update(gameTime);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {

            mGame.mSpriteBatch.Draw(mPrizeTextures[mPrizeType], mDrawingRectangle, Color.White);

            base.Draw(gameTime);
        }

    }
}
