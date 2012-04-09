using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace ObjectGL
{
    public partial class Context
    {
        private class ProgramAspect
        {
            readonly RedundantInt programBinding = new RedundantInt(GL.UseProgram);

            internal void BindProgramForDrawing(int programHandle)
            {
                programBinding.Set(programHandle);
            }
        }
    }
    
}
