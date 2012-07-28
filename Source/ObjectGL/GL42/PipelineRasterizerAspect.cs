﻿#region License
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

namespace ObjectGL.GL42
{
    public partial class Pipeline
    {
        public class RasterizerAspect
        {
            public PolygonMode PolygonModeFront { get; set; }
            public PolygonMode PolygonModeBack { get; set; }
            public CullFaceMode CullFace { get; set; }
            public FrontFaceDirection FrontFace { get; set; }
            public bool CullFaceEnable { get; set; }
            public bool ScissorEnable { get; set; }
            public bool MultisampleEnable { get; set; }
            public bool LineSmoothEnable { get; set; }

            public RasterizerAspect()
            {
                SetDefault();
            }

            public void SetDefault()
            {
                PolygonModeFront = PolygonMode.Fill;
                PolygonModeBack = PolygonMode.Fill;
                CullFace = CullFaceMode.Back;
                FrontFace = FrontFaceDirection.Ccw;
                CullFaceEnable = false;
                ScissorEnable = false;
                MultisampleEnable = true;
                LineSmoothEnable = false;
            }
        }
    }
}