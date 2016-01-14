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
using ObjectGL.Api.Objects.Resources.Buffers;
using ObjectGL.Helper;

namespace ObjectGL.Api.Objects.Resources.Images
{
    public static class TextureExtensions
    {
        public static int CalculateMipWidth(this ITexture texture, int level)
        {
            return TextureHelper.CalculateMipSize(level, texture.Width);
        }

        public static int CalculateMipHeight(this ITexture texture, int level)
        {
            return TextureHelper.CalculateMipSize(level, texture.Height);
        }

        public static int CalculateMipDepth(this ITexture texture, int level)
        {
            return TextureHelper.CalculateMipSize(level, texture.Depth);
        }

        public static void SetData(this ITexture1D texture, int level, IntPtr data, FormatColor format, FormatType type, IBuffer pixelUnpackBuffer = null)
        {
            texture.SetData(level, 0, texture.CalculateMipWidth(level), data, format, type, pixelUnpackBuffer);
        }

        public static void SetDataCompressed(this ITexture1D texture, int level, IntPtr data, int compressedSize, IBuffer pixelUnpackBuffer = null)
        {
            texture.SetDataCompressed(level, 0, texture.CalculateMipWidth(level), data, compressedSize, pixelUnpackBuffer);
        }

        public static void SetData(this ITexture1DArray texture, int level, int slice, IntPtr data, FormatColor format, FormatType type, IBuffer pixelUnpackBuffer = null)
        {
            texture.SetData(level, 0, slice, texture.CalculateMipWidth(level), 1, data, format, type, pixelUnpackBuffer);
        }

        public static void SetDataCompressed(this ITexture1DArray texture, int level, int slice, IntPtr data, int compressedSize, IBuffer pixelUnpackBuffer = null)
        {
            texture.SetDataCompressed(level, 0, slice, texture.CalculateMipWidth(level), 1, data, compressedSize, pixelUnpackBuffer);
        }

        public static void SetData(this ITexture2D texture, int level, IntPtr data, FormatColor format, FormatType type, IBuffer pixelUnpackBuffer = null)
        {
            texture.SetData(level, 0, 0, texture.CalculateMipWidth(level), texture.CalculateMipHeight(level), data, format, type, pixelUnpackBuffer);
        }

        public static void SetDataCompressed(this ITexture2D texture, int level, IntPtr data, int compressedSize, IBuffer pixelUnpackBuffer = null)
        {
            texture.SetDataCompressed(level, 0, 0, texture.CalculateMipWidth(level), texture.CalculateMipHeight(level), data, compressedSize, pixelUnpackBuffer);
        }

        public static void SetData(this ITexture2DArray texture, int level, int slice, IntPtr data, FormatColor format, FormatType type, IBuffer pixelUnpackBuffer = null)
        {
            texture.SetData(level, 0, 0, slice, texture.CalculateMipWidth(level), texture.CalculateMipHeight(level), 1, data, format, type, pixelUnpackBuffer);
        }

        public static void SetDataCompressed(this ITexture2DArray texture, int level, int slice, IntPtr data, int compressedSize, IBuffer pixelUnpackBuffer = null)
        {
            texture.SetDataCompressed(level, 0, 0, slice, texture.CalculateMipWidth(level), texture.CalculateMipHeight(level), 1, data, compressedSize, pixelUnpackBuffer);
        }

        public static void SetData(this ITexture3D texture, int level, IntPtr data, FormatColor format, FormatType type, IBuffer pixelUnpackBuffer = null)
        {
            texture.SetData(level, 0, 0, 0, texture.CalculateMipWidth(level), texture.CalculateMipHeight(level), texture.CalculateMipDepth(level), data, format, type, pixelUnpackBuffer);
        }

        public static void SetDataCompressed(this ITexture3D texture, int level, IntPtr data, int compressedSize, IBuffer pixelUnpackBuffer = null)
        {
            texture.SetDataCompressed(level, 0, 0, 0, texture.CalculateMipWidth(level), texture.CalculateMipHeight(level), texture.CalculateMipDepth(level), data, compressedSize, pixelUnpackBuffer);
        }

        public static void SetData(this ITextureCubemap texture, int level, int faceIndex, IntPtr data, FormatColor format, FormatType type, IBuffer pixelUnpackBuffer = null)
        {
            texture.SetData(level, faceIndex, 0, 0, texture.CalculateMipWidth(level), texture.CalculateMipHeight(level), data, format, type, pixelUnpackBuffer);
        }

        public static void SetDataCompressed(this ITextureCubemap texture, int level, int faceIndex, IntPtr data, int compressedSize, IBuffer pixelUnpackBuffer = null)
        {
            texture.SetDataCompressed(level, faceIndex, 0, 0, texture.CalculateMipWidth(level), texture.CalculateMipHeight(level), data, compressedSize, pixelUnpackBuffer);
        }

        public static void SetData(this ITextureCubemapArray texture, int level, int faceIndex, IntPtr data, FormatColor format, FormatType type, IBuffer pixelUnpackBuffer = null)
        {
            texture.SetData(level, 0, 0, faceIndex, texture.CalculateMipWidth(level), texture.CalculateMipHeight(level), 1, data, format, type, pixelUnpackBuffer);
        }

        public static void SetDataCompressed(this ITextureCubemapArray texture, int level, int faceIndex, IntPtr data, int compressedSize, IBuffer pixelUnpackBuffer = null)
        {
            texture.SetDataCompressed(level, 0, 0, faceIndex, texture.CalculateMipWidth(level), texture.CalculateMipHeight(level), 1, data, compressedSize, pixelUnpackBuffer);
        }
    }
}