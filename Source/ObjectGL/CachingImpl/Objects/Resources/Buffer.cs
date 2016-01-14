#region License
/*
Copyright (c) 2012-2016 ObjectGL Project - Daniil Rodin

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
using ObjectGL.Api.Context;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;

namespace ObjectGL.CachingImpl.Objects.Resources
{
    internal unsafe class Buffer : IBuffer
    {
        private readonly IContext context;
        private readonly uint handle;
        private readonly BufferTarget target;
        private readonly int sizeInBytes;
        private readonly BufferUsageHint usage;

        private IGL GL { get { return context.GL; } }

        public uint Handle { get { return handle; } }
        public GLObjectType GLObjectType { get { return GLObjectType.Resource; } }
        public ResourceType ResourceType { get { return ResourceType.Buffer; } }
        public BufferTarget Target { get { return target; } }
        public int SizeInBytes { get { return sizeInBytes; } }
        public BufferUsageHint Usage { get { return usage; } }

        public Buffer(IContext context, BufferTarget target, int sizeInBytes, BufferUsageHint usage, IntPtr initialData)
        {
            this.context = context;
            this.target = target;
            this.sizeInBytes = sizeInBytes;
            this.usage = usage;

            uint handleProxy;
            GL.GenBuffers(1, &handleProxy);
            handle = handleProxy;

            context.Bindings.Buffers.ByTarget(target).Set(this);
            GL.BufferData((int)target, (IntPtr)sizeInBytes, initialData, (int)usage);
        }

        public void Dispose()
        {
            uint handleProxy = handle;
            GL.DeleteBuffers(1, &handleProxy);
        }

        public IntPtr Map(int offset, int length, MapAccess access)
        {
            context.Bindings.Buffers.ByTarget(target).Set(this);
            return GL.MapBufferRange((int)target, (IntPtr)offset, (IntPtr)length, (int)access);
        }

        public bool Unmap()
        {
            context.Bindings.Buffers.ByTarget(target).Set(this);
            return GL.UnmapBuffer((int)target);
        }

        public void SetData(int offset, int size, IntPtr data)
        {
            context.Bindings.Buffers.ByTarget(target).Set(this);
            GL.BufferSubData((int)target, (IntPtr)offset, (IntPtr)size, data);
        }

        public void Recreate(IntPtr data)
        {
            context.Bindings.Buffers.ByTarget(target).Set(this);
            GL.BufferData((int)target, (IntPtr)sizeInBytes, data, (int)usage);
        }
    }
}
