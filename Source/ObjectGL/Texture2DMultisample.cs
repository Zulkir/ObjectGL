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
    public class Texture2DMultisample : Texture
    {
        readonly int width;
        readonly int height;
        readonly int samples;

        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public int Samples { get { return samples; } }

        public Texture2DMultisample(Context currentContext,
                         int width, int height, int samples, bool fixedSampleLocations,
                         PixelInternalFormat internalFormat)
            : base(TextureTarget.Texture2DMultisample, internalFormat, 1, 1)
        {
            this.width = width;
            this.height = height;
            this.samples = samples;

            currentContext.BindTexture(Target, Handle);

            GL.TexImage2DMultisample((TextureTargetMultisample)Target, samples, internalFormat, width, height, fixedSampleLocations);
        }
    }
}