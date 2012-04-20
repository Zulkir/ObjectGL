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
        readonly int handle;

        FramebufferAttachment[] colorAttachments;
        FramebufferAttachment depthAttachment;
        FramebufferAttachment stencilAttachment;

        public int Handle { get { return handle; } }

        public unsafe Framebuffer(Context currentContext)
        {
            int handleProxy;
            GL.GenFramebuffers(1, &handleProxy);
            handle = handleProxy;

            colorAttachments = new FramebufferAttachment[currentContext.Capabilities.MaxColorAttachments];
        }

        public void AttachColorTexture1D(int attachmentSlot, Texture1D texture, int layer)
        {
            
        }

        public unsafe void Dispose()
        {
            int handleProxy;
            GL.DeleteFramebuffers(1, &handleProxy);
        }
    }
}
