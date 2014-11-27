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

using ObjectGL.Api.Objects;

namespace ObjectGL.Api.Context.Subsets
{
    public interface IContextDrawActions
    {
        void Arrays(BeginMode mode, int firstVertex, int vertexCount);
        void ArraysIndirect(BeginMode mode, int argsBufferOffset);
        void ArraysInstanced(BeginMode mode, int firstVertex, int vertexCountPerInstance, int instanceCount);
        void ArraysInstancedBaseInstance(BeginMode mode, int firstVertex, int vertexCountPerInstance, int instanceCount, int baseInstance);
        void Elements(BeginMode mode, int indexCount, DrawElementsType indexType, int indexBufferOffset);
        void ElementsBaseVertex(BeginMode mode, int indexCount, DrawElementsType indexType, int indexBufferOffset, int baseVertex);
        void ElementsIndirect(BeginMode mode, DrawElementsType indexType, int argsBufferOffset);
        void ElementsInstanced(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount);
        void ElementsInstancedBaseInstance(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount, int baseInstance);
        void ElementsInstancedBaseVertex(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount, int baseVertex);
        void ElementsInstancedBaseVertexBaseInstance(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount, int baseVertex, int baseInstance);
        void RangeElements(BeginMode mode, int minVertexIndex, int maxVertexIndex, int indexCount, DrawElementsType indexType, int indexBufferOffset);
        void RangeElementsBaseVertex(BeginMode mode, int minVertexIndex, int maxVertexIndex, int indexCount, DrawElementsType indexType, int indexBufferOffset, int baseVertex);
        void TransformFeedback(BeginMode mode, ITransformFeedback transformFeedback);
        void TransformFeedbackInstanced(BeginMode mode, ITransformFeedback transformFeedback, int instanceCount);
        void TransformFeedbackStream(BeginMode mode, ITransformFeedback transformFeedback, int stream);
        void TransformFeedbackStreamInstanced(BeginMode mode, ITransformFeedback transformFeedback, int stream, int instanceCount);
    }
}