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
    public class TextureCubemap : Texture
    {
        readonly int width;
        readonly int height;

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        TextureCubemap(Context currentContext,
                  int width, int height,
                  PixelInternalFormat internalFormat, Func<CubemapFace, int, Data> getInitialDataForMip,
                  Action<TextureTarget, int, PixelInternalFormat, int, int, IntPtr> glTexImage)
            : base(TextureTarget.TextureCubeMap, internalFormat, 1, CalculateMipCount(width, height))
        {
            this.width = width;
            this.height = height;

            currentContext.BindTexture(Target, Handle);

            int mipWidth = width;
            int mipHeight = height;

            for (int j = (int)CubemapFace.PositiveX; j < (int)CubemapFace.PositiveX + 6; j++)
            {
                for (int i = 0; i < MipCount; i++)
                {
                    Data data = getInitialDataForMip((CubemapFace)j, i);
                    glTexImage((TextureTarget)j, i, internalFormat, mipWidth, mipHeight, data.Pointer);
                    data.UnpinPointer();

                    mipWidth = Math.Max(mipWidth / 2, 1);
                    mipHeight = Math.Max(mipHeight / 2, 1);
                }
            }
        }

        public TextureCubemap(Context currentContext,
                         int width, int height,
                         PixelInternalFormat internalFormat, PixelFormat format, PixelType type,
                         Func<CubemapFace, int, Data> getInitialDataForMip)
            : this(currentContext, width, height, internalFormat, getInitialDataForMip,
                   (tt, l, f, w, h, p) => GL.TexImage2D(tt, l, f, w, h, 0, format, type, p))
        {
        }

        public TextureCubemap(Context currentContext,
                         int width, int height,
                         PixelInternalFormat internalFormat, Func<int, int> getComressedImageSizeForMip,
                         Func<CubemapFace, int, Data> getCompressedInitialDataForMip)
            : this(currentContext, width, height, internalFormat, getCompressedInitialDataForMip,
                   (tt, l, f, w, h, p) => GL.CompressedTexImage2D(tt, l, f, w, h, 0, getComressedImageSizeForMip(l), p))
        {
        }
    }
}