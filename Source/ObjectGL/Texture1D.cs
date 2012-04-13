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
    public class Texture1D : Texture
    {
        readonly int width;

        public int Width { get { return width; } }

        public Texture1D(Context currentContext,
            int width,
            PixelInternalFormat internalFormat, PixelFormat format, PixelType type, Func<int, Data> initialDataForMip)
            : base(TextureTarget.Texture1D, internalFormat, 1, CalculateMipCount(width))
        {
            this.width = width;

            currentContext.BindTexture(Target, Handle);

            int mipHeight = width;
            for (int i = 0; i < MipCount; i++)
            {
                var data = initialDataForMip(i);
                GL.TexImage1D(Target, i, internalFormat, mipHeight, 0, format, type, data.Pointer);
                data.UnpinPointer();

                mipHeight = Math.Max(mipHeight / 2, 1);
            }
        }

        public Texture1D(Context currentContext,
            int width,
            PixelInternalFormat internalFormat, Func<int, int> getComressedImageSizeForMip, Func<int, Data> getCompressedInitialDataForMip)
            : base(TextureTarget.Texture1D, internalFormat, 1, CalculateMipCount(width))
        {
            this.width = width;

            currentContext.BindTexture(Target, Handle);

            int mipWidth = width;
            for (int i = 0; i < MipCount; i++)
            {
                var data = getCompressedInitialDataForMip(i);
                GL.CompressedTexImage1D(Target, i, internalFormat, mipWidth, 0, getComressedImageSizeForMip(i), data.Pointer);
                data.UnpinPointer();

                mipWidth = Math.Max(mipWidth / 2, 1);
            }
        }
    }
}
