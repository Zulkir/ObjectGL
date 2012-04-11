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
using OpenTK.Graphics.OpenGL;

namespace ObjectGL
{
    public abstract class Texture : IDisposable
    {
        readonly int handle;

        readonly TextureTarget target;
        readonly PixelInternalFormat internalFormat;
        readonly int arraySize;
        readonly int mipCount;

        int baseLevel;
        int maxLevel;
        float lodBias;

        public int Handle { get { return handle; } }

        public TextureTarget Target { get { return target; } }
        public PixelInternalFormat InternalFormat { get { return internalFormat; } }
        public int ArraySize { get { return arraySize; } }
        public int MipCount { get { return mipCount; } }

        /*
        TextureMinFilter minFilter;
        TextureMagFilter magFilter;
        TextureWrapMode wrapS = TextureWrapMode.Repeat;
        TextureWrapMode wrapT = TextureWrapMode.Repeat;
        TextureWrapMode wrapR = TextureWrapMode.Repeat;
        float lodBias;
        uint maxAnisotropy;
        TextureCompareMode compareMode;
        CompareFunc compareFunc;
        Color4 borderColor;
        float minLod = -1000f;
        float maxLod = 1000f;
        float maxAnisotropy = 16f;
        */

        protected unsafe Texture(TextureTarget target, 
            PixelInternalFormat internalFormat, int arraySize, int mipCount)
        {
            this.target = target;
            this.internalFormat = internalFormat;
            this.arraySize = arraySize;
            this.mipCount = mipCount;

            int handleProxy;
            GL.GenTextures(1, &handleProxy);
            handle = handleProxy;
        }

        public void SetBaseLevel(Context currentContext, int value)
        {
            if (baseLevel == value) return;
            currentContext.BindTexture(target, handle);
            GL.TexParameter(target, TextureParameterName.TextureBaseLevel, value);
            baseLevel = value;
        }

        public void SetMaxLevel(Context currentContext, int value)
        {
            if (maxLevel == value) return;
            currentContext.BindTexture(target, handle);
            GL.TexParameter(target, TextureParameterName.TextureMaxLevel, value);
            maxLevel = value;
        }

        public void SetLodBias(Context currentContext, int value)
        {
            if (lodBias == value) return;
            currentContext.BindTexture(target, handle);
            GL.TexParameter(target, TextureParameterName.TextureLodBias, value);
            lodBias = value;
        }

        public void GenerateMipmap(Context currentContext)
        {
            currentContext.BindTexture(target, handle);
            GL.GenerateMipmap((GenerateMipmapTarget)target);
        }

        public unsafe void Dispose()
        {
            int handleProxy = handle;
            GL.DeleteTextures(1, &handleProxy);
        }


        public static int CalculateMipCount(int width, int height = 0, int depth = 0)
        {
            int largestDimension = Math.Max(Math.Max(width, height), depth);

            int result = 0;
            while (largestDimension > 0)
            {
                largestDimension >>= 1;
                result++;
            }

            return result;
        }
    }
}
