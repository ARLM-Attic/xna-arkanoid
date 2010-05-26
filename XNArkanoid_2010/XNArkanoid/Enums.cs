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

namespace XNArkanoid
{
    public enum eGameState
    {
        None,
        Loading,
        Paused,
        Running,
        DialogMain,
        DialogContinue,
    }

    public enum eScreenMode
    {
        Portrait,
        Landscape,
    }

    public enum ePrizes
    {
        None,
        BreakToNextLevel,
        CatchBall,
        DisruptionIntoThreeBalls,
        ExpandVaus,
        Laser,
        PlayerExtend,
        SpeedDown,
    }
   
    public enum eBallState
    {
        Normal,
        Dying,
        Dead,
    }

    public enum eVausState
    {
        Normal,
        Laser,
        Catcher,
        BiggerVaus,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum eSounds
    {
        CoinUp,
        Death,
        Hit1,
        Hit2,
        Hit3,
        Hit4,
        Intro,
        NewLevel,
        menu_advance,
        menu_select,
        menu_select_1,
        menu_select_4,
        menu_back,
        menu_badselect,
        menu_scroll,
        menu_select2,
        menu_select3,
        GameOver,
    }

}
