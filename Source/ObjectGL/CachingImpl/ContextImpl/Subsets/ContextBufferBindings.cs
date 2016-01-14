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
using System.Collections.Generic;
using System.Linq;
using ObjectGL.Api;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Subsets;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;
using IContext = ObjectGL.Api.Context.IContext;

namespace ObjectGL.CachingImpl.ContextImpl.Subsets
{
    public class ContextBufferBindings : IContextBufferBindings
    {
        public IBinding<IBuffer> Array { get; private set; }
        public IBinding<IBuffer> CopyRead { get; private set; }
        public IBinding<IBuffer> CopyWrite { get; private set; }
        public IBinding<IBuffer> ElementArray { get; private set; }
        public IBinding<IBuffer> PixelPack { get; private set; }
        public IBinding<IBuffer> PixelUnpack { get; private set; }
        public IBinding<IBuffer> Texture { get; private set; }
        public IBinding<IBuffer> DrawIndirect { get; private set; }
        public IBinding<IBuffer> TransformFeedback { get; private set; }
        public IBinding<IBuffer> Uniform { get; private set; }
        public IBinding<IBuffer> ShaderStorage { get; private set; }
        public IBinding<IBuffer> DispatchIndirect { get; private set; }
        public IBinding<IBuffer> Query { get; private set; }
        public IBinding<IBuffer> AtomicCounter { get; private set; }
        public IReadOnlyList<IBinding<BufferRange>> UniformIndexed { get; private set; }

        public ContextBufferBindings(IContext context, IImplementation implementation)
        {
            Array = new BufferBinding(context, BufferTarget.Array);
            CopyRead = new BufferBinding(context, BufferTarget.CopyRead);
            CopyWrite = new BufferBinding(context, BufferTarget.CopyWrite);
            ElementArray = new Binding<IBuffer>(context, (c, o) =>
            {
                c.Bindings.VertexArray.Set(null);
                c.GL.BindBuffer((int)All.ElementArrayBuffer, o.SafeGetHandle());
            });
            PixelPack = new BufferBinding(context, BufferTarget.PixelPack);
            PixelUnpack = new BufferBinding(context, BufferTarget.PixelUnpack);
            Texture = new BufferBinding(context, BufferTarget.Texture);
            DrawIndirect = new BufferBinding(context, BufferTarget.DrawIndirect);
            TransformFeedback = new Binding<IBuffer>(context, (c, o) =>
            {
                c.Bindings.TransformFeedback.Set(null);
                c.GL.BindBuffer((int)All.TransformFeedbackBuffer, o.SafeGetHandle());
            });
            Uniform = new BufferBinding(context, BufferTarget.Uniform);
            ShaderStorage = new BufferBinding(context, BufferTarget.ShaderStorage);
            DispatchIndirect = new BufferBinding(context, BufferTarget.DispatchIndirect);
            Query = new BufferBinding(context, BufferTarget.Query);
            AtomicCounter = new BufferBinding(context, BufferTarget.AtomicCounter);
            
            UniformIndexed = Enumerable.Range(0, implementation.MaxUniformBufferBindings)
                .Select(i => new Binding<BufferRange>(context, (c, o) =>
                {
                    if (o.Buffer == null || o.Offset == 0 && o.Size == o.Buffer.SizeInBytes)
                        c.GL.BindBufferBase((int)BufferTarget.Uniform, (uint)i, o.Buffer.SafeGetHandle());
                    else
                        c.GL.BindBufferRange((int)BufferTarget.Uniform, (uint)i, o.Buffer.SafeGetHandle(), (IntPtr)o.Offset, (IntPtr)o.Size);
                }))
                .ToArray();
        }

        public IBinding<IBuffer> ByTarget(BufferTarget target)
        {
            switch (target)
            {
                case BufferTarget.Array: return Array;
                case BufferTarget.ElementArray: return ElementArray;
                case BufferTarget.PixelPack: return PixelPack;
                case BufferTarget.PixelUnpack: return PixelUnpack;
                case BufferTarget.Uniform: return Uniform;
                case BufferTarget.Texture: return Texture;
                case BufferTarget.TransformFeedback: return TransformFeedback;
                case BufferTarget.CopyRead: return CopyRead;
                case BufferTarget.CopyWrite: return CopyWrite;
                case BufferTarget.DrawIndirect: return DrawIndirect;
                case BufferTarget.ShaderStorage: return ShaderStorage;
                case BufferTarget.DispatchIndirect: return DispatchIndirect;
                case BufferTarget.Query: return Query;
                case BufferTarget.AtomicCounter: return AtomicCounter;
                default: throw new ArgumentOutOfRangeException("target");
            }
        }
    }
}