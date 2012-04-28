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
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ObjectGL
{
    public partial class Context
    {
        readonly IGraphicsContext nativeContext;

        readonly Capabilities capabilities;
        readonly Pipeline pipeline;

        readonly ProgramAspect program;
        readonly BuffersAspect buffers;
        readonly TexturesAspect textures;
        readonly SamplersAspect samplers;
        readonly FramebufferAspect framebuffers;
        readonly RasterizerAspect rasterizer;
        readonly DepthStencilAspect depthStencil;
        readonly BlendAspect blend;

        public Capabilities Capabilities { get { return capabilities; } }
        public Pipeline Pipeline { get { return pipeline; } }

        public Context(IGraphicsContext nativeContext)
        {
            this.nativeContext = nativeContext;

            GL.Enable(EnableCap.TextureCubeMapSeamless);

            capabilities = new Capabilities();
            pipeline = new Pipeline(this);

            program = new ProgramAspect();
            buffers = new BuffersAspect(capabilities);
            textures = new TexturesAspect(capabilities);
            samplers = new SamplersAspect(capabilities);
            framebuffers = new FramebufferAspect();
            rasterizer = new RasterizerAspect();
            depthStencil = new DepthStencilAspect();
            blend = new BlendAspect(capabilities);
        }

        internal void BindVertexArray(int vertexArrayHandle)
        {
            buffers.BindVertexArray(vertexArrayHandle);
        }

        internal void BindBuffer(BufferTarget target, int bufferHandle)
        {
            buffers.BindBuffer(target, bufferHandle);
        }

        internal void BindTexture(TextureTarget target, int textureHandle)
        {
            textures.BindTexture(target, textureHandle);
        }

        internal void BindRenderbuffer(int renderbufferHandle)
        {
            framebuffers.BindRenderbuffer(renderbufferHandle);
        }

        internal void BindDrawFramebuffer(int framebufferHandle)
        {
            framebuffers.BindDrawFramebuffer(framebufferHandle);
        }

        internal void BindReadFramebuffer(int framebufferHandle)
        {
            framebuffers.BindReadFramebuffer(framebufferHandle);
        }

        internal FramebufferTarget BindAnyFramebuffer(int framebufferHandle)
        {
            return framebuffers.BindAnyFramebuffer(framebufferHandle);
        }

        internal void ConsumePipeline()
        {
            program.ConsumePipelineProgram(pipeline.Program);
            buffers.ConsumePipelineUniformBuffers(pipeline.UniformBuffers);
            buffers.ConsumePipelineTransformFeedbackBuffers(pipeline.TransformFeedbackBuffers);
            buffers.ConsumePipelineVertexArray(pipeline.VertexArray);
            textures.ConsumePipelineTextures(pipeline.Textures);
            samplers.ConsumePipelineSamplers(pipeline.Samplers, pipeline.Textures.EnabledTextureRange);
            framebuffers.ConsumePipelineFramebuffer(pipeline.Framebuffer);
            rasterizer.ConsumePipelineRasterizer(pipeline.Rasterizer);
            depthStencil.ConsumePipelineDepthStencil(pipeline.DepthStencil);
            blend.ConsumePipelineBlend(pipeline.Blend);
        }

        public void SetViewport(int x, int y, int width, int height)
        {
            GL.Viewport(x, y, width, height);
        }

        public unsafe void ClearWindowColor(Color4 color)
        {
            framebuffers.BindDrawFramebuffer(0);
            GL.ClearBuffer(ClearBuffer.Color, 0, (float*)&color);
        }

        public void ClearWindowDepthStencil(DepthStencil mask, float depth, int stencil)
        {
            var clearBuffer =
                mask == DepthStencil.Both ? ClearBuffer.DepthStencil :
                mask == DepthStencil.Depth ? ClearBuffer.Depth :
                mask == DepthStencil.Stencil ? ClearBuffer.Stencil : 0;

            if (clearBuffer == 0)
                throw new ArgumentException("'mask' can not be 'None'");

            framebuffers.BindDrawFramebuffer(0);
            GL.ClearBuffer(clearBuffer, 0, depth, stencil);
        }
    }
}
