#region License
/*
Copyright (c) 2012 Daniil Rodin

This software is provided 'as-is', without any express or implied
warranty. In no event will the authors be held liable for any damages
arising from the use of this software.

Permission is granted to anyone to use this software for any purpose,
including commercial applications, and to alter it and redistribute it
freely, subject to the following restrictions:

   1. The origin of this software must not be misrepresented; you must not
   claim that you wrote the original software. If you use this software
   in a product, an acknowledgment in the product documentation would be
   appreciated but is not required.

   2. Altered source versions must be plainly marked as such, and must not be
   misrepresented as being the original software.

   3. This notice may not be removed or altered from any source
   distribution.
*/
#endregion

using System;
using OpenTK.Graphics.OpenGL;

namespace ObjectGL
{
    public class Framebuffer : IDisposable
    {
        delegate void GLFramebufferSomething(FramebufferTarget framebufferTarget, FramebufferAttachment framebufferAttachment, ref FramebufferAttachmentDescription description);

        readonly int handle;

        readonly FramebufferAttachmentDescription[] colorAttachments;
        int enabledColorAttachmentsRange;

        FramebufferAttachmentDescription depthAttachment;
        FramebufferAttachmentDescription stencilAttachment;

        public int Handle { get { return handle; } }

        public unsafe Framebuffer(Context currentContext)
        {
            int handleProxy;
            GL.GenFramebuffers(1, &handleProxy);
            handle = handleProxy;

            colorAttachments = new FramebufferAttachmentDescription[currentContext.Capabilities.MaxColorAttachments];
        }

        #region Color
        void AttachColor(Context currentContext, int index, ref FramebufferAttachmentDescription newDesc, GLFramebufferSomething glFramebufferSomething)
        {
            if (FramebufferAttachmentDescription.Equals(ref newDesc, ref colorAttachments[index])) return;

            var framebufferTarget = currentContext.BindAnyFramebuffer(handle);
            glFramebufferSomething(framebufferTarget, (FramebufferAttachment)((int)FramebufferAttachment.ColorAttachment0 + index), ref newDesc);
            colorAttachments[index] = newDesc;

            if (index >= enabledColorAttachmentsRange)
                enabledColorAttachmentsRange = index + 1;
        }

        public void AttachColorRenderbuffer(Context currentContext, int index, Renderbuffer renderbuffer)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Renderbufer,
                Renderbuffer = renderbuffer,
            };

