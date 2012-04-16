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
    public class Texture3D : Texture
    {
        readonly int width;
        readonly int height;
        readonly int depth;

        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public int Depth { get { return depth; } }

        public Texture3D(Context currentContext,
            int width, int height, int depth,
            PixelInternalFormat internalFormat, PixelFormat format, PixelType type, Func<int, Data> initialDataForMip)
            : base(TextureTarget.Texture3D, internalFormat, 1, CalculateMipCount(width, height, depth))
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
                var data = initialDataForMip(i);
                GL.TexImage3D(Target, i, internalFormat, mipWidth, mipHeight, mipDepth, 0, format, type, data.Pointer);
                data.UnpinPointer();

                mipWidth = Math.Max(mipWidth / 2, 1);
                mipHeight = Math.Max(mipHeight / 2, 1);
                mipDepth = Math.Max(mipDepth / 2, 1);
            }
        }

        public Texture3D(Context currentContext,
            int width, int height, int depth,
            PixelInternalFormat internalFormat, Func<int, int> getComressedImageSizeForMip, Func<int, Data> getCompressedInitialDataForMip)
            : base(TextureTarget.Texture3D, internalFormat, 1, CalculateMipCount(width, height, depth))
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
                var data = getCompressedInitialDataForMip(i);
                GL.CompressedTexImage3D(Target, i, internalFormat, mipWidth, mipHeight, mipDepth, 0, getComressedImageSizeForMip(i), data.Pointer);
                data.UnpinPointer();

                mipWidth = Math.Max(mipWidth / 2, 1);
                mipHeight = Math.Max(height / 2, 1);
                mipDepth = Math.Max(mipDepth / 2, 1);
            }
        }
    }
}