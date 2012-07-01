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

namespace ObjectGL.GL42
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
