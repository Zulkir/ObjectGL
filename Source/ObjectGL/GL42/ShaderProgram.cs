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
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace ObjectGL.GL42
{
    public class ShaderProgram : IContextObject
    {
        readonly int handle;

        public int Handle { get { return handle; } }
        public ContextObjectType ContextObjectType { get { return ContextObjectType.ShaderProgram; } }

        private ShaderProgram(int handle)
        {
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
            Context currentContext,
            ShaderProgramDescription description,
            out ShaderProgram program,
            out string errors)
        {
            if (description.VertexShaders == null) throw new ArgumentNullException("description.VertexShaders");
            if (description.FragmentShaders == null) throw new ArgumentNullException("description.FragmentShaders");

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

            int handle = GL.CreateProgram();
            program = new ShaderProgram(handle);

            description.TesselationControlShaders = description.TesselationControlShaders ?? EmptyTessControlShaderArray;
            description.TesselationEvaluationShaders = description.TesselationEvaluationShaders ?? EmptyTessEvalShaderArray;
            description.GeometryShaders = description.GeometryShaders ?? EmptyGeometryShaderArray;

            foreach (var shader in description.VertexShaders)
                GL.AttachShader(handle, shader.Handle);
            foreach (var shader in description.TesselationControlShaders)
                GL.AttachShader(handle, shader.Handle);
            foreach (var shader in description.TesselationEvaluationShaders)
                GL.AttachShader(handle, shader.Handle);
            foreach (var shader in description.GeometryShaders)
                GL.AttachShader(handle, shader.Handle);
            foreach (var shader in description.FragmentShaders)
                GL.AttachShader(handle, shader.Handle);
            
            for (int i = 0; i < description.VertexAttributeNames.Length; i++)
                if (description.VertexAttributeNames[i] != null)
                    GL.BindAttribLocation(handle, i, description.VertexAttributeNames[i]);

            if (description.TransformFeedbackAttributeNames.Length > 0)
                GL.TransformFeedbackVaryings(handle, description.TransformFeedbackAttributeNames.Length, 
                    description.TransformFeedbackAttributeNames, description.TransformFeedbackMode);

            GL.LinkProgram(handle);

            int isLinked;
            GL.GetProgram(handle, ProgramParameter.LinkStatus, &isLinked);
            if (isLinked == 0)
            {
                program = null;
                errors = GL.GetProgramInfoLog(handle);
                return false;
            }

            for (int i = 0; i < description.VertexAttributeNames.Length; i++)
            {
                if (description.VertexAttributeNames[i] != null && GL.GetAttribLocation(handle, description.VertexAttributeNames[i]) != i)
                    throw new ArgumentException(string.Format("Vertex attribute '{0}' is not present in the program.", description.VertexAttributeNames[i]));
            }

            for (int i = 0; i < description.UniformBufferNames.Length; i++)
            {
                if (description.UniformBufferNames[i] == null) continue;

                int programSpecificIndex = GL.GetUniformBlockIndex(handle, description.UniformBufferNames[i]);
                if (programSpecificIndex == (int)Version31.InvalidIndex)
                {
                    program = null;
                    errors = string.Format("Uniform Bufffer '{0}' not found.", description.UniformBufferNames[i]);
                    return false;
                }

                GL.UniformBlockBinding(handle, programSpecificIndex, i);
            }

            currentContext.UseProgram(program);

            for (int i = 0; i < description.SamplerNames.Length; i++)
            {
                if (description.SamplerNames[i] == null) continue;

                int samplerUniformLocation = GL.GetUniformLocation(handle, description.SamplerNames[i]);
                GL.Uniform1(samplerUniformLocation, i);
            }

            errors = null;
            return true;
        }
    }
}
