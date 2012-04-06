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

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ObjectGL
{
    public partial class Pipeline
    {
        public class BlendAspect
        {
            #region Nested classes
            public class Target
            {
                #region Nested classes
                public class Part
                {
                    public BlendEquationMode EquationMode { get; set; }
                    public BlendingFactorSrc SrcFactor { get; set; }
                    public BlendingFactorDest DestFactor { get; set; }

                    internal Part()
                    {
                        EquationMode = BlendEquationMode.FuncAdd;
                        SrcFactor = BlendingFactorSrc.One;
                        DestFactor = BlendingFactorDest.Zero;
                    }
                }
                #endregion

                public Part Color { get; private set; }
                public Part Alpha { get; private set; }

                internal Target()
                {
                    Color = new Part();
                    Alpha = new Part();
                }
            }

            public class TargetCollection
            {
                readonly Target[] targets;

                public Target this[int targetIndex]
                {
                    get { return targets[targetIndex]; }
                }

                internal TargetCollection(int maxDrawBuffers)
                {
                    targets = new Target[maxDrawBuffers];
                    for (int i = 0; i < targets.Length; i++)
                    {
                        targets[i] = new Target();
                    }
                }
            }
            #endregion

            public bool BlendEnable { get; set; }
            public bool AlphaToCoverageEnable { get; set; }
            public Color4 BlendColor { get; set; }
            public TargetCollection Targets { get; private set; }

            internal BlendAspect(int maxDrawBuffers)
            {
                BlendEnable = false;
                AlphaToCoverageEnable = false;
                BlendColor = new Color4(0f, 0f, 0f, 0f);
                Targets = new TargetCollection(maxDrawBuffers);
            }
        }
    }
}
