/* --------------------------------------------------------------------------------------------------------
 * Author: Iñaki Ayucar (http://graphicdna.blogspot.com)
 * Date: 1/05/2010
 * 
 * This software is distributed "for free" for any non-commercial usage. The software is provided “as-is.” 
 * You bear the risk of using it. The author give no express warranties, guarantees or conditions.
 * If you use this software in another project, you agree to mention the author and source.
 ---------------------------------------------------------------------------------------------------------*/
#region Dependencies
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
#endregion

namespace XNArkanoid.Input
{
    /// <summary>
    /// Useful class that wraps some game pad stuff to give you indication of single button presses
    /// by remembering previous state. Right now its one shot which means if you call a Pressed function 
    /// that will 'remove' the press.
    /// 
    /// Keyboard support should be mapped in here based on PlayerIndex.
    /// PlayerIndex.One Key mapping is (Keys for player one to use)
    /// PlayerIndex.Two Key mapping is (Keys for Player two to use)
    /// Players Three => Infinity are not supported on a keyboard!
    /// </summary>
    public enum GamePadKeys
    {
        Start = 0,
        Back,
        A,
        B,
        X,
        Y,
        Up,
        Down,
        Left,
        Right,
        LeftTrigger,
        RightTrigger,
        ThumbstickLeftXMin,
        ThumbstickLeftXMax,
        ThumbstickLeftYMin,
        ThumbstickLeftYMax,
        ThumbstickRightXMin,
        ThumbstickRightXMax,
        ThumbstickRightYMin,
        ThumbstickRightYMax
    };

    public class GamePadHelper
    {
        private PlayerIndex player;
        private Keymap keyMapping = new Keymap();
        private KeyboardState keyState;
        private Game game;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="player">Which player. Not that you can't use PlayerIndex.Any with this helper</param>
        public GamePadHelper(PlayerIndex player)
        {
            //Need to store the player. If you try to store a reference to the GamePad here it seems to 'forget'
            this.player = player;

            if (player == PlayerIndex.One)
            {
                keyMapping.Add(GamePadKeys.Start, XNArkanoidGame.Settings.Player1Start);
                keyMapping.Add(GamePadKeys.Back, XNArkanoidGame.Settings.Player1Back);
                keyMapping.Add(GamePadKeys.A, XNArkanoidGame.Settings.Player1A);
                keyMapping.Add(GamePadKeys.B, XNArkanoidGame.Settings.Player1B);
                keyMapping.Add(GamePadKeys.X, XNArkanoidGame.Settings.Player1X);
                keyMapping.Add(GamePadKeys.Y, XNArkanoidGame.Settings.Player1Y);
                keyMapping.Add(GamePadKeys.Up, XNArkanoidGame.Settings.Player1Up);
                keyMapping.Add(GamePadKeys.Down, XNArkanoidGame.Settings.Player1Down);
                keyMapping.Add(GamePadKeys.Left, XNArkanoidGame.Settings.Player1Left);
                keyMapping.Add(GamePadKeys.Right, XNArkanoidGame.Settings.Player1Right);
                keyMapping.Add(GamePadKeys.LeftTrigger, XNArkanoidGame.Settings.Player1LeftTrigger);
                keyMapping.Add(GamePadKeys.RightTrigger, XNArkanoidGame.Settings.Player1RightTrigger);
                keyMapping.Add(GamePadKeys.ThumbstickLeftXMin, XNArkanoidGame.Settings.Player1ThumbstickLeftXmin);
                keyMapping.Add(GamePadKeys.ThumbstickLeftXMax, XNArkanoidGame.Settings.Player1ThumbstickLeftXmax);
                keyMapping.Add(GamePadKeys.ThumbstickLeftYMin, XNArkanoidGame.Settings.Player1ThumbstickLeftYmin);
                keyMapping.Add(GamePadKeys.ThumbstickLeftYMax, XNArkanoidGame.Settings.Player1ThumbstickLeftYmax);
                keyMapping.Add(GamePadKeys.ThumbstickRightXMin, XNArkanoidGame.Settings.Player1ThumbstickRightXmin);
                keyMapping.Add(GamePadKeys.ThumbstickRightXMax, XNArkanoidGame.Settings.Player1ThumbstickRightXmax);
                keyMapping.Add(GamePadKeys.ThumbstickRightYMin, XNArkanoidGame.Settings.Player1ThumbstickRightYmin);
                keyMapping.Add(GamePadKeys.ThumbstickRightYMax, XNArkanoidGame.Settings.Player1ThumbstickRightYmax);

            }

            if (player == PlayerIndex.Two)
            {
                keyMapping.Add(GamePadKeys.Start, XNArkanoidGame.Settings.Player2Start);
                keyMapping.Add(GamePadKeys.Back, XNArkanoidGame.Settings.Player2Back);
                keyMapping.Add(GamePadKeys.A, XNArkanoidGame.Settings.Player2A);
                keyMapping.Add(GamePadKeys.B, XNArkanoidGame.Settings.Player2B);
                keyMapping.Add(GamePadKeys.X, XNArkanoidGame.Settings.Player2X);
                keyMapping.Add(GamePadKeys.Y, XNArkanoidGame.Settings.Player2Y);
                keyMapping.Add(GamePadKeys.Up, XNArkanoidGame.Settings.Player2Up);
                keyMapping.Add(GamePadKeys.Down, XNArkanoidGame.Settings.Player2Down);
                keyMapping.Add(GamePadKeys.Left, XNArkanoidGame.Settings.Player2Left);
                keyMapping.Add(GamePadKeys.Right, XNArkanoidGame.Settings.Player2Right);
                keyMapping.Add(GamePadKeys.LeftTrigger, XNArkanoidGame.Settings.Player2LeftTrigger);
                keyMapping.Add(GamePadKeys.RightTrigger, XNArkanoidGame.Settings.Player2RightTrigger);
                keyMapping.Add(GamePadKeys.ThumbstickLeftXMin, XNArkanoidGame.Settings.Player2ThumbstickLeftXmin);
                keyMapping.Add(GamePadKeys.ThumbstickLeftXMax, XNArkanoidGame.Settings.Player2ThumbstickLeftXmax);
                keyMapping.Add(GamePadKeys.ThumbstickLeftYMin, XNArkanoidGame.Settings.Player2ThumbstickLeftYmin);
                keyMapping.Add(GamePadKeys.ThumbstickLeftYMax, XNArkanoidGame.Settings.Player2ThumbstickLeftYmax);
            }
        }

