#region License
/*
Copyright (c) 2012 Daniil Rodin

This software is provided 'as-is', without any express or implied
warranty. In no event will the authors be held liable for any damages
arising from the use of this software.

Permission is granted to anyone to use this software for any purpose,
including commercial applications, and to alter it and redistribute it
freely, subject to the following restrictions:

   1. The origin of this software must not be misrepresented; you must not
   claim that you wrote the original software. If you use this software
   in a product, an acknowledgment in the product documentation would be
   appreciated but is not required.

   2. Altered source versions must be plainly marked as such, and must not be
   misrepresented as being the original software.

   3. This notice may not be removed or altered from any source
   distribution.
*/
#endregion

using OpenTK.Graphics.OpenGL;

namespace ObjectGL.GL42
{
    public partial class Context
    {
        class ScissorBoxesAspect
        {
            #region Nested classes
            private struct ScissorBox
            {
                public int X;
                public int Y;
                public int Width;
                public int Height;

                public void Initialize()
                {
                    Width = Height = -1;
                }

                public static bool ConsumePipelineViewport(ref ScissorBox scissorBox, Pipeline.ScissorBoxesAspect.ScissorBox pipelineScissorBox)
                {
                    if (pipelineScissorBox.X != scissorBox.X || pipelineScissorBox.Y != scissorBox.Y ||
                        pipelineScissorBox.Width != scissorBox.Width || pipelineScissorBox.Height != scissorBox.Height)
                    {
                        scissorBox.X = pipelineScissorBox.X;
                        scissorBox.Y = pipelineScissorBox.Y;
                        scissorBox.Width = pipelineScissorBox.Width;
                        scissorBox.Height = pipelineScissorBox.Height;
                        return true;
                    }
                    return false;
                }
            }
            #endregion

            readonly ScissorBox[] scissorBoxes;

            public ScissorBoxesAspect(Implementation implementation)
            {
                scissorBoxes = new ScissorBox[implementation.MaxViewports];
                for (int i = 0; i < scissorBoxes.Length; i++)
                    scissorBoxes[0].Initialize();
            }

            public void ConsumePipelineScissorBoxes(Pipeline.ScissorBoxesAspect pipelineScissorBoxes, int enabledViewportCount)
            {
                if (enabledViewportCount == 1)
                {
                    if (ScissorBox.ConsumePipelineViewport(ref scissorBoxes[0], pipelineScissorBoxes[0]))
                        GL.Scissor(scissorBoxes[0].X, scissorBoxes[0].Y, scissorBoxes[0].Width, scissorBoxes[0].Height);
                }
                else
                {
                    for (int i = 0; i < enabledViewportCount; i++)
                    {
                        if (ScissorBox.ConsumePipelineViewport(ref scissorBoxes[i], pipelineScissorBoxes[i]))
                            GL.ScissorIndexed(i, scissorBoxes[i].X, scissorBoxes[i].Y, scissorBoxes[i].Width, scissorBoxes[i].Height);
                    }
                }
            }
        }
    }
}
