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

namespace ObjectGL.GL42
{
    struct FramebufferAttachmentDescription
    {
        public FramebufferAttachmentType Type;
        public TextureTarget TextureTarget;
        public Texture Texture;
        public Renderbuffer Renderbuffer;
        
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