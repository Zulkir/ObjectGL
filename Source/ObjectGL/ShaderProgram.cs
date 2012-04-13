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
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace ObjectGL
{
    public class ShaderProgram
    {
        readonly int handle;
        readonly UniformBufferBinding[] uniformBufferBindings;
        readonly Buffer[] uniformBuffers;

        public int Handle { get { return handle; } }

        internal UniformBufferBinding[] UniformBufferBindings { get { return uniformBufferBindings; } }
        internal Buffer[] UniformBuffers { get { return uniformBuffers; } }

        private ShaderProgram(int handle, UniformBufferBinding[] uniformBufferBindings)
        {
            this.handle = handle;
            this.uniformBufferBindings = uniformBufferBindings;
            uniformBuffers = new Buffer[uniformBufferBindings.Length];
        }

        public int GetUniformBufferIndex(string name)
        {
            return Enumerable.Range(0, uniformBufferBindings.Length).First(i => uniformBufferBindings[i].BufferName == name);
        }

        public void SetUniformBuffer(int index, Buffer buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");

            uniformBuffers[index] = buffer;
        }

        public static unsafe bool TryLink(
            IEnumerable<VertexShader> vertexShaders,
            IEnumerable<FragmentShader> fragmentShaders,
            string[] uniformBufferNames,
            out ShaderProgram program,
            out string errors)
        {
            if (vertexShaders == null) throw new ArgumentNullException("vertexShaders");
            if (fragmentShaders == null) throw new ArgumentNullException("fragmentShaders");

            uniformBufferNames = uniformBufferNames ?? new string[0];
            if (uniformBufferNames.Any(string.IsNullOrEmpty))
                throw new ArgumentException("All uniform buffer names must be non-null and non-empty.");
            if (uniformBufferNames.Any(x => uniformBufferNames.Count(y => y == x) != 1))
                throw new ArgumentException("All uniform buffer names must be unique.");

            int handle = GL.CreateProgram();

            foreach (var shader in vertexShaders)
                GL.AttachShader(handle, shader.Handle);
            foreach (var shader in fragmentShaders)
                GL.AttachShader(handle, shader.Handle);

            GL.LinkProgram(handle);

            int isLinked;
            GL.GetProgram(handle, ProgramParameter.LinkStatus, &isLinked);
            if (isLinked == 0)
            {
                program = null;
                errors = GL.GetProgramInfoLog(handle);
                return false;
            }

            var uniformBufferBindings = new UniformBufferBinding[uniformBufferNames.Length];

            for (int i = 0; i < uniformBufferNames.Length; i++)
            {
                int programSpecificIndex = GL.GetUniformBlockIndex(handle, uniformBufferNames[i]);
                if (programSpecificIndex == (int)Version31.InvalidIndex)
                {
                    program = null;
                    errors = string.Format("Uniform Bufffer '{0}' not found.", uniformBufferNames[i]);
                    return false;
                }

                GL.UniformBlockBinding(handle, programSpecificIndex, i);
                uniformBufferBindings[i] = new UniformBufferBinding
                {
                    BufferName = uniformBufferNames[i],
                    ProgramSpecificIndex = programSpecificIndex
                };
            }


            program = new ShaderProgram(handle, uniformBufferBindings);
            errors = null;
            return true;
        }
    }
}
