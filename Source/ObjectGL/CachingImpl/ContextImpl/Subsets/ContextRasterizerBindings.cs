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
using ObjectGL.Api.Context.Subsets;

namespace ObjectGL.CachingImpl.ContextImpl.Subsets
{
    public class ContextRasterizerBindings : IContextRasterizerBindings
    {
        public IBinding<PolygonMode> PolygonModeFront { get; set; }
        public IBinding<PolygonMode> PolygonModeBack { get; set; }
        public IBinding<CullFaceMode> CullFace { get; set; }
        public IBinding<FrontFaceDirection> FrontFace { get; set; }
        public IBinding<bool> CullFaceEnable { get; set; }
        public IBinding<bool> ScissorEnable { get; set; }
        public IBinding<bool> MultisampleEnable { get; set; }
        public IBinding<bool> LineSmoothEnable { get; set; }

        public ContextRasterizerBindings(IContext context)
        {
            PolygonModeFront = new Binding<PolygonMode>(context, (c, x) => c.GL.PolygonMode((int)All.Front, (int)x));
            PolygonModeBack = new Binding<PolygonMode>(context, (c, x) => c.GL.PolygonMode((int)All.Back, (int)x));
            CullFace = new Binding<CullFaceMode>(context, (c, x) => c.GL.CullFace((int)x));
            FrontFace = new Binding<FrontFaceDirection>(context, (c, x) => c.GL.FrontFace((int)x));
            CullFaceEnable = new EnableCapBinding(context, EnableCap.CullFace);
            ScissorEnable = new EnableCapBinding(context, EnableCap.ScissorTest);
            MultisampleEnable = new EnableCapBinding(context, EnableCap.Multisample);
            LineSmoothEnable = new EnableCapBinding(context, EnableCap.LineSmooth);
        }
    }
}