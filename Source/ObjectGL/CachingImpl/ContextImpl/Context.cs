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

using System.Collections.Generic;
using System.Linq;
using ObjectGL.Api;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Subsets;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;
using ObjectGL.CachingImpl.ContextImpl.Subsets;
using IContext = ObjectGL.Api.Context.IContext;

namespace ObjectGL.CachingImpl.ContextImpl
{
    public class Context : IContext
    {
        public IGL GL { get; private set; }
        public IImplementation Implementation { get; private set; }

        public IContextBufferBindings Buffers { get; private set; }
        public IContextTextureBindings Textures { get; private set; }
        public IContextFramebufferBindings Framebuffer { get; private set; }
        public IContextScreenClippingBindings ScreenClipping { get; private set; }
        public IContextRasterizerBindings Rasterizer { get; private set; }
        public IContextDepthStencilBindings DepthStencil { get; private set; }
        public IContextBlendBindings Blend { get; private set; }

        public IBinding<IShaderProgram> Program { get; private set; }
        public IBinding<int> PatchVertexCount { get; private set; }
        public IBinding<IVertexArray> VertexArray { get; private set; }
        public IBinding<ITransformFeedback> TransformFeedback { get; private set; }
        public IBinding<IRenderbuffer> Renderbuffer { get; private set; }
        public IBinding<int> UnpackAlignment { get; private set; }
        public IReadOnlyList<IBinding<ISampler>> Samplers { get; private set; }

        public Context(IGL gl, IImplementation implementation)
        {
            GL = gl;
            Implementation = implementation;

            Buffers = new ContextBufferBindings(this, implementation);
            Textures = new ContextTextureBindings(this, implementation);
            Framebuffer = new ContextFramebufferBindings(this);
            ScreenClipping = new ContextScreenClippingBindings(this, implementation);
            Rasterizer = new ContextRasterizerBindings(this);
            DepthStencil = new ContextDepthStencilBindings(this);
            Blend = new ContextBlendBindings(this, implementation);

            Program = new Binding<IShaderProgram>(this, (c, o) => c.GL.UseProgram(o.SafeGetHandle()));
            PatchVertexCount = new Binding<int>(this, (c, x) => { if (x != 0) c.GL.PatchParameter((int)All.PatchVertices, x); });
            VertexArray = new Binding<IVertexArray>(this, (c, o) => c.GL.BindVertexArray(o.SafeGetHandle()));
            TransformFeedback = new Binding<ITransformFeedback>(this, (c, o) => c.GL.BindTransformFeedback((int)All.TransformFeedback, o.SafeGetHandle()));
            Renderbuffer = new Binding<IRenderbuffer>(this, (c, o) => c.GL.BindRenderbuffer((int)RenderbufferTarget.Renderbuffer, o.SafeGetHandle()));
            UnpackAlignment = new Binding<int>(this, (c, x) => c.GL.PixelStore((int)All.UnpackAlignment, x));
            Samplers = Enumerable.Range(0, implementation.MaxCombinedTextureImageUnits)
                .Select(i => new Binding<ISampler>(this, (c, o) => c.GL.BindSampler((uint)i, o.SafeGetHandle())))
                .ToArray();
        }

        
    }
}