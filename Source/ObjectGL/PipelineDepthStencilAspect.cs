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
    public partial class Pipeline
    {
        public class DepthStencilAspect
        {
            #region Nested class Side
            public class Side
            {
                public int StencilWriteMask { get; set; }
                public int StencilFuncMask { get; set; }
                public StencilFunction StencilFunc { get; set; }
                public int StencilRef { get; set; }
                public StencilOp StencilFailOp { get; set; }
                public StencilOp DepthFailOp { get; set; }
                public StencilOp DepthPassOp { get; set; }

                internal Side()
                {
                    StencilWriteMask = 1;
                    StencilFunc = StencilFunction.Always;
                    StencilRef = 0;
                    StencilFuncMask = 1;
                    StencilFailOp = StencilOp.Keep;
                    DepthFailOp = StencilOp.Keep;
                    DepthPassOp = StencilOp.Keep;
                }
            }
            #endregion

            public bool DepthTestEnable { get; set; }
            public bool DepthMask { get; set; }
            public DepthFunction DepthFunc { get; set; }
            public bool StencilTestEnable { get; set; }
            public Side Front { get; private set; }
            public Side Back { get; private set; }

            internal DepthStencilAspect()
            {
                DepthTestEnable = false;
                DepthMask = true;
                DepthFunc = DepthFunction.Less;
                StencilTestEnable = false;
                Front = new Side();
                Back = new Side();
            }
        }
    }
}
