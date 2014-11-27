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

using ObjectGL.Api.Context;
using ObjectGL.Api.Objects;

namespace ObjectGL.Api
{
    public interface IOldContext
    {
        IGL GL { get; }
        IImplementation Implementation { get; }
        IPipeline Pipeline { get; }
        IContextObjectFactory Create { get; }

        void ClearWindowColor(Color4 color);
        void ClearWindowDepthStencil(DepthStencil mask, float depth, int stencil);
        void DrawArrays(BeginMode mode, int firstVertex, int vertexCount);
        void DrawArraysIndirect(BeginMode mode, int argsBufferOffset);
        void DrawArraysInstanced(BeginMode mode, int firstVertex, int vertexCountPerInstance, int instanceCount);
        void DrawArraysInstancedBaseInstance(BeginMode mode, int firstVertex, int vertexCountPerInstance, int instanceCount, int baseInstance);
        void DrawElements(BeginMode mode, int indexCount, DrawElementsType indexType, int indexBufferOffset);
        void DrawElementsBaseVertex(BeginMode mode, int indexCount, DrawElementsType indexType, int indexBufferOffset, int baseVertex);
        void DrawElementsIndirect(BeginMode mode, DrawElementsType indexType, int argsBufferOffset);
        void DrawElementsInstanced(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount);
        void DrawElementsInstancedBaseInstance(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount, int baseInstance);
        void DrawElementsInstancedBaseVertex(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount, int baseVertex);
        void DrawElementsInstancedBaseVertexBaseInstance(BeginMode mode, int indexCountPerInstance, DrawElementsType indexType, int indexBufferOffset, int instanceCount, int baseVertex, int baseInstance);
        void DrawRangeElements(BeginMode mode, int minVertexIndex, int maxVertexIndex, int indexCount, DrawElementsType indexType, int indexBufferOffset);
        void DrawRangeElementsBaseVertex(BeginMode mode, int minVertexIndex, int maxVertexIndex, int indexCount, DrawElementsType indexType, int indexBufferOffset, int baseVertex);
        void DrawTransformFeedback(BeginMode mode, ITransformFeedback transformFeedback);
        void DrawTransformFeedbackInstanced(BeginMode mode, ITransformFeedback transformFeedback, int instanceCount);
        void DrawTransformFeedbackStream(BeginMode mode, ITransformFeedback transformFeedback, int stream);
        void DrawTransformFeedbackStreamInstanced(BeginMode mode, ITransformFeedback transformFeedback, int stream, int instanceCount);
        void BlitFramebuffer(IFramebuffer src, int srcX0, int srcY0, int srcX1, int srcY1, IFramebuffer dst, int dstX0, int dstY0, int dstX1, int dstY1, ClearBufferMask mask, BlitFramebufferFilter filter);
        void BeginTransformFeedback(ITransformFeedback transformFeedback, BeginFeedbackMode beginFeedbackMode);
        void EndTransformFeedback();
        void SwapBuffers();
    }
}