#region License
/*
Copyright (c) 2010-2014 ObjectGL Project - Daniil Rodin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion

using System;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;

namespace ObjectGL.CachingImpl.Objects.Resources
{
    internal class TextureCubemapArray : Texture, ITextureCubemapArray
    {
        public TextureCubemapArray(Context context,
                                   int width,
                                   int cubeCount,
                                   int mipCount,
                                   Format internalFormat)
            : base(context, TextureTarget.TextureCubeMapArray, width, width, 1, internalFormat, cubeCount, mipCount)
        {
            Context.BindTexture(Target, this);
            int mipWidth = width;
            for (int i = 0; i < MipCount; i++)
            {
                GL.TexImage3D((int)Target, i, (int)internalFormat, mipWidth, mipWidth, cubeCount, 0, (int)GetAppropriateFormatColor(internalFormat), (int)GetAppropriateFormatType(internalFormat), IntPtr.Zero);
                mipWidth = Math.Max(mipWidth / 2, 1);
            }
        }

        public TextureCubemapArray(Context context,
                                   int width,
                                   int cubeCount,
                                   int mipCount,
                                   Format internalFormat,
                                   Func<int, IntPtr> getInitialDataForMip,
                                   FormatColor format, 
                                   FormatType type,
                                   Func<int, ByteAlignment> getRowByteAlignmentForMip)
            : base(context, TextureTarget.TextureCubeMapArray, width, width, 1, internalFormat, cubeCount, mipCount)
        {
            Context.BindTexture(Target, this);
            int mipWidth = width;
            for (int i = 0; i < MipCount; i++)
            {
                Context.SetUnpackAlignment(getRowByteAlignmentForMip(i));
                GL.TexImage3D((int)Target, i, (int)internalFormat, mipWidth, mipWidth, cubeCount, 0, (int)format, (int)type, getInitialDataForMip(i));
                mipWidth = Math.Max(mipWidth / 2, 1);
            }
        }

        public TextureCubemapArray(Context context,
                                   int width, 
                                   int cubeCount, 
                                   int mipCount,
                                   Format internalFormat,
                                   Func<int, IntPtr> getCompressedInitialDataForMip,
                                   Func<int, int> getComressedImageSizeForMip)
            : base(context, TextureTarget.TextureCubeMapArray, width, width, 1, internalFormat, cubeCount, mipCount)
        {
            Context.BindTexture(Target, this);
            int mipWidth = width;
            for (int i = 0; i < MipCount; i++)
            {
                GL.CompressedTexImage3D((int)Target, i, (int)internalFormat, mipWidth, mipWidth, cubeCount, 0, getComressedImageSizeForMip(i), getCompressedInitialDataForMip(i));
                mipWidth = Math.Max(mipWidth / 2, 1);
            }
        }

        public void SetData(int slice, CubemapFace face, int level, IntPtr data, FormatColor format, FormatType type, ByteAlignment unpackAlignment = ByteAlignment.Four)
        {
            Context.SetUnpackAlignment(unpackAlignment);
            Context.BindTexture(Target, this);
            GL.TexSubImage3D((int)Target, level, 0, 0, slice * 6 + (face - CubemapFace.PositiveX), CalculateMipSize(level, Width), CalculateMipSize(level, Height), 1, (int)format, (int)type, data);
        }

        public void SetData(int slice, CubemapFace face, int level, IntPtr data, int compressedSize)
        {
            Context.BindTexture(Target, this);
            GL.CompressedTexSubImage3D((int)Target, level, 0, 0, slice * 6 + (face - CubemapFace.PositiveX), CalculateMipSize(level, Width), CalculateMipSize(level, Height), 1, (int)InternalFormat, compressedSize, data);
        }
    }
}