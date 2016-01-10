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
    internal class ContextScissorBoxesAspect
    {
        readonly IGL gl;
        readonly ContextScissorBox[] scissorBoxes;

        public ContextScissorBoxesAspect(IGL gl, Implementation implementation)
        {
            this.gl = gl;
            scissorBoxes = new ContextScissorBox[implementation.MaxViewports];
            for (int i = 0; i < scissorBoxes.Length; i++)
                scissorBoxes[0].Initialize();
        }

        public void ConsumePipelineScissorBoxes(PipelineScissorBoxesAspect pipelineScissorBoxes, PipelineViewportsAspect pipelineViewports)
        {
            if (pipelineViewports.EnabledViewportCount == 1)
            {
                if (ContextScissorBox.ConsumePipelineViewport(ref scissorBoxes[0], pipelineScissorBoxes[0]))
                    gl.Scissor(scissorBoxes[0].X, scissorBoxes[0].Y, scissorBoxes[0].Width, scissorBoxes[0].Height);
            }
            else
            {
                for (uint i = 0; i < pipelineViewports.EnabledViewportCount; i++)
                {
                    if (ContextScissorBox.ConsumePipelineViewport(ref scissorBoxes[i], pipelineScissorBoxes[i]))
                        gl.ScissorIndexed(i, scissorBoxes[i].X, scissorBoxes[i].Y, scissorBoxes[i].Width, scissorBoxes[i].Height);
                }
            }
        }
    }
}