        public GamePadState State
        {
            get
            {
                return state;
            }
        }

        private bool AWasReleased;
        private bool BWasReleased;
        private bool YWasReleased;
        private bool XWasReleased;
        private bool StartWasReleased;
        private bool BackWasReleased;
        private bool UpWasReleased;
        private bool DownWasReleased;
        private bool LeftWasReleased;
        private bool RightWasReleased;
        private bool LeftTriggerWasReleased;
        private bool RightTriggerWasReleased;
        // Ive added this vars to allow thumb stick to be read as buttons
        private bool ThumbStickLeftUpWasReleased;
        private bool ThumbStickLeftDownWasReleased;
        private bool ThumbStickLeftLeftWasReleased;
        private bool ThumbStickLeftRightWasReleased;
        private bool ThumbStickRightUpWasReleased;
        private bool ThumbStickRightDownWasReleased;
        private bool ThumbStickRightLeftWasReleased;
        private bool ThumbStickRightRightWasReleased;


        private bool kbAWasReleased;
        private bool kbBWasReleased;
        private bool kbYWasReleased;
        private bool kbXWasReleased;
        private bool kbStartWasReleased;
        private bool kbBackWasReleased;
        private bool kbUpWasReleased;
        private bool kbDownWasReleased;
        private bool kbLeftWasReleased;
        private bool kbRightWasReleased;
        private bool kbLeftTriggerWasReleased;
        private bool kbRightTriggerWasReleased;

        private GamePadState state;

        public float ThumbStickLeftX
        {
            get
            {
                float result = 0.0f;
                if (state.IsConnected)
                    result = state.ThumbSticks.Left.X;
                if (keyState.IsKeyDown(keyMapping.Get(GamePadKeys.ThumbstickLeftXMin)))
                    result = -1.0f;
                if (keyState.IsKeyDown(keyMapping.Get(GamePadKeys.ThumbstickLeftXMax)))
                    result = 1.0f;
                return result;
            }
        }

