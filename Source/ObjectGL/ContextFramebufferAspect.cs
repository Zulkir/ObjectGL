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

using OpenTK.Graphics.OpenGL;

namespace ObjectGL
{
    public partial class Context
    {
        private class FramebufferAspect
        {
            readonly RedundantInt drawFramebufferBinding = new RedundantInt(h => GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, h));
            readonly RedundantInt readFramebufferBinding = new RedundantInt(h => GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, h));
            readonly RedundantInt renderbufferBinding = new RedundantInt(h => GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, h));

            public void BindDrawFramebuffer(int framebufferHandle)
            {
                drawFramebufferBinding.Set(framebufferHandle);
            }

            public void BindReadFramebuffer(int framebufferHandle)
            {
                readFramebufferBinding.Set(framebufferHandle);
            }

            public FramebufferTarget BindAnyFramebuffer(int framebufferHandle)
            {
                if (drawFramebufferBinding.Get() == framebufferHandle) return FramebufferTarget.DrawFramebuffer;
                if (readFramebufferBinding.Get() == framebufferHandle) return FramebufferTarget.ReadFramebuffer;
                readFramebufferBinding.Set(framebufferHandle);
                return FramebufferTarget.ReadFramebuffer;
            }

            public void BindRenderbuffer(int renderbufferHandle)
            {
                renderbufferBinding.Set(renderbufferHandle);
            }

            
        }
    }
}
