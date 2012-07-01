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

namespace ObjectGL.GL42
{
    public partial class Context
    {
        readonly IGraphicsContext nativeContext;

        readonly Implementation implementation;
        readonly Pipeline pipeline;

        readonly ProgramAspect programAspect;
        readonly BuffersAspect buffersAspect;
        readonly TexturesAspect texturesAspect;
        readonly SamplersAspect samplersAspect;
        readonly FramebufferAspect framebuffersAspect;
        readonly ViewportsAspect viewportsAspect;
        readonly RasterizerAspect rasterizerAspect;
        readonly DepthStencilAspect depthStencilAspect;
        readonly BlendAspect blendAspect;

        public Implementation Implementation { get { return implementation; } }
        public Pipeline Pipeline { get { return pipeline; } }

        public unsafe Context(IGraphicsContext nativeContext)
        {
            if (nativeContext == null)
                throw new ArgumentNullException("nativeContext");

            this.nativeContext = nativeContext;
            /*
            int major, minor;
            GL.GetInteger(GetPName.MajorVersion, &major);
            GL.GetInteger(GetPName.MinorVersion, &minor);

            if (major < 4 || (major == 4 && minor < 2))
                throw new NotSupportedException(
                    string.Format("OpengGL 4.2 support is required for ObjectGL v42, but the current OpenGL implementation is only {0}.{1}", 
                    major, minor));*/

            GL.Enable(EnableCap.TextureCubeMapSeamless);

            implementation = new Implementation();
            pipeline = new Pipeline(this);

            programAspect = new ProgramAspect();
            buffersAspect = new BuffersAspect(implementation);
            texturesAspect = new TexturesAspect(implementation);
            samplersAspect = new SamplersAspect(implementation);
            framebuffersAspect = new FramebufferAspect();
            viewportsAspect = new ViewportsAspect(implementation);
            rasterizerAspect = new RasterizerAspect();
            depthStencilAspect = new DepthStencilAspect();
            blendAspect = new BlendAspect(implementation);
        }

        #region Bind
        internal void UseProgram(ShaderProgram program)
        {
            programAspect.UseProgram(program);
        }

        internal void BindVertexArray(VertexArray vertexArray)
        {
            buffersAspect.BindVertexArray(vertexArray);
        }

        internal void BindBuffer(BufferTarget target, Buffer buffer)
        {
            buffersAspect.BindBuffer(target, buffer);
        }

        internal void BindTexture(TextureTarget target, Texture texture)
        {
            texturesAspect.BindTexture(target, texture);
        }

        internal void BindRenderbuffer(Renderbuffer renderbuffer)
        {
            framebuffersAspect.BindRenderbuffer(renderbuffer);
        }

        internal void BindDrawFramebuffer(Framebuffer framebuffer)
        {
            framebuffersAspect.BindDrawFramebuffer(framebuffer);
        }

        internal void BindReadFramebuffer(Framebuffer framebuffer)
        {
            framebuffersAspect.BindReadFramebuffer(framebuffer);
        }

        internal FramebufferTarget BindAnyFramebuffer(Framebuffer framebuffer)
        {
            return framebuffersAspect.BindAnyFramebuffer(framebuffer);
        }
        #endregion

        internal void ConsumePipeline()
        {
            programAspect.ConsumePipelineProgram(pipeline.Program);
            buffersAspect.ConsumePipelineUniformBuffers(pipeline.UniformBuffers);
            buffersAspect.ConsumePipelineTransformFeedbackBuffers(pipeline.TransformFeedbackBuffers);
            buffersAspect.ConsumePipelineVertexArray(pipeline.VertexArray);
            texturesAspect.ConsumePipelineTextures(pipeline.Textures);
            samplersAspect.ConsumePipelineSamplers(pipeline.Samplers, pipeline.Textures.EnabledTextureRange);
            framebuffersAspect.ConsumePipelineFramebuffer(pipeline.Framebuffer);
            viewportsAspect.ConsumePipelineViewports(pipeline.Viewports);
            rasterizerAspect.ConsumePipelineRasterizer(pipeline.Rasterizer);
            depthStencilAspect.ConsumePipelineDepthStencil(pipeline.DepthStencil);
            blendAspect.ConsumePipelineBlend(pipeline.Blend);
        }

        public unsafe void ClearWindowColor(Color4 color)
        {
            framebuffersAspect.BindDrawFramebuffer(null);
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

            framebuffersAspect.BindDrawFramebuffer(null);
            GL.ClearBuffer(clearBuffer, 0, depth, stencil);
        }

        #region Draw
        public void DrawArrays(BeginMode mode, int first, int count)
        {
            ConsumePipeline();
            GL.DrawArrays(mode, first, count);
        }

        public void DrawArraysIndirect(BeginMode mode, int offset)
        {
            ConsumePipeline();
            GL.DrawArraysIndirect((ArbDrawIndirect)mode, (IntPtr)offset);
        }

        public void DrawArraysInstanced(BeginMode mode, int first, int count, int primcount)
        {
            ConsumePipeline();
            GL.DrawArraysInstanced(mode, first, count, primcount);
        }

        // todo: DrawArraysInstancedBaseInstance

        public void DrawElements(BeginMode mode, int count, DrawElementsType type, int offset)
        {
            ConsumePipeline();
            GL.DrawElements(mode, count, type, offset);
        }

        public void DrawElementsBaseVertex(BeginMode mode, int count, DrawElementsType type, int offset, int basevertex)
        {
            ConsumePipeline();
            GL.DrawElementsBaseVertex(mode, count, type, (IntPtr)offset, basevertex);
        }

        public void DrawElementsIndirect(BeginMode mode, DrawElementsType type, int offset)
        {
            ConsumePipeline();
            GL.DrawElementsIndirect((ArbDrawIndirect)mode, (ArbDrawIndirect)type, (IntPtr)offset);
        }

        public void DrawElementsInstanced(BeginMode mode, int count, DrawElementsType type, int offset, int primcount)
        {
            ConsumePipeline();
            GL.DrawElementsInstanced(mode, count, type, (IntPtr)offset, primcount);
        }

        // todo: DrawElementsInstancedBaseInstance

        public void DrawElementsInstancedBaseVertex(BeginMode mode, int count, DrawElementsType type, int offset, int primcount, int basevertex)
        {
            ConsumePipeline();
            GL.DrawElementsInstancedBaseVertex(mode, count, type, (IntPtr)offset, primcount, basevertex);
        }

        // todo: DrawElementsInstancedBaseVertexBaseInstance

        public void DrawRangeElements(BeginMode mode, int start, int end, int count, DrawElementsType type, int offset)
        {
            ConsumePipeline();
            GL.DrawRangeElements(mode, start, end, count, type, (IntPtr)offset);
        }

        public void DrawRangeElementsBaseVertex(BeginMode mode, int start, int end, int count, DrawElementsType type, int offset, int basevertex)
        {
            ConsumePipeline();
            GL.DrawRangeElementsBaseVertex(mode, start, end, count, type, (IntPtr)offset, basevertex);
        }

        // todo: DrawTransformFeedback

        // todo: DrawTransformFeedbackInstanced

        // todo: DrawTransformFeedbackStream

        // todo: DrawTransformFeedbackStreamInstanced

        #endregion

        public void SwapBuffers()
        {
            nativeContext.SwapBuffers();
        }
    }
}
