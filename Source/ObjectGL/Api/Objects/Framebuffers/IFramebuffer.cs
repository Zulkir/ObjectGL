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

using ObjectGL.Api.Objects.Resources.Images;

namespace ObjectGL.Api.Objects.Framebuffers
{
    public interface IFramebuffer : IGLObject
    {
        void AttachRenderbuffer(FramebufferAttachmentPoint attachmentPoint, IRenderbuffer renderbuffer);
        void AttachTextureImage(FramebufferAttachmentPoint attachmentPoint, ITexture1D texture, int level);
        void AttachTextureImage(FramebufferAttachmentPoint attachmentPoint, ITexture1DArray texture, int level, int layer);
        void AttachTextureImage(FramebufferAttachmentPoint attachmentPoint, ITexture2D texture, int level);
        void AttachTextureImage(FramebufferAttachmentPoint attachmentPoint, ITexture2DArray texture, int level, int layer);
        void AttachTextureImage(FramebufferAttachmentPoint attachmentPoint, ITexture2DMultisample texture);
        void AttachTextureImage(FramebufferAttachmentPoint attachmentPoint, ITexture2DMultisampleArray texture, int layer);
        void AttachTextureImage(FramebufferAttachmentPoint attachmentPoint, ITexture3D texture, int level, int depthLayer);
        void AttachTextureImage(FramebufferAttachmentPoint attachmentPoint, ITextureCubemap texture, int level, CubemapFace cubemapFace);
        void AttachTextureImage(FramebufferAttachmentPoint attachmentPoint, ITextureCubemapArray texture, int level, int arrayIndex, CubemapFace cubemapFace);
        void AttachTextureAsLayeredImage(FramebufferAttachmentPoint attachmentPoint, ITexture1DArray texture, int level);
        void AttachTextureAsLayeredImage(FramebufferAttachmentPoint attachmentPoint, ITexture2DArray texture, int level);
        void AttachTextureAsLayeredImage(FramebufferAttachmentPoint attachmentPoint, ITexture2DMultisampleArray texture);
        void AttachTextureAsLayeredImage(FramebufferAttachmentPoint attachmentPoint, ITextureCubemap texture, int level);
        void AttachTextureAsLayeredImage(FramebufferAttachmentPoint attachmentPoint, ITextureCubemapArray texture, int level);
        void DetachColorStartingFrom(int startIndex);
        void Detach(FramebufferAttachmentPoint attachmentPoint);
        void ClearColor(int index, Color4 color);
        void ClearDepthStencil(DepthStencil target, float depth, int stencil);
    }
}