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

using ObjectGL.Api;
using ObjectGL.Api.Context;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;

namespace ObjectGL.CachingImpl.Objects
{
    internal class TransformFeedback : ITransformFeedback
    {
        private readonly IContext context;
        private readonly uint handle;

        private readonly IBuffer[] transformFeedbackBuffers;
        private uint enabledBufferRange;

        private IGL GL { get { return context.GL; } }

        public uint Handle { get { return handle; } }
        public GLObjectType GLObjectType { get { return GLObjectType.TransformFeedback; } }

        public unsafe TransformFeedback(IContext context)
        {
            this.context = context;
            uint handleProxy;
            GL.GenTransformFeedbacks(1, &handleProxy);
            handle = handleProxy;

            transformFeedbackBuffers = new IBuffer[context.Implementation.MaxTransformFeedbackBuffers];
        }

        public void SetBuffer(uint index, IBuffer buffer)
        {
            if (transformFeedbackBuffers[index] == buffer)
                return;
            context.Bindings.TransformFeedback.Set(this);
            GL.BindBufferBase((int)BufferTarget.TransformFeedback, index, buffer.SafeGetHandle());
            transformFeedbackBuffers[index] = buffer;

            if (buffer != null)
            {
                if (index >= enabledBufferRange)
                    enabledBufferRange = index + 1;
            }
            else
            {
                if (index == enabledBufferRange - 1)
                    while (enabledBufferRange > 0 && transformFeedbackBuffers[enabledBufferRange - 1] == null)
                        enabledBufferRange--;
            }
        }

        public void UnsetBuffersStartingFrom(uint startIndex)
        {
            if (startIndex >= enabledBufferRange)
                return;
            for (uint i = startIndex; i < enabledBufferRange; i++)
                SetBuffer(i, null);
        }

        public unsafe void Dispose()
        {
            uint handleProxy = handle;
            GL.DeleteTransformFeedbacks(1, &handleProxy);
        }
    }
}
