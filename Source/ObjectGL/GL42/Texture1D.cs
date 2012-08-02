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
    public class Texture1D : Texture
    {
        Texture1D(Context currentContext,
                  int width, int mipCount,
                  Format internalFormat, Func<int, Data> getInitialDataForMip,
                  Action<TextureTarget, int, PixelInternalFormat, int, IntPtr> glTexImage)
            : base(TextureTarget.Texture1D, width, 1, 1, internalFormat, 1, mipCount == 0 ? CalculateMipCount(width) : mipCount)
        {
            currentContext.BindTexture(Target, this);

            int mipWidth = width;

            for (int i = 0; i < MipCount; i++)
            {
                Data data = getInitialDataForMip(i);
                glTexImage(TextureTarget.Texture1D, i, (PixelInternalFormat)internalFormat, mipWidth, data.Pointer);
                data.UnpinPointer();

                mipWidth = Math.Max(mipWidth/2, 1);
            }
        }

        public Texture1D(Context currentContext,
                         int width, int mipCount,
                         Format internalFormat)
            : this(currentContext, width, mipCount, internalFormat, i => new Data(IntPtr.Zero),
                   (tt, l, f, w, p) => GL.TexImage1D(tt, l, f, w, 0,
                       (PixelFormat)GetAppropriateFormatColor(internalFormat), (PixelType)GetAppropriateFormatType(internalFormat), p))
        {
        }

        public Texture1D(Context currentContext,
                         int width, int mipCount,
                         Format internalFormat, FormatColor format, FormatType type,
                         Func<int, Data> getInitialDataForMip)
            : this(currentContext, width, mipCount, internalFormat, getInitialDataForMip,
                   (tt, l, f, w, p) => GL.TexImage1D(tt, l, f, w, 0, (PixelFormat)format, (PixelType)type, p))
        {
        }

        public Texture1D(Context currentContext,
                         int width, int mipCount,
                         Format internalFormat, Func<int, int> getComressedImageSizeForMip,
                         Func<int, Data> getCompressedInitialDataForMip)
            : this(currentContext, width, mipCount, internalFormat, getCompressedInitialDataForMip,
                   (tt, l, f, w, p) => GL.CompressedTexImage1D(tt, l, f, w, 0, getComressedImageSizeForMip(l), p))
        {
        }
    }
}