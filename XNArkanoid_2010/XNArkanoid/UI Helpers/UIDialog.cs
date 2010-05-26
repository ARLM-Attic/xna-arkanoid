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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNA = Microsoft.Xna.Framework;

namespace XNArkanoid.UI
{
    public class UIDialog:DrawableGameComponent
    {
        public delegate void ControlSelectedDelegate(UIControl pQueControl, int pIdx);
        public event ControlSelectedDelegate ControlSelected = null;

        protected List<UIControl> mControls = null;
        protected int mFocusIdx = -1;
        public int FocusIdx
        {
            get { return mFocusIdx; }

            set
            {
                mFocusIdx = value;

                if (mControls != null)
                {
                    if (mFocusIdx >= mControls.Count)
                        mFocusIdx = 0;
                    if (mFocusIdx < 0)
                        mFocusIdx = mControls.Count - 1;
                }
            }
        }
        protected XNArkanoidGame mGame;



        /// <summary>
        /// 
        /// </summary>
        public UIDialog(Game pGame):base(pGame)
        {
            if(pGame is XNArkanoidGame)
                mGame = pGame as XNArkanoidGame;

            mControls = new List<UIControl>();
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            this.mFocusIdx = 0;
            base.Initialize();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadAllContent"></param>
        protected override void LoadContent()
        {
            // TODO: Add any other stuff you would like here...

            base.LoadContent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // Check Mouse
            Microsoft.Xna.Framework.Input.MouseState st = Mouse.GetState();


            foreach (UIControl ctrl in mControls)
            {
                int rectEndX = st.X + (int)ctrl.Size.X;
                int rectEndY = st.Y + (int)ctrl.Size.Y;

                if (st.X < ctrl.Pos.X || st.X > rectEndX || st.Y < ctrl.Pos.Y || st.Y > rectEndY)
                    continue;

                if (st.LeftButton == ButtonState.Pressed)
                {
                    int idx = mControls.IndexOf(ctrl);

                    if (idx == FocusIdx)
                    {
                        Sound.Sound.Play(eSounds.menu_advance);
                        ControlSelected(ctrl, this.mControls.IndexOf(ctrl));
                    }
                    else
                    {
                        Sound.Sound.Play(eSounds.menu_select2);
                        this.FocusIdx = this.mControls.IndexOf(ctrl);
                    }
                }
               
            }
            

            // Check Gamepads
            if (Input.XInputHelper.GamePads[PlayerIndex.One].DownPressed || Input.XInputHelper.GamePads[PlayerIndex.One].LeftThumbStick_Down)
            {
                Sound.Sound.Play(eSounds.menu_select2);
                this.FocusIdx++;
            }
            else if (Input.XInputHelper.GamePads[PlayerIndex.One].UpPressed || Input.XInputHelper.GamePads[PlayerIndex.One].LeftThumbStick_Up)
            {
                Sound.Sound.Play(eSounds.menu_select2);
                this.FocusIdx--;
            }
            else if (Input.XInputHelper.GamePads[PlayerIndex.One].StartPressed || Input.XInputHelper.GamePads[PlayerIndex.One].APressed)
            {
                Sound.Sound.Play(eSounds.menu_advance);
                if (ControlSelected != null)
                    ControlSelected(this.mControls[this.mFocusIdx], this.mFocusIdx);
            }
                

            foreach (UIControl ctrl in mControls)
                ctrl.Update(gameTime);
            base.Update(gameTime);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            for(int i=0;i<this.mControls.Count;i++)
            {
                if (this.mControls[i].Visible)
                {
                    this.mControls[i].Focus = i == this.mFocusIdx;
                    this.mControls[i].Draw(gameTime);
                }
            }
            base.Draw(gameTime);
        }

    }
}
