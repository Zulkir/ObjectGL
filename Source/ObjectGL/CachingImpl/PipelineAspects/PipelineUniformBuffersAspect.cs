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
    internal class PipelineUniformBuffersAspect : IPipelineUniformBuffersAspect
    {
        private readonly IBuffer[] uniformBuffers;
        private int enabledUniformBufferRange;

        public int EnabledUniformBufferRange { get { return enabledUniformBufferRange; } }
        public int Count { get { return uniformBuffers.Length; } }

        internal PipelineUniformBuffersAspect(Context context)
        {
            uniformBuffers = new IBuffer[context.Implementation.MaxUniformBufferBindings];
        }

        public IBuffer this[int binding]
        {
            get
            {
                return uniformBuffers[binding];
            }
            set
            {
                if (binding < 0 || binding >= uniformBuffers.Length) 
                    throw new ArgumentOutOfRangeException("binding");

                uniformBuffers[binding] = value;

                if (value == null && binding == enabledUniformBufferRange - 1)
                    while (enabledUniformBufferRange > 0 && uniformBuffers[enabledUniformBufferRange - 1] == null)
                        enabledUniformBufferRange--;
                else if (value != null && binding >= enabledUniformBufferRange)
                    enabledUniformBufferRange = binding + 1;
            }
        }

        public void UnsetAllStartingFrom(int binding)
        {
            if (enabledUniformBufferRange > binding)
                enabledUniformBufferRange = binding;
        }
    }
}
