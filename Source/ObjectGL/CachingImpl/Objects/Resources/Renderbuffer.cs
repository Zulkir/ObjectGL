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

using ObjectGL.Api.Context;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;

namespace ObjectGL.CachingImpl.Objects.Resources
{
    internal class Renderbuffer : IRenderbuffer
    {
        private readonly IContext context;
        private readonly uint handle;

        private readonly Format internalFormat;

        private readonly int width;
        private readonly int height;
        private readonly int samples;

        private IGL GL { get { return context.GL; } }

        public uint Handle { get { return handle; } }
        public GLObjectType GLObjectType { get { return GLObjectType.Resource; } }
        public ResourceType ResourceType { get { return ResourceType.Renderbuffer; } }
        public RenderbufferTarget Target { get { return RenderbufferTarget.Renderbuffer; } }
        public Format InternalFormat { get { return internalFormat; } }

        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public int Samples { get { return samples; } }

        public unsafe Renderbuffer(IContext context, int width, int height, Format internalFormat, int samples = 0)
        {
            this.context = context;
            this.internalFormat = internalFormat;
            this.width = width;
            this.height = height;
            this.samples = samples;

            uint handleProxy;
            GL.GenRenderbuffers(1, &handleProxy);
            handle = handleProxy;

            context.Bindings.Renderbuffer.Set(this);

            if (samples == 0)
                GL.RenderbufferStorage((int)RenderbufferTarget.Renderbuffer, (int)internalFormat, width, height);
            else
                GL.RenderbufferStorageMultisample((int)RenderbufferTarget.Renderbuffer, samples, (int)internalFormat, width, height);
        }

        public unsafe void Dispose()
        {
            uint handleProxy = handle;
            GL.DeleteRenderbuffers(1, &handleProxy);
        }
    }
}