            AttachColor(currentContext, index, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferRenderbuffer(ft, fa, RenderbufferTarget.Renderbuffer, d.Renderbuffer.Handle));
        }

        public void AttachColorTexture1D(Context currentContext, int index, Texture1D texture, int level)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture1D,
                Texture = texture,
                Level = level
            };

            AttachColor(currentContext, index, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTexture1D(ft, fa, d.TextureTarget, d.Texture.Handle, d.Level));
        }

        public void AttachColorTexture2D(Context currentContext, int index, Texture2D texture, int level)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture2D,
                Texture = texture,
                Level = level
            };

            AttachColor(currentContext, index, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTexture2D(ft, fa, d.TextureTarget, d.Texture.Handle, d.Level));
        }

        public void AttachColorTexture3D(Context currentContext, int index, Texture3D texture, int level, int depthLayer)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture3D,
                Texture = texture,
                Level = level,
                Layer = depthLayer
            };

            AttachColor(currentContext, index, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTextureLayer(ft, fa, d.Texture.Handle, d.Level, d.Layer));
        }

        public void DetachColor(Context currentContext, int index)
        {
            if (colorAttachments[index].Type == FramebufferAttachmentType.Disabled) return;

            var framebufferTarget = currentContext.BindAnyFramebuffer(handle);
            GL.FramebufferTexture2D(framebufferTarget, (FramebufferAttachment)((int)FramebufferAttachment.ColorAttachment0 + index), TextureTarget.Texture2D, 0, 0);
            colorAttachments[index].Type = FramebufferAttachmentType.Disabled;

            if (enabledColorAttachmentsRange == index)
            {
                while (enabledColorAttachmentsRange > 0 && colorAttachments[enabledColorAttachmentsRange - 1].Type == FramebufferAttachmentType.Disabled)
                {
                    enabledColorAttachmentsRange--;
                }
            }
        }

        public void DetachColorStartingFrom(Context currentContext, int startIndex)
        {
            if (startIndex >= enabledColorAttachmentsRange) return;

            for (int i = startIndex; i < colorAttachments.Length; i++)
            {
                DetachColor(currentContext, i);
            }

            enabledColorAttachmentsRange = startIndex;
            while (enabledColorAttachmentsRange > 0 && colorAttachments[enabledColorAttachmentsRange - 1].Type == FramebufferAttachmentType.Disabled)
            {
                enabledColorAttachmentsRange--;
            }
        }
        #endregion

        #region DepthStencil
        void AttachDepthStencil(Context currentContext, DepthStencil target, ref FramebufferAttachmentDescription newDesc, GLFramebufferSomething glFramebufferSomething)
        {
            if (target == DepthStencil.None) throw new ArgumentException("'target' cannot be 'None'");

            if (((target & DepthStencil.Depth) == 0   || FramebufferAttachmentDescription.Equals(ref newDesc, ref depthAttachment)) &&
                ((target & DepthStencil.Stencil) == 0 || FramebufferAttachmentDescription.Equals(ref newDesc, ref stencilAttachment))) return;

            var framebufferTarget = currentContext.BindAnyFramebuffer(handle);
            var framebufferAttachment = target == DepthStencil.Both 
                ? FramebufferAttachment.DepthStencilAttachment
                : target == DepthStencil.Depth
                ? FramebufferAttachment.DepthAttachment
                : target == DepthStencil.Stencil
                ? FramebufferAttachment.StencilAttachment
                : 0;
            glFramebufferSomething(framebufferTarget, framebufferAttachment, ref newDesc);

            if ((target & DepthStencil.Depth) != 0)
                depthAttachment = newDesc;

            if ((target & DepthStencil.Stencil) != 0)
                stencilAttachment = newDesc;
        }

        public void AttachDepthStencilRenderbuffer(Context currentContext, DepthStencil target, Renderbuffer renderbuffer)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Renderbufer,
                Renderbuffer = renderbuffer,
            };

            AttachDepthStencil(currentContext, target, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferRenderbuffer(ft, fa, RenderbufferTarget.Renderbuffer, d.Renderbuffer.Handle));
        }

        public void AttachDepthStencilTexture1D(Context currentContext, DepthStencil target, Texture1D texture, int level)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture1D,
                Texture = texture,
                Level = level
            };

            AttachDepthStencil(currentContext, target, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTexture1D(ft, fa, d.TextureTarget, d.Texture.Handle, d.Level));
        }

        public void AttachDepthStencilTexture2D(Context currentContext, DepthStencil target, Texture2D texture, int level)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture2D,
                Texture = texture,
                Level = level
            };

            AttachDepthStencil(currentContext, target, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTexture2D(ft, fa, d.TextureTarget, d.Texture.Handle, d.Level));
        }

        public void AttachDepthStencilTexture3D(Context currentContext, DepthStencil target, Texture3D texture, int level, int depthLayer)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture3D,
                Texture = texture,
                Level = level,
                Layer = depthLayer
            };

            AttachDepthStencil(currentContext, target, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTextureLayer(ft, fa, d.Texture.Handle, d.Level, d.Layer));
        }

        public void DetachColor(Context currentContext, DepthStencil target)
        {
            if (target == DepthStencil.None) throw new ArgumentException("'target' cannot be 'None'");

            if (((target & DepthStencil.Depth) == 0 || depthAttachment.Type == FramebufferAttachmentType.Disabled) &&
                ((target & DepthStencil.Stencil) == 0 || stencilAttachment.Type == FramebufferAttachmentType.Disabled)) return;

            var framebufferTarget = currentContext.BindAnyFramebuffer(handle);
            var framebufferAttachment = target == DepthStencil.Both
                ? FramebufferAttachment.DepthStencilAttachment
                : target == DepthStencil.Depth
                ? FramebufferAttachment.DepthAttachment
                : target == DepthStencil.Stencil
                ? FramebufferAttachment.StencilAttachment
                : 0;
            GL.FramebufferTexture2D(framebufferTarget, framebufferAttachment, TextureTarget.Texture2D, 0, 0);

            if ((target & DepthStencil.Depth) != 0)
                depthAttachment.Type = FramebufferAttachmentType.Disabled;

            if ((target & DepthStencil.Stencil) != 0)
                stencilAttachment.Type = FramebufferAttachmentType.Disabled;
        }
        #endregion

        public unsafe void Dispose()
        {
            int handleProxy;
            GL.DeleteFramebuffers(1, &handleProxy);
        }
    }
}
