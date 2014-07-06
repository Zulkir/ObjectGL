#region License
/*
Copyright (c) 2010-2014 ObjectGL Project - Daniil Rodin

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
using ObjectGL.Api.Objects;
using ObjectGL.CachingImpl.Utilitties;

namespace ObjectGL.CachingImpl.ContextAspects
{
    internal class ContextFramebufferAspect
    {
        readonly RedundantObject<IFramebuffer> drawFramebufferBinding;
        readonly RedundantObject<IFramebuffer> readFramebufferBinding;

        public ContextFramebufferAspect(IGL gl)
        {
            drawFramebufferBinding = new RedundantObject<IFramebuffer>(gl, (g, o) => g.BindFramebuffer((int)All.DrawFramebuffer, o.SafeGetHandle()));
            readFramebufferBinding = new RedundantObject<IFramebuffer>(gl, (g, o) => g.BindFramebuffer((int)All.ReadFramebuffer, o.SafeGetHandle()));
        }

        public void BindDrawFramebuffer(IFramebuffer framebuffer)
        {
            drawFramebufferBinding.Set(framebuffer);
        }

        public void BindReadFramebuffer(IFramebuffer framebuffer)
        {
            readFramebufferBinding.Set(framebuffer);
        }

        public All BindAnyFramebuffer(IFramebuffer framebuffer)
        {
            if (drawFramebufferBinding.HasValueSet(framebuffer)) return All.DrawFramebuffer;
            if (readFramebufferBinding.HasValueSet(framebuffer)) return All.ReadFramebuffer;
            drawFramebufferBinding.Set(framebuffer);
            return All.DrawFramebuffer;
        }

        public void ConsumePipelineFramebuffer(IFramebuffer framebuffer)
        {
            drawFramebufferBinding.Set(framebuffer);
        }
    }
}
