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

using System.Collections.Generic;
using System.Linq;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.ObjectBindings;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources.Images;
using ObjectGL.Api.Objects.Samplers;
using ObjectGL.Api.Objects.Shaders;
using ObjectGL.Api.Objects.TransformFeedbacks;
using ObjectGL.Api.Objects.VertexArrays;

namespace ObjectGL.CachingImpl.Context.ObjectBindings
{
    public class ContextBindings : IContextBindings
    {
        public IContextBufferBindings Buffers { get; private set; }
        public IContextTextureBindings Textures { get; private set; }
        public IContextFramebufferBindings Framebuffers { get; private set; }
        public IBinding<IShaderProgram> Program { get; private set; }
        public IBinding<IVertexArray> VertexArray { get; private set; }
        public IBinding<ITransformFeedback> TransformFeedback { get; private set; }
        public IBinding<IRenderbuffer> Renderbuffer { get; private set; }
        public IReadOnlyList<IBinding<ISampler>> Samplers { get; private set; }

        public ContextBindings(IContext context, IContextCaps caps)
        {
            Buffers = new ContextBufferBindings(context, caps);
            Textures = new ContextTextureBindings(context, caps);
            Framebuffers = new ContextFramebufferBindings(context);
            Program = new Binding<IShaderProgram>(context, (c, o) => c.GL.UseProgram(o.SafeGetHandle()));
            VertexArray = new Binding<IVertexArray>(context, (c, o) => c.GL.BindVertexArray(o.SafeGetHandle()));
            TransformFeedback = new Binding<ITransformFeedback>(context, (c, o) => c.GL.BindTransformFeedback((int)All.TransformFeedback, o.SafeGetHandle()));
            Renderbuffer = new Binding<IRenderbuffer>(context, (c, o) => c.GL.BindRenderbuffer((int)RenderbufferTarget.Renderbuffer, o.SafeGetHandle()));
            Samplers = Enumerable.Range(0, caps.MaxCombinedTextureImageUnits)
                .Select(i => new Binding<ISampler>(context, (c, o) => c.GL.BindSampler((uint)i, o.SafeGetHandle())))
                .ToArray();
        }
    }
}