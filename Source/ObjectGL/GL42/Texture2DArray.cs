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

namespace ObjectGL.GL42
{
    public class Texture2DArray : Texture
    {
        Texture2DArray(Context currentContext,
                       int width, int height, int sliceCount, int mipCount,
                       Format internalFormat, Func<int, Data> getInitialDataForMip,
                       Action<TextureTarget, int, PixelInternalFormat, int, int, int, IntPtr> glTexImage)
            : base(TextureTarget.Texture2DArray, width, height, 1, internalFormat, sliceCount, mipCount == 0 ? CalculateMipCount(width, height) : mipCount)
        {
            currentContext.BindTexture(Target, this);

            int mipWidth = width;
            int mipHeight = height;

            for (int i = 0; i < MipCount; i++)
            {
                Data data = getInitialDataForMip(i);
                glTexImage(Target, i, (PixelInternalFormat)internalFormat, mipWidth, mipHeight, sliceCount, data.Pointer);
                data.UnpinPointer();

                mipWidth = Math.Max(mipWidth/2, 1);
                mipHeight = Math.Max(mipHeight/2, 1);
            }
        }

        public Texture2DArray(Context currentContext,
                              int width, int height, int sliceCount, int mipCount,
                              Format internalFormat)
            : this(currentContext, width, height, sliceCount, mipCount, internalFormat, i => new Data(IntPtr.Zero),
                   (tt, l, f, w, h, s, p) => GL.TexImage3D(tt, l, f, w, h, s, 0,
                       (PixelFormat)GetAppropriateFormatColor(internalFormat), (PixelType)GetAppropriateFormatType(internalFormat), p))
        {
        }

        public Texture2DArray(Context currentContext,
                              int width, int height, int sliceCount, int mipCount,
                              Format internalFormat, FormatColor format, FormatType type,
                              Func<int, Data> getInitialDataForMip)
            : this(currentContext, width, height, sliceCount, mipCount, internalFormat, getInitialDataForMip,
                   (tt, l, f, w, h, s, p) => GL.TexImage3D(tt, l, f, w, h, s, 0, (PixelFormat)format, (PixelType)type, p))
        {
        }

        public Texture2DArray(Context currentContext,
                              int width, int height, int sliceCount, int mipCount,
                              Format internalFormat, Func<int, int> getComressedImageSizeForMip,
                              Func<int, Data> getCompressedInitialDataForMip)
            : this(currentContext, width, height, sliceCount, mipCount, internalFormat, getCompressedInitialDataForMip,
                   (tt, l, f, w, h, s, p) =>
                   GL.CompressedTexImage3D(tt, l, f, w, h, s, 0, getComressedImageSizeForMip(l), p))
        {
        }
    }
}