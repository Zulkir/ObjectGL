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

using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Subsets;

namespace ObjectGL.CachingImpl.ContextImpl.Subsets
{
    public class RawContextDepthStencilBindings : IContextDepthStencilBindings
    {
        public IContextDepthStencilSideBindings Front { get; private set; }
        public IContextDepthStencilSideBindings Back { get; private set; }
        public IBinding<bool> DepthTestEnable { get; set; }
        public IBinding<bool> DepthMask { get; set; }
        public IBinding<DepthFunction> DepthFunc { get; set; }
        public IBinding<bool> StencilTestEnable { get; set; }

        public RawContextDepthStencilBindings(IContext context)
        {
            Front = new RawContextDepthStencilSideBindings(context, (int)All.Front);
            Back = new RawContextDepthStencilSideBindings(context, (int)All.Back);
            DepthTestEnable = new EnableCapBinding(context, EnableCap.DepthTest);
            DepthMask = new Binding<bool>(context, (c, x) => c.GL.DepthMask(x));
            DepthFunc = new Binding<DepthFunction>(context, (c, x) => c.GL.DepthFunc((int)x));
            StencilTestEnable = new EnableCapBinding(context, EnableCap.StencilTest);
        }
    }
}