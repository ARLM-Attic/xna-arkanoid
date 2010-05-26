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
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Xml.Serialization;


namespace XNArkanoid
{
    /// <summary>
    /// The Setting class handles loading and saving of global application settings.
    /// The normal .Net classes (System.Configuration) for doing this are not available on the CF (and therefore 360)
    /// </summary>
    public class Settings
    {

        #region Keyboard Settings
        /// <summary>
        /// Keyboard settings for two players
        /// Note: not allowing extensibility for more than 2 players
        /// </summary>

        // player 1
        public Keys Player1Start = Keys.LeftControl;
        public Keys Player1Back = Keys.LeftShift;
        public Keys Player1A = Keys.V;
        public Keys Player1B = Keys.G;
        public Keys Player1X = Keys.F;
        public Keys Player1Y = Keys.T;
        public Keys Player1ThumbstickLeftXmin = Keys.A;
        public Keys Player1ThumbstickLeftXmax = Keys.D;
        public Keys Player1ThumbstickLeftYmin = Keys.S;
        public Keys Player1ThumbstickLeftYmax = Keys.W;
        public Keys Player1ThumbstickRightXmin = Keys.A;
        public Keys Player1ThumbstickRightXmax = Keys.D;
        public Keys Player1ThumbstickRightYmin = Keys.S;
        public Keys Player1ThumbstickRightYmax = Keys.W;
        public Keys Player1Left = Keys.A;
        public Keys Player1Right = Keys.D;
        public Keys Player1Down = Keys.S;
        public Keys Player1Up = Keys.W;
        public Keys Player1LeftTrigger = Keys.Q;
        public Keys Player1RightTrigger = Keys.E;

        // player 2
        public Keys Player2Start = Keys.RightControl;
        public Keys Player2Back = Keys.RightShift;
        public Keys Player2A = Keys.Home;
        public Keys Player2B = Keys.End;
        public Keys Player2X = Keys.PageUp;
        public Keys Player2Y = Keys.PageDown;
        public Keys Player2ThumbstickLeftXmin = Keys.Left;
        public Keys Player2ThumbstickLeftXmax = Keys.Right;
        public Keys Player2ThumbstickLeftYmin = Keys.Down;
        public Keys Player2ThumbstickLeftYmax = Keys.Up;
        public Keys Player2Left = Keys.Left;
        public Keys Player2Right = Keys.Right;
        public Keys Player2Down = Keys.Down;
        public Keys Player2Up = Keys.Up;
        public Keys Player2LeftTrigger = Keys.Insert;
        public Keys Player2RightTrigger = Keys.Delete;
        #endregion

        public int ScreenWidth;
        public int ScreenHeight;
        public eScreenMode ScreenMode = eScreenMode.Portrait;
        public float ScoreAreaPercent = 0.35f;

        /// <summary>
        /// 
        /// </summary>
        public Settings()
        {

            ScreenHeight = 800;
            ScreenWidth = 480;
        }

        #region Load/Save code
        /// <summary>
        /// Saves the current settings
        /// </summary>
        /// <param name="filename">The filename to save to</param>
        public void Save()
        {
            Stream stream = XNArkanoidGame.mIsolatedStorage.OpenFile("Settings.xml", FileMode.OpenOrCreate);

            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            serializer.Serialize(stream, this);
            stream.Close();
        }

        /// <summary>
        /// Loads settings from a file
        /// </summary>
        /// <param name="filename">The filename to load</param>
        public static Settings Load()
        {
            Settings sett = new Settings();

            if (!XNArkanoidGame.mIsolatedStorage.FileExists("Settings.xml"))
                return sett;
            
            Stream stream = XNArkanoidGame.mIsolatedStorage.OpenFile("Settings.xml", FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            return (Settings)serializer.Deserialize(stream);
        }
        #endregion
    }
}
