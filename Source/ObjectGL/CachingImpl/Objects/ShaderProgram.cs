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

using System;
using System.Linq;
using ObjectGL.Api;
using ObjectGL.Api.Context;
using ObjectGL.Api.Objects;

namespace ObjectGL.CachingImpl.Objects
{
    internal class ShaderProgram : IShaderProgram
    {
        private readonly IContext context;
        private readonly uint handle;

        private IGL GL { get { return context.GL; } }

        public uint Handle { get { return handle; } }
        public GLObjectType GLObjectType { get { return GLObjectType.ShaderProgram; } }

        private ShaderProgram(IContext context, uint handle)
        {
            this.context = context;
            this.handle = handle;
        }

        public void Dispose()
        {
            GL.DeleteProgram(handle);
        }

        static readonly string[] EmptyStringArray = new string[0];
        static readonly TesselationControlShader[] EmptyTessControlShaderArray = new TesselationControlShader[0];
        static readonly TesselationEvaluationShader[] EmptyTessEvalShaderArray = new TesselationEvaluationShader[0];
        static readonly GeometryShader[] EmptyGeometryShaderArray = new GeometryShader[0];

        public static unsafe bool TryLink(
            IContext context,
            ShaderProgramDescription description,
            out ShaderProgram program,
            out string errors)
        {
            var gl = context.GL;

            description.VertexAttributeNames = description.VertexAttributeNames ?? EmptyStringArray;
            if (description.VertexAttributeNames.Any(x => x != null && description.VertexAttributeNames.Count(y => y == x) != 1))
                throw new ArgumentException("All non-null attribute names must be unique.");

            description.UniformBufferNames = description.UniformBufferNames ?? EmptyStringArray;
            if (description.UniformBufferNames.Any(x => x != null && description.UniformBufferNames.Count(y => y == x) != 1))
                throw new ArgumentException("All non-null uniform buffer names must be unique.");

            description.TransformFeedbackAttributeNames = description.TransformFeedbackAttributeNames ?? EmptyStringArray;
            if (description.TransformFeedbackAttributeNames.Any(x => !x.StartsWith("gl_") &&
                description.TransformFeedbackAttributeNames.Count(y => y == x) != 1))
                throw new ArgumentException("All transform feedback attribute names (except gl_*** strings) must be unique.");

            description.SamplerNames = description.SamplerNames ?? EmptyStringArray;
            if (description.SamplerNames.Any(x => x != null && description.SamplerNames.Count(y => y == x) != 1))
                throw new ArgumentException("All non-null sampler names must be unique.");

            uint handle = gl.CreateProgram();
            program = new ShaderProgram(context, handle);

            description.TesselationControlShaders = description.TesselationControlShaders ?? EmptyTessControlShaderArray;
            description.TesselationEvaluationShaders = description.TesselationEvaluationShaders ?? EmptyTessEvalShaderArray;
            description.GeometryShaders = description.GeometryShaders ?? EmptyGeometryShaderArray;

            foreach (var shader in description.VertexShaders)
                gl.AttachShader(handle, shader.Handle);
            foreach (var shader in description.TesselationControlShaders)
                gl.AttachShader(handle, shader.Handle);
            foreach (var shader in description.TesselationEvaluationShaders)
                gl.AttachShader(handle, shader.Handle);
            foreach (var shader in description.GeometryShaders)
                gl.AttachShader(handle, shader.Handle);
            foreach (var shader in description.FragmentShaders)
                gl.AttachShader(handle, shader.Handle);
            
            for (uint i = 0; i < description.VertexAttributeNames.Length; i++)
                if (description.VertexAttributeNames[i] != null)
                    gl.BindAttribLocation(handle, i, description.VertexAttributeNames[i]);

            if (description.TransformFeedbackAttributeNames.Length > 0)
                gl.TransformFeedbackVaryings(handle, description.TransformFeedbackAttributeNames.Length, 
                    description.TransformFeedbackAttributeNames, (int)description.TransformFeedbackMode);

            gl.LinkProgram(handle);

            int isLinked;
            gl.GetProgram(handle, (int)All.LinkStatus, &isLinked);
            if (isLinked == 0)
            {
                program = null;
                errors = gl.GetProgramInfoLog(handle);
                return false;
            }

            for (int i = 0; i < description.VertexAttributeNames.Length; i++)
            {
                if (description.VertexAttributeNames[i] != null && gl.GetAttribLocation(handle, description.VertexAttributeNames[i]) != i)
                    throw new ArgumentException(string.Format("Vertex attribute '{0}' is not present in the program.", description.VertexAttributeNames[i]));
            }

            for (uint i = 0; i < description.UniformBufferNames.Length; i++)
            {
                if (description.UniformBufferNames[i] == null) continue;

                uint programSpecificIndex = gl.GetUniformBlockIndex(handle, description.UniformBufferNames[i]);
                if (programSpecificIndex == (uint)All.InvalidIndex)
                {
                    program = null;
                    errors = string.Format("Uniform Bufffer '{0}' not found.", description.UniformBufferNames[i]);
                    return false;
                }

                gl.UniformBlockBinding(handle, programSpecificIndex, i);
            }

            context.Bindings.Program.Set(program);

            for (int i = 0; i < description.SamplerNames.Length; i++)
            {
                if (description.SamplerNames[i] == null) continue;

                int samplerUniformLocation = gl.GetUniformLocation(handle, description.SamplerNames[i]);
                gl.Uniform(samplerUniformLocation, i);
            }

            errors = null;
            return true;
        }
    }
}
