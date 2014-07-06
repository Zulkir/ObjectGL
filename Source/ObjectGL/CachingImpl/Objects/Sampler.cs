#region License
/*
Copyright (c) 2010-2014 ObjectGL Project - Daniil Rodin

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

using ObjectGL.Api;
using ObjectGL.Api.Objects;

namespace ObjectGL.CachingImpl.Objects
{
    internal class Sampler : ISampler
    {
        private readonly Context context;
        private readonly uint handle;

        private TextureMinFilter minFilter = TextureMinFilter.Linear;
        private TextureMagFilter magFilter = TextureMagFilter.Linear;
        private TextureWrapMode wrapS = TextureWrapMode.Repeat;
        private TextureWrapMode wrapT = TextureWrapMode.Repeat;
        private TextureWrapMode wrapR = TextureWrapMode.Repeat;
        private float maxAnisotropy = 1f;
        private TextureCompareMode compareMode = TextureCompareMode.None;
        private CompareFunc compareFunc;
        private Color4 borderColor;
        private float minLod = -1000f;
        private float maxLod = 1000f;

        private IGL GL { get { return context.GL; }}

        public uint Handle { get { return handle; } }
        public ContextObjectType ContextObjectType { get { return ContextObjectType.Sampler; } }

        public unsafe Sampler(Context context)
        {
            this.context = context;
            uint handleProxy;
            GL.GenSamplers(1, &handleProxy);
            handle = handleProxy;
        }

        public unsafe void Dispose()
        {
            uint handleProxy = handle;
            GL.DeleteSamplers(1, &handleProxy);
        }

        public void SetMinFilter(TextureMinFilter filter)
        {
            if (minFilter == filter) return;
            GL.SamplerParameter(handle, (int)All.TextureMinFilter, (int)filter);
            minFilter = filter;
        }

        public void SetMagFilter(TextureMagFilter filter)
        {
            if (magFilter == filter) return;
            GL.SamplerParameter(handle, (int)All.TextureMagFilter, (int)filter);
            magFilter = filter;
        }

        public void SetWrapS(TextureWrapMode mode)
        {
            if (wrapS == mode) return;
            GL.SamplerParameter(handle, (int)All.TextureWrapS, (int)mode);
            wrapS = mode;
        }

        public void SetWrapT(TextureWrapMode mode)
        {
            if (wrapT == mode) return;
            GL.SamplerParameter(handle, (int)All.TextureWrapT, (int)mode);
            wrapT = mode;
        }

        public void SetWrapR(TextureWrapMode mode)
        {
            if (wrapR == mode) return;
            GL.SamplerParameter(handle, (int)All.TextureWrapR, (int)mode);
            wrapR = mode;
        }

        public void SetMaxAnisotropy(float value)
        {
            if (maxAnisotropy == value) return;
            GL.SamplerParameter(handle, (int)All.TextureMaxAnisotropyExt, value);
            maxAnisotropy = value;
        }

        public void SetCompareMode(TextureCompareMode mode)
        {
            if (compareMode == mode) return;
            GL.SamplerParameter(handle, (int)All.TextureCompareMode, (int)mode);
            compareMode = mode;
        }

        public void SetCompareFunc(CompareFunc func)
        {
            if (compareFunc == func) return;
            GL.SamplerParameter(handle, (int)All.TextureCompareFunc, (int)func);
            compareFunc = func;
        }
        public unsafe void SetBorderColor(Color4 color)
        {
            if (Color4.AreEqual(ref borderColor, ref color)) return;
            GL.SamplerParameter(handle, (int)All.TextureBorderColor, (float*)&color);
            borderColor = color;
        }

        public void SetMinLod(float value)
        {
            if (minLod == value) return;
            GL.SamplerParameter(handle, (int)All.TextureMinLod, value);
            minLod = value;
        }

        public void SetMaxLod(float value)
        {
            if (maxLod == value) return;
            GL.SamplerParameter(handle, (int)All.TextureMaxLod, value);
            maxLod = value;
        }
    }
}
