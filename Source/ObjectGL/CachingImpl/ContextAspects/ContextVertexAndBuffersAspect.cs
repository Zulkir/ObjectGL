#region License
/*
Copyright (c) 2012-2014 ObjectGL Project - Daniil Rodin

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
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;
using ObjectGL.CachingImpl.PipelineAspects;
using ObjectGL.CachingImpl.Utilitties;

namespace ObjectGL.CachingImpl.ContextAspects
{
    internal class ContextVertexAndBuffersAspect
    {
        readonly RedundantInt patchVertexCountBinding;
        readonly RedundantObject<IVertexArray> vertexArrayBinding;
        readonly RedundantObject<ITransformFeedback> transformFeedbackBinding;

        readonly RedundantObject<IBuffer> arrayBufferBinding;
        readonly RedundantObject<IBuffer> copyReadBufferBinding;
        readonly RedundantObject<IBuffer> copyWriteBufferBinding;
        readonly RedundantObject<IBuffer> elementArrayBufferBinding;
        readonly RedundantObject<IBuffer> pixelPackBufferBinding;
        readonly RedundantObject<IBuffer> pixelUnpackBufferBinding;
        readonly RedundantObject<IBuffer> textureBufferBinding;
        readonly RedundantObject<IBuffer> drawIndirectBufferBinding;
        readonly RedundantObject<IBuffer> transformFeedbackBufferBinding;
        readonly RedundantObject<IBuffer> uniformBufferBinding;
        readonly RedundantObject<IBuffer>[] uniformBufferIndexedBindingsArray;

        int boundUniformBufferRange = 0;

        public ContextVertexAndBuffersAspect(IGL gl, IImplementation implementation)
        {
            patchVertexCountBinding = new RedundantInt(gl, (g, x) => { if (x != 0) g.PatchParameter((int)All.PatchVertices, x); });
            vertexArrayBinding = new RedundantObject<IVertexArray>(gl, (g, o) => g.BindVertexArray(o.SafeGetHandle()));
            transformFeedbackBinding = new RedundantObject<ITransformFeedback>(gl, (g, o) => g.BindTransformFeedback((int)All.TransformFeedback, o.SafeGetHandle()));

            arrayBufferBinding = new RedundantObject<IBuffer>(gl, (g, o) => g.BindBuffer((int)BufferTarget.ArrayBuffer, o.SafeGetHandle()));
            copyReadBufferBinding = new RedundantObject<IBuffer>(gl, (g, o) => g.BindBuffer((int)BufferTarget.CopyReadBuffer, o.SafeGetHandle()));
            copyWriteBufferBinding = new RedundantObject<IBuffer>(gl, (g, o) => g.BindBuffer((int)BufferTarget.CopyWriteBuffer, o.SafeGetHandle()));
            elementArrayBufferBinding = new RedundantObject<IBuffer>(gl, (g, o) => g.BindBuffer((int)BufferTarget.ElementArrayBuffer, o.SafeGetHandle()));
            pixelPackBufferBinding = new RedundantObject<IBuffer>(gl, (g, o) => g.BindBuffer((int)BufferTarget.PixelPackBuffer, o.SafeGetHandle()));
            pixelUnpackBufferBinding = new RedundantObject<IBuffer>(gl, (g, o) => g.BindBuffer((int)BufferTarget.PixelUnpackBuffer, o.SafeGetHandle()));
            textureBufferBinding = new RedundantObject<IBuffer>(gl, (g, o) => g.BindBuffer((int)BufferTarget.TextureBuffer, o.SafeGetHandle()));
            drawIndirectBufferBinding = new RedundantObject<IBuffer>(gl, (g, o) => g.BindBuffer((int)BufferTarget.DrawIndirectBuffer, o.SafeGetHandle()));
            transformFeedbackBufferBinding = new RedundantObject<IBuffer>(gl, (g, o) => g.BindBuffer((int)BufferTarget.TransformFeedbackBuffer, o.SafeGetHandle()));
            uniformBufferBinding = new RedundantObject<IBuffer>(gl, (g, o) => g.BindBuffer((int)BufferTarget.UniformBuffer, o.SafeGetHandle()));

            uniformBufferIndexedBindingsArray = new RedundantObject<IBuffer>[implementation.MaxUniformBufferBindings];
            for (uint i = 0; i < implementation.MaxUniformBufferBindings; i++)
            {
                uint iLoc = i;
                uniformBufferIndexedBindingsArray[i] = new RedundantObject<IBuffer>(gl, (g, o) =>
                {
                    g.BindBufferBase((int)BufferTarget.UniformBuffer, iLoc, o.SafeGetHandle());
                    uniformBufferBinding.OnOutsideChange(o);
                });
            }
        }

        public void BindVertexArray(IVertexArray vertexArray)
        {
            vertexArrayBinding.Set(vertexArray);
        }

        public void BindTransformFeedback(ITransformFeedback transformFeedback)
        {
            transformFeedbackBinding.Force(transformFeedback);
        }

        public void BindBuffer(BufferTarget target, IBuffer buffer)
        {
            switch (target)
            {
                case BufferTarget.ArrayBuffer: arrayBufferBinding.Set(buffer); return;
                case BufferTarget.CopyReadBuffer: copyReadBufferBinding.Set(buffer); return;
                case BufferTarget.CopyWriteBuffer: copyWriteBufferBinding.Set(buffer); return;
                case BufferTarget.PixelPackBuffer: pixelPackBufferBinding.Set(buffer); return;
                case BufferTarget.PixelUnpackBuffer: pixelUnpackBufferBinding.Set(buffer); return;
                case BufferTarget.TextureBuffer: textureBufferBinding.Set(buffer); return;
                case BufferTarget.DrawIndirectBuffer: drawIndirectBufferBinding.Set(buffer); return;
                case BufferTarget.UniformBuffer: uniformBufferBinding.Set(buffer); return;
                case BufferTarget.ElementArrayBuffer:
                    {
                        vertexArrayBinding.Set(null);
                        elementArrayBufferBinding.Set(buffer);
                        return;
                    }
                case BufferTarget.TransformFeedbackBuffer:
                    {
                        transformFeedbackBinding.Set(null);
                        transformFeedbackBufferBinding.Set(buffer);
                        return;
                    }
                default: throw new ArgumentOutOfRangeException("target");
            }
        }

        public void ConsumePipelinePatchVertexCount(int patchVertexCount)
        {
            patchVertexCountBinding.Set(patchVertexCount);
        }

        public void ConsumePipelineVertexArray(IVertexArray vertexArray)
        {
            BindVertexArray(vertexArray);
        }

        public void ConsumePipelineDrawIndirectBuffer(IBuffer drawIndirectBuffer)
        {
            BindBuffer(BufferTarget.DrawIndirectBuffer, drawIndirectBuffer);
        }

        public void ConsumePipelineUniformBuffers(PipelineUniformBuffersAspect pipelineUniformBuffers)
        {
            for (int i = 0; i < pipelineUniformBuffers.EnabledUniformBufferRange; i++)
                uniformBufferIndexedBindingsArray[i].Set(pipelineUniformBuffers[i]);
            for (int i = pipelineUniformBuffers.EnabledUniformBufferRange; i < boundUniformBufferRange; i++)
                uniformBufferIndexedBindingsArray[i].Set(null);
            boundUniformBufferRange = pipelineUniformBuffers.EnabledUniformBufferRange;
        }
    }
}
