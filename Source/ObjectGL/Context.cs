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
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ObjectGL
{
    public partial class Context
    {
        readonly GraphicsContext nativeContext;

        public Capabilities Capabilities { get; private set; }
        public Pipeline Pipeline { get; private set; }

        public Context(GraphicsContext nativeContext)
        {
            this.nativeContext = nativeContext;

            Capabilities = new Capabilities();
            Pipeline = new Pipeline(this);

            

            

            samplerBindings = new RedundantInt[Capabilities.MaxCombinedTextureImageUnits];
            for (int i = 0; i < Capabilities.MaxCombinedTextureImageUnits; i++)
            {
                int iLoc = i;
                samplerBindings[i] = new RedundantInt(h => GL.BindSampler(iLoc, h));
            }
        }

        #region Buffers
        
        #endregion

        #region Textures
        
        #endregion

        #region Samplers
        readonly RedundantInt[] samplerBindings;

        internal void BindSamplerForDrawing(int unit, int samplerHandle)
        {
            samplerBindings[unit].Set(samplerHandle);
        }
        #endregion

        #region Programs
        readonly RedundantInt programBinding = new RedundantInt(GL.UseProgram);

        internal void BindProgramForDrawing(int programHandle)
        {
            programBinding.Set(programHandle);
        }
        #endregion

        #region Render States
        
        #endregion
    }
}