        public float ThumbStickLeftY
        {
            get
            {
                float result = 0.0f;
                if (state.IsConnected)
                    result = state.ThumbSticks.Left.Y;
                if (keyState.IsKeyDown(keyMapping.Get(GamePadKeys.ThumbstickLeftYMin)))
                    result = -1.0f;
                if (keyState.IsKeyDown(keyMapping.Get(GamePadKeys.ThumbstickLeftYMax)))
                    result = 1.0f;
                return result;
            }
        }
        /// <summary>
        /// Ive added this property to allow read sticks as buttons
        /// </summary>
        public bool LeftThumbStick_Up
        {
            get
            {
                return this.checkPressed(state.ThumbSticks.Left.Y, ref this.ThumbStickLeftUpWasReleased);
            }
        }
        public bool LeftThumbStick_Down
        {
            get
            {
                return this.checkPressedNegative(state.ThumbSticks.Left.Y, ref this.ThumbStickLeftDownWasReleased);
            }
        }
        public bool LeftThumbStick_Left
        {
            get
            {
                return this.checkPressedNegative(state.ThumbSticks.Left.X, ref this.ThumbStickLeftLeftWasReleased);
            }
        }
        public bool LeftThumbStick_Right
        {
            get
            {
                return this.checkPressed(state.ThumbSticks.Left.X, ref this.ThumbStickLeftRightWasReleased);
            }
        }

        public bool RightThumbStick_Up
        {
            get
            {
                return this.checkPressed(state.ThumbSticks.Right.Y, ref this.ThumbStickRightUpWasReleased);
            }
        }
        public bool RightThumbStick_Down
        {
            get
            {
                return this.checkPressedNegative(state.ThumbSticks.Right.Y, ref this.ThumbStickRightDownWasReleased);
            }
        }
        public bool RightThumbStick_Left
        {
            get
            {
                return this.checkPressedNegative(state.ThumbSticks.Right.X, ref this.ThumbStickRightLeftWasReleased);
            }
        }
        public bool RightThumbStick_Right
        {
            get
            {
                return this.checkPressed(state.ThumbSticks.Right.X, ref this.ThumbStickRightRightWasReleased);
            }
        }



        public float ThumbStickRightX
        {
            get
            {
                float result = 0.0f;
                if (state.IsConnected)
                    result = state.ThumbSticks.Right.X;
                if (keyState.IsKeyDown(keyMapping.Get(GamePadKeys.ThumbstickRightXMin)))
                    result = -1.0f;
                if (keyState.IsKeyDown(keyMapping.Get(GamePadKeys.ThumbstickRightXMax)))
                    result = 1.0f;
                return result;
            }
        }

        public float ThumbStickRightY
        {
            get
            {
                float result = 0.0f;
                if (state.IsConnected)
                    result = state.ThumbSticks.Right.Y;
                if (keyState.IsKeyDown(keyMapping.Get(GamePadKeys.ThumbstickRightYMin)))
                    result = -1.0f;
                if (keyState.IsKeyDown(keyMapping.Get(GamePadKeys.ThumbstickRightYMax)))
                    result = 1.0f;
                return result;
            }
        }
        /// <summary>
        /// Has the left trigger been pressed
        /// </summary>
        public bool LeftTriggerPressed
        {
            get
            {
                return ((checkPressed(state.Triggers.Left, ref LeftTriggerWasReleased))
                     || (checkPressed(keyState.IsKeyDown(keyMapping.Get(GamePadKeys.LeftTrigger)), ref kbLeftTriggerWasReleased)));
            }
        }

        /// <summary>
        /// Has the right trigger been pressed
        /// </summary>
        public bool RightTriggerPressed
        {
            get
            {
                return ((checkPressed(state.Triggers.Right, ref RightTriggerWasReleased))
                     || (checkPressed(keyState.IsKeyDown(keyMapping.Get(GamePadKeys.RightTrigger)), ref kbRightTriggerWasReleased)));
            }
        }

        /// <summary>
        /// Has the A button been pressed
        /// </summary>
        public bool APressed
        {
            get
            {
                return ((checkPressed(state.Buttons.A, ref AWasReleased))
                     || (checkPressed(keyState.IsKeyDown(keyMapping.Get(GamePadKeys.A)), ref kbAWasReleased)));
            }
        }

