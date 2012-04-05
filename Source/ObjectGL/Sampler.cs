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

namespace ObjectGL
{
    public class Sampler : IDisposable
    {
        readonly int handle;

        public int Handle { get { return handle; } }

        TextureMinFilter minFilter = TextureMinFilter.Linear;
        TextureMagFilter magFilter = TextureMagFilter.Linear;
        TextureWrapMode wrapS = TextureWrapMode.Repeat;
        TextureWrapMode wrapT = TextureWrapMode.Repeat;
        TextureWrapMode wrapR = TextureWrapMode.Repeat;
        float maxAnisotropy = 1f;
        TextureCompareMode compareMode = TextureCompareMode.None;
        CompareFunc compareFunc;
        Color4 borderColor;
        float minLod = -1000f;
        float maxLod = 1000f;

        public unsafe Sampler()
        {
            int handleProxy;
            GL.GenSamplers(1, &handleProxy);
            handle = handleProxy;
        }

        public unsafe void Dispose()
        {
            int handleProxy = handle;
            GL.DeleteSamplers(1, &handleProxy);
        }

        public void SetMinFilter(TextureMinFilter minFilter)
        {
            if (this.minFilter == minFilter) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureMinFilter, (int)minFilter);
            this.minFilter = minFilter;
        }

        public void SetMagFilter(TextureMagFilter magFilter)
        {
            if (this.magFilter == magFilter) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureMagFilter, (int)magFilter);
            this.magFilter = magFilter;
        }

        public void SetWrapS(TextureWrapMode wrapS)
        {
            if (this.wrapS == wrapS) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureWrapS, (int)wrapS);
            this.wrapS = wrapS;
        }

        public void SetWrapT(TextureWrapMode wrapT)
        {
            if (this.wrapT == wrapT) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureWrapT, (int)wrapT);
            this.wrapT = wrapT;
        }

        public void SetWrapR(TextureWrapMode wrapR)
        {
            if (this.wrapR == wrapR) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureWrapR, (int)wrapR);
            this.wrapR = wrapR;
        }

        public void SetMaxAnisotropy(float maxAnisotropy)
        {
            if (this.maxAnisotropy == maxAnisotropy) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureMaxAnisotropyExt, maxAnisotropy);
            this.maxAnisotropy = maxAnisotropy;
        }

        public void SetCompareMode(TextureCompareMode compareMode)
        {
            if (this.compareMode == compareMode) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureCompareMode, (int)compareMode);
            this.compareMode = compareMode;
        }

        public void SetCompareFunc(CompareFunc compareFunc)
        {
            if (this.compareFunc == compareFunc) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureCompareFunc, (int)compareFunc);
            this.compareFunc = compareFunc;
        }

        public unsafe void SetBorderColor(Color4 borderColor)
        {
            if (Helpers.ColorsEqual(ref this.borderColor, ref borderColor)) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureBorderColor, (float*)&borderColor);
        }

        public unsafe void SetBorderColor(float* borderColor)
        {
            if (Helpers.ColorsEqual(ref this.borderColor, ref *(Color4*)borderColor)) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureBorderColor, (float*)&borderColor);
        }

        public void SetMinLod(float minLod)
        {
            if (this.minLod == minLod) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureMinLod, minLod);
            this.minLod = minLod;
        }

        public void SetMaxLod(float maxLod)
        {
            if (this.maxLod == maxLod) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureMaxLod, maxLod);
            this.maxLod = maxLod;
        }
    }
}
