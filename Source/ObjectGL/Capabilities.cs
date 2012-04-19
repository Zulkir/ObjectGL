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

using OpenTK.Graphics.OpenGL;

namespace ObjectGL
{
    public class Capabilities
    {
        public int MaxVertexAttributes { get; private set; }
        public int MaxCombinedTextureImageUnits { get; private set; }
        public int MaxUniformBufferBindings { get; private set; }
        public int MaxTransformFeedbackBuffers { get; private set; }
        public int MaxDrawBuffers { get; private set; }
        public int MaxColorAttachments { get; private set; }

        internal unsafe Capabilities()
        {
            int localInt;

            GL.GetInteger(GetPName.MaxVertexAttribs, &localInt);
            MaxVertexAttributes = localInt;

            GL.GetInteger(GetPName.MaxCombinedTextureImageUnits, &localInt);
            MaxCombinedTextureImageUnits = localInt;

            GL.GetInteger(GetPName.MaxUniformBufferBindings, &localInt);
            MaxUniformBufferBindings = localInt;

            GL.GetInteger(GetPName.MaxTransformFeedbackBuffers, &localInt);
            MaxTransformFeedbackBuffers = localInt;

            GL.GetInteger(GetPName.MaxDrawBuffers, &localInt);
            MaxDrawBuffers = localInt;

            GL.GetInteger(GetPName.MaxColorAttachments, &localInt);
            MaxColorAttachments = localInt;
        }
    }
}
