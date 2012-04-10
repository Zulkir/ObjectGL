﻿#region License
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
            readonly RedundantInt[] transormFeedbackBufferIndexedBindings;
            readonly RedundantInt[] uniformBufferIndexedBindings;

            int actualTransformFeedbackBuffer;
            int actualUniformBuffer;

            int boundTransformFeedbackBufferRange = 0;
            int boundUniformBufferRange = 0;

            public BuffersAspect(Capabilities capabilities)
            {
                transormFeedbackBufferIndexedBindings = new RedundantInt[capabilities.MaxTransformFeedbackBuffers];
                for (int i = 0; i < capabilities.MaxTransformFeedbackBuffers; i++)
                {
                    int iLoc = i;
                    transormFeedbackBufferIndexedBindings[i] = new RedundantInt(h =>
                    {
                        GL.BindBufferBase(BufferTarget.TransformFeedbackBuffer, iLoc, h);
                        actualTransformFeedbackBuffer = h;
                    });
                }

                uniformBufferIndexedBindings = new RedundantInt[capabilities.MaxUniformBufferBindings];
                for (int i = 0; i < capabilities.MaxUniformBufferBindings; i++)
                {
                    int iLoc = i;
                    uniformBufferIndexedBindings[i] = new RedundantInt(h =>
                    {
                        GL.BindBufferBase(BufferTarget.UniformBuffer, iLoc, h);
                        actualUniformBuffer = h;
                    });
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
                    case BufferTarget.TransformFeedbackBuffer:
                        {
                            if (actualTransformFeedbackBuffer == bufferHandle) return;
                            transormFeedbackBufferIndexedBindings[0].Force(bufferHandle);
                            actualTransformFeedbackBuffer = bufferHandle;
                            return;
                        }
                    case BufferTarget.UniformBuffer:
                        {
                            if (actualUniformBuffer == bufferHandle) return;
                            uniformBufferIndexedBindings[0].Force(bufferHandle);
                            actualUniformBuffer = bufferHandle;
                            return;
                        }
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
                    uniformBufferIndexedBindings[i].Set(pipelineUniformBuffers[i].Handle);
                }

                for (int i = pipelineUniformBuffers.EnabledUniformBufferRange; i < boundUniformBufferRange; i++)
                {
                    uniformBufferIndexedBindings[i].Set(0);
                }

                boundUniformBufferRange = pipelineUniformBuffers.EnabledUniformBufferRange;
            }
        }
    }
}