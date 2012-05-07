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
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ObjectGL
{
    public class Framebuffer : IContextObject
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

            colorAttachments = new FramebufferAttachmentDescription[currentContext.Implementation.MaxColorAttachments];
        }

        #region Attach
        void Attach(Context currentContext, FramebufferAttachmentPoint attachmentPoint, ref FramebufferAttachmentDescription newDesc, GLFramebufferSomething glFramebufferSomething)
        {
            #region Check redundancy
            if ((FramebufferAttachmentPoint.Color0 <= attachmentPoint && attachmentPoint <= FramebufferAttachmentPoint.Color15))
            {
                if (FramebufferAttachmentDescription.Equals(ref newDesc, ref colorAttachments[attachmentPoint - FramebufferAttachmentPoint.Color0])) return;
            }
            else if (attachmentPoint == FramebufferAttachmentPoint.DepthStencil)
            {
                if (FramebufferAttachmentDescription.Equals(ref newDesc, ref depthAttachment) &&
                    FramebufferAttachmentDescription.Equals(ref newDesc, ref stencilAttachment))
                    return;
            }
            else if (attachmentPoint == FramebufferAttachmentPoint.Depth)
            {
                if (FramebufferAttachmentDescription.Equals(ref newDesc, ref depthAttachment)) return;
            }
            else if (attachmentPoint == FramebufferAttachmentPoint.Stencil)
            {
                if (FramebufferAttachmentDescription.Equals(ref newDesc, ref stencilAttachment)) return;
            }
            else
            {
                throw new ArgumentOutOfRangeException("attachmentPoint");
            }
            #endregion

            var framebufferTarget = currentContext.BindAnyFramebuffer(handle);
            glFramebufferSomething(framebufferTarget, (FramebufferAttachment)attachmentPoint, ref newDesc);

            #region Update stored description
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
            #endregion
        }

        public void AttachRenderbuffer(Context currentContext, FramebufferAttachmentPoint attachmentPoint, Renderbuffer renderbuffer)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Renderbufer,
                Renderbuffer = renderbuffer,
            };

            Attach(currentContext, attachmentPoint, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferRenderbuffer(ft, fa, RenderbufferTarget.Renderbuffer, d.Renderbuffer.Handle));
        }

        public void AttachTextureImage(Context currentContext, FramebufferAttachmentPoint attachmentPoint, Texture1D texture, int level)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture1D,
                Texture = texture,
                Level = level
            };

            Attach(currentContext, attachmentPoint, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTexture1D(ft, fa, d.TextureTarget, d.Texture.Handle, d.Level));
        }

        public void AttachTextureImage(Context currentContext, FramebufferAttachmentPoint attachmentPoint, Texture1DArray texture, int level, int layer)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture1DArray,
                Texture = texture,
                Level = level,
                Layer = layer
            };

            Attach(currentContext, attachmentPoint, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTextureLayer(ft, fa, d.Texture.Handle, d.Level, d.Layer));
        }

        public void AttachTextureAsLayeredImage(Context currentContext, FramebufferAttachmentPoint attachmentPoint, Texture1DArray texture, int level)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.TextureLayers,
                TextureTarget = TextureTarget.Texture1DArray,
                Texture = texture,
                Level = level
            };

            Attach(currentContext, attachmentPoint, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTexture(ft, fa, d.Texture.Handle, d.Level));
        }

        public void AttachTextureImage(Context currentContext, FramebufferAttachmentPoint attachmentPoint, Texture2D texture, int level)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture2D,
                Texture = texture,
                Level = level
            };

            Attach(currentContext, attachmentPoint, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTexture2D(ft, fa, d.TextureTarget, d.Texture.Handle, d.Level));
        }

        public void AttachTextureImage(Context currentContext, FramebufferAttachmentPoint attachmentPoint, Texture2DArray texture, int level, int layer)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture2DArray,
                Texture = texture,
                Level = level,
                Layer = layer
            };

            Attach(currentContext, attachmentPoint, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTextureLayer(ft, fa, d.Texture.Handle, d.Level, d.Layer));
        }

        public void AttachTextureAsLayeredImage(Context currentContext, FramebufferAttachmentPoint attachmentPoint, Texture2DArray texture, int level)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.TextureLayers,
                TextureTarget = TextureTarget.Texture2DArray,
                Texture = texture,
                Level = level
            };

            Attach(currentContext, attachmentPoint, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTexture(ft, fa, d.Texture.Handle, d.Level));
        }

        public void AttachTextureImage(Context currentContext, FramebufferAttachmentPoint attachmentPoint, Texture2DMultisample texture)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture2DMultisample,
                Texture = texture
            };

            Attach(currentContext, attachmentPoint, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTexture2D(ft, fa, d.TextureTarget, d.Texture.Handle, 0));
        }

        public void AttachTextureImage(Context currentContext, FramebufferAttachmentPoint attachmentPoint, Texture2DMultisampleArray texture, int layer)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture2DMultisampleArray,
                Texture = texture,
                Layer = layer
            };

            Attach(currentContext, attachmentPoint, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTextureLayer(ft, fa, d.Texture.Handle, d.Level, 0));
        }

        public void AttachTextureAsLayeredImage(Context currentContext, FramebufferAttachmentPoint attachmentPoint, Texture2DMultisampleArray texture)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.TextureLayers,
                TextureTarget = TextureTarget.Texture2DMultisampleArray,
                Texture = texture
            };

            Attach(currentContext, attachmentPoint, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTexture(ft, fa, d.Texture.Handle, 0));
        }

        public void AttachTextureImage(Context currentContext, FramebufferAttachmentPoint attachmentPoint, Texture3D texture, int level, int depthLayer)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.Texture3D,
                Texture = texture,
                Level = level,
                Layer = depthLayer
            };

            Attach(currentContext, attachmentPoint, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTextureLayer(ft, fa, d.Texture.Handle, d.Level, d.Layer));
        }

        public void AttachTextureAsLayeredImage(Context currentContext, FramebufferAttachmentPoint attachmentPoint, Texture3D texture, int level)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.TextureLayers,
                TextureTarget = TextureTarget.Texture3D,
                Texture = texture,
                Level = level
            };

            Attach(currentContext, attachmentPoint, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTexture(ft, fa, d.Texture.Handle, d.Level));
        }

        public void AttachTextureImage(Context currentContext, FramebufferAttachmentPoint attachmentPoint, TextureCubemap texture, int level, CubemapFace cubemapFace)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.TextureCubeMap,
                Texture = texture,
                Level = level,
                Layer = cubemapFace - CubemapFace.PositiveX
            };

            Attach(currentContext, attachmentPoint, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTexture2D(ft, fa, d.Layer + TextureTarget.TextureCubeMapPositiveX, d.Texture.Handle, d.Level));
        }

        public void AttachTextureAsLayeredImage(Context currentContext, FramebufferAttachmentPoint attachmentPoint, TextureCubemap texture, int level)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.TextureLayers,
                TextureTarget = TextureTarget.TextureCubeMap,
                Texture = texture,
                Level = level
            };

            Attach(currentContext, attachmentPoint, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTexture(ft, fa, d.Texture.Handle, d.Level));
        }

        public void AttachTextureImage(Context currentContext, FramebufferAttachmentPoint attachmentPoint, TextureCubemapArray texture, int level, int arrayIndex, CubemapFace cubemapFace)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.Texture,
                TextureTarget = TextureTarget.TextureCubeMapArray,
                Texture = texture,
                Level = level,
                Layer = 6 * arrayIndex + cubemapFace - CubemapFace.PositiveX
            };

            Attach(currentContext, attachmentPoint, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTextureLayer(ft, fa, d.Layer, d.Texture.Handle, d.Level));
        }

        public void AttachTextureAsLayeredImage(Context currentContext, FramebufferAttachmentPoint attachmentPoint, TextureCubemapArray texture, int level)
        {
            var newDesc = new FramebufferAttachmentDescription
            {
                Type = FramebufferAttachmentType.TextureLayers,
                TextureTarget = TextureTarget.TextureCubeMapArray,
                Texture = texture,
                Level = level
            };

            Attach(currentContext, attachmentPoint, ref newDesc, (FramebufferTarget ft, FramebufferAttachment fa, ref FramebufferAttachmentDescription d) =>
                GL.FramebufferTexture(ft, fa, d.Layer, d.Texture.Handle));
        }

        public void Detach(Context currentContext, FramebufferAttachmentPoint attachmentPoint)
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

            var framebufferTarget = currentContext.BindAnyFramebuffer(handle);
            GL.FramebufferTexture2D(framebufferTarget, (FramebufferAttachment)attachmentPoint, TextureTarget.Texture2D, 0, 0);

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

        public void DetachColorStartingFrom(Context currentContext, int startIndex)
        {
            if (startIndex >= enabledColorAttachmentsRange) return;

            for (int i = startIndex; i < colorAttachments.Length; i++)
            {
                Detach(currentContext, i + FramebufferAttachmentPoint.Color0);
            }

            enabledColorAttachmentsRange = startIndex;
            while (enabledColorAttachmentsRange > 0 && colorAttachments[enabledColorAttachmentsRange - 1].Type == FramebufferAttachmentType.Disabled)
            {
                enabledColorAttachmentsRange--;
            }
        }
        #endregion

        public unsafe void ClearColor(Context currentContext, int index, Color4 color)
        {
            currentContext.BindDrawFramebuffer(handle);
            GL.ClearBuffer(ClearBuffer.Color, index, (float*)&color);
        }

        public void ClearDepthStencil(Context currentContext, DepthStencil target, float depth, int stencil)
        {
            currentContext.BindDrawFramebuffer(handle);

            ClearBuffer clearBuffer;
            switch (target)
            {
                case DepthStencil.Both: clearBuffer = ClearBuffer.DepthStencil; break;
                case DepthStencil.Depth: clearBuffer = ClearBuffer.Depth; break;
                case DepthStencil.Stencil: clearBuffer = ClearBuffer.Stencil; break;
                default: throw new ArgumentException("'target' must be either 'Both', 'Depth', or 'Stencil'");
            }

            GL.ClearBuffer(clearBuffer, 0, depth, stencil);
        }

        public unsafe void Dispose()
        {
            int handleProxy;
            GL.DeleteFramebuffers(1, &handleProxy);
        }
    }
}
