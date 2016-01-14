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

namespace ObjectGL.Api.Objects.Resources.Images
{
    public enum Format
    {
        Rgba32f = 34836,
        Rgba32ui = 36208,
        Rgba32i = 36226,
        Rgb32f = 34837,
        Rgb32ui = 36209,
        Rgb32i = 36227,
        Rgba16f = 34842,
        Rgba16 = 32859,
        Rgba16ui = 36214,
        Rgba16sn = 0x8F9B,
        Rgba16i = 36232,
        Rg32f = 33328,
        Rg32ui = 33340,
        Rg32i = 33339,
        Depth32fStencil8 = 36013,
        Rgb10A2 = 32857,
        Rgb10A2ui = 36975,
        R11fG11fB10f = 35898,
        Rgba8 = 32856,
        Srgb8Alpha8 = 35907,
        Rgba8ui = 36220,
        Rgba8sn = 0x8F97,
        Rgba8i = 36238,
        Rg16f = 33327,
        Rg16 = 33324,
        Rg16ui = 33338,
        Rg16sn = 0x8F99,
        Rg16i = 33337,
        DepthComponent32f = 36012,
        R32f = 33326,
        R32ui = 33334,
        R32i = 33333,
        Depth24Stencil8 = 35056,
        Rg8 = 33323,
        Rg8ui = 33336,
        Rg8sn = 0x8F95,
        Rg8i = 33335,
        R16f = 33325,
        DepthComponent16 = 33189,
        R16 = 33322,
        R16ui = 33332,
        R16sn = 0x8F98,
        R16i = 33331,
        R8 = 33321,
        R8ui = 33330,
        R8sn = 0x8F94,
        R8i = 33329,
        // A8
        // R1
        Rgb9E5 = 35901,
        // rg_bg
        // gr_gb
        CompressedRgbaS3tcDxt1Ext = 33777,
        CompressedSrgbAlphaS3tcDxt1Ext = 35917,
        CompressedRgbaS3tcDxt3Ext = 33778,
        CompressedSrgbAlphaS3tcDxt3Ext = 35918,
        CompressedRgbaS3tcDxt5Ext = 33779,
        CompressedSrgbAlphaS3tcDxt5Ext = 35919,
        CompressedRedRgtc1 = 36283,
        CompressedSignedRedRgtc1 = 36284,
        CompressedRgRgtc2 = 36285,
        CompressedSignedRgRgtc2 = 36286,
        CompressedRgbaBptcUf = 0x8E8F,
        CompressedRgbaBptcSf = 0x8E8E,
        CompressedRgbaBptc = 0x8E8C,
        CompressedSrgbAlphaBptc = 0x8E8D
    }
}