        /// <summary>
        /// Has the B button been pressed
        /// </summary>
        public bool BPressed
        {
            get
            {
                return ((checkPressed(state.Buttons.B, ref BWasReleased))
                     || (checkPressed(keyState.IsKeyDown(keyMapping.Get(GamePadKeys.B)), ref kbBWasReleased)));
            }
        }

        /// <summary>
        /// Has the Y button been pressed
        /// </summary>
        public bool YPressed
        {
            get
            {
                return ((checkPressed(state.Buttons.Y, ref YWasReleased))
                     || (checkPressed(keyState.IsKeyDown(keyMapping.Get(GamePadKeys.Y)), ref kbYWasReleased)));
            }
        }

        /// <summary>
        /// Has the X button been pressed
        /// </summary>
        public bool XPressed
        {
            get
            {
                return ((checkPressed(state.Buttons.X, ref XWasReleased))
                     || (checkPressed(keyState.IsKeyDown(keyMapping.Get(GamePadKeys.X)), ref kbXWasReleased)));
            }
        }

        /// <summary>
        /// Has the start button been pressed
        /// </summary>
        public bool StartPressed
        {
            get
            {
                return ((checkPressed(state.Buttons.Start, ref StartWasReleased))
                     || (checkPressed(keyState.IsKeyDown(keyMapping.Get(GamePadKeys.Start)), ref kbStartWasReleased)));
            }
        }

        /// <summary>
        /// Has the back button been pressed
        /// </summary>
        public bool BackPressed
        {
            get
            {
                return ((checkPressed(state.Buttons.Back, ref BackWasReleased))
                     || (checkPressed(keyState.IsKeyDown(keyMapping.Get(GamePadKeys.Back)), ref kbBackWasReleased)));
            }
        }

        /// <summary>
        /// Has the up dpad been pressed
        /// </summary>
        public bool UpPressed
        {
            get
            {
                return ((checkPressed(state.DPad.Up, ref UpWasReleased))
                     || (checkPressed(keyState.IsKeyDown(keyMapping.Get(GamePadKeys.Up)), ref kbUpWasReleased)));
            }
        }

        /// <summary>
        /// Has the down dpad been pressed
        /// </summary>
        public bool DownPressed
        {
            get
            {
                return ((checkPressed(state.DPad.Down, ref DownWasReleased))
                     || (checkPressed(keyState.IsKeyDown(keyMapping.Get(GamePadKeys.Down)), ref kbDownWasReleased)));
            }
        }

        /// <summary>
        /// Has the left dpad been pressed
        /// </summary>
        public bool LeftPressed
        {
            get
            {
                return ((checkPressed(state.DPad.Left, ref LeftWasReleased))
                     || (checkPressed(keyState.IsKeyDown(keyMapping.Get(GamePadKeys.Left)), ref kbLeftWasReleased)));
            }
        }      

        /// <summary>
        /// Has the right dpad been pressed
        /// </summary>
        public bool RightPressed
        {
            get
            {
                return ((checkPressed(state.DPad.Right, ref RightWasReleased))
                     || (checkPressed(keyState.IsKeyDown(keyMapping.Get(GamePadKeys.Right)), ref kbRightWasReleased)));
            }
        }
        /// <summary>
        /// Ive added this property to allow a continous press detection (no need to release)
        /// </summary>
        public bool LeftPressedContinous
        {
            get
            {
                return state.DPad.Left == ButtonState.Pressed;
            }
        }
        /// <summary>
        /// Ive added this property to allow a continous press detection (no need to release)
        /// </summary>
        public bool RightPressedContinous
        {
            get
            {
                return state.DPad.Right == ButtonState.Pressed;
            }
        }
        /// <summary>
        /// Ive added this property to allow a continous press detection (no need to release)
        /// </summary>
        public bool UpPressedContinous
        {
            get
            {
                return state.DPad.Up == ButtonState.Pressed;
            }
        }
        /// <summary>
        /// Ive added this property to allow a continous press detection (no need to release)
        /// </summary>
        public bool DownPressedContinous
        {
            get
            {
                return state.DPad.Down == ButtonState.Pressed;
            }
        }

        private bool checkPressed(ButtonState buttonState, ref bool controlWasReleased)
        {
            //Buttons are considered pressed when their state = Pressed
            return checkPressed(buttonState == ButtonState.Pressed, ref controlWasReleased);
        }

