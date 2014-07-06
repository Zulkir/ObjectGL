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

using System;
using ObjectGL.Api.Objects.Resources;
using ObjectGL.Api.PipelineAspects;

namespace ObjectGL.CachingImpl.PipelineAspects
{
    internal class PipelineTexturesAspect : IPipelineTexturesAspect
    {
        private readonly ITexture[] textures;
        private int enabledTextureRange;

        public int EnabledTextureRange { get { return enabledTextureRange; } }

        internal PipelineTexturesAspect(Context context)
        {
            textures = new ITexture[context.Implementation.MaxCombinedTextureImageUnits];
        }

        public ITexture this[int unit]
        {
            get
            {
                return textures[unit];
            }
            set
            {
                if (unit < 0 || unit >= textures.Length) 
                    throw new ArgumentOutOfRangeException("unit");

                textures[unit] = value;

                if (value == null && unit == enabledTextureRange - 1)
                    while (enabledTextureRange > 0 && textures[enabledTextureRange - 1] == null)
                        enabledTextureRange--;
                else if (value != null && unit >= enabledTextureRange)
                    enabledTextureRange = unit + 1;
            }
        }

        public void UnsetAllStartingFrom(int unit)
        {
            if (enabledTextureRange > unit)
                enabledTextureRange = unit;
        }
    }
}
