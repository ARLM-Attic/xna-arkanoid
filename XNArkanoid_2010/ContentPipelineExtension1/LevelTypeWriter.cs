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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;


using TWrite = System.String;

namespace XNArkanoid.Descs
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class ContentTypeWriter1 : ContentTypeWriter<LevelDesc>
    {
        protected override void Write(ContentWriter output, LevelDesc value)
        {
            output.Write(value.FrameAssetName);
            output.Write(value.PatternAssetName);
            foreach (BrickDesc desc in value.Bricks)
            {
                output.WriteObject<BrickDesc>(desc);
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "XNArkanoid.TypeReaders.LevelTypeReader, XNArkanoid";
        }
    }
}
