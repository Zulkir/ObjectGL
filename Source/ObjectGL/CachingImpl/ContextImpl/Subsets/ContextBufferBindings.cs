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
using System.Collections.Generic;
using System.Linq;
using ObjectGL.Api;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Subsets;
using ObjectGL.Api.Objects.Resources;
using IContext = ObjectGL.Api.Context.IContext;

namespace ObjectGL.CachingImpl.ContextImpl.Subsets
{
    public class ContextBufferBindings : IContextBufferBindings
    {
        private readonly BufferBinding array;
        private readonly BufferBinding copyRead;
        private readonly BufferBinding copyWrite;
        private readonly BufferBinding elementArray;
        private readonly BufferBinding pixelPack;
        private readonly BufferBinding pixelUnpack;
        private readonly BufferBinding texture;
        private readonly BufferBinding drawIndirect;
        private readonly BufferBinding transformFeedback;
        private readonly BufferBinding uniform;
        private readonly Binding<BufferRange>[] uniformIndexed;

        public ContextBufferBindings(IContext context, IImplementation implementation)
        {
            array = new BufferBinding(context, BufferTarget.ArrayBuffer);
            copyRead = new BufferBinding(context, BufferTarget.CopyReadBuffer);
            copyWrite = new BufferBinding(context, BufferTarget.CopyWriteBuffer);
            elementArray = new BufferBinding(context, BufferTarget.ElementArrayBuffer);
            pixelPack = new BufferBinding(context, BufferTarget.PixelPackBuffer);
            pixelUnpack = new BufferBinding(context, BufferTarget.PixelUnpackBuffer);
            texture = new BufferBinding(context, BufferTarget.TextureBuffer);
            drawIndirect = new BufferBinding(context, BufferTarget.DrawIndirectBuffer);
            transformFeedback = new BufferBinding(context, BufferTarget.TransformFeedbackBuffer);
            uniform = new BufferBinding(context, BufferTarget.UniformBuffer);

            uniformIndexed = Enumerable.Range(0, implementation.MaxUniformBufferBindings)
                .Select(i => new Binding<BufferRange>(context, (c, o) =>
                {
                    if (o.Buffer == null || o.Offset == 0 && o.Size == o.Buffer.SizeInBytes)
                        c.GL.BindBufferBase((int)BufferTarget.UniformBuffer, (uint)i, o.Buffer.SafeGetHandle());
                    else
                        c.GL.BindBufferRange((int)BufferTarget.UniformBuffer, (uint)i, o.Buffer.SafeGetHandle(), (IntPtr)o.Offset, (IntPtr)o.Size);
                }))
                .ToArray();
        }

        public IBinding<IBuffer> Array { get { return array; } }
        public IBinding<IBuffer> CopyRead { get { return copyRead; } }
        public IBinding<IBuffer> CopyWrite { get { return copyWrite; } }
        public IBinding<IBuffer> ElementArray { get { return elementArray; } }
        public IBinding<IBuffer> PixelPack { get { return pixelPack; } }
        public IBinding<IBuffer> PixelUnpack { get { return pixelUnpack; } }
        public IBinding<IBuffer> Texture { get { return texture; } }
        public IBinding<IBuffer> DrawIndirect { get { return drawIndirect; } }
        public IBinding<IBuffer> TransformFeedback { get { return transformFeedback; } }
        public IBinding<IBuffer> Uniform { get { return uniform; } }
        public IReadOnlyList<IBinding<BufferRange>> UniformIndexed { get { return uniformIndexed; } }
    }
}