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

        public int Handle { get { return handle; } }

        private ShaderProgram(int handle)
        {
            this.handle = handle;
        }

        public static unsafe bool TryLink(
            IEnumerable<VertexShader> vertexShaders,
            IEnumerable<FragmentShader> fragmentShaders,
            string[] attributeNames,
            string[] uniformBufferNames,
            out ShaderProgram program,
            out string errors)
        {
            if (vertexShaders == null) throw new ArgumentNullException("vertexShaders");
            if (fragmentShaders == null) throw new ArgumentNullException("fragmentShaders");

            attributeNames = attributeNames ?? new string[0];
            if (attributeNames.Any(x => attributeNames.Count(y => y == x) != 1))
                throw new ArgumentException("All attribute names must be unique.");

            uniformBufferNames = uniformBufferNames ?? new string[0];
            if (uniformBufferNames.Any(x => uniformBufferNames.Count(y => y == x) != 1))
                throw new ArgumentException("All uniform buffer names must be unique.");

            int handle = GL.CreateProgram();

            foreach (var shader in vertexShaders)
                GL.AttachShader(handle, shader.Handle);
            foreach (var shader in fragmentShaders)
                GL.AttachShader(handle, shader.Handle);

            for (int i = 0; i < attributeNames.Length; i++)
            {
                if (attributeNames[i] != null)
                    GL.BindAttribLocation(handle, i, attributeNames[i]);
            }

            GL.LinkProgram(handle);

            int isLinked;
            GL.GetProgram(handle, ProgramParameter.LinkStatus, &isLinked);
            if (isLinked == 0)
            {
                program = null;
                errors = GL.GetProgramInfoLog(handle);
                return false;
            }

            for (int i = 0; i < attributeNames.Length; i++)
            {
                if (attributeNames[i] != null && GL.GetAttribLocation(handle, attributeNames[i]) != i)
                    throw new ArgumentException(string.Format("Vertex attribute '{0}' is not present in the program.", attributeNames[i]));
            }

            for (int i = 0; i < uniformBufferNames.Length; i++)
            {
                if (uniformBufferNames[i] == null) continue;

                int programSpecificIndex = GL.GetUniformBlockIndex(handle, uniformBufferNames[i]);
                if (programSpecificIndex == (int)Version31.InvalidIndex)
                {
                    program = null;
                    errors = string.Format("Uniform Bufffer '{0}' not found.", uniformBufferNames[i]);
                    return false;
                }

                GL.UniformBlockBinding(handle, programSpecificIndex, i);
            }

            program = new ShaderProgram(handle);
            errors = null;
            return true;
        }
    }
}
