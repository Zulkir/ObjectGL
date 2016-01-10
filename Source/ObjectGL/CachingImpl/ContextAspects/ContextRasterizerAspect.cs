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

using ObjectGL.Api.Context;
using ObjectGL.CachingImpl.PipelineAspects;
using ObjectGL.CachingImpl.Utilitties;

namespace ObjectGL.CachingImpl.ContextAspects
{
    internal class ContextRasterizerAspect
    {
        private readonly IGL gl;

        private PolygonMode polygonModeFront = PolygonMode.Fill;
        private PolygonMode polygonModeBack = PolygonMode.Fill;

        private readonly RedundantInt cullFace;
        private readonly RedundantInt frontFace;
        private readonly RedundantEnable cullFaceEnable;
        private readonly RedundantEnable scissorEnable;
        private readonly RedundantEnable multisampleEnble;
        private readonly RedundantEnable lineSmoothEnable;

        public ContextRasterizerAspect(IGL gl)
        {
            this.gl = gl;
            cullFace = new RedundantInt(gl, (g, x) => g.CullFace(x));
            frontFace = new RedundantInt(gl, (g, x) => g.FrontFace(x));
            cullFaceEnable = new RedundantEnable(gl, EnableCap.CullFace);
            scissorEnable = new RedundantEnable(gl, EnableCap.ScissorTest);
            multisampleEnble = new RedundantEnable(gl, EnableCap.Multisample);
            lineSmoothEnable = new RedundantEnable(gl, EnableCap.LineSmooth);
        }

        public void SetScissorEnable(bool enable)
        {
            scissorEnable.Set(enable);
        }

        public void ConsumePipelineRasterizer(PipelineRasterizerAspect pipelineRasterizer)
        {
            if (polygonModeFront != pipelineRasterizer.PolygonModeFront && polygonModeBack != pipelineRasterizer.PolygonModeBack)
            {
                if (pipelineRasterizer.PolygonModeFront == pipelineRasterizer.PolygonModeBack)
                {
                    gl.PolygonMode((int)All.FrontAndBack, (int)pipelineRasterizer.PolygonModeFront);
                }
                else
                {
                    if (polygonModeFront != pipelineRasterizer.PolygonModeFront)
                        gl.PolygonMode((int)All.Front, (int)pipelineRasterizer.PolygonModeFront);
                    if (polygonModeBack != pipelineRasterizer.PolygonModeBack)
                        gl.PolygonMode((int)All.Back, (int)pipelineRasterizer.PolygonModeBack);
                }

                polygonModeFront = pipelineRasterizer.PolygonModeFront;
                polygonModeBack = pipelineRasterizer.PolygonModeBack;
            }

            cullFace.Set((int)pipelineRasterizer.CullFace);
            frontFace.Set((int)pipelineRasterizer.FrontFace);
            cullFaceEnable.Set(pipelineRasterizer.CullFaceEnable);
            scissorEnable.Set(pipelineRasterizer.ScissorEnable);
            multisampleEnble.Set(pipelineRasterizer.MultisampleEnable);
            lineSmoothEnable.Set(pipelineRasterizer.LineSmoothEnable);
        }
    }
}
