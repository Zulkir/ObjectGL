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

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ObjectGL.v42
{
    public class Sampler : IContextObject
    {
        readonly int handle;

        public int Handle { get { return handle; } }
        public ContextObjectType ContextObjectType { get { return ContextObjectType.Sampler; } }

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

        public void SetMinFilter(TextureMinFilter filter)
        {
            if (minFilter == filter) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureMinFilter, (int)filter);
            minFilter = filter;
        }

        public void SetMagFilter(TextureMagFilter filter)
        {
            if (magFilter == filter) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureMagFilter, (int)filter);
            magFilter = filter;
        }

        public void SetWrapS(TextureWrapMode mode)
        {
            if (wrapS == mode) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureWrapS, (int)mode);
            wrapS = mode;
        }

        public void SetWrapT(TextureWrapMode mode)
        {
            if (wrapT == mode) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureWrapT, (int)mode);
            wrapT = mode;
        }

        public void SetWrapR(TextureWrapMode mode)
        {
            if (wrapR == mode) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureWrapR, (int)mode);
            wrapR = mode;
        }

        public void SetMaxAnisotropy(float value)
        {
            if (maxAnisotropy == value) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureMaxAnisotropyExt, value);
            maxAnisotropy = value;
        }

        public void SetCompareMode(TextureCompareMode mode)
        {
            if (compareMode == mode) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureCompareMode, (int)mode);
            compareMode = mode;
        }

        public void SetCompareFunc(CompareFunc func)
        {
            if (compareFunc == func) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureCompareFunc, (int)func);
            compareFunc = func;
        }

        public unsafe void SetBorderColor(Color4 color)
        {
            if (Helpers.ColorsEqual(ref borderColor, ref color)) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureBorderColor, (float*)&color);
            borderColor = color;
        }

        public unsafe void SetBorderColor(float* color)
        {
            if (Helpers.ColorsEqual(ref borderColor, ref *(Color4*)color)) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureBorderColor, (float*)&color);
            borderColor = *(Color4*)color;
        }

        public void SetMinLod(float value)
        {
            if (minLod == value) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureMinLod, value);
            minLod = value;
        }

        public void SetMaxLod(float value)
        {
            if (maxLod == value) return;
            GL.SamplerParameter(handle, SamplerParameter.TextureMaxLod, value);
            maxLod = value;
        }
    }
}
