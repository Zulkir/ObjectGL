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

using System;
using OpenTK.Graphics.OpenGL;

namespace ObjectGL.v42
{
    public partial class Context
    {
        class ViewportsAspect
        {
            #region Nested classes
            private struct Viewport
            {
                public float X;
                public float Y;
                public float Width;
                public float Height;
                public float Near;
                public float Far;

                public void Initialize()
                {
                    Width = Height = -1f;
                    Far = 1f;
                }

                public static bool ConsumePipelineViewport(ref Viewport viewport, Pipeline.ViewportsAspect.Viewport pipelineViewport)
                {
                    if (pipelineViewport.X != viewport.X || pipelineViewport.Y != viewport.Y ||
                        pipelineViewport.Width != viewport.Width || pipelineViewport.Height != viewport.Height)
                    {
                        viewport.X = pipelineViewport.X;
                        viewport.Y = pipelineViewport.Y;
                        viewport.Width = pipelineViewport.Width;
                        viewport.Height = pipelineViewport.Height;
                        return true;
                    }
                    return false;
                }

                public static bool ConsumePipelineDepthRange(ref Viewport viewport, Pipeline.ViewportsAspect.Viewport pipelineViewport)
                {
                    if (pipelineViewport.Near != viewport.Near || pipelineViewport.Far != viewport.Far)
                    {
                        viewport.Near = pipelineViewport.Near;
                        viewport.Far = pipelineViewport.Far;
                        return true;
                    }
                    return false;
                }

                public static bool IsIntegralViewport(ref Viewport viewport)
                {
                    return
                        viewport.X == Math.Round(viewport.X) &&
                        viewport.Y == Math.Round(viewport.Y) &&
                        viewport.Width == Math.Round(viewport.Width) &&
                        viewport.Height == Math.Round(viewport.Height);
                }
            }
            #endregion

            readonly Viewport[] viewports;

            public ViewportsAspect(Implementation implementation)
            {
                viewports = new Viewport[implementation.MaxViewports];
                for (int i = 0; i < viewports.Length; i++)
                {
                    viewports[0].Initialize();
                }
            }

            public void ConsumePipelineViewports(Pipeline.ViewportsAspect pipelineViewports)
            {
                if (pipelineViewports.EnabledViewportCount == 1)
                {
                    if (Viewport.ConsumePipelineViewport(ref viewports[0], pipelineViewports[0]))
                        if (Viewport.IsIntegralViewport(ref viewports[0]))
                            GL.Viewport((int) viewports[0].X, (int) viewports[0].Y, (int) viewports[0].Width, (int) viewports[0].Height);
                        else
                            GL.ViewportIndexed(0, viewports[0].X, viewports[0].Y, viewports[0].Width, viewports[0].Height);
                    if (Viewport.ConsumePipelineDepthRange(ref viewports[0], pipelineViewports[0]))
                        GL.DepthRange(viewports[0].Near, viewports[0].Far);
                }
                else
                {
                    for (int i = 0; i < pipelineViewports.EnabledViewportCount; i++)
                    {
                        if (Viewport.ConsumePipelineViewport(ref viewports[i], pipelineViewports[i]))
                            GL.ViewportIndexed(i, viewports[i].X, viewports[i].Y, viewports[i].Width, viewports[i].Height);
                        if (Viewport.ConsumePipelineDepthRange(ref viewports[i], pipelineViewports[i]))
                            GL.DepthRangeIndexed(i, viewports[i].Near, viewports[i].Far);
                    }
                }
            }
        }
    }

    
}
