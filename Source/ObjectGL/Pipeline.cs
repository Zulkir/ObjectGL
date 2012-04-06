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

namespace ObjectGL
{
    public partial class Pipeline
    {
        readonly Context context;

        public Context Context { get { return context; } }

        VertexArray vertexArray;
        readonly TexturesAspect textures;
        readonly SamplersAspect samplers;
        
        ShaderProgram program;
        readonly UniformBuffersAspect uniformBuffers;

        readonly RasterizerAspect rasterizer;


        internal Pipeline(Context context)
        {
            this.context = context;

            textures = new TexturesAspect(context.Capabilities.MaxCombinedTextureImageUnits);
            samplers = new SamplersAspect(context.Capabilities.MaxCombinedTextureImageUnits);
            uniformBuffers = new UniformBuffersAspect(context.Capabilities.MaxUniformBufferBindings);
            rasterizer = new RasterizerAspect();
        }

        #region Setters
        public VertexArray VertexArray
        {
            set
            {
                if (vertexArray == null) throw new ArgumentNullException("value");

                vertexArray = value;
            }
        }

        public TexturesAspect Textures { get { return textures; } }
        public SamplersAspect Samplers { get { return samplers; } }

        public ShaderProgram Program
        {
            set
            {
                if (value == null) throw new ArgumentNullException("value");

                program = value;
            }
        }

        public UniformBuffersAspect UniformBuffers { get { return uniformBuffers; } }

        public RasterizerAspect Rasterizer { get { return rasterizer; } }
        #endregion

        #region Draw

        #endregion
    }
}
