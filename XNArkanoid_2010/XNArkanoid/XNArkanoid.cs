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
using XNArkanoid.Components;

namespace XNArkanoid
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class XNArkanoidGame : Microsoft.Xna.Framework.Game
    {

        private static Settings mSettings = null;
        public GraphicsDeviceManager mGraphics;
        private Texture2D mLogoTaito;
        public SpriteBatch mSpriteBatch = null;
        public SpriteFont mSpriteFont = null;
        private UIGame.GUIMainScreen mMainDialog = null;
        private UIGame.GUIContinueScreen mContinueDialog = null;
        public KeyboardState mKeyboardState;
        public static eGameState mGameState = eGameState.None;
        public Vaus mVaus;
        public Rectangle mFrameRectangle;
        private Rectangle mScoreRectangle;
        public Rectangle mGameRectangle;
        private int mCurrentLevelIdx = -1;
        public Components.Level mCurrentLevel;
        public List<Components.Level> mLevels = new List<Components.Level>();
        public static float mScreenWidthMultiplier, mScreenHeightMultiplier;
        public static System.IO.IsolatedStorage.IsolatedStorageFile mIsolatedStorage;

        #region Props
        public static Settings Settings
        {
            get { return mSettings; }
            set { mSettings = value; }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public XNArkanoidGame()
        {
            State.Game = this;

            mIsolatedStorage = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();
            XNArkanoidGame.mGameState = eGameState.Loading;
            
            mGraphics = new GraphicsDeviceManager(this);
            mSettings = Settings.Load();
            mGraphics.PreferredBackBufferWidth = mSettings.ScreenWidth;
            mGraphics.PreferredBackBufferHeight = mSettings.ScreenHeight;

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            mVaus = new Vaus(this);

            // Create dialogs
            mMainDialog = new UIGame.GUIMainScreen(this);
            mMainDialog.ControlSelected += new global::XNArkanoid.UI.UIDialog.ControlSelectedDelegate(mMainDialog_ControlSelected);

            mContinueDialog = new UIGame.GUIContinueScreen(this);
            mContinueDialog.PlayerContinues += new EventHandler(mContinueDialog_PlayerContinues);
            mContinueDialog.CounterReachedZero += new EventHandler(mContinueDialog_CounterReachedZero);

            Level.AllBallsFallen += new Level.AllBallsFallenDelegate(Level_AllBallsFallen);
            Level.EndLevel += new Level.EndLevelDelegate(Level_EndLevel);

            XNArkanoidGame.mGameState = eGameState.DialogMain;
        }    
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // This multiplier adjust graphics to other resolutions. WP7 always has the same resolution, so it´s not used in this version
            mScreenWidthMultiplier = 1f;// (float)mGraphics.GraphicsDevice.Viewport.Width / 800f;
            mScreenHeightMultiplier = 1f;// (float)mGraphics.GraphicsDevice.Viewport.Height / 600f;

            int frameWidth  = 17;
            int frameHeight = 29;
            int scoreArea;

            switch (Settings.ScreenMode)
            {
                case eScreenMode.Portrait:
                    scoreArea = (int)(Settings.ScoreAreaPercent * 800f);
                    mFrameRectangle = new Rectangle(0, scoreArea, 480, 800);
                    mScoreRectangle = new Rectangle(0, 0, 480, scoreArea);
                    break;
                case eScreenMode.Landscape:
                    scoreArea = (int)(Settings.ScoreAreaPercent * 800f);
                    mFrameRectangle = new Rectangle(0, scoreArea, 480, 800);
                    mScoreRectangle = new Rectangle(0, 0, 480, scoreArea);
                    break;
            }
            mGameRectangle = new Rectangle(mFrameRectangle.X + frameWidth, mFrameRectangle.Y + frameHeight, mFrameRectangle.Width - mFrameRectangle.X - (frameWidth * 2), (mFrameRectangle.Height - mFrameRectangle.Y - frameHeight));
          
            // Initialize sound
            Sound.Sound.Initialize();

            // Now that we have all the levels loaded, register them as GameComponents
            Components.Add(mMainDialog);
            Components.Add(mContinueDialog);        

            base.Initialize();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void OnExiting(object sender, EventArgs args)
        {
            // Save settings on exit
            mSettings.Save();

            Sound.Sound.Dispose();

            base.OnExiting(sender, args);
        }     
        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            mSpriteBatch = new SpriteBatch(mGraphics.GraphicsDevice);
            mSpriteFont = Content.Load<SpriteFont>(@"Content\SpriteFont1");            
            mLogoTaito = Content.Load<Texture2D>(@"Content\logoTaito");

            Brick.LoadTextures(this);
            Prize.LoadTextures(this);
            Ball.LoadTextures(this);
            Vaus.LoadTextures(this);

            // Give 20 pixels of fall area height below vaus
            mVaus.mPosition = new Vector2(mVaus.mPosition.X, mGraphics.GraphicsDevice.Viewport.Height - 20 - Vaus.cHeight);

            // Load levels. Should be done after sprite creation
            //Microsoft.Xna.Framework.Color colo = new Microsoft.Xna.Framework.Color(228, 255, 0);
            //uint a = colo.PackedValue;

            mLevels.Clear();
            Level lev = Content.Load<Level>(@"Content\Levels\Level1");
            lev.UpdateDrawingRectangles(mGameRectangle);
            mLevels.Add(lev);
            foreach (Level level in mLevels)
                level.LoadContent();

            base.LoadContent();

            XNArkanoidGame.mGameState = eGameState.DialogMain;
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Update Input
            mKeyboardState = Keyboard.GetState();
            Input.XInputHelper.Update(this, mKeyboardState);

            // Update game 
            switch (XNArkanoidGame.mGameState)
            {
                case eGameState.Loading:
                    break;
                case eGameState.Running:
                    // Update Levels
                    mCurrentLevel.Update(gameTime);

                    // Update Vaus
                    mVaus.Update(gameTime);
                    break;
                case eGameState.Paused:
                    break;
            }
            base.Update(gameTime);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            mGraphics.GraphicsDevice.Clear(Color.Black);
          
            base.Draw(gameTime);

            mSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            switch (XNArkanoidGame.mGameState)
            {
                case eGameState.Loading:
                    //mSpriteBatch.Draw(this.mLogo, new Rectangle(0, 0, mGraphics.GraphicsDevice.Viewport.Width, mGraphics.GraphicsDevice.Viewport.Height), Color.White);
                    break;
                case eGameState.Running:
                    // Draw Level
                    if (mCurrentLevel != null)
                        mCurrentLevel.Draw(gameTime);

                    // Draw Vaus
                    if (mCurrentLevel.mLevelInitializeCue != null && mCurrentLevel.mLevelInitializeCue.State != SoundState.Playing)
                    {
                        int addx = 0, addy = 0;
                        Rectangle rect;
                        for (int i = 0; i < mVaus.mLivesRemaining; i++)
                        {
                            rect = new Rectangle(mScoreRectangle.X + 10 + addx, mScoreRectangle.Y + 50 + addy, 45, 10);
                            mSpriteBatch.Draw(Vaus.mTexture, rect, Color.White);
                            addx += 55;
                            if ((i - 2) % 3 == 0)
                            {
                                addx = 0;
                                addy += 20;
                            }
                            // Only draw first 12 lives
                            if (i >= 11)
                                break;
                        }

                        // Draw Score
                        Vector2 pos = new Vector2(mScoreRectangle.X + 20, mScoreRectangle.Y + 120);
                        mSpriteBatch.DrawString(mSpriteFont, "SCORE", pos, Color.Red);
                        pos = new Vector2(mScoreRectangle.X + 60, mScoreRectangle.Y + 150);
                        mSpriteBatch.DrawString(mSpriteFont, this.mVaus.mScore.ToString(), pos, Color.White);

                        // Draw Round
                        pos = new Vector2(mScoreRectangle.X + 20, mScoreRectangle.Y + 220);
                        mSpriteBatch.DrawString(mSpriteFont, "ROUND", pos, Color.Red);
                        pos = new Vector2(mScoreRectangle.X + 60, mScoreRectangle.Y + 250);
                        mSpriteBatch.DrawString(mSpriteFont, this.mCurrentLevelIdx.ToString(), pos, Color.White);

                        // Draw logo Taito
                        rect = new Rectangle(mScoreRectangle.Width - 200 , mScoreRectangle.Y + 50, 200, 200);
                        mSpriteBatch.Draw(mLogoTaito, rect, Color.White);


                        // Draw Vaus
                        mVaus.Draw(gameTime);
                    }
                    break;
            }


            mSpriteBatch.End(); 
        }

        #region Dialog Events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pControl"></param>
        /// <param name="pIdx"></param>
        void mMainDialog_ControlSelected(UI.UIControl pControl, int pIdx)
        {
            if (pControl == mMainDialog.mNewGameButton)
                this.StartNewGame();
            else if (pControl == mMainDialog.mExitButton)
                this.Exit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mContinueDialog_CounterReachedZero(object sender, EventArgs e)
        {
            XNArkanoidGame.mGameState = eGameState.DialogMain; 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mContinueDialog_PlayerContinues(object sender, EventArgs e)
        {
            this.ContinueGame();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void ContinueGame()
        {
            mVaus.mLivesRemaining = 3;
            mVaus.VausState = eVausState.Normal;

            mCurrentLevel.Reset();
            mGameState = eGameState.Running;
            SoundEffectInstance returnValue = Sound.Sound.Play(eSounds.NewLevel);
        }
        /// <summary>
        /// 
        /// </summary>
        private void StartNewGame()
        {
            mVaus.mLivesRemaining = 3;
            mVaus.VausState = eVausState.Normal;

            mCurrentLevelIdx = 0;
            mCurrentLevel = mLevels[0];
            mCurrentLevel.Reset();

            mGameState = eGameState.Running;
            SoundEffectInstance returnValue = Sound.Sound.Play(eSounds.NewLevel);
        }

        #region Level Events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQueNivel"></param>
        void Level_AllBallsFallen(Level pLevel)
        {
            mVaus.mLivesRemaining--;

            if (mVaus.mLivesRemaining <= 0)
            {
                mContinueDialog.Counter = 10;
                XNArkanoidGame.mGameState = eGameState.DialogContinue;
            }
            else
            {
                mCurrentLevel.Reset();
                mGameState = eGameState.Running;
                SoundEffectInstance returnValue = Sound.Sound.Play(eSounds.NewLevel);
                mVaus.VausState = eVausState.Normal;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pLevel"></param>
        void Level_EndLevel(Level pLevel)
        {
            if (mCurrentLevelIdx < mLevels.Count - 1)
            {
                mCurrentLevelIdx++;
                mCurrentLevel = mLevels[mCurrentLevelIdx];
                mVaus.VausState = eVausState.Normal;
            }
            else
            {
                this.Exit();
            }
        }

        #endregion
    }
}