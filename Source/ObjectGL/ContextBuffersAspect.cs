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
using OpenTK.Graphics.OpenGL;

namespace ObjectGL
{
    public partial class Context
    {
        private class BuffersAspect
        {
            readonly RedundantInt vertexArrayBinding = new RedundantInt(GL.BindVertexArray);

            readonly RedundantInt arrayBufferBinding = new RedundantInt(h => GL.BindBuffer(BufferTarget.ArrayBuffer, h));
            readonly RedundantInt copyReadBufferBinding = new RedundantInt(h => GL.BindBuffer(BufferTarget.CopyReadBuffer, h));
            readonly RedundantInt copyWriteBufferBinding = new RedundantInt(h => GL.BindBuffer(BufferTarget.CopyWriteBuffer, h));
            readonly RedundantInt elementArrayBufferBinding = new RedundantInt(h => GL.BindBuffer(BufferTarget.ElementArrayBuffer, h));
            readonly RedundantInt pixelPackBufferBinding = new RedundantInt(h => GL.BindBuffer(BufferTarget.PixelPackBuffer, h));
            readonly RedundantInt pixelUnpackBufferBinding = new RedundantInt(h => GL.BindBuffer(BufferTarget.PixelUnpackBuffer, h));
            readonly RedundantInt textureBufferBinding = new RedundantInt(h => GL.BindBuffer(BufferTarget.TextureBuffer, h));
            readonly RedundantInt drawIndirectBufferBinding = new RedundantInt(h => GL.BindBuffer(BufferTarget.DrawIndirectBuffer, h));
            readonly RedundantInt transformFeedbackBufferBinding = new RedundantInt(h => GL.BindBuffer(BufferTarget.TransformFeedbackBuffer, h));
            readonly RedundantInt uniformBufferBinding = new RedundantInt(h => GL.BindBuffer(BufferTarget.UniformBuffer, h));
            readonly RedundantInt[] transormFeedbackBufferIndexedBindingsArray;
            readonly RedundantInt[] uniformBufferIndexedBindingsArray;

            int boundTransformFeedbackBufferRange = 0;
            int boundUniformBufferRange = 0;

            public BuffersAspect(Implementation implementation)
            {
                transormFeedbackBufferIndexedBindingsArray = new RedundantInt[implementation.MaxTransformFeedbackBuffers];
                for (int i = 0; i < implementation.MaxTransformFeedbackBuffers; i++)
                {
                    int iLoc = i;
                    transormFeedbackBufferIndexedBindingsArray[i] = new RedundantInt(h => GL.BindBufferBase(BufferTarget.TransformFeedbackBuffer, iLoc, h));
                }

                uniformBufferIndexedBindingsArray = new RedundantInt[implementation.MaxUniformBufferBindings];
                for (int i = 0; i < implementation.MaxUniformBufferBindings; i++)
                {
                    int iLoc = i;
                    uniformBufferIndexedBindingsArray[i] = new RedundantInt(h => GL.BindBufferBase(BufferTarget.UniformBuffer, iLoc, h));
                }
            }

            public void BindVertexArray(int vertexArrayHandle)
            {
                vertexArrayBinding.Set(vertexArrayHandle);
            }

            public void BindBuffer(BufferTarget target, int bufferHandle)
            {
                if (target == BufferTarget.ElementArrayBuffer)
                    vertexArrayBinding.Set(0);

                switch (target)
                {
                    case BufferTarget.ArrayBuffer: arrayBufferBinding.Set(bufferHandle); return;
                    case BufferTarget.CopyReadBuffer: copyReadBufferBinding.Set(bufferHandle); return;
                    case BufferTarget.CopyWriteBuffer: copyWriteBufferBinding.Set(bufferHandle); return;
                    case BufferTarget.ElementArrayBuffer: elementArrayBufferBinding.Set(bufferHandle); return;
                    case BufferTarget.PixelPackBuffer: pixelPackBufferBinding.Set(bufferHandle); return;
                    case BufferTarget.PixelUnpackBuffer: pixelUnpackBufferBinding.Set(bufferHandle); return;
                    case BufferTarget.TextureBuffer: textureBufferBinding.Set(bufferHandle); return;
                    case BufferTarget.DrawIndirectBuffer: drawIndirectBufferBinding.Set(bufferHandle); return;
                    case BufferTarget.TransformFeedbackBuffer: transformFeedbackBufferBinding.Set(bufferHandle); return;
                    case BufferTarget.UniformBuffer: uniformBufferBinding.Set(bufferHandle); return;
                    default: throw new ArgumentOutOfRangeException("target");
                }
            }

            public void ConsumePipelineVertexArray(VertexArray vertexArray)
            {
                BindVertexArray(vertexArray.Handle);
            }

            public void ConsumePipelineUniformBuffers(Pipeline.UniformBuffersAspect pipelineUniformBuffers)
            {
                for (int i = 0; i < pipelineUniformBuffers.EnabledUniformBufferRange; i++)
                {
                    uniformBufferIndexedBindingsArray[i].Set(pipelineUniformBuffers[i].Handle);
                }

                for (int i = pipelineUniformBuffers.EnabledUniformBufferRange; i < boundUniformBufferRange; i++)
                {
                    uniformBufferIndexedBindingsArray[i].Set(0);
                }

                boundUniformBufferRange = pipelineUniformBuffers.EnabledUniformBufferRange;
            }

            public void ConsumePipelineTransformFeedbackBuffers(Pipeline.TransformFeedbackBufferAspect transformFeedbackBuffers)
            {
                for (int i = 0; i < transformFeedbackBuffers.EnabledTransformFeedbackBufferRange; i++)
                {
                    transormFeedbackBufferIndexedBindingsArray[i].Set(transformFeedbackBuffers[i].Handle);
                }

                for (int i = transformFeedbackBuffers.EnabledTransformFeedbackBufferRange; i < boundTransformFeedbackBufferRange; i++)
                {
                    transormFeedbackBufferIndexedBindingsArray[i].Set(0);
                }

                boundTransformFeedbackBufferRange = transformFeedbackBuffers.EnabledTransformFeedbackBufferRange;
            }
        }
    }
}
