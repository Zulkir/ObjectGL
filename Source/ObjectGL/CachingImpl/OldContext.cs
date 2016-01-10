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

using System;
using ObjectGL.Api;
using ObjectGL.Api.Context;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;
using ObjectGL.CachingImpl.ContextAspects;
using ObjectGL.CachingImpl.PipelineAspects;

namespace ObjectGL.CachingImpl
{
    public class OldContext : IOldContext
    {
        private readonly IGL gl;
        private readonly INativeGraphicsContext nativeContext;

        private readonly Implementation implementation;
        private readonly Pipeline pipeline;
        private readonly ContextObjectFactory factory;

        private readonly ContextProgramAspect programAspect;
        private readonly ContextVertexAndBuffersAspect vertexAndBuffersAspect;
        private readonly ContextRenderbufferAspect renderbufferAspect;
        private readonly ContextTexturesAspect texturesAspect;
        private readonly ContextPixelPackUnpackAspect pixelPackUnpackAspect;
        private readonly ContextSamplersAspect samplersAspect;
        private readonly ContextFramebufferAspect framebuffersAspect;
        private readonly ContextViewportsAspect viewportsAspect;
        private readonly ContextScissorBoxesAspect scissorBoxesAspect;
        private readonly ContextRasterizerAspect rasterizerAspect;
        private readonly ContextDepthStencilAspect depthStencilAspect;
        private readonly ContextBlendAspect blendAspect;

        public IGL GL { get { return gl; } }
        public IImplementation Implementation { get { return implementation; } }
        public IPipeline Pipeline { get { return pipeline; } }
        public IContextObjectFactory Create { get { return factory; } }

        public OldContext(IGL gl, INativeGraphicsContext nativeContext)
        {
            this.gl = gl;
            if (nativeContext == null)
                throw new ArgumentNullException("nativeContext");

            this.nativeContext = nativeContext;

            gl.Enable((int)EnableCap.TextureCubeMapSeamless);

            implementation = new Implementation(gl);
            pipeline = new Pipeline(this);
            //factory = new ContextObjectFactory(this);

            programAspect = new ContextProgramAspect(gl);
            vertexAndBuffersAspect = new ContextVertexAndBuffersAspect(gl, implementation);
            renderbufferAspect = new ContextRenderbufferAspect(gl);
            texturesAspect = new ContextTexturesAspect(gl, implementation);
            pixelPackUnpackAspect = new ContextPixelPackUnpackAspect(gl);
            samplersAspect = new ContextSamplersAspect(gl, implementation);
            framebuffersAspect = new ContextFramebufferAspect(gl);
            viewportsAspect = new ContextViewportsAspect(gl, implementation);
            scissorBoxesAspect = new ContextScissorBoxesAspect(gl, implementation);
            rasterizerAspect = new ContextRasterizerAspect(gl);
            depthStencilAspect = new ContextDepthStencilAspect(gl);
            blendAspect = new ContextBlendAspect(gl, implementation);
        }

        #region Bind
        internal void UseProgram(IShaderProgram program)
        {
            programAspect.UseProgram(program);
        }

        internal void BindVertexArray(IVertexArray vertexArray)
        {
            vertexAndBuffersAspect.BindVertexArray(vertexArray);
        }

        internal void BindTransformFeedback(ITransformFeedback transformFeedback)
        {
            vertexAndBuffersAspect.BindTransformFeedback(transformFeedback);
        }

        internal void BindBuffer(BufferTarget target, IBuffer buffer)
        {
            vertexAndBuffersAspect.BindBuffer(target, buffer);
        }

        internal void BindTexture(TextureTarget target, ITexture texture)
        {
            texturesAspect.BindTexture(target, texture);
        }

        internal void BindRenderbuffer(IRenderbuffer renderbuffer)
        {
            renderbufferAspect.BindRenderbuffer(renderbuffer);
        }

        internal void BindDrawFramebuffer(IFramebuffer framebuffer)
        {
            framebuffersAspect.BindDrawFramebuffer(framebuffer);
        }

        internal void BindReadFramebuffer(IFramebuffer framebuffer)
        {
            framebuffersAspect.BindReadFramebuffer(framebuffer);
        }

        internal All BindAnyFramebuffer(IFramebuffer framebuffer)
        {
            return framebuffersAspect.BindAnyFramebuffer(framebuffer);
        }
        #endregion

        #region Set
        internal void SetUnpackAlignment(ByteAlignment alignment)
        {
            pixelPackUnpackAspect.SetUnpackAlignment(alignment);
        }
        #endregion

