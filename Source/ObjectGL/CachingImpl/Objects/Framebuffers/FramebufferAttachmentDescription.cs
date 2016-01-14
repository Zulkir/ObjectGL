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

using System;
using ObjectGL.Api.Objects.Resources.Images;

namespace ObjectGL.CachingImpl.Objects.Framebuffers
{
    internal struct FramebufferAttachmentDescription
    {
        public FramebufferAttachmentType Type;
        public TextureTarget TextureTarget;
        public ITexture Texture;
        public IRenderbuffer Renderbuffer;
        
        public int Level;
        public int Layer;

        public static bool Equals(ref FramebufferAttachmentDescription a1, ref FramebufferAttachmentDescription a2)
        {
            switch (a1.Type)
            {
                case FramebufferAttachmentType.Disabled:
                    return a2.Type == FramebufferAttachmentType.Disabled;
                case FramebufferAttachmentType.Renderbufer:
                    return a2.Type == FramebufferAttachmentType.Renderbufer &&
                           a1.Renderbuffer == a2.Renderbuffer;
                case FramebufferAttachmentType.Texture:
                case FramebufferAttachmentType.TextureLayers:
                    return a1.Type == a2.Type &&
                           a1.TextureTarget == a2.TextureTarget &&
                           a1.Texture == a2.Texture &&
                           a1.Level == a2.Level &&
                           a1.Layer == a2.Layer;
                default: throw new ArgumentOutOfRangeException("a1.Type");
            }
        }
    }
}