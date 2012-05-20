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

namespace ObjectGL.v42
{
    public class Buffer : IResource
    {
        readonly int handle;
        readonly BufferTarget creationTarget;
        readonly int sizeInBytes;
        readonly BufferUsageHint usage;

        public int Handle { get { return handle; } }
        public ContextObjectType ContextObjectType { get { return ContextObjectType.Resource; } }
        public ResourceType ResourceType { get { return ResourceType.Buffer; } }
        public BufferTarget CreationTarget { get { return creationTarget; } }
        public int SizeInBytes { get { return sizeInBytes; } }
        public BufferUsageHint Usage { get { return usage; } }

        public Buffer(Context currentContext, BufferTarget creationTarget, int sizeInBytes, BufferUsageHint usage)
            : this(currentContext, creationTarget, sizeInBytes, usage, IntPtr.Zero) { }

        public unsafe Buffer(Context currentContext, BufferTarget creationTarget, int sizeInBytes, BufferUsageHint usage, Data initialData)
        {
            this.creationTarget = creationTarget;
            this.sizeInBytes = sizeInBytes;
            this.usage = usage;

            int handleProxy;
            GL.GenBuffers(1, &handleProxy);
            handle = handleProxy;

            currentContext.BindBuffer(creationTarget, this);
            GL.BufferData(creationTarget, (IntPtr)sizeInBytes, initialData.Pointer, usage);
            initialData.UnpinPointer();
        }

        public void SetData(Context currentContext, BufferTarget editingTarget, IntPtr data)
        {
            currentContext.BindBuffer(editingTarget, this);
            GL.BufferData(editingTarget, (IntPtr)sizeInBytes, data, usage);
        }

        public unsafe void Dispose()
        {
            int handleProxy = handle;
            GL.DeleteBuffers(1, &handleProxy);
        }
    }
}
