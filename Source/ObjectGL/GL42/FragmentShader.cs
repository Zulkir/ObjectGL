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

using System.Collections;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace ObjectGL.GL42
{
    public class FragmentShader : Shader, IEnumerable<FragmentShader>
    {
        private FragmentShader(int handle)
            : base(handle, ShaderType.FragmentShader)
        {
            
        }

        public IEnumerator<FragmentShader> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this;
        }

        public static bool TryCompile(string source, out FragmentShader shader, out string errors)
        {
            return TryCompile(source, ShaderType.FragmentShader, h => new FragmentShader(h), out shader, out errors);
        }
    }
}
