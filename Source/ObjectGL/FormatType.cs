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

namespace ObjectGL
{
    public enum FormatType
    {
        Byte = 5120,
        UnsignedByte = 5121,
        Short = 5122,
        UnsignedShort = 5123,
        Int = 5124,
        UnsignedInt = 5125,
        Float = 5126,
        UnsignedByte332 = 32818,
        UnsignedByte233Reversed = 33634,
        UnsignedShort565 = 33635,
        UnsignedShort565Reversed = 33636,
        UnsignedShort4444 = 32819,
        UnsignedShort4444Reversed = 33637,
        UnsignedShort5551 = 32820,
        UnsignedShort1555Reversed = 33638,
        UnsignedInt8888 = 32821,
        UnsignedInt8888Reversed = 33639,
        UnsignedInt1010102 = 32822,
        UnsignedInt2101010Reversed = 33640,

        // todo: remove when glTexStorage is available

        HalfFloat = 5131,
        Bitmap = 6656,
        UnsignedInt248 = 34042,
        UnsignedInt10F11F11FRev = 35899,
        UnsignedInt5999Rev = 35902,
        Float32UnsignedInt248Rev = 36269,
    }
}
