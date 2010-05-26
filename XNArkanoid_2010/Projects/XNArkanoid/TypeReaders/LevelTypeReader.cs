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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// TODO: replace this with the type you want to read.
using TRead = System.String;

namespace XNArkanoid.TypeReaders
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content
    /// Pipeline to read the specified data type from binary .xnb format.
    /// 
    /// Unlike the other Content Pipeline support classes, this should
    /// be a part of your main game project, and not the Content Pipeline
    /// Extension Library project.
    /// </summary>
    public class LevelTypeReader : ContentTypeReader<XNArkanoid.Components.Level>
    {
        protected override XNArkanoid.Components.Level Read(ContentReader input, XNArkanoid.Components.Level existingInstance)
        {
            XNArkanoid.Components.Level lev = new XNArkanoid.Components.Level(State.Game);

            lev.FrameAssetName = input.ReadString();
            lev.PatternAssetName = input.ReadString();


            try
            {
                XNArkanoid.Components.Brick brk = input.ReadObject<XNArkanoid.Components.Brick>();
                while (brk != null)
                {
                    lev.mBricks.Add(brk);

                    try { brk = input.ReadObject<XNArkanoid.Components.Brick>(); }
                    catch (System.Exception ex) { break; }
                }

            }
            catch (System.Exception ex) { }

            return lev;
        }
    }
}
