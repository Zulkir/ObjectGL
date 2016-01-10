#region License
/*
Copyright (c) 2012-2016 ObjectGL Project - Daniil Rodin

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

using ObjectGL.CachingImpl.PipelineAspects;

namespace ObjectGL.CachingImpl.ContextAspects
{
    internal class ContextViewportsAspect
    {
        readonly IGL gl;
        readonly ContextViewport[] viewports;

        public ContextViewportsAspect(IGL gl, Implementation implementation)
        {
            this.gl = gl;
            viewports = new ContextViewport[implementation.MaxViewports];
            for (int i = 0; i < viewports.Length; i++)
                viewports[0].Initialize();
        }

        public void ConsumePipelineViewports(PipelineViewportsAspect pipelineViewports)
        {
            if (pipelineViewports.EnabledViewportCount == 1)
            {
                if (ContextViewport.ConsumePipelineViewport(ref viewports[0], pipelineViewports[0]))
                    if (ContextViewport.IsIntegralViewport(ref viewports[0]))
                        gl.Viewport((int)viewports[0].X, (int)viewports[0].Y, (int)viewports[0].Width, (int)viewports[0].Height);
                    else
                        gl.ViewportIndexed(0, viewports[0].X, viewports[0].Y, viewports[0].Width, viewports[0].Height);
                if (ContextViewport.ConsumePipelineDepthRange(ref viewports[0], pipelineViewports[0]))
                    gl.DepthRange(viewports[0].Near, viewports[0].Far);
            }
            else
            {
                for (uint i = 0; i < pipelineViewports.EnabledViewportCount; i++)
                {
                    if (ContextViewport.ConsumePipelineViewport(ref viewports[i], pipelineViewports[i]))
                        gl.ViewportIndexed(i, viewports[i].X, viewports[i].Y, viewports[i].Width, viewports[i].Height);
                    if (ContextViewport.ConsumePipelineDepthRange(ref viewports[i], pipelineViewports[i]))
                        gl.DepthRangeIndexed(i, viewports[i].Near, viewports[i].Far);
                }
            }
        }
    }
}
