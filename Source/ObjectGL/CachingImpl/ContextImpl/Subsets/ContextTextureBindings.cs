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
using ObjectGL.Api;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Subsets;
using ObjectGL.Api.Objects.Resources;
using IContext = ObjectGL.Api.Context.IContext;

namespace ObjectGL.CachingImpl.ContextImpl.Subsets
{
    public class ContextTextureBindings : IContextTextureBindings
    {
        public IBinding<int> ActiveUnit { get; private set; }
        public IReadOnlyList<IBinding<ITexture>> Units { get; private set; }
        public int EditingIndex { get; set; }

        public ContextTextureBindings(IContext context, IImplementation implementation)
        {
            ActiveUnit = new Binding<int>(context, (c, x) => c.GL.ActiveTexture((int)All.Texture0 + x));
            Units = Enumerable.Range(0, implementation.MaxCombinedTextureImageUnits)
                .Select(i => new Binding<ITexture>(context, (c, o) =>
                {
                    c.Bindings.Textures.ActiveUnit.Set(i);
                    c.GL.BindTexture((int)o.Target, o.SafeGetHandle());
                }))
                .ToArray();
        }
    }
}