        private bool checkPressed(float triggerState, ref bool controlWasReleased)
        {
            //Triggers are considered pressed when their value >0
            return checkPressed(triggerState > 0, ref controlWasReleased);
        }
        /// <summary>
        /// Ive added tihs method to allow thumbstick to be read as buttons
        /// </summary>
        /// <param name="triggerState"></param>
        /// <param name="controlWasReleased"></param>
        /// <returns></returns>
        private bool checkPressedNegative(float triggerState, ref bool controlWasReleased)
        {
            //Triggers are considered pressed when their value >0
            return checkPressed(triggerState < 0, ref controlWasReleased);
        }

        private bool checkPressed(bool pressed, ref bool controlWasReleased)
        {
            bool returnValue = controlWasReleased && pressed;
            if (game.IsActive)
            {
                //If the item is currently pressed then reset the 'released' indicators
                if (returnValue)
                {
                    controlWasReleased = false;
                }
            }
            else
            {
                return false;  // Control can never be pressed, game is not the active application!
            }

            return returnValue;
        }

        /// <summary>
        /// Updates the states. Should be called once per frame in the game loop otherwise the IsPressed functions
        /// won't work
        /// </summary>
        public void Update(Game game, KeyboardState keyState)
        {
            state = GamePad.GetState(player);
            this.keyState = keyState;
            this.game = game;

            if (player == PlayerIndex.One)
            {
                //Check which buttons have been released so we can detect presses
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player1A)) kbAWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player1B)) kbBWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player1Y)) kbYWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player1X)) kbXWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player1Start)) kbStartWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player1Back)) kbBackWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player1Up)) kbUpWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player1Down)) kbDownWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player1Left)) kbLeftWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player1Right)) kbRightWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player1LeftTrigger)) kbLeftTriggerWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player1RightTrigger)) kbRightTriggerWasReleased = true;
            }
            else
            {
                //Check which buttons have been released so we can detect presses
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player2A)) kbAWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player2B)) kbBWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player2Y)) kbYWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player2X)) kbXWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player2Start)) kbStartWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player2Back)) kbBackWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player2Up)) kbUpWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player2Down)) kbDownWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player2Left)) kbLeftWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player2Right)) kbRightWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player2LeftTrigger)) kbLeftTriggerWasReleased = true;
                if (keyState.IsKeyUp(XNArkanoidGame.Settings.Player2RightTrigger)) kbRightTriggerWasReleased = true;
            }

            if (state.IsConnected)
            {
                //Check which buttons have been released so we can detect presses
                if (state.Buttons.A == ButtonState.Released) AWasReleased = true;
                if (state.Buttons.B == ButtonState.Released) BWasReleased = true;
                if (state.Buttons.Y == ButtonState.Released) YWasReleased = true;
                if (state.Buttons.X == ButtonState.Released) XWasReleased = true;
                if (state.Buttons.Start == ButtonState.Released) StartWasReleased = true;
                if (state.Buttons.Back == ButtonState.Released) BackWasReleased = true;
                if (state.DPad.Up == ButtonState.Released) UpWasReleased = true;
                if (state.DPad.Down == ButtonState.Released) DownWasReleased = true;
                if (state.DPad.Left == ButtonState.Released) LeftWasReleased = true;
                if (state.DPad.Right == ButtonState.Released) RightWasReleased = true;
                if (state.Triggers.Left == 0f) LeftTriggerWasReleased = true;
                if (state.Triggers.Right == 0f) RightTriggerWasReleased = true;
                // Ive added this code to allow thumbsticks to be read as buttons
                if (state.ThumbSticks.Left.Y == 0f) { ThumbStickLeftUpWasReleased = true; ThumbStickLeftDownWasReleased = true; }
                if (state.ThumbSticks.Left.X == 0f) { ThumbStickLeftLeftWasReleased = true; ThumbStickLeftRightWasReleased = true; }
                if (state.ThumbSticks.Right.Y == 0f) { ThumbStickRightUpWasReleased = true; ThumbStickRightDownWasReleased = true; }
                if (state.ThumbSticks.Right.X == 0f) { ThumbStickRightLeftWasReleased = true; ThumbStickRightRightWasReleased = true; }

            }
        }
    }
}
