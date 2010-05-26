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
    public class BrickTypeReader : ContentTypeReader<XNArkanoid.Components.Brick>
    {
        protected override XNArkanoid.Components.Brick Read(ContentReader input, XNArkanoid.Components.Brick existingInstance)
        {
            XNArkanoid.Components.Brick brk = new Components.Brick(State.Game);

            brk.Position = input.ReadVector2();
            brk.Score = input.ReadInt32();
            brk.Hits = input.ReadInt32();
            brk.Prize = (ePrizes)Enum.Parse(typeof(ePrizes), input.ReadString(), false);
            brk.Size = input.ReadVector2();

            // Color comes in a packed form, as UInt32
            uint packedColor = input.ReadUInt32();
            brk.Color = new Color((byte)(packedColor >> 16), (byte)(packedColor >> 8), (byte)(packedColor), (byte)(packedColor >> 24));
            return brk;
        }
    }
}
