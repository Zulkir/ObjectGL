﻿#region License
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
    public class Renderbuffer : IDisposable
    {
        readonly int handle;

        readonly RenderbufferTarget target;
        readonly PixelInternalFormat internalFormat;

        readonly int width;
        readonly int height;
        readonly int samples;

        public int Handle { get { return handle; } }

        public RenderbufferTarget Target { get { return target; } }
        public PixelInternalFormat InternalFormat { get { return internalFormat; } }

        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public int Samples { get { return samples; } }

        public unsafe Renderbuffer(Context currentContext, PixelInternalFormat internalFormat, int width, int height, int samples = 0)
        {
            this.target = RenderbufferTarget.Renderbuffer;
            this.internalFormat = internalFormat;
            this.width = width;
            this.height = height;
            this.samples = samples;

            int handleProxy;
            GL.GenRenderbuffers(1, &handleProxy);
            handle = handleProxy;

            currentContext.BindRenderbuffer(handle);

            if (samples == 0)
            {
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, (RenderbufferStorage)internalFormat, width, height);
            }
            else
            {
                GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, samples, (RenderbufferStorage)internalFormat, width, height);
            }
            
        }

        public unsafe void Dispose()
        {
            int handleProxy = handle;
            GL.DeleteRenderbuffers(1, &handleProxy);
        }
    }
}