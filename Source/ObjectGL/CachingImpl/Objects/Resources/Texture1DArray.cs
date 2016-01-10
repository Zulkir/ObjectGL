#region License
/*
Copyright (c) 2012-2016 ObjectGL Project - Daniil Rodin

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
using ObjectGL.Api.Context;
using ObjectGL.Api.Objects.Resources;

namespace ObjectGL.CachingImpl.Objects.Resources
{
    internal class Texture1DArray : Texture, ITexture1DArray
    {
        public Texture1DArray(IContext context, int width, int sliceCount, int mipCount, Format internalFormat)
            : base(context, TextureTarget.Texture1DArray, width, 1, 1, internalFormat, sliceCount, mipCount)
        {
            Context.Bindings.Textures.Units[Context.Bindings.Textures.EditingIndex].Set(this);
            GL.TexStorage2D((int)Target, mipCount, (int)internalFormat, width, sliceCount);
        }

        public void SetData(int level, int xOffset, int sliceOffset, int width, int sliceCount, IntPtr data, FormatColor format, FormatType type, IBuffer pixelUnpackBuffer)
        {
            Context.Bindings.Buffers.PixelUnpack.Set(pixelUnpackBuffer);
            Context.Bindings.Textures.Units[Context.Bindings.Textures.EditingIndex].Set(this);
            GL.TexSubImage2D((int)Target, level, xOffset, sliceOffset, width, sliceCount, (int)format, (int)type, data);
        }

        public void SetDataCompressed(int level, int xOffset, int sliceOffset, int width, int sliceCount, IntPtr data, int compressedSize, IBuffer pixelUnpackBuffer)
        {
            Context.Bindings.Buffers.PixelUnpack.Set(pixelUnpackBuffer);
            Context.Bindings.Textures.Units[Context.Bindings.Textures.EditingIndex].Set(this);
            GL.CompressedTexSubImage2D((int)Target, level, xOffset, sliceOffset, width, sliceCount, (int)InternalFormat, compressedSize, data);
        }
    }
}