﻿#region License
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

using ObjectGL.Api.Context;
using ObjectGL.Api.Objects.Resources;

namespace ObjectGL.CachingImpl.Objects.Resources
{
    internal class Texture2DMultisample : Texture, ITexture2DMultisample
    {
        private readonly int samples;
        private readonly bool fixedSampleLocations;

        public int Samples { get { return samples; } }
        public bool FixedSampleLocations { get { return fixedSampleLocations; } }

        public Texture2DMultisample(IContext context, int width, int height, int samples, Format internalFormat, bool fixedSampleLocations = false)
            : base(context, TextureTarget.Texture2DMultisample, width, height, 1, internalFormat, 1, 1)
        {
            this.samples = samples;
            this.fixedSampleLocations = fixedSampleLocations;
            Context.Bindings.Textures.Units[Context.Bindings.Textures.EditingIndex].Set(this);
            GL.TexStorage2DMultisample((int)Target, samples, (int)internalFormat, width, height, fixedSampleLocations);
        }
    }
}