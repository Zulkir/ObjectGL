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

using ObjectGL.Api.PipelineAspects;
using ObjectGL.CachingImpl.PipelineAspects;
using ObjectGL.CachingImpl.Utilitties;

namespace ObjectGL.CachingImpl.ContextAspects
{
    internal class ContextDepthStencilAspect
    {
        private readonly IGL gl;
        private readonly RedundantEnable depthTestEnable;
        private readonly RedundantBool depthMask;
        private readonly RedundantInt depthFunc;
        private readonly RedundantEnable stencilTestEnable;
        private ContextDepthStencilSide front;
        private ContextDepthStencilSide back;

        public ContextDepthStencilAspect(IGL gl)
        {
            this.gl = gl;
            depthTestEnable = new RedundantEnable(gl, EnableCap.DepthTest);
            depthMask = new RedundantBool(gl, (g, x) => g.DepthMask(x));
            depthFunc = new RedundantInt(gl, (g, x) => g.DepthFunc(x));
            stencilTestEnable = new RedundantEnable(gl, EnableCap.StencilTest);
            front.Initialize();
            back.Initialize();
        }

        public void SetDepthMask(bool mask)
        {
            depthMask.Set(mask);
        }

        public void ConsumePipelineDepthStencil(PipelineDepthStencilAspect pipelineDepthStencil)
        {
            // todo: ignore fields when disabled

            depthTestEnable.Set(pipelineDepthStencil.DepthTestEnable);
            depthMask.Set(pipelineDepthStencil.DepthMask);
            depthFunc.Set((int)pipelineDepthStencil.DepthFunc);
            stencilTestEnable.Set(pipelineDepthStencil.StencilTestEnable);

            ConsumeSide(All.Front, ref front, pipelineDepthStencil.Front);
            ConsumeSide(All.Back, ref back, pipelineDepthStencil.Back);
        }

        private void ConsumeSide(All face, ref ContextDepthStencilSide side, IPipelineDepthStencilSide pipelineSide)
        {
            if (side.StencilWriteMask != pipelineSide.StencilWriteMask)
            {
                gl.StencilMaskSeparate((int)face, (uint)pipelineSide.StencilWriteMask);
                side.StencilWriteMask = pipelineSide.StencilWriteMask;
            }

            if (side.StencilFuncMask != pipelineSide.StencilFuncMask ||
                side.StencilFunc != pipelineSide.StencilFunc ||
                side.StencilRef != pipelineSide.StencilRef)
            {
                gl.StencilFuncSeparate((int)face, (int)pipelineSide.StencilFunc, pipelineSide.StencilRef, (uint)pipelineSide.StencilFuncMask);
                side.StencilFuncMask = pipelineSide.StencilFuncMask;
                side.StencilFunc = pipelineSide.StencilFunc;
                side.StencilRef = pipelineSide.StencilRef;
            }

            if (side.StencilFailOp != pipelineSide.StencilFailOp ||
                side.DepthFailOp != pipelineSide.DepthFailOp ||
                side.DepthPassOp != pipelineSide.DepthPassOp)
            {
                gl.StencilOpSeparate((int)face, (int)pipelineSide.StencilFailOp, (int)pipelineSide.DepthFailOp, (int)pipelineSide.DepthPassOp);
                side.StencilFailOp = pipelineSide.StencilFailOp;
                side.DepthFailOp = pipelineSide.DepthFailOp;
                side.DepthPassOp = pipelineSide.DepthPassOp;
            }
        }
    }
}
