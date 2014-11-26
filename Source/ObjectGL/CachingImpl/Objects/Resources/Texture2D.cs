﻿#region License
/*
Copyright (c) 2012-2014 ObjectGL Project - Daniil Rodin

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
        public Texture2D(Context context, int width, int height, int mipCount, Format internalFormat)
            : base(context, TextureTarget.Texture2D, width, height, 1, internalFormat, 1, mipCount)
        {
            Context.BindTexture(Target, this);
            GL.TexStorage2D((int)Target, mipCount, (int)internalFormat, width, height);
        }

        public void SetData(int level, int xOffset, int yOffset, int width, int height, IntPtr data, FormatColor format, FormatType type, IBuffer pixelUnpackBuffer)
        {
            Context.BindBuffer(BufferTarget.PixelUnpackBuffer, pixelUnpackBuffer);
            Context.BindTexture(Target, this);
            GL.TexSubImage2D((int)Target, level, xOffset, yOffset, width, height, (int)format, (int)type, data);
        }

        public void SetDataCompressed(int level, int xOffset, int yOffset, int width, int height, IntPtr data, int compressedSize, IBuffer pixelUnpackBuffer)
        {
            Context.BindBuffer(BufferTarget.PixelUnpackBuffer, pixelUnpackBuffer);
            Context.BindTexture(Target, this);
            GL.CompressedTexSubImage2D((int)Target, level, xOffset, yOffset, width, height, (int)InternalFormat, compressedSize, data);
        }
    }
}