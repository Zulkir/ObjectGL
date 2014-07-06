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

using System;
using ObjectGL.Api;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;

namespace ObjectGL.CachingImpl.Objects
{
    internal class Framebuffer : IFramebuffer
    {
        private readonly Context context;
        private readonly uint handle;
        private readonly FramebufferAttachmentDescription[] colorAttachments;
        private int enabledColorAttachmentsRange;

        private FramebufferAttachmentDescription depthAttachment;
        private FramebufferAttachmentDescription stencilAttachment;

        private IGL GL { get { return context.GL; } }

        public uint Handle { get { return handle; } }
        public ContextObjectType ContextObjectType { get { return ContextObjectType.Framebuffer; } }

        public unsafe Framebuffer(Context context)
        {
            this.context = context;
            uint handleProxy;
            GL.GenFramebuffers(1, &handleProxy);
            handle = handleProxy;

            colorAttachments = new FramebufferAttachmentDescription[context.Implementation.MaxColorAttachments];
        }

        private bool IsRedundant(FramebufferAttachmentPoint attachmentPoint, ref FramebufferAttachmentDescription newDesc)
        {
            switch (attachmentPoint)
            {
                case FramebufferAttachmentPoint.Color0:
                case FramebufferAttachmentPoint.Color1:
                case FramebufferAttachmentPoint.Color2:
                case FramebufferAttachmentPoint.Color3:
                case FramebufferAttachmentPoint.Color4:
                case FramebufferAttachmentPoint.Color5:
                case FramebufferAttachmentPoint.Color6:
                case FramebufferAttachmentPoint.Color7:
                case FramebufferAttachmentPoint.Color8:
                case FramebufferAttachmentPoint.Color9:
                case FramebufferAttachmentPoint.Color10:
                case FramebufferAttachmentPoint.Color11:
                case FramebufferAttachmentPoint.Color12:
                case FramebufferAttachmentPoint.Color13:
                case FramebufferAttachmentPoint.Color14:
                case FramebufferAttachmentPoint.Color15:
                    return FramebufferAttachmentDescription.Equals(ref newDesc, ref colorAttachments[attachmentPoint - FramebufferAttachmentPoint.Color0]);
                case FramebufferAttachmentPoint.DepthStencil:
                    return FramebufferAttachmentDescription.Equals(ref newDesc, ref depthAttachment) &&
                           FramebufferAttachmentDescription.Equals(ref newDesc, ref stencilAttachment);
                case FramebufferAttachmentPoint.Depth:
                    return FramebufferAttachmentDescription.Equals(ref newDesc, ref depthAttachment);
                case FramebufferAttachmentPoint.Stencil:
                    return FramebufferAttachmentDescription.Equals(ref newDesc, ref stencilAttachment);
                default:
                    throw new ArgumentOutOfRangeException("attachmentPoint");
            }
        }

        private void UpdateStoredDescription(FramebufferAttachmentPoint attachmentPoint, ref FramebufferAttachmentDescription newDesc)
        {
            if ((FramebufferAttachmentPoint.Color0 <= attachmentPoint && attachmentPoint <= FramebufferAttachmentPoint.Color15))
            {
                int index = attachmentPoint - FramebufferAttachmentPoint.Color0;
                colorAttachments[index] = newDesc;
                if (index >= enabledColorAttachmentsRange)
                    enabledColorAttachmentsRange = index + 1;

            }
            else if (attachmentPoint == FramebufferAttachmentPoint.DepthStencil || attachmentPoint == FramebufferAttachmentPoint.Depth)
            {
                depthAttachment = newDesc;
            }
            else if (attachmentPoint == FramebufferAttachmentPoint.DepthStencil || attachmentPoint == FramebufferAttachmentPoint.Stencil)
            {
                stencilAttachment = newDesc;
            }
        }

        public void AttachRenderbuffer(FramebufferAttachmentPoint attachmentPoint, IRenderbuffer renderbuffer)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Renderbufer,
                Renderbuffer = renderbuffer,
            };

            if (IsRedundant(attachmentPoint, ref newDesc))
                return;
            var framebufferTarget = context.BindAnyFramebuffer(this);
            GL.FramebufferRenderbuffer((int)framebufferTarget, (int)attachmentPoint, (int)RenderbufferTarget.Renderbuffer, renderbuffer.Handle);
            UpdateStoredDescription(attachmentPoint, ref newDesc);
        }

        public void AttachTextureImage(FramebufferAttachmentPoint attachmentPoint, ITexture1D texture, int level)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture1D,
                Texture = texture,
                Level = level
            };

            if (IsRedundant(attachmentPoint, ref newDesc))
                return;
            var framebufferTarget = context.BindAnyFramebuffer(this);
            //gl.FramebufferTexture((int)framebufferTarget, (int)attachmentPoint, texture.Handle, level);
            GL.FramebufferTexture2D((int)framebufferTarget, (int)attachmentPoint, (int)texture.Target, texture.Handle, level);
            UpdateStoredDescription(attachmentPoint, ref newDesc);
        }

        public void AttachTextureImage(FramebufferAttachmentPoint attachmentPoint, ITexture1DArray texture, int level, int layer)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture1DArray,
                Texture = texture,
                Level = level,
                Layer = layer
            };

            if (IsRedundant(attachmentPoint, ref newDesc))
                return;
            var framebufferTarget = context.BindAnyFramebuffer(this);
            GL.FramebufferTextureLayer((int)framebufferTarget, (int)attachmentPoint, texture.Handle, level, layer);
            UpdateStoredDescription(attachmentPoint, ref newDesc);
        }

        public void AttachTextureAsLayeredImage(FramebufferAttachmentPoint attachmentPoint, ITexture1DArray texture, int level)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.TextureLayers,
                TextureTarget = TextureTarget.Texture1DArray,
                Texture = texture,
                Level = level
            };

            if (IsRedundant(attachmentPoint, ref newDesc))
                return;
            var framebufferTarget = context.BindAnyFramebuffer(this);
            //gl.FramebufferTexture((int)framebufferTarget, (int)attachmentPoint, texture.Handle, level);
            GL.FramebufferTexture2D((int)framebufferTarget, (int)attachmentPoint, (int)texture.Target, texture.Handle, level);
            UpdateStoredDescription(attachmentPoint, ref newDesc);
        }

        public void AttachTextureImage(FramebufferAttachmentPoint attachmentPoint, ITexture2D texture, int level)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture2D,
                Texture = texture,
                Level = level
            };

            if (IsRedundant(attachmentPoint, ref newDesc))
                return;
            var framebufferTarget = context.BindAnyFramebuffer(this);
            //gl.FramebufferTexture((int)framebufferTarget, (int)attachmentPoint, texture.Handle, level);
            GL.FramebufferTexture2D((int)framebufferTarget, (int)attachmentPoint, (int)texture.Target, texture.Handle, level);
            UpdateStoredDescription(attachmentPoint, ref newDesc);
        }

        public void AttachTextureImage(FramebufferAttachmentPoint attachmentPoint, ITexture2DArray texture, int level, int layer)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture2DArray,
                Texture = texture,
                Level = level,
                Layer = layer
            };

            if (IsRedundant(attachmentPoint, ref newDesc))
                return;
            var framebufferTarget = context.BindAnyFramebuffer(this);
            GL.FramebufferTextureLayer((int)framebufferTarget, (int)attachmentPoint, texture.Handle, level, layer);
            UpdateStoredDescription(attachmentPoint, ref newDesc);
        }

        public void AttachTextureAsLayeredImage(FramebufferAttachmentPoint attachmentPoint, ITexture2DArray texture, int level)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.TextureLayers,
                TextureTarget = TextureTarget.Texture2DArray,
                Texture = texture,
                Level = level
            };

            if (IsRedundant(attachmentPoint, ref newDesc))
                return;
            var framebufferTarget = context.BindAnyFramebuffer(this);
            //gl.FramebufferTexture((int)framebufferTarget, (int)attachmentPoint, texture.Handle, level);
            GL.FramebufferTexture2D((int)framebufferTarget, (int)attachmentPoint, (int)texture.Target, texture.Handle, level);
            UpdateStoredDescription(attachmentPoint, ref newDesc);
        }

        public void AttachTextureImage(FramebufferAttachmentPoint attachmentPoint, ITexture2DMultisample texture)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture2DMultisample,
                Texture = texture
            };

            if (IsRedundant(attachmentPoint, ref newDesc))
                return;
            var framebufferTarget = context.BindAnyFramebuffer(this);
            //gl.FramebufferTexture((int)framebufferTarget, (int)attachmentPoint, texture.Handle, 0);
            GL.FramebufferTexture2D((int)framebufferTarget, (int)attachmentPoint, (int)texture.Target, texture.Handle, 0);
            UpdateStoredDescription(attachmentPoint, ref newDesc);
        }

        public void AttachTextureImage(FramebufferAttachmentPoint attachmentPoint, ITexture2DMultisampleArray texture, int layer)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture2DMultisampleArray,
                Texture = texture,
                Layer = layer
            };

            if (IsRedundant(attachmentPoint, ref newDesc))
                return;
            var framebufferTarget = context.BindAnyFramebuffer(this);
            GL.FramebufferTextureLayer((int)framebufferTarget, (int)attachmentPoint, texture.Handle, 0, layer);
            UpdateStoredDescription(attachmentPoint, ref newDesc);
        }

        public void AttachTextureAsLayeredImage(FramebufferAttachmentPoint attachmentPoint, ITexture2DMultisampleArray texture)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.TextureLayers,
                TextureTarget = TextureTarget.Texture2DMultisampleArray,
                Texture = texture
            };

            if (IsRedundant(attachmentPoint, ref newDesc))
                return;
            var framebufferTarget = context.BindAnyFramebuffer(this);
            //gl.FramebufferTexture((int)framebufferTarget, (int)attachmentPoint, texture.Handle, 0);
            GL.FramebufferTexture2D((int)framebufferTarget, (int)attachmentPoint, (int)texture.Target, texture.Handle, 0);
            UpdateStoredDescription(attachmentPoint, ref newDesc);
        }

        public void AttachTextureImage(FramebufferAttachmentPoint attachmentPoint, ITexture3D texture, int level, int depthLayer)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture3D,
                Texture = texture,
                Level = level,
                Layer = depthLayer
            };

            if (IsRedundant(attachmentPoint, ref newDesc))
                return;
            var framebufferTarget = context.BindAnyFramebuffer(this);
            GL.FramebufferTextureLayer((int)framebufferTarget, (int)attachmentPoint, texture.Handle, level, depthLayer);
            UpdateStoredDescription(attachmentPoint, ref newDesc);
        }

        public void AttachTextureImage(FramebufferAttachmentPoint attachmentPoint, ITextureCubemap texture, int level, CubemapFace cubemapFace)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.TextureCubeMap,
                Texture = texture,
                Level = level,
                Layer = cubemapFace - CubemapFace.PositiveX
            };

            if (IsRedundant(attachmentPoint, ref newDesc))
                return;
            var framebufferTarget = context.BindAnyFramebuffer(this);
            GL.FramebufferTexture2D((int)framebufferTarget, (int)attachmentPoint, (int)cubemapFace, texture.Handle, level);
            UpdateStoredDescription(attachmentPoint, ref newDesc);
        }

        public void AttachTextureAsLayeredImage(FramebufferAttachmentPoint attachmentPoint, ITextureCubemap texture, int level)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.TextureLayers,
                TextureTarget = TextureTarget.TextureCubeMap,
                Texture = texture,
                Level = level
            };

            if (IsRedundant(attachmentPoint, ref newDesc))
                return;
            var framebufferTarget = context.BindAnyFramebuffer(this);
            //gl.FramebufferTexture(ft, fa, d.Texture.Handle, d.Level);
            GL.FramebufferTexture2D((int)framebufferTarget, (int)attachmentPoint, (int)texture.Target, texture.Handle, level);
            UpdateStoredDescription(attachmentPoint, ref newDesc);
        }

        public void AttachTextureImage(FramebufferAttachmentPoint attachmentPoint, ITextureCubemapArray texture, int level, int arrayIndex, CubemapFace cubemapFace)
        {
            var layer = 6 * arrayIndex + cubemapFace - CubemapFace.PositiveX;
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.TextureCubeMapArray,
                Texture = texture,
                Level = level,
                Layer = layer
            };

            if (IsRedundant(attachmentPoint, ref newDesc))
                return;
            var framebufferTarget = context.BindAnyFramebuffer(this);
            GL.FramebufferTextureLayer((int)framebufferTarget, (int)attachmentPoint, texture.Handle, layer, level);
            UpdateStoredDescription(attachmentPoint, ref newDesc);
        }

        public void AttachTextureAsLayeredImage(FramebufferAttachmentPoint attachmentPoint, ITextureCubemapArray texture, int level)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.TextureLayers,
                TextureTarget = TextureTarget.TextureCubeMapArray,
                Texture = texture,
                Level = level
            };

            if (IsRedundant(attachmentPoint, ref newDesc))
                return;
            var framebufferTarget = context.BindAnyFramebuffer(this);
            //gl.FramebufferTexture(ft, fa, d.Texture.Handle, d.Level);
            GL.FramebufferTexture2D((int)framebufferTarget, (int)attachmentPoint, (int)texture.Target, texture.Handle, level);
            UpdateStoredDescription(attachmentPoint, ref newDesc);
        }

        public void Detach(FramebufferAttachmentPoint attachmentPoint)
        {
            #region Check redundancy
            if ((FramebufferAttachmentPoint.Color0 <= attachmentPoint && attachmentPoint <= FramebufferAttachmentPoint.Color15))
            {
                if (colorAttachments[attachmentPoint - FramebufferAttachmentPoint.Color0].Type == FramebufferAttachmentType.Disabled) return;
            }
            else if (attachmentPoint == FramebufferAttachmentPoint.DepthStencil)
            {
                if (depthAttachment.Type == FramebufferAttachmentType.Disabled &&
                    stencilAttachment.Type == FramebufferAttachmentType.Disabled)
                    return;
            }
            else if (attachmentPoint == FramebufferAttachmentPoint.Depth)
            {
                if (depthAttachment.Type == FramebufferAttachmentType.Disabled) return;
            }
            else if (attachmentPoint == FramebufferAttachmentPoint.Stencil)
            {
                if (stencilAttachment.Type == FramebufferAttachmentType.Disabled) return;
            }
            else
            {
                throw new ArgumentOutOfRangeException("attachmentPoint");
            }
            #endregion

            var framebufferTarget = context.BindAnyFramebuffer(this);
            GL.FramebufferTexture2D((int)framebufferTarget, (int)attachmentPoint, (int)TextureTarget.Texture2D, 0, 0);

            #region Update stored description
            if ((FramebufferAttachmentPoint.Color0 <= attachmentPoint && attachmentPoint <= FramebufferAttachmentPoint.Color15))
            {
                int index = attachmentPoint - FramebufferAttachmentPoint.Color0;
                colorAttachments[index].Type = FramebufferAttachmentType.Disabled;
                while (enabledColorAttachmentsRange > 0 && colorAttachments[enabledColorAttachmentsRange - 1].Type == FramebufferAttachmentType.Disabled)
                {
                    enabledColorAttachmentsRange--;
                }
            }
            else if (attachmentPoint == FramebufferAttachmentPoint.DepthStencil || attachmentPoint == FramebufferAttachmentPoint.Depth)
            {
                depthAttachment.Type = FramebufferAttachmentType.Disabled;
            }
            else if (attachmentPoint == FramebufferAttachmentPoint.DepthStencil || attachmentPoint == FramebufferAttachmentPoint.Stencil)
            {
                stencilAttachment.Type = FramebufferAttachmentType.Disabled;
            }
            #endregion
        }

        public void DetachColorStartingFrom(int startIndex)
        {
            if (startIndex >= enabledColorAttachmentsRange) return;

            for (int i = startIndex; i < colorAttachments.Length; i++)
                Detach(i + FramebufferAttachmentPoint.Color0);

            enabledColorAttachmentsRange = startIndex;
            while (enabledColorAttachmentsRange > 0 && colorAttachments[enabledColorAttachmentsRange - 1].Type == FramebufferAttachmentType.Disabled)
                enabledColorAttachmentsRange--;
        }

        public unsafe void ClearColor(int index, Color4 color)
        {
            context.BindDrawFramebuffer(this);
            context.PrepareForClear();
            GL.ClearBuffer((int)All.Color, index, (float*)&color);
        }

        public unsafe void ClearDepthStencil(DepthStencil target, float depth, int stencil)
        {
            context.BindDrawFramebuffer(this);
            context.PrepareForClear();

            switch (target)
            {
                case DepthStencil.Both:
                    GL.ClearBuffer((int)All.DepthStencil, 0, depth, stencil);
                    return;
                case DepthStencil.Depth:
                    GL.ClearBuffer((int)All.Depth, 0, &depth);
                    return;
                case DepthStencil.Stencil:
                    GL.ClearBuffer((int)All.Stencil, 0, &stencil);
                    return;
                default:
                    throw new ArgumentException("'target' must be either 'Both', 'Depth', or 'Stencil'");
            }
        }

        public unsafe void Dispose()
        {
            uint handleProxy;
            GL.DeleteFramebuffers(1, &handleProxy);
        }
    }
}
