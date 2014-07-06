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

namespace ObjectGL.CachingImpl.Objects
{
    internal class VertexArray : IVertexArray
    {
        private readonly Context context;
        private readonly uint handle;

        private readonly VertexAttributeDescription[] vertexAttributes;
        private uint enabledVertexAttributesRange;
        private IBuffer elementArrayBuffer;

        private IGL GL { get { return context.GL; } }

        public uint Handle { get { return handle; } }
        public ContextObjectType ContextObjectType { get { return ContextObjectType.VertexArray; } }

        public unsafe VertexArray(Context context)
        {
            this.context = context;

            uint handleProxy;
            GL.GenVertexArrays(1, &handleProxy);
            handle = handleProxy;

            elementArrayBuffer = null;
            vertexAttributes = new VertexAttributeDescription[context.Implementation.MaxVertexAttributes];
        }

        public void SetElementArrayBuffer(IBuffer buffer)
        {
            if (elementArrayBuffer == buffer) 
                return;
            context.BindVertexArray(this);
            GL.BindBuffer((int)BufferTarget.ElementArrayBuffer, buffer.SafeGetHandle());
            elementArrayBuffer = buffer;
        }

        public void SetVertexAttributeF(uint index, IBuffer buffer, VertexAttributeDimension dimension, VertexAttribPointerType type, bool normalized, int stride, int offset, uint divisor = 0)
        {
            var newDesc = new VertexAttributeDescription
            {
                IsEnabled = true,
                SetFunction = All.Float,
                Dimension = dimension,
                Type = (int)type,
                IsNormalized = normalized,
                Stride = stride,
                Offset = offset,
                Divisor = divisor,
                Buffer = buffer
            };

            if (VertexAttributeDescription.Equals(ref vertexAttributes[index], ref newDesc)) return;

            context.BindVertexArray(this);

            if (!vertexAttributes[index].IsEnabled)
                GL.EnableVertexAttribArray(index);

            context.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.VertexAttribPointer(index, (int)dimension, (int)type, normalized, stride, (IntPtr)offset);

            if (vertexAttributes[index].Divisor != divisor)
                GL.VertexAttribDivisor(index, divisor);

            vertexAttributes[index] = newDesc;

            if (index >= enabledVertexAttributesRange)
                enabledVertexAttributesRange = index + 1;
        }

        public void SetVertexAttributeI(uint index, IBuffer buffer, VertexAttributeDimension dimension, VertexAttribIPointerType type, int stride, int offset, uint divisor = 0)
        {
            var newDesc = new VertexAttributeDescription
            {
                IsEnabled = true,
                SetFunction = All.Int,
                Dimension = dimension,
                Type = (int)type,
                Stride = stride,
                Offset = offset,
                Divisor = divisor,
                Buffer = buffer
            };

            if (VertexAttributeDescription.Equals(ref vertexAttributes[index], ref newDesc)) return;

            context.BindVertexArray(this);

            if (!vertexAttributes[index].IsEnabled)
                GL.EnableVertexAttribArray(index);

            context.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.VertexAttribIPointer(index, (int)dimension, (int)type, stride, (IntPtr)offset);

            if (vertexAttributes[index].Divisor != divisor)
                GL.VertexAttribDivisor(index, divisor);

            vertexAttributes[index] = newDesc;

            if (index >= enabledVertexAttributesRange)
                enabledVertexAttributesRange = index + 1;
        }

        public void DisableVertexAttribute(uint index)
        {
            if (index >= enabledVertexAttributesRange) return;
            if (!vertexAttributes[index].IsEnabled) return;

            context.BindVertexArray(this);
            GL.DisableVertexAttribArray(index);

            vertexAttributes[index].IsEnabled = false;

            if (index == enabledVertexAttributesRange - 1)
                while (enabledVertexAttributesRange > 0 && !vertexAttributes[enabledVertexAttributesRange - 1].IsEnabled)
                    enabledVertexAttributesRange--;
        }

        public void DisableVertexAttributesStartingFrom(uint startIndex)
        {
            if (startIndex >= enabledVertexAttributesRange) 
                return;
            for (uint i = startIndex; i < enabledVertexAttributesRange; i++)
                DisableVertexAttribute(i);
        }

        public unsafe void Dispose()
        {
            uint handleProxy = handle;
            GL.DeleteVertexArrays(1, &handleProxy);
        }
    }
}
