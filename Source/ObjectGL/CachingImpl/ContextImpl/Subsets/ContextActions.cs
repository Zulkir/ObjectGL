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
using ObjectGL.Api.Objects;

namespace ObjectGL.CachingImpl.ContextImpl.Subsets
{
    public class ContextActions : IContextActions
    {
        private readonly IContext context;

        public IContextDrawActions Draw { get; private set; }

        public ContextActions(IContext context)
        {
            this.context = context;
            Draw = new ContextDrawActions(context.GL);
        }

        public unsafe void ClearWindowColor(Color4 color)
        {
            context.Bindings.Framebuffers.Draw.Set(null);
            context.GL.ClearBuffer((int)All.Color, 0, (float*)&color);
        }

        public void ClearWindowDepthStencil(DepthStencil target, float depth, int stencil)
        {
            context.Bindings.Framebuffers.Draw.Set(null);
            context.GL.ClearBuffer((int)target, 0, depth, stencil);
        }

        public void BlitFramebuffer(IFramebuffer src, IFramebuffer dst, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, ClearBufferMask mask, BlitFramebufferFilter filter)
        {
            context.Bindings.Framebuffers.Read.Set(src);
            context.Bindings.Framebuffers.Draw.Set(dst);
            context.GL.BlitFramebuffer(srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, (uint)mask, (int)filter);
        }

        public void BeginTransformFeedback(ITransformFeedback transformFeedback, BeginFeedbackMode beginFeedbackMode)
        {
            context.Bindings.TransformFeedback.Set(transformFeedback);
            context.GL.BeginTransformFeedback((int)beginFeedbackMode);
        }

        public void EndTransformFeedback()
        {
            context.GL.EndTransformFeedback();
        }
    }
}