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
    public class VertexArray : IDisposable
    {
        readonly int handle;

        readonly VertexAttributeDescription[] vertexAttributes;
        int enabledVertexAttributesRange;
        Buffer elementArrayBuffer;

        public int Handle { get { return handle; } }

        public unsafe VertexArray(Context currentContext)
        {
            int handleProxy;
            GL.GenVertexArrays(1, &handleProxy);
            handle = handleProxy;

            elementArrayBuffer = null;
            vertexAttributes = new VertexAttributeDescription[currentContext.Capabilities.MaxVertexAttributes];
        }

        public void SetElementArrayBuffer(Context currentContext, Buffer buffer)
        {
            if (elementArrayBuffer == buffer) return;

            currentContext.BindVertexArray(handle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer != null ? buffer.Handle : 0);
            elementArrayBuffer = buffer;
        }

        public void DisableVertexAttribute(Context currentContext, int index)
        {
            if (index >= enabledVertexAttributesRange) return;
            if (!vertexAttributes[index].IsEnabled) return;

            currentContext.BindVertexArray(handle);
            GL.DisableVertexAttribArray(index);

            vertexAttributes[index].IsEnabled = false;

            if (enabledVertexAttributesRange == index)
            {
                while (enabledVertexAttributesRange > 0 && !vertexAttributes[enabledVertexAttributesRange - 1].IsEnabled)
                {
                    enabledVertexAttributesRange--;
                }   
            }
        }

        public void DisableVertexAttributesStartingFrom(Context currentContext, int startIndex)
        {
            if (startIndex >= enabledVertexAttributesRange) return;

            for (int i = startIndex; i < vertexAttributes.Length; i++)
            {
                DisableVertexAttribute(currentContext, i);
            }

            enabledVertexAttributesRange = startIndex;
            while (enabledVertexAttributesRange > 0 && !vertexAttributes[enabledVertexAttributesRange - 1].IsEnabled)
            {
                enabledVertexAttributesRange--;
            }   
        }

        public void SetVertexAttributeF(Context currentContext, int index, Buffer buffer,
            byte dimension, VertexAttribPointerType type, bool normalized, int stride, int offset)
        {
            var newDesc = new VertexAttributeDescription
            {
                IsEnabled = true,
                SetFunction = VertexArraySetFunction.Float,
                Dimension = dimension,
                Type = (int)type,
                IsNormalized = normalized,
                Stride = stride,
                Offset = offset,
                Buffer = buffer
            };

            if (VertexAttributeDescription.Equals(ref vertexAttributes[index], ref newDesc)) return;

            currentContext.BindVertexArray(handle);

            if (!vertexAttributes[index].IsEnabled)
                GL.EnableVertexAttribArray(index);

            currentContext.BindBuffer(BufferTarget.ArrayBuffer, buffer.Handle);
            GL.VertexAttribPointer(index, dimension, type, normalized, stride, offset);

            vertexAttributes[index] = newDesc;

            if (index >= enabledVertexAttributesRange)
                enabledVertexAttributesRange = index + 1;
        }

        public void SetVertexAttributeI(Context currentContext, int index, Buffer buffer,
            byte dimension, VertexAttribIPointerType type, int stride, int offset)
        {
            var newDesc = new VertexAttributeDescription
            {
                IsEnabled = true,
                SetFunction = VertexArraySetFunction.Int,
                Dimension = dimension,
                Type = (int)type,
                Stride = stride,
                Offset = offset,
                Buffer = buffer
            };

            if (VertexAttributeDescription.Equals(ref vertexAttributes[index], ref newDesc)) return;

            currentContext.BindVertexArray(handle);

            if (!vertexAttributes[index].IsEnabled)
                GL.EnableVertexAttribArray(index);

            currentContext.BindBuffer(BufferTarget.ArrayBuffer, buffer.Handle);
            GL.VertexAttribIPointer(index, dimension, type, stride, (IntPtr)offset);

            vertexAttributes[index] = newDesc;

            if (index >= enabledVertexAttributesRange)
                enabledVertexAttributesRange = index + 1;
        }

        public unsafe void Dispose()
        {
            int handleProxy = handle;
            GL.DeleteVertexArrays(1, &handleProxy);
        }
    }
}
