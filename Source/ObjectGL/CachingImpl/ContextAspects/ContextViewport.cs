#region License
/*
Copyright (c) 2012-2014 ObjectGL Project - Daniil Rodin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion

using System;
using ObjectGL.Api.PipelineAspects;

namespace ObjectGL.CachingImpl.ContextAspects
{
    internal struct ContextViewport
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

        public static bool ConsumePipelineViewport(ref ContextViewport viewport, IPipelineViewport pipelineViewport)
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

        public static bool ConsumePipelineDepthRange(ref ContextViewport viewport, IPipelineViewport pipelineViewport)
        {
            if (pipelineViewport.Near != viewport.Near || pipelineViewport.Far != viewport.Far)
            {
                viewport.Near = pipelineViewport.Near;
                viewport.Far = pipelineViewport.Far;
                return true;
            }
            return false;
        }

        public static bool IsIntegralViewport(ref ContextViewport viewport)
        {
            return
                viewport.X == Math.Round(viewport.X) &&
                viewport.Y == Math.Round(viewport.Y) &&
                viewport.Width == Math.Round(viewport.Width) &&
                viewport.Height == Math.Round(viewport.Height);
        }
    }
}