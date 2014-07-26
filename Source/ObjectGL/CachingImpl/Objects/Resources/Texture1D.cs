#region License
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
    internal class Texture1D : Texture, ITexture1D
    {
        public Texture1D(Context context, int width, int mipCount, Format internalFormat)
            : base(context, TextureTarget.Texture1D, width, 1, 1, internalFormat, 1, mipCount)
        {
            Context.BindTexture(Target, this);
            GL.TexStorage1D((int)Target, mipCount, (int)internalFormat, width);
        }

        public void SetData(int level, int xOffset, int width, IntPtr data, FormatColor format, FormatType type, IBuffer pixelUnpackBuffer)
        {
            Context.BindBuffer(BufferTarget.PixelUnpackBuffer, pixelUnpackBuffer);
            Context.BindTexture(Target, this);
            GL.TexSubImage1D((int)Target, level, xOffset, width, (int)format, (int)type, data);
        }

        public void SetDataCompressed(int level, int xOffset, int width, IntPtr data, int compressedSize, IBuffer pixelUnpackBuffer)
        {
            Context.BindBuffer(BufferTarget.PixelUnpackBuffer, pixelUnpackBuffer);
            Context.BindTexture(Target, this);
            GL.CompressedTexSubImage1D((int)Target, level, xOffset, width, (int)InternalFormat, compressedSize, data);
        }
    }
}