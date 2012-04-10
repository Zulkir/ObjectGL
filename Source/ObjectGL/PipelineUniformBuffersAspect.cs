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

namespace ObjectGL
{
    public partial class Pipeline
    {
        public class UniformBuffersAspect
        {
            readonly Context context;

            readonly Buffer[] uniformBuffers;
            int enabledUniformBufferRange;

            internal int EnabledUniformBufferRange { get { return enabledUniformBufferRange; } }

            internal UniformBuffersAspect(Context context)
            {
                this.context = context;
                uniformBuffers = new Buffer[context.Capabilities.MaxUniformBufferBindings];
            }

            public Buffer this[int binding]
            {
                get
                {
                    return uniformBuffers[binding];
                }
                set
                {
                    if (binding < 0 || binding >= uniformBuffers.Length) throw new ArgumentOutOfRangeException("binding");

                    uniformBuffers[binding] = value;

                    if (value == null && binding == enabledUniformBufferRange - 1)
                    {
                        while (uniformBuffers[enabledUniformBufferRange - 1] == null)
                        {
                            enabledUniformBufferRange--;
                        }
                    }
                    else if (value != null && binding >= enabledUniformBufferRange)
                    {
                        enabledUniformBufferRange = binding + 1;
                    }
                }
            }

            public void UnsetAllStartingFrom(int binding)
            {
                if (enabledUniformBufferRange > binding)
                {
                    enabledUniformBufferRange = binding;
                }
            }
        }
    }
}