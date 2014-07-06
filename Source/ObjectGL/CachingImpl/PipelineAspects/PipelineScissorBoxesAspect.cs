#region License
/*
Copyright (c) 2010-2014 ObjectGL Project - Daniil Rodin

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

using ObjectGL.Api.PipelineAspects;

namespace ObjectGL.CachingImpl.PipelineAspects
{
    internal class PipelineScissorBoxesAspect : IPipelineScissorBoxesAspect
    {
        readonly IPipelineScissorBox[] scissorBoxes;

        public IPipelineScissorBox this[uint viewportIndex]
        {
            get { return scissorBoxes[viewportIndex]; }
        }

        internal unsafe PipelineScissorBoxesAspect(Context context)
        {
            var gl = context.GL;
            var scissorData = new int[4];
            fixed (int* pScissorData = scissorData)
                gl.GetInteger((int)All.ScissorBox, pScissorData);

            scissorBoxes = new IPipelineScissorBox[context.Implementation.MaxViewports];
            for (int i = 0; i < scissorBoxes.Length; i++)
                scissorBoxes[i] = new PipelineScissorBox(scissorData[2], scissorData[3]);
        }
    }
}