        #region Prepare
        internal void PrepareForClear()
        {
            rasterizerAspect.SetScissorEnable(false);
            depthStencilAspect.SetDepthMask(true);
        }
        #endregion

        private void ConsumePipeline()
        {
            programAspect.ConsumePipelineProgram(pipeline.Program);
            vertexAndBuffersAspect.ConsumePipelinePatchVertexCount(pipeline.PatchVertexCount);
            vertexAndBuffersAspect.ConsumePipelineVertexArray(pipeline.VertexArray);
            vertexAndBuffersAspect.ConsumePipelineDrawIndirectBuffer(pipeline.DrawIndirectBuffer);
            vertexAndBuffersAspect.ConsumePipelineUniformBuffers((PipelineUniformBuffersAspect)pipeline.UniformBuffers);
            texturesAspect.ConsumePipelineTextures((PipelineTexturesAspect)pipeline.Textures);
            samplersAspect.ConsumePipelineSamplers((PipelineSamplersAspect)pipeline.Samplers, (PipelineTexturesAspect)pipeline.Textures);
            framebuffersAspect.ConsumePipelineFramebuffer(pipeline.Framebuffer);
            viewportsAspect.ConsumePipelineViewports((PipelineViewportsAspect)pipeline.Viewports);
            scissorBoxesAspect.ConsumePipelineScissorBoxes((PipelineScissorBoxesAspect)pipeline.ScissorBoxes, (PipelineViewportsAspect)pipeline.Viewports);
            rasterizerAspect.ConsumePipelineRasterizer((PipelineRasterizerAspect)pipeline.Rasterizer);
            depthStencilAspect.ConsumePipelineDepthStencil((PipelineDepthStencilAspect)pipeline.DepthStencil);
            blendAspect.ConsumePipelineBlend((PipelineBlendAspect)pipeline.Blend);
        }

        public unsafe void ClearWindowColor(Color4 color)
        {
            framebuffersAspect.BindDrawFramebuffer(null);
            gl.ClearBuffer((int)All.Color, 0, (float*)&color);
        }

        public void ClearWindowDepthStencil(DepthStencil mask, float depth, int stencil)
        {
            var clearBuffer =
                mask == DepthStencil.Both ? All.DepthStencil :
                mask == DepthStencil.Depth ? All.Depth :
                mask == DepthStencil.Stencil ? All.Stencil : 0;

            if (clearBuffer == 0)
                throw new ArgumentException("'mask' can not be 'None'");

            framebuffersAspect.BindDrawFramebuffer(null);
            gl.ClearBuffer((int)clearBuffer, 0, depth, stencil);
        }

        #region Draw
        public void DrawArrays(BeginMode mode, int firstVertex, int vertexCount)
        {
            ConsumePipeline();
            gl.DrawArrays((int)mode, firstVertex, vertexCount);
        }

        public void DrawArraysIndirect(BeginMode mode, int argsBufferOffset)
        {
            ConsumePipeline();
            gl.DrawArraysIndirect((int)mode, (IntPtr)argsBufferOffset);
        }

        public void DrawArraysInstanced(BeginMode mode, int firstVertex, int vertexCountPerInstance, int instanceCount)
        {
            ConsumePipeline();
            gl.DrawArraysInstanced((int)mode, firstVertex, vertexCountPerInstance, instanceCount);
        }

        public void DrawArraysInstancedBaseInstance(BeginMode mode, int firstVertex, int vertexCountPerInstance, int instanceCount, int baseInstance)
        {
            ConsumePipeline();
            gl.DrawArraysInstancedBaseInstance((int)mode, firstVertex, vertexCountPerInstance, instanceCount, (uint)baseInstance);
        }

        public void DrawElements(BeginMode mode, int indexCount, DrawElementsType indexType, int indexBufferOffset)
        {
            ConsumePipeline();
            gl.DrawElements((int)mode, indexCount, (int)indexType, (IntPtr)indexBufferOffset);
        }

        public void DrawElementsBaseVertex(BeginMode mode, int indexCount, DrawElementsType indexType, int indexBufferOffset, int baseVertex)
        {
            ConsumePipeline();
            gl.DrawElementsBaseVertex((int)mode, indexCount, (int)indexType, (IntPtr)indexBufferOffset, baseVertex);
        }

        public void DrawElementsIndirect(BeginMode mode, DrawElementsType indexType, int argsBufferOffset)
        {
            ConsumePipeline();
            gl.DrawElementsIndirect((int)mode, (int)indexType, (IntPtr)argsBufferOffset);
        }

