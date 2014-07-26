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
    internal class Texture3D : Texture, ITexture3D
    {
        public Texture3D(Context context, int width, int height, int depth, int mipCount, Format internalFormat)
            : base(context, TextureTarget.ProxyTexture3D, width, height, depth, internalFormat, 1, mipCount)
        {
            Context.BindTexture(Target, this);
            GL.TexStorage3D((int)Target, mipCount, (int)internalFormat, width, height, depth);
        }

        public void SetData(int level, int xOffset, int yOffset, int zOffset, int width, int height, int depth, IntPtr data, FormatColor format, FormatType type, IBuffer pixelUnpackBuffer)
        {
            Context.BindBuffer(BufferTarget.PixelUnpackBuffer, pixelUnpackBuffer);
            Context.BindTexture(Target, this);
            GL.TexSubImage3D((int)Target, level, xOffset, yOffset, zOffset, width, height, depth, (int)format, (int)type, data);
        }

        public void SetDataCompressed(int level, int xOffset, int yOffset, int zOffset, int width, int height, int depth, IntPtr data, int compressedSize, IBuffer pixelUnpackBuffer)
        {
            Context.BindBuffer(BufferTarget.PixelUnpackBuffer, pixelUnpackBuffer);
            Context.BindTexture(Target, this);
            GL.CompressedTexSubImage3D((int)Target, level, xOffset, yOffset, zOffset, width, height, depth, (int)InternalFormat, compressedSize, data);
        }
    }
}