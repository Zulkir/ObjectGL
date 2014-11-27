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
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Subsets;
using ObjectGL.Api.Objects;

namespace ObjectGL.CachingImpl.ContextAspects
{
    public class ContextDrawActions : IContextDrawActions
    {
        private IGL gl;

        public ContextDrawActions(IGL gl)
        {
            this.gl = gl;
        }

        public void Arrays(BeginMode mode, int firstVertex, int vertexCount)
        {
            gl.DrawArrays((int)mode, firstVertex, vertexCount);
        }

        public void ArraysIndirect(BeginMode mode, int argsBufferOffset)
        {
            gl.DrawArraysIndirect((int)mode, (IntPtr)argsBufferOffset);
        }

        public void ArraysInstanced(BeginMode mode, int firstVertex, int vertexCountPerInstance, int instanceCount)
        {
            gl.DrawArraysInstanced((int)mode, firstVertex, vertexCountPerInstance, instanceCount);
        }

        public void ArraysInstancedBaseInstance(BeginMode mode, int firstVertex, int vertexCountPerInstance, int instanceCount, int baseInstance)
        {
            gl.DrawArraysInstancedBaseInstance((int)mode, firstVertex, vertexCountPerInstance, instanceCount, (uint)baseInstance);
        }

        public void Elements(BeginMode mode, int indexCount, DrawElementsType indexType, int indexBufferOffset)
        {
            gl.DrawElements((int)mode, indexCount, (int)indexType, (IntPtr)indexBufferOffset);
        }

        public void ElementsBaseVertex(BeginMode mode, int indexCount, DrawElementsType indexType, int indexBufferOffset, int baseVertex)
        {
            gl.DrawElementsBaseVertex((int)mode, indexCount, (int)indexType, (IntPtr)indexBufferOffset, baseVertex);
        }

        public void ElementsIndirect(BeginMode mode, DrawElementsType indexType, int argsBufferOffset)
        {
            gl.DrawElementsIndirect((int)mode, (int)indexType, (IntPtr)argsBufferOffset);
        }

        public void ElementsInstanced(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount)
        {
            gl.DrawElementsInstanced((int)mode, indexCountPerInstance, (int)indexType, (IntPtr)indexBufferOffset, instanceCount);
        }

        public void ElementsInstancedBaseInstance(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount, int baseInstance)
        {
            gl.DrawElementsInstancedBaseInstance((int)mode, indexCountPerInstance, (int)indexType, (IntPtr)indexBufferOffset, instanceCount, (uint)baseInstance);
        }

        public void ElementsInstancedBaseVertex(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount, int baseVertex)
        {
            gl.DrawElementsInstancedBaseVertex((int)mode, indexCountPerInstance, (int)indexType, (IntPtr)indexBufferOffset, instanceCount, baseVertex);
        }

        public void ElementsInstancedBaseVertexBaseInstance(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount, int baseVertex, int baseInstance)
        {
            gl.DrawElementsInstancedBaseVertexBaseInstance((int)mode, indexCountPerInstance, (int)indexType, (IntPtr)indexBufferOffset, instanceCount, baseVertex, (uint)baseInstance);
        }

        public void RangeElements(BeginMode mode, int minVertexIndex, int maxVertexIndex, int indexCount, DrawElementsType indexType, int indexBufferOffset)
        {
            gl.DrawRangeElements((int)mode, (uint)minVertexIndex, (uint)maxVertexIndex, indexCount, (int)indexType, (IntPtr)indexBufferOffset);
        }

        public void RangeElementsBaseVertex(BeginMode mode, int minVertexIndex, int maxVertexIndex, int indexCount, DrawElementsType indexType, int indexBufferOffset, int baseVertex)
        {
            gl.DrawRangeElementsBaseVertex((int)mode, (uint)minVertexIndex, (uint)maxVertexIndex, indexCount, (int)indexType, (IntPtr)indexBufferOffset, baseVertex);
        }

        public void TransformFeedback(BeginMode mode, ITransformFeedback transformFeedback)
        {
            if (transformFeedback == null)
                throw new ArgumentNullException("transformFeedback");
            gl.DrawTransformFeedback((int)mode, transformFeedback.Handle);
        }

        public void TransformFeedbackInstanced(BeginMode mode, ITransformFeedback transformFeedback, int instanceCount)
        {
            if (transformFeedback == null)
                throw new ArgumentNullException("transformFeedback");
            gl.DrawTransformFeedbackInstanced((int)mode, transformFeedback.Handle, instanceCount);
        }

        public void TransformFeedbackStream(BeginMode mode, ITransformFeedback transformFeedback, int stream)
        {
            if (transformFeedback == null)
                throw new ArgumentNullException("transformFeedback");
            gl.DrawTransformFeedbackStream((int)mode, transformFeedback.Handle, (uint)stream);
        }

        public void TransformFeedbackStreamInstanced(BeginMode mode, ITransformFeedback transformFeedback, int stream, int instanceCount)
        {
            if (transformFeedback == null)
                throw new ArgumentNullException("transformFeedback");
            gl.DrawTransformFeedbackStreamInstanced((int)mode, transformFeedback.Handle, (uint)stream, instanceCount);
        }
    }
}