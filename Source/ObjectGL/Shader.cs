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
    public abstract class Shader : IDisposable
    {
        readonly protected int handle;
        readonly protected ShaderType type;

        public int Handle { get { return handle; } }

        protected Shader(int handle, ShaderType type)
        {
            this.handle = handle;
            this.type = type;
        }

        public void Dispose()
        {
            GL.DeleteShader(handle);
        }

        protected static unsafe bool TryCompile<T>(string source, ShaderType shaderType, Func<int, T> createShader, out T shader, out string errors)
            where T : Shader
        {
            int handle = GL.CreateShader(shaderType);
            GL.ShaderSource(handle, source);
            GL.CompileShader(handle);

            int compileStatus;
            GL.GetShader(handle, ShaderParameter.CompileStatus, &compileStatus);

            if (compileStatus == 0)
            {
                shader = null;
                errors = GL.GetShaderInfoLog(handle);
                return false;
            }

            shader = createShader(handle);
            errors = null;
            return true;
        }
    }
}
