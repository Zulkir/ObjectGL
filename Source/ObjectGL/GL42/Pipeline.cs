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

namespace ObjectGL.GL42
{
    public partial class Pipeline
    {
        readonly Context context;

        public Context Context { get { return context; } }

        ShaderProgram program;
        readonly UniformBuffersAspect uniformBuffers;

        VertexArray vertexArray;
        int patchVertexCount;
        Buffer drawIndirectBuffer;
        readonly TexturesAspect textures;
        readonly SamplersAspect samplers;

        Framebuffer framebuffer;
        readonly ViewportsAspect viewports;

        readonly RasterizerAspect rasterizer;
        readonly DepthStencilAspect depthStencil;
        readonly BlendAspect blend;

        internal Pipeline(Context context)
        {
            this.context = context;

            uniformBuffers = new UniformBuffersAspect(context);
            textures = new TexturesAspect(context);
            samplers = new SamplersAspect(context);
            viewports = new ViewportsAspect(context);
            rasterizer = new RasterizerAspect();
            depthStencil = new DepthStencilAspect();
            blend = new BlendAspect(context);
        }

        #region Setters
        public ShaderProgram Program
        {
            get { return program; }
            set { program = value; }
        }

        public UniformBuffersAspect UniformBuffers { get { return uniformBuffers; } }

        public VertexArray VertexArray
        {
            get { return vertexArray; }
            set { vertexArray = value; }
        }

        public Buffer DrawIndirectBuffer
        {
            get { return drawIndirectBuffer; }
            set { drawIndirectBuffer = value; }
        }

        public int PatchVertexCount
        {
            get { return patchVertexCount; }
            set { patchVertexCount = value; }
        }

        public TexturesAspect Textures { get { return textures; } }
        public SamplersAspect Samplers { get { return samplers; } }

        public Framebuffer Framebuffer
        {
            get { return framebuffer; }
            set { framebuffer = value; }
        }

        public ViewportsAspect Viewports { get { return viewports; } }

        public RasterizerAspect Rasterizer { get { return rasterizer; } }
        public DepthStencilAspect DepthStencil { get { return depthStencil; } }
        public BlendAspect Blend { get { return blend; } }
        #endregion

        
    }
}
