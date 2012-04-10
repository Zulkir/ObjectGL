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
using OpenTK.Graphics.OpenGL;

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
        readonly DepthStencilAspect depthStencil;
        readonly BlendAspect blend;


        internal Pipeline(Context context)
        {
            this.context = context;

            textures = new TexturesAspect(context);
            samplers = new SamplersAspect(context);
            uniformBuffers = new UniformBuffersAspect(context);
            rasterizer = new RasterizerAspect();
            depthStencil = new DepthStencilAspect();
            blend = new BlendAspect(context.Capabilities.MaxDrawBuffers);
        }

        #region Setters
        public VertexArray VertexArray
        {
            get { return vertexArray; }
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
            get { return program; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");

                program = value;
            }
        }

        public UniformBuffersAspect UniformBuffers { get { return uniformBuffers; } }

        public RasterizerAspect Rasterizer { get { return rasterizer; } }
        public DepthStencilAspect DepthStencil { get { return depthStencil; } }
        public BlendAspect Blend { get { return blend; } }
        #endregion

        #region Draw
        public void DrawArrays(BeginMode mode, int first, int count)
        {
            context.ConsumePipeline();
            GL.DrawArrays(mode, first, count);
        }

        // todo: DrawArraysIndirect

        public void DrawArraysInstanced(BeginMode mode, int first, int count, int primcount)
        {
            context.ConsumePipeline();
            GL.DrawArraysInstanced(mode, first, count, primcount);
        }

        // todo: DrawArraysInstancedBaseInstance

        public void DrawElements(BeginMode mode, int count, DrawElementsType type, int offset)
        {
            context.ConsumePipeline();
            GL.DrawElements(mode, count, type, offset);
        }

        public void DrawElementsBaseVertex(BeginMode mode, int count, DrawElementsType type, int offset, int basevertex)
        {
            context.ConsumePipeline();
            GL.DrawElementsBaseVertex(mode, count, type, (IntPtr)offset, basevertex);
        } 

        // todo: DrawElementsIndirect

        public void DrawElementsInstanced(BeginMode mode, int count, DrawElementsType type, int offset, int primcount)
        {
            context.ConsumePipeline();
            GL.DrawElementsInstanced(mode, count, type, (IntPtr)offset, primcount);
        }

        // todo: DrawElementsInstancedBaseInstance

        public void DrawElementsInstancedBaseVertex(BeginMode mode, int count, DrawElementsType type, int offset, int primcount, int basevertex)
        {
            context.ConsumePipeline();
            GL.DrawElementsInstancedBaseVertex(mode, count, type, (IntPtr)offset, primcount, basevertex);
        }

        // todo: DrawElementsInstancedBaseVertexBaseInstance

        public void DrawRangeElements(BeginMode mode, int start, int end, int count, DrawElementsType type, int offset)
        {
            context.ConsumePipeline();
            GL.DrawRangeElements(mode, start, end, count, type, (IntPtr)offset);
        }

        public void DrawRangeElementsBaseVertex(BeginMode mode, int start, int end, int count, DrawElementsType type, int offset, int basevertex)
        {
            context.ConsumePipeline();
            GL.DrawRangeElementsBaseVertex(mode, start, end, count, type, (IntPtr)offset, basevertex);
        }

        // todo: DrawTransformFeedback

        // todo: DrawTransformFeedbackInstanced

        // todo: DrawTransformFeedbackStream

        // todo: DrawTransformFeedbackStreamInstanced

        #endregion
    }
}
