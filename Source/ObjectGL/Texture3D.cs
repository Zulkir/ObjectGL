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
    public class Texture3D : Texture
    {
        readonly int width;
        readonly int height;
        readonly int depth;

        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public int Depth { get { return depth; } }

        Texture3D(Context currentContext,
                  int width, int height, int depth, int mipCount,
                  Format internalFormat, Func<int, Data> getInitialDataForMip,
                  Action<TextureTarget, int, PixelInternalFormat, int, int, int, IntPtr> glTexImage)
            : base(TextureTarget.Texture3D, internalFormat, 1, mipCount == 0 ? CalculateMipCount(width, height, depth) : mipCount)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;

            currentContext.BindTexture(Target, Handle);

            int mipWidth = width;
            int mipHeight = height;
            int mipDepth = depth;

            for (int i = 0; i < MipCount; i++)
            {
                Data data = getInitialDataForMip(i);
                glTexImage(Target, i, (PixelInternalFormat)internalFormat, mipWidth, mipHeight, mipDepth, data.Pointer);
                data.UnpinPointer();

                mipWidth = Math.Max(mipWidth/2, 1);
                mipHeight = Math.Max(mipHeight/2, 1);
                mipDepth = Math.Max(mipDepth/2, 1);
            }
        }

        public Texture3D(Context currentContext,
                         int width, int height, int depth, int mipCount,
                         Format internalFormat)
            : this(currentContext, width, height, depth, mipCount, internalFormat, i => new Data(IntPtr.Zero), 
                   (tt, l, f, w, h, d, p) => GL.TexImage3D(tt, l, f, w, h, d, 0,
                       (PixelFormat)GetAppropriateFormatColor(internalFormat), (PixelType)GetAppropriateFormatType(internalFormat), p))
        {
        }

        public Texture3D(Context currentContext,
                         int width, int height, int depth, int mipCount,
                         Format internalFormat, FormatColor format, FormatType type,
                         Func<int, Data> getInitialDataForMip)
            : this(currentContext, width, height, depth, mipCount, internalFormat, getInitialDataForMip,
                   (tt, l, f, w, h, d, p) => GL.TexImage3D(tt, l, f, w, h, d, 0, (PixelFormat)format, (PixelType)type, p))
        {
        }

        public Texture3D(Context currentContext,
                         int width, int height, int depth, int mipCount,
                         Format internalFormat, Func<int, int> getComressedImageSizeForMip,
                         Func<int, Data> getCompressedInitialDataForMip)
            : this(currentContext, width, height, depth, mipCount, internalFormat, getCompressedInitialDataForMip,
                   (tt, l, f, w, h, d, p) =>
                   GL.CompressedTexImage3D(tt, l, f, w, h, d, 0, getComressedImageSizeForMip(l), p))
        {
        }
    }
}