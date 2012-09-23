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
        class RasterizerAspect
        {
            PolygonMode polygonModeFront = PolygonMode.Fill;
            PolygonMode polygonModeBack = PolygonMode.Fill;

            readonly RedundantInt cullFace = new RedundantInt(x => GL.CullFace((CullFaceMode)x));
            readonly RedundantInt frontFace = new RedundantInt(x => GL.FrontFace((FrontFaceDirection)x));
            readonly RedundantEnable cullFaceEnable = new RedundantEnable(EnableCap.CullFace);
            readonly RedundantEnable scissorEnable = new RedundantEnable(EnableCap.ScissorTest);
            readonly RedundantEnable multisampleEnble = new RedundantEnable(EnableCap.Multisample);
            readonly RedundantEnable lineSmoothEnable = new RedundantEnable(EnableCap.LineSmooth);

            public void SetScissorEnable(bool enable)
            {
                scissorEnable.Set(enable);
            }

            public void ConsumePipelineRasterizer(Pipeline.RasterizerAspect pipelineRasterizer)
            {
                if (polygonModeFront != pipelineRasterizer.PolygonModeFront && polygonModeBack != pipelineRasterizer.PolygonModeBack)
                {
                    if (pipelineRasterizer.PolygonModeFront == pipelineRasterizer.PolygonModeBack)
                    {
                        GL.PolygonMode(MaterialFace.FrontAndBack, pipelineRasterizer.PolygonModeFront);
                    }
                    else
                    {
                        if (polygonModeFront != pipelineRasterizer.PolygonModeFront)
                            GL.PolygonMode(MaterialFace.Front, pipelineRasterizer.PolygonModeFront);
                        if (polygonModeBack != pipelineRasterizer.PolygonModeBack)
                            GL.PolygonMode(MaterialFace.Back, pipelineRasterizer.PolygonModeBack);
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
}
