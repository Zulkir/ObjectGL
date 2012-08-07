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

using System;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ObjectGL.GL42
{
    static class Helpers
    {
        public static bool ColorsEqual(ref Color4 c1, ref Color4 c2)
        {
            return
                c1.R == c2.R &&
                c1.G == c2.G &&
                c1.B == c2.B &&
                c1.A == c2.A;
        }

        public static int ObjectHandle(IContextObject contextObject)
        {
            return contextObject != null ? contextObject.Handle : 0;
        }

        public static int CubemapFaceIndex(CubemapFace face)
        {
            switch (face)
            {
                case CubemapFace.PositiveX: return 0;
                case CubemapFace.NegativeX: return 1;
                case CubemapFace.PositiveY: return 2;
                case CubemapFace.NegativeY: return 3;
                case CubemapFace.PositiveZ: return 4;
                case CubemapFace.NegativeZ: return 5;
                default: throw new ArgumentOutOfRangeException("face");
            }
        }

        public static int SizeOfFormat(SizedInternalFormat internalFormat)
        {
            switch (internalFormat)
            {
                case SizedInternalFormat.Rgba32f:
                case SizedInternalFormat.Rgba32ui:
                case SizedInternalFormat.Rgba32i:
                    return 16;
                case SizedInternalFormat.Rgba16f:
                case SizedInternalFormat.Rgba16:
                case SizedInternalFormat.Rgba16ui:
                case SizedInternalFormat.Rgba16i:
                case SizedInternalFormat.Rg32f:
                case SizedInternalFormat.Rg32ui:
                case SizedInternalFormat.Rg32i:
                    return 8;
                case SizedInternalFormat.Rgba8:
                case SizedInternalFormat.Rgba8ui:
                case SizedInternalFormat.Rgba8i:
                case SizedInternalFormat.Rg16f:
                case SizedInternalFormat.Rg16:
                case SizedInternalFormat.Rg16ui:
                case SizedInternalFormat.Rg16i:
                    return 4;
                case SizedInternalFormat.R32f:
                case SizedInternalFormat.R32ui:
                case SizedInternalFormat.R32i:
                    return 4;
                case SizedInternalFormat.Rg8:
                case SizedInternalFormat.Rg8ui:
                case SizedInternalFormat.Rg8i:
                case SizedInternalFormat.R16f:
                case SizedInternalFormat.R16:
                case SizedInternalFormat.R16ui:
                case SizedInternalFormat.R16i:
                    return 2;
                case SizedInternalFormat.R8:
                case SizedInternalFormat.R8ui:
                case SizedInternalFormat.R8i:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException("internalFormat");
            }
        }
    }
}