        public void DrawElementsInstanced(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount)
        {
            ConsumePipeline();
            gl.DrawElementsInstanced((int)mode, indexCountPerInstance, (int)indexType, (IntPtr)indexBufferOffset, instanceCount);
        }

        public void DrawElementsInstancedBaseInstance(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount, int baseInstance)
        {
            ConsumePipeline();
            gl.DrawElementsInstancedBaseInstance((int)mode, indexCountPerInstance, (int)indexType, (IntPtr)indexBufferOffset, instanceCount, (uint)baseInstance);
        }

        public void DrawElementsInstancedBaseVertex(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount, int baseVertex)
        {
            ConsumePipeline();
            gl.DrawElementsInstancedBaseVertex((int)mode, indexCountPerInstance, (int)indexType, (IntPtr)indexBufferOffset, instanceCount, baseVertex);
        }

        public void DrawElementsInstancedBaseVertexBaseInstance(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount, int baseVertex, int baseInstance)
        {
            ConsumePipeline();
            gl.DrawElementsInstancedBaseVertexBaseInstance((int)mode, indexCountPerInstance, (int)indexType, (IntPtr)indexBufferOffset, instanceCount, baseVertex, (uint)baseInstance);
        }

        public void DrawRangeElements(BeginMode mode, int minVertexIndex, int maxVertexIndex, int indexCount, DrawElementsType indexType, int indexBufferOffset)
        {
            ConsumePipeline();
            gl.DrawRangeElements((int)mode, (uint)minVertexIndex, (uint)maxVertexIndex, indexCount, (int)indexType, (IntPtr)indexBufferOffset);
        }

        public void DrawRangeElementsBaseVertex(BeginMode mode, int minVertexIndex, int maxVertexIndex, int indexCount, DrawElementsType indexType, int indexBufferOffset, int baseVertex)
        {
            ConsumePipeline();
            gl.DrawRangeElementsBaseVertex((int)mode, (uint)minVertexIndex, (uint)maxVertexIndex, indexCount, (int)indexType, (IntPtr)indexBufferOffset, baseVertex);
        }

        public void DrawTransformFeedback(BeginMode mode, ITransformFeedback transformFeedback)
        {
            if (transformFeedback == null)
                throw new ArgumentNullException("transformFeedback");
            ConsumePipeline();
            gl.DrawTransformFeedback((int)mode, transformFeedback.Handle);
        }

        public void DrawTransformFeedbackInstanced(BeginMode mode, ITransformFeedback transformFeedback, int instanceCount)
        {
            if (transformFeedback == null)
                throw new ArgumentNullException("transformFeedback");
            ConsumePipeline();
            gl.DrawTransformFeedbackInstanced((int)mode, transformFeedback.Handle, instanceCount);
        }

        public void DrawTransformFeedbackStream(BeginMode mode, ITransformFeedback transformFeedback, int stream)
        {
            if (transformFeedback == null)
                throw new ArgumentNullException("transformFeedback");
            ConsumePipeline();
            gl.DrawTransformFeedbackStream((int)mode, transformFeedback.Handle, (uint)stream);
        }

        public void DrawTransformFeedbackStreamInstanced(BeginMode mode, ITransformFeedback transformFeedback, int stream, int instanceCount)
        {
            if (transformFeedback == null)
                throw new ArgumentNullException("transformFeedback");
            ConsumePipeline();
            gl.DrawTransformFeedbackStreamInstanced((int)mode, transformFeedback.Handle, (uint)stream, instanceCount);
        }
        #endregion

        public void BlitFramebuffer(IFramebuffer src, int srcX0, int srcY0, int srcX1, int srcY1, IFramebuffer dst, int dstX0, int dstY0, int dstX1, int dstY1, ClearBufferMask mask, BlitFramebufferFilter filter)
        {
            framebuffersAspect.BindReadFramebuffer(src);
            framebuffersAspect.BindDrawFramebuffer(dst);
            gl.BlitFramebuffer(srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, (uint)mask, (int)filter);
        }

        public void BeginTransformFeedback(ITransformFeedback transformFeedback, BeginFeedbackMode beginFeedbackMode)
        {
            vertexAndBuffersAspect.BindTransformFeedback(transformFeedback);
            gl.BeginTransformFeedback((int)beginFeedbackMode);
        }

        public void EndTransformFeedback()
        {
            gl.EndTransformFeedback();
        }

        public void SwapBuffers()
        {
            nativeContext.SwapBuffers();
        }
    }
}
