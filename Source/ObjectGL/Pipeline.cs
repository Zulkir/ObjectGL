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
    public class Pipeline
    {
        readonly Context context;

        public Context Context { get { return context; } }

        VertexArray vertexArray;
        PipelineTextures textures;

        readonly Sampler[] samplers;
        ShaderProgram program;
        readonly Buffer[] uniformBuffers;
        int enabledUniformBufferRange;

        PolygonMode polygonMode;
        CullFaceMode cullFaceMode;
        FrontFaceDirection frontFaceDirection;
        bool scissorEnabled;
        bool multisampleEnabled = true;




        internal Pipeline(Context context)
        {
            this.context = context;

            textures = new PipelineTextures(context.Capabilities.MaxCombinedTextureImageUnits);
            samplers = new Sampler[context.Capabilities.MaxCombinedTextureImageUnits];
            uniformBuffers = new Buffer[context.Capabilities.MaxUniformBufferBindings];
        }

        #region Setters
        #region VertexArray
        public VertexArray VertexArray
        {
            set
            {
                if (vertexArray == null) throw new ArgumentNullException("value");

                vertexArray = value;
            }
        }
        #endregion

        #region Textures and Samplers


        

        public void SetSampler(int unit, Sampler sampler)
        {
            if (unit < 0 || unit >= samplers.Length) throw new ArgumentOutOfRangeException("unit");
            if (sampler == null) throw new ArgumentNullException("sampler");

            samplers[unit] = sampler;
        }
        #endregion

        #region Program and Uniform Buffers
        public void SetProgram(ShaderProgram program)
        {
            if (program == null) throw new ArgumentNullException("program");

            this.program = program;
        }

        public void SetUniformBuffer(int binding, Buffer uniformBuffer)
        {
            if (binding < 0 || binding >= uniformBuffers.Length) throw new ArgumentOutOfRangeException("binding");

            uniformBuffers[binding] = uniformBuffer;

            if (uniformBuffer == null && binding == enabledUniformBufferRange - 1)
            {
                while (uniformBuffers[enabledUniformBufferRange - 1] == null)
                {
                    enabledUniformBufferRange--;
                }
            }
            else if (uniformBuffer != null && binding >= enabledUniformBufferRange)
            {
                enabledUniformBufferRange = binding + 1;
            }
        }

        public void UnsetAllUniformBuffersStartingFrom(int binding)
        {
            if (enabledUniformBufferRange > binding)
            {
                enabledUniformBufferRange = binding;
            }
        }
        #endregion

        #region Rasterizer State

        #endregion
        #endregion

        #region Draw

        #endregion
    }
}
