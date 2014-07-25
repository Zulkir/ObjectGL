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
using ObjectGL.Api.Objects.Resources;

namespace ObjectGL.CachingImpl.Objects.Resources
{
    internal unsafe class Buffer : IBuffer
    {
        private readonly Context context;
        private readonly uint handle;
        private readonly BufferTarget creationTarget;
        private readonly int sizeInBytes;
        private readonly BufferUsageHint usage;

        private IGL GL { get { return context.GL; } }

        public uint Handle { get { return handle; } }
        public ContextObjectType ContextObjectType { get { return ContextObjectType.Resource; } }
        public ResourceType ResourceType { get { return ResourceType.Buffer; } }
        public BufferTarget CreationTarget { get { return creationTarget; } }
        public int SizeInBytes { get { return sizeInBytes; } }
        public BufferUsageHint Usage { get { return usage; } }

        public Buffer(Context context, BufferTarget creationTarget, int sizeInBytes, BufferUsageHint usage, IntPtr initialData)
        {
            this.context = context;
            this.creationTarget = creationTarget;
            this.sizeInBytes = sizeInBytes;
            this.usage = usage;

            uint handleProxy;
            GL.GenBuffers(1, &handleProxy);
            handle = handleProxy;

            context.BindBuffer(creationTarget, this);
            GL.BufferData((int)creationTarget, (IntPtr)sizeInBytes, initialData, (int)usage);
        }

        public void Recreate(BufferTarget editingTarget, IntPtr data)
        {
            context.BindBuffer(editingTarget, this);
            GL.BufferData((int)editingTarget, (IntPtr)sizeInBytes, data, (int)usage);
        }

        public void Dispose()
        {
            uint handleProxy = handle;
            GL.DeleteBuffers(1, &handleProxy);
        }
    }
}
