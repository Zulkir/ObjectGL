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

using OpenTK.Graphics.OpenGL;

namespace ObjectGL.GL42
{
    public class TransformFeedback : IContextObject
    {
        readonly int handle;

        readonly Buffer[] transformFeedbackBuffers;
        int enabledBufferRange;

        public int Handle { get { return handle; } }
        public ContextObjectType ContextObjectType { get { return ContextObjectType.TransformFeedback; } }

        public unsafe TransformFeedback(Context currentContext)
        {
            int handleProxy;
            GL.GenTransformFeedback(1, &handleProxy);
            handle = handleProxy;

            transformFeedbackBuffers = new Buffer[currentContext.Implementation.MaxTransformFeedbackBuffers];
        }

        public void SetBuffer(Context currentContext, int index, Buffer buffer)
        {
            if (transformFeedbackBuffers[index] == buffer)
                return;
            currentContext.BindTransformFeedback(this);
            GL.BindBufferBase(BufferTarget.TransformFeedbackBuffer, index, Helpers.ObjectHandle(buffer));
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

        public void UnsetBuffersStartingFrom(Context currentContext, int startIndex)
        {
            if (startIndex >= enabledBufferRange)
                return;
            for (int i = startIndex; i < enabledBufferRange; i++)
                SetBuffer(currentContext, i, null);
        }

        public unsafe void Dispose()
        {
            int handleProxy = handle;
            GL.DeleteVertexArrays(1, &handleProxy);
        }
    }
}
