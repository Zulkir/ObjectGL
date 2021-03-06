﻿#region License
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
using ObjectGL.Api.Context.ObjectBindings;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Framebuffers;

namespace ObjectGL.CachingImpl.Context.ObjectBindings
{
    public class ContextFramebufferBindings : IContextFramebufferBindings
    {
        public IBinding<IFramebuffer> Draw { get; private set; }
        public IBinding<IFramebuffer> Read { get; private set; }
        public FramebufferTarget EditingTarget { get; set; }

        public ContextFramebufferBindings(IContext context)
        {
            Draw = new Binding<IFramebuffer>(context, (c, o) => c.GL.BindFramebuffer((int)All.DrawFramebuffer, o.SafeGetHandle()));
            Read = new Binding<IFramebuffer>(context, (c, o) => c.GL.BindFramebuffer((int)All.ReadFramebuffer, o.SafeGetHandle()));
            EditingTarget = FramebufferTarget.Draw;
        }

        public IBinding<IFramebuffer> ByTarget(FramebufferTarget target)
        {
            return target == FramebufferTarget.Draw ? Draw : Read;
        }
    }
}