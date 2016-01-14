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
using ObjectGL.Api.Context.States.DepthStencil;

namespace ObjectGL.CachingImpl.Context.States
{
    public class ContextDepthStencilSideBindings : IContextDepthStencilSideBindings
    {
        public IBinding<StencilFunctionSettings> StencilFunctionSettings { get; private set; }
        public IBinding<StencilOperationSettings> StencilOperationSettings { get; private set; }
        public IBinding<int> StencilWriteMask { get; set; }

        public ContextDepthStencilSideBindings(IContext context, int face)
        {
            StencilFunctionSettings = new Binding<StencilFunctionSettings>(context, (c, x) => c.GL.StencilFuncSeparate(face, (int)x.Function, x.Reference, x.Mask));
            StencilOperationSettings = new Binding<StencilOperationSettings>(context, (c, x) => c.GL.StencilOpSeparate(face, (int)x.StencilFail, (int)x.DepthFail, (int)x.DepthPass));
            StencilWriteMask = new Binding<int>(context, (c, x) => c.GL.StencilMaskSeparate(face, (uint)x));
        }
    }
}