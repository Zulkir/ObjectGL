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
using ObjectGL.CachingImpl.ContextImpl.Subsets;

namespace ObjectGL.CachingImpl.ContextImpl
{
    public class Context : IContext
    {
        public IGL GL { get; private set; }
        public INativeGraphicsContext NativeContext { get; private set; }
        public IImplementation Implementation { get; private set; }

        public IContextObjectFactory Create { get; private set; }
        public IContextBindings Bindings { get; private set; }
        public IContextStates States { get; private set; }
        public IContextActions Actions { get; private set; }

        public Context(IGL gl, INativeGraphicsContext nativeContext)
        {
            GL = gl;
            NativeContext = nativeContext;
            Implementation = new Implementation(gl);

            Create = new ContextObjectFactory(this);
            Bindings = new ContextBindings(this, Implementation);
            States = new ContextStates(this, Implementation);
            Actions = new ContextActions(this);
        }
    }
}