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
        private class DepthStencilAspect
        {
            private struct Side
            {
                public int StencilWriteMask;
                public int StencilFuncMask;
                public StencilFunction StencilFunc;
                public int StencilRef;
                public StencilOp StencilFailOp;
                public StencilOp DepthFailOp;
                public StencilOp DepthPassOp;

                public void Initialize()
                {
                    StencilWriteMask = 1;
                    StencilFunc = StencilFunction.Always;
                    StencilRef = 0;
                    StencilFuncMask = 1;
                    StencilFailOp = StencilOp.Keep;
                    DepthFailOp = StencilOp.Keep;
                    DepthPassOp = StencilOp.Keep;
                }
            }

            readonly RedundantEnable depthTestEnable = new RedundantEnable(EnableCap.DepthTest);
            readonly RedundantBool depthMask = new RedundantBool(GL.DepthMask);
            readonly RedundantInt depthFunc = new RedundantInt(x => GL.DepthFunc((DepthFunction)x));
            readonly RedundantEnable stencilTestEnable = new RedundantEnable(EnableCap.StencilTest);
            Side front = new Side();
            Side back = new Side();

            public DepthStencilAspect()
            {
                front.Initialize();
                back.Initialize();
            }

            public void SetDepthMask(bool mask)
            {
                depthMask.Set(mask);
            }

            public void ConsumePipelineDepthStencil(Pipeline.DepthStencilAspect pipelineDepthStencil)
            {
                // todo: ignore fields when disabled

                depthTestEnable.Set(pipelineDepthStencil.DepthTestEnable);
                depthMask.Set(pipelineDepthStencil.DepthMask);
                depthFunc.Set((int)pipelineDepthStencil.DepthFunc);
                stencilTestEnable.Set(pipelineDepthStencil.StencilTestEnable);

                ConsumeSide(StencilFace.Front, ref front, pipelineDepthStencil.Front);
                ConsumeSide(StencilFace.Back, ref back, pipelineDepthStencil.Back);
            }

            void ConsumeSide(StencilFace face, ref Side side, Pipeline.DepthStencilAspect.Side pipelineSide)
            {
                if (side.StencilWriteMask != pipelineSide.StencilWriteMask)
                {
                    GL.StencilMaskSeparate(face, pipelineSide.StencilWriteMask);
                    side.StencilWriteMask = pipelineSide.StencilWriteMask;
                }

                if (side.StencilFuncMask != pipelineSide.StencilFuncMask ||
                    side.StencilFunc != pipelineSide.StencilFunc ||
                    side.StencilRef != pipelineSide.StencilRef)
                {
                    GL.StencilFuncSeparate((Version20) face, pipelineSide.StencilFunc, pipelineSide.StencilRef, pipelineSide.StencilFuncMask);
                    side.StencilFuncMask = pipelineSide.StencilFuncMask;
                    side.StencilFunc = pipelineSide.StencilFunc;
                    side.StencilRef = pipelineSide.StencilRef;
                }

                if (side.StencilFailOp != pipelineSide.StencilFailOp ||
                    side.DepthFailOp != pipelineSide.DepthFailOp ||
                    side.DepthPassOp != pipelineSide.DepthPassOp)
                {
                    GL.StencilOpSeparate(face, pipelineSide.StencilFailOp, pipelineSide.DepthFailOp, pipelineSide.DepthPassOp);
                    side.StencilFailOp = pipelineSide.StencilFailOp;
                    side.DepthFailOp = pipelineSide.DepthFailOp;
                    side.DepthPassOp = pipelineSide.DepthPassOp;
                }
            }
        }
    }
}
