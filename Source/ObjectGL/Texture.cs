﻿#region License
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
        readonly protected int handle;

        readonly protected TextureTarget target;
        readonly protected PixelInternalFormat internalFormat;
        readonly protected int arraySize;
        readonly protected int mipCount;

        public int Handle { get { return handle; } }
        public TextureTarget Target { get { return target; } }
        public PixelInternalFormat InternalFormat { get { return internalFormat; } }
        public int ArraySize { get { return arraySize; } }
        public int MipCount { get { return mipCount; } }

        int baseLevel;
        int maxLevel;
        float lodBias;

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

        public void SetBaseLevel(Context currentContext, int baseLevel)
        {
            if (this.baseLevel == baseLevel) return;
            currentContext.BindTexture(target, handle);
            GL.TexParameter(target, TextureParameterName.TextureBaseLevel, baseLevel);
            this.baseLevel = baseLevel;
        }

        public void SetMaxLevel(Context currentContext, int maxLevel)
        {
            if (this.maxLevel == maxLevel) return;
            currentContext.BindTexture(target, handle);
            GL.TexParameter(target, TextureParameterName.TextureMaxLevel, maxLevel);
            this.maxLevel = maxLevel;
        }

        public void SetLodBias(Context currentContext, int lodBias)
        {
            if (this.lodBias == lodBias) return;
            currentContext.BindTexture(target, handle);
            GL.TexParameter(target, TextureParameterName.TextureLodBias, lodBias);
            this.lodBias = lodBias;
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