#region License
/*
Copyright (c) 2012-2014 ObjectGL Project - Daniil Rodin

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
using ObjectGL.Api;
using ObjectGL.Api.Objects;

namespace ObjectGL.CachingImpl.Objects
{
    internal abstract class Shader : IShader
    {
        readonly IGL gl;
        readonly uint handle;
        readonly ShaderType type;

        public uint Handle { get { return handle; } }
        public GLObjectType GLObjectType { get { return GLObjectType.Shader; } }
        public ShaderType Type { get { return type; } }

        protected Shader(IGL gl, uint handle, ShaderType type)
        {
            this.gl = gl;
            this.handle = handle;
            this.type = type;
        }

        public void Dispose()
        {
            gl.DeleteShader(handle);
        }

        protected static unsafe bool TryCompile<T>(IGL gl, string source, ShaderType shaderType, Func<IGL, uint, T> createShader, out T shader, out string errors)
            where T : Shader
        {
            uint handle = gl.CreateShader((int)shaderType);
            gl.ShaderSource(handle, source);
            gl.CompileShader(handle);

            int compileStatus;
            gl.GetShader(handle, (int)All.CompileStatus, &compileStatus);

            if (compileStatus == 0)
            {
                shader = null;
                errors = gl.GetShaderInfoLog(handle);
                return false;
            }

            shader = createShader(gl, handle);
            errors = null;
            return true;
        }
    }
}
