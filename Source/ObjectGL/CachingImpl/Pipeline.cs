#region License
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

using ObjectGL.Api;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;
using ObjectGL.Api.PipelineAspects;
using ObjectGL.CachingImpl.PipelineAspects;

namespace ObjectGL.CachingImpl
{
    internal class Pipeline : IPipeline
    {
        private IShaderProgram program;
        private readonly PipelineUniformBuffersAspect uniformBuffers;

        private IVertexArray vertexArray;
        private int patchVertexCount;
        private IBuffer drawIndirectBuffer;
        private readonly IPipelineTexturesAspect textures;
        private readonly IPipelineSamplersAspect samplers;

        private IFramebuffer framebuffer;
        private readonly IPipelineViewportsAspect viewports;
        private readonly IPipelineScissorBoxesAspect scissorBoxes;

        private readonly IPipelineRasterizerAspect rasterizer;
        private readonly IPipelineDepthStencilAspect depthStencil;
        private readonly IPipelineBlendAspect blend;

        internal Pipeline(OldContext context)
        {
            uniformBuffers = new PipelineUniformBuffersAspect(context);
            textures = new PipelineTexturesAspect(context);
            samplers = new PipelineSamplersAspect(context);
            viewports = new PipelineViewportsAspect(context);
            scissorBoxes = new PipelineScissorBoxesAspect(context);
            rasterizer = new PipelineRasterizerAspect();
            depthStencil = new PipelineDepthStencilAspect();
            blend = new PipelineBlendAspect(context);
        }

        public IShaderProgram Program
        {
            get { return program; }
            set { program = value; }
        }

        public IPipelineUniformBuffersAspect UniformBuffers { get { return uniformBuffers; } }

        public IVertexArray VertexArray
        {
            get { return vertexArray; }
            set { vertexArray = value; }
        }

        public IBuffer DrawIndirectBuffer
        {
            get { return drawIndirectBuffer; }
            set { drawIndirectBuffer = value; }
        }

        public int PatchVertexCount
        {
            get { return patchVertexCount; }
            set { patchVertexCount = value; }
        }

        public IPipelineTexturesAspect Textures { get { return textures; } }
        public IPipelineSamplersAspect Samplers { get { return samplers; } }

        public IFramebuffer Framebuffer
        {
            get { return framebuffer; }
            set { framebuffer = value; }
        }

        public IPipelineViewportsAspect Viewports { get { return viewports; } }
        public IPipelineScissorBoxesAspect ScissorBoxes { get { return scissorBoxes; } }

        public IPipelineRasterizerAspect Rasterizer { get { return rasterizer; } }
        public IPipelineDepthStencilAspect DepthStencil { get { return depthStencil; } }
        public IPipelineBlendAspect Blend { get { return blend; } }
    }
}
