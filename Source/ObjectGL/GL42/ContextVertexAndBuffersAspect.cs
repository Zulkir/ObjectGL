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

namespace ObjectGL.GL42
{
    public partial class Context
    {
        private class VertexAndBuffersAspect
        {
            readonly RedundantInt patchVertexCountBinding = new RedundantInt(x => { if (x != 0) GL.PatchParameter(PatchParameterInt.PatchVertices, x); });
            readonly RedundantObject<VertexArray> vertexArrayBinding = new RedundantObject<VertexArray>(o => GL.BindVertexArray(Helpers.ObjectHandle(o)));
            readonly RedundantObject<TransformFeedback> transformFeedbackBinding = new RedundantObject<TransformFeedback>(o => GL.BindTransformFeedback(TransformFeedbackTarget.TransformFeedback, Helpers.ObjectHandle(o))); 

            readonly RedundantObject<Buffer> arrayBufferBinding = new RedundantObject<Buffer>(o => GL.BindBuffer(BufferTarget.ArrayBuffer, Helpers.ObjectHandle(o)));
            readonly RedundantObject<Buffer> copyReadBufferBinding = new RedundantObject<Buffer>(o => GL.BindBuffer(BufferTarget.CopyReadBuffer, Helpers.ObjectHandle(o)));
            readonly RedundantObject<Buffer> copyWriteBufferBinding = new RedundantObject<Buffer>(o => GL.BindBuffer(BufferTarget.CopyWriteBuffer, Helpers.ObjectHandle(o)));
            readonly RedundantObject<Buffer> elementArrayBufferBinding = new RedundantObject<Buffer>(o => GL.BindBuffer(BufferTarget.ElementArrayBuffer, Helpers.ObjectHandle(o)));
            readonly RedundantObject<Buffer> pixelPackBufferBinding = new RedundantObject<Buffer>(o => GL.BindBuffer(BufferTarget.PixelPackBuffer, Helpers.ObjectHandle(o)));
            readonly RedundantObject<Buffer> pixelUnpackBufferBinding = new RedundantObject<Buffer>(o => GL.BindBuffer(BufferTarget.PixelUnpackBuffer, Helpers.ObjectHandle(o)));
            readonly RedundantObject<Buffer> textureBufferBinding = new RedundantObject<Buffer>(o => GL.BindBuffer(BufferTarget.TextureBuffer, Helpers.ObjectHandle(o)));
            readonly RedundantObject<Buffer> drawIndirectBufferBinding = new RedundantObject<Buffer>(o => GL.BindBuffer(BufferTarget.DrawIndirectBuffer, Helpers.ObjectHandle(o)));
            readonly RedundantObject<Buffer> transformFeedbackBufferBinding = new RedundantObject<Buffer>(o => GL.BindBuffer(BufferTarget.TransformFeedbackBuffer, Helpers.ObjectHandle(o)));
            readonly RedundantObject<Buffer> uniformBufferBinding = new RedundantObject<Buffer>(o => GL.BindBuffer(BufferTarget.UniformBuffer, Helpers.ObjectHandle(o)));
            readonly RedundantObject<Buffer>[] uniformBufferIndexedBindingsArray;

            int boundUniformBufferRange = 0;

            public VertexAndBuffersAspect(Implementation implementation)
            {
                uniformBufferIndexedBindingsArray = new RedundantObject<Buffer>[implementation.MaxUniformBufferBindings];
                for (int i = 0; i < implementation.MaxUniformBufferBindings; i++)
                {
                    int iLoc = i;
                    uniformBufferIndexedBindingsArray[i] = new RedundantObject<Buffer>(o =>
                        {
                            GL.BindBufferBase(BufferTarget.UniformBuffer, iLoc, Helpers.ObjectHandle(o));
                            uniformBufferBinding.OnOutsideChange(o);
                        });
                }
            }

            public void BindVertexArray(VertexArray vertexArray)
            {
                vertexArrayBinding.Set(vertexArray);
            }

            public void BindTransformFeedback(TransformFeedback transformFeedback)
            {
                transformFeedbackBinding.Force(transformFeedback);
            }

            public void BindBuffer(BufferTarget target, Buffer buffer)
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

            public void ConsumePipelineVertexArray(VertexArray vertexArray)
            {
                BindVertexArray(vertexArray);
            }

            public void ConsumePipelineDrawIndirectBuffer(Buffer drawIndirectBuffer)
            {
                BindBuffer(BufferTarget.DrawIndirectBuffer, drawIndirectBuffer);
            }

            public void ConsumePipelineUniformBuffers(Pipeline.UniformBuffersAspect pipelineUniformBuffers)
            {
                for (int i = 0; i < pipelineUniformBuffers.EnabledUniformBufferRange; i++)
                    uniformBufferIndexedBindingsArray[i].Set(pipelineUniformBuffers[i]);
                for (int i = pipelineUniformBuffers.EnabledUniformBufferRange; i < boundUniformBufferRange; i++)
                    uniformBufferIndexedBindingsArray[i].Set(null);
                boundUniformBufferRange = pipelineUniformBuffers.EnabledUniformBufferRange;
            }
        }
    }
}
