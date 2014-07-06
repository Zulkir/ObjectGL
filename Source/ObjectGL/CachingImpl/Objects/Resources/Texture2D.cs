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
using ObjectGL.Api.Objects.Resources;

namespace ObjectGL.CachingImpl.Objects.Resources
{
    internal class Texture2D : Texture, ITexture2D
    {
        public Texture2D(Context context,
                         int width, 
                         int height, 
                         int mipCount,
                         Format internalFormat)
            : base(context, TextureTarget.Texture2D, width, height, 1, internalFormat, 1, mipCount)
        {
            Context.BindTexture(Target, this);
            int mipWidth = width;
            int mipHeight = height;
            for (int i = 0; i < MipCount; i++)
            {
                GL.TexImage2D((int)Target, i, (int)internalFormat, mipWidth, mipHeight, 0, (int)GetAppropriateFormatColor(internalFormat), (int)GetAppropriateFormatType(internalFormat), IntPtr.Zero);
                mipWidth = Math.Max(mipWidth / 2, 1);
                mipHeight = Math.Max(mipHeight / 2, 1);
            }
        }

        public Texture2D(Context context,
                         int width, 
                         int height, 
                         int mipCount,
                         Format internalFormat, 
                         Func<int, IntPtr> getInitialDataForMip,
                         FormatColor format, 
                         FormatType type,
                         Func<int, ByteAlignment> getRowByteAlignmentForMip)
            : base(context, TextureTarget.Texture2D, width, height, 1, internalFormat, 1, mipCount)
        {
            Context.BindTexture(Target, this);
            int mipWidth = width;
            int mipHeight = height;
            for (int i = 0; i < MipCount; i++)
            {
                Context.SetUnpackAlignment(getRowByteAlignmentForMip(i));
                GL.TexImage2D((int)Target, i, (int)internalFormat, mipWidth, mipHeight, 0, (int)format, (int)type, getInitialDataForMip(i));
                mipWidth = Math.Max(mipWidth / 2, 1);
                mipHeight = Math.Max(mipHeight / 2, 1);
            }
        }

        public Texture2D(Context context,
                         int width, 
                         int height, 
                         int mipCount,
                         Format internalFormat, 
                         Func<int, IntPtr> getCompressedInitialDataForMip,
                         Func<int, int> getComressedImageSizeForMip)
            : base(context, TextureTarget.Texture2D, width, height, 1, internalFormat, 1, mipCount)
        {
            Context.BindTexture(Target, this);
            int mipWidth = width;
            int mipHeight = height;
            for (int i = 0; i < MipCount; i++)
            {
                GL.CompressedTexImage2D((int)Target, i, (int)internalFormat, mipWidth, mipHeight, 0, getComressedImageSizeForMip(i), getCompressedInitialDataForMip(i));
                mipWidth = Math.Max(mipWidth / 2, 1);
                mipHeight = Math.Max(mipHeight / 2, 1);
            }
        }

        public void SetData(int level,IntPtr data, FormatColor format, FormatType type, ByteAlignment unpackAlignment = ByteAlignment.Four)
        {
            Context.SetUnpackAlignment(unpackAlignment);
            Context.BindTexture(Target, this);
            GL.TexImage2D((int)Target, level, (int)InternalFormat, CalculateMipSize(level, Width), CalculateMipSize(level, Height), 0, (int)format, (int)type, data);
        }

        public void SetData(int level, IntPtr data, int compressedSize)
        {
            Context.BindTexture(Target, this);
            GL.CompressedTexImage2D((int)Target, level, (int)InternalFormat, CalculateMipSize(level, Width), CalculateMipSize(level, Height), 0, compressedSize, data);
        }
    }
}