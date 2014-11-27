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

using ObjectGL.Api;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Subsets;

namespace ObjectGL.CachingImpl.ContextImpl.Subsets
{
    public class ContextStates : IContextStates
    {
        public IContextScreenClippingBindings ScreenClipping { get; private set; }
        public IContextRasterizerBindings Rasterizer { get; private set; }
        public IContextDepthStencilBindings DepthStencil { get; private set; }
        public IContextBlendBindings Blend { get; private set; }

        public IBinding<int> PatchVertexCount { get; private set; }
        public IBinding<int> UnpackAlignment { get; private set; }

        public ContextStates(IContext context, IImplementation implementation)
        {
            ScreenClipping = new ContextScreenClippingBindings(context, implementation);
            Rasterizer = new ContextRasterizerBindings(context);
            DepthStencil = new ContextDepthStencilBindings(context);
            Blend = new ContextBlendBindings(context, implementation);

            PatchVertexCount = new Binding<int>(context, (c, x) => { if (x != 0) c.GL.PatchParameter((int)All.PatchVertices, x); });
            UnpackAlignment = new Binding<int>(context, (c, x) => c.GL.PixelStore((int)All.UnpackAlignment, x));
        }
    }
}