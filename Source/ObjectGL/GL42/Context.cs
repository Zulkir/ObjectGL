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
        readonly VertexAndBuffersAspect vertexAndBuffersAspect;
        readonly RenderbufferAspect renderbufferAspect;
        readonly TexturesAspect texturesAspect;
        readonly SamplersAspect samplersAspect;
        readonly FramebufferAspect framebuffersAspect;
        readonly ViewportsAspect viewportsAspect;
        readonly ScissorBoxesAspect scissorBoxesAspect;
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
            vertexAndBuffersAspect = new VertexAndBuffersAspect(implementation);
            renderbufferAspect = new RenderbufferAspect();
            texturesAspect = new TexturesAspect(implementation);
            samplersAspect = new SamplersAspect(implementation);
            framebuffersAspect = new FramebufferAspect();
            viewportsAspect = new ViewportsAspect(implementation);
            scissorBoxesAspect = new ScissorBoxesAspect(implementation);
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
            vertexAndBuffersAspect.BindVertexArray(vertexArray);
        }

        internal void BindTransformFeedback(TransformFeedback transformFeedback)
        {
            vertexAndBuffersAspect.BindTransformFeedback(transformFeedback);
        }

        internal void BindBuffer(BufferTarget target, Buffer buffer)
        {
            vertexAndBuffersAspect.BindBuffer(target, buffer);
        }

        internal void BindTexture(TextureTarget target, Texture texture)
        {
            texturesAspect.BindTexture(target, texture);
        }

        internal void BindRenderbuffer(Renderbuffer renderbuffer)
        {
            renderbufferAspect.BindRenderbuffer(renderbuffer);
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

        #region Set
        internal void SetUnpackAlignment(ByteAlignment alignment)
        {
            renderbufferAspect.SetUnpackAlignment(alignment);
        }
        #endregion

        #region Prepare
        internal void PrepareForClear()
        {
            rasterizerAspect.SetScissorEnable(false);
            depthStencilAspect.SetDepthMask(true);
        }
        #endregion

        internal void ConsumePipeline()
        {
            programAspect.ConsumePipelineProgram(pipeline.Program);
            vertexAndBuffersAspect.ConsumePipelinePatchVertexCount(pipeline.PatchVertexCount);
            vertexAndBuffersAspect.ConsumePipelineVertexArray(pipeline.VertexArray);
            vertexAndBuffersAspect.ConsumePipelineDrawIndirectBuffer(pipeline.DrawIndirectBuffer);
            vertexAndBuffersAspect.ConsumePipelineUniformBuffers(pipeline.UniformBuffers);
            texturesAspect.ConsumePipelineTextures(pipeline.Textures);
            samplersAspect.ConsumePipelineSamplers(pipeline.Samplers, pipeline.Textures.EnabledTextureRange);
            framebuffersAspect.ConsumePipelineFramebuffer(pipeline.Framebuffer);
            viewportsAspect.ConsumePipelineViewports(pipeline.Viewports);
            scissorBoxesAspect.ConsumePipelineScissorBoxes(pipeline.ScissorBoxes, pipeline.Viewports.EnabledViewportCount);
            rasterizerAspect.ConsumePipelineRasterizer(pipeline.Rasterizer);
            depthStencilAspect.ConsumePipelineDepthStencil(pipeline.DepthStencil);
            blendAspect.ConsumePipelineBlend(pipeline.Blend);
        }

#if Debug
        public void Consume00() {programAspect.ConsumePipelineProgram(pipeline.Program);                                                           }
        public void Consume01() {vertexAndBuffersAspect.ConsumePipelinePatchVertexCount(pipeline.PatchVertexCount);                                }
        public void Consume02() {vertexAndBuffersAspect.ConsumePipelineVertexArray(pipeline.VertexArray);                                          }
        public void Consume03() {vertexAndBuffersAspect.ConsumePipelineDrawIndirectBuffer(pipeline.DrawIndirectBuffer);                            }
        public void Consume04() {vertexAndBuffersAspect.ConsumePipelineUniformBuffers(pipeline.UniformBuffers);                                    }
        public void Consume05() {texturesAspect.ConsumePipelineTextures(pipeline.Textures);                                                        }
        public void Consume06() {samplersAspect.ConsumePipelineSamplers(pipeline.Samplers, pipeline.Textures.EnabledTextureRange);                 }
        public void Consume07() {framebuffersAspect.ConsumePipelineFramebuffer(pipeline.Framebuffer);                                              }
        public void Consume08() {viewportsAspect.ConsumePipelineViewports(pipeline.Viewports);                                                     }
        public void Consume09() {scissorBoxesAspect.ConsumePipelineScissorBoxes(pipeline.ScissorBoxes, pipeline.Viewports.EnabledViewportCount);   }
        public void Consume10() {rasterizerAspect.ConsumePipelineRasterizer(pipeline.Rasterizer);                                                  }
        public void Consume11() {depthStencilAspect.ConsumePipelineDepthStencil(pipeline.DepthStencil);                                            }
        public void Consume12() { blendAspect.ConsumePipelineBlend(pipeline.Blend); }
#endif

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
        public void DrawArrays(BeginMode mode, int firstVertex, int vertexCount)
        {
            ConsumePipeline();
            GL.DrawArrays(mode, firstVertex, vertexCount);
        }

        public void DrawArraysIndirect(BeginMode mode, int argsBufferOffset)
        {
            ConsumePipeline();
            GL.DrawArraysIndirect((ArbDrawIndirect)mode, (IntPtr)argsBufferOffset);
        }

        public void DrawArraysInstanced(BeginMode mode, int firstVertex, int vertexCountPerInstance, int instanceCount)
        {
            ConsumePipeline();
            GL.DrawArraysInstanced(mode, firstVertex, vertexCountPerInstance, instanceCount);
        }

        public void DrawArraysInstancedBaseInstance(BeginMode mode, int firstVertex, int vertexCountPerInstance, int instanceCount, int baseInstance)
        {
            ConsumePipeline();
            GL.DrawArraysInstancedBaseInstance(mode, firstVertex, vertexCountPerInstance, instanceCount, (uint)baseInstance);
        }

        public void DrawElements(BeginMode mode, int indexCount, DrawElementsType indexType, int indexBufferOffset)
        {
            ConsumePipeline();
            GL.DrawElements(mode, indexCount, indexType, indexBufferOffset);
        }

        public void DrawElementsBaseVertex(BeginMode mode, int indexCount, DrawElementsType indexType, int indexBufferOffset, int baseVertex)
        {
            ConsumePipeline();
            GL.DrawElementsBaseVertex(mode, indexCount, indexType, (IntPtr)indexBufferOffset, baseVertex);
        }

        public void DrawElementsIndirect(BeginMode mode, DrawElementsType indexType, int argsBufferOffset)
        {
            ConsumePipeline();
            GL.DrawElementsIndirect((ArbDrawIndirect)mode, (ArbDrawIndirect)indexType, (IntPtr)argsBufferOffset);
        }

        public void DrawElementsInstanced(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount)
        {
            ConsumePipeline();
            GL.DrawElementsInstanced(mode, indexCountPerInstance, indexType, (IntPtr)indexBufferOffset, instanceCount);
        }

        public void DrawElementsInstancedBaseInstance(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount, int baseInstance)
        {
            ConsumePipeline();
            GL.DrawElementsInstancedBaseInstance(mode, indexCountPerInstance, indexType, (IntPtr)indexBufferOffset, instanceCount, (uint)baseInstance);
        }

        public void DrawElementsInstancedBaseVertex(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount, int baseVertex)
        {
            ConsumePipeline();
            GL.DrawElementsInstancedBaseVertex(mode, indexCountPerInstance, indexType, (IntPtr)indexBufferOffset, instanceCount, baseVertex);
        }

        public void DrawElementsInstancedBaseVertexBaseInstance(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount, int baseVertex, int baseInstance)
        {
            ConsumePipeline();
            GL.DrawElementsInstancedBaseVertexBaseInstance(mode, indexCountPerInstance, indexType, (IntPtr)indexBufferOffset, instanceCount, baseVertex, (uint)baseInstance);
        }

        public void DrawRangeElements(BeginMode mode, int minVertexIndex, int maxVertexIndex, int indexCount, DrawElementsType indexType, int indexBufferOffset)
        {
            ConsumePipeline();
            GL.DrawRangeElements(mode, minVertexIndex, maxVertexIndex, indexCount, indexType, (IntPtr)indexBufferOffset);
        }

        public void DrawRangeElementsBaseVertex(BeginMode mode, int minVertexIndex, int maxVertexIndex, int indexCount, DrawElementsType indexType, int indexBufferOffset, int baseVertex)
        {
            ConsumePipeline();
            GL.DrawRangeElementsBaseVertex(mode, minVertexIndex, maxVertexIndex, indexCount, indexType, (IntPtr)indexBufferOffset, baseVertex);
        }

        public void DrawTransformFeedback(BeginMode mode, TransformFeedback transformFeedback)
        {
            if (transformFeedback == null)
                throw new ArgumentNullException("transformFeedback");
            ConsumePipeline();
            GL.DrawTransformFeedback(mode, transformFeedback.Handle);
        }

        public void DrawTransformFeedbackInstanced(BeginMode mode, TransformFeedback transformFeedback, int instanceCount)
        {
            if (transformFeedback == null)
                throw new ArgumentNullException("transformFeedback");
            ConsumePipeline();
            GL.DrawTransformFeedbackInstanced(mode, (uint)transformFeedback.Handle, instanceCount);
        }

        public void DrawTransformFeedbackStream(BeginMode mode, TransformFeedback transformFeedback, int stream)
        {
            if (transformFeedback == null)
                throw new ArgumentNullException("transformFeedback");
            ConsumePipeline();
            GL.DrawTransformFeedbackStream(mode, transformFeedback.Handle, stream);
        }

        public void DrawTransformFeedbackStreamInstanced(BeginMode mode, TransformFeedback transformFeedback, int stream, int instanceCount)
        {
            if (transformFeedback == null)
                throw new ArgumentNullException("transformFeedback");
            ConsumePipeline();
            GL.DrawTransformFeedbackStreamInstanced(mode, (uint)transformFeedback.Handle, (uint)stream, instanceCount);
        }

        #endregion

        public void BlitFramebuffer(
            Framebuffer src, int srcX0, int srcY0, int srcX1, int srcY1,
            Framebuffer dst, int dstX0, int dstY0, int dstX1, int dstY1,
            ClearBufferMask mask, BlitFramebufferFilter filter)
        {
            framebuffersAspect.BindReadFramebuffer(src);
            framebuffersAspect.BindDrawFramebuffer(dst);
            GL.BlitFramebuffer(srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter);
        }

        public void BeginTransformFeedback(TransformFeedback transformFeedback, BeginFeedbackMode beginFeedbackMode)
        {
            vertexAndBuffersAspect.BindTransformFeedback(transformFeedback);
            GL.BeginTransformFeedback(beginFeedbackMode);
        }

        public void EndTransformFeedback()
        {
            GL.EndTransformFeedback();
        }

        public void SwapBuffers()
        {
            nativeContext.SwapBuffers();
        }
    }
}
