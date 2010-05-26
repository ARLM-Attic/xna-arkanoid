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
using Microsoft.Xna.Framework.Input;
using XNArkanoid.UI;

namespace XNArkanoid.UIGame
{
    public class GUIMainScreen : UIDialog
    {
        protected SpriteBatch mSpriteBatch = null;
        public UIButton mNewGameButton, mOptionsButtons, mExitButton;

        /// <summary>
        /// 
        /// </summary>
        public GUIMainScreen(Game pGame):base(pGame)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            switch (XNArkanoidGame.mGameState)
            {
                case eGameState.DialogMain:
                    base.Update(gameTime);
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            switch (XNArkanoidGame.mGameState)
            {
                case eGameState.DialogMain:
                    mSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    base.Draw(gameTime);
                    mSpriteBatch.End();
                    break;
            }
        }
        /// <summary>
        /// Create all controls that will be displayed in the dialog and load all the content related
        /// </summary>
        /// <param name="loadAllContent"></param>
        protected override void LoadContent()
        {
            // Create spritebatch
            this.mSpriteBatch = new SpriteBatch(mGame.mGraphics.GraphicsDevice);

            // Create buttons
            mNewGameButton = new UIButton(this.Game, this.mSpriteBatch);
            mNewGameButton.FocusAnimationSpeed = 0.9f;
            mNewGameButton.Texture = mGame.Content.Load<Texture2D>(@"Content\Textures\UI\NewGame");
            mNewGameButton.AlignMode = eAlignMode.TopLeft;
            mNewGameButton.Pos = new Vector2(40, 40);
            mNewGameButton.Size = new Vector2(192, 48);
            this.mControls.Add(mNewGameButton);

            mOptionsButtons = new UIButton(this.Game, this.mSpriteBatch);
            mOptionsButtons.FocusAnimationSpeed = 0.9f;
            mOptionsButtons.Texture = mGame.Content.Load<Texture2D>(@"Content\Textures\UI\Options");
            mOptionsButtons.AlignMode = eAlignMode.TopLeft;
            mOptionsButtons.Pos = new Vector2(40, 120);
            mOptionsButtons.Size = new Vector2(192, 48);
            this.mControls.Add(mOptionsButtons);

            mExitButton = new UIButton(this.Game, this.mSpriteBatch);
            mExitButton.FocusAnimationSpeed = 0.9f;
            mExitButton.Texture = mGame.Content.Load<Texture2D>(@"Content\Textures\UI\Exit");
            mExitButton.AlignMode = eAlignMode.TopLeft;
            mExitButton.Pos = new Vector2(40, 200);
            mExitButton.Size = new Vector2(192, 48);
            this.mControls.Add(mExitButton);

            base.LoadContent();
        }

    }
}
