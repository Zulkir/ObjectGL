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

namespace ObjectGL.GL42
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
                    public BlendFunc EquationMode { get; set; }
                    public BlendFactor SrcFactor { get; set; }
                    public BlendFactor DestFactor { get; set; }

                    internal Part()
                    {
                        SetDefault();
                    }

                    public void SetDefault()
                    {
                        EquationMode = BlendFunc.Add;
                        SrcFactor = BlendFactor.One;
                        DestFactor = BlendFactor.Zero;
                    }
                }
                #endregion

                public Part Color { get; private set; }
                public Part Alpha { get; private set; }
                public bool SeparateAlphaBlendEnable { get; set; }

                internal Target()
                {
                    Color = new Part();
                    Alpha = new Part();
                    SeparateAlphaBlendEnable = false;
                }

                public void SetDefault()
                {
                    Color.SetDefault();
                    Alpha.SetDefault();
                    SeparateAlphaBlendEnable = false;
                }
            }

            public class TargetCollection
            {
                readonly Target[] targets;

                public int Count { get { return targets.Length; } }

                public Target this[int targetIndex]
                {
                    get { return targets[targetIndex]; }
                }

                internal TargetCollection(int maxDrawBuffers)
                {
                    targets = new Target[maxDrawBuffers];
                    for (int i = 0; i < targets.Length; i++)
                        targets[i] = new Target();
                }
            }
            #endregion

            public TargetCollection Targets { get; private set; }
            public Color4 BlendColor { get; set; }
            public uint SampleMask { get; set; }
            public bool BlendEnable { get; set; }
            public bool AlphaToCoverageEnable { get; set; }
            public bool IndependentBlendEnable { get; set; }

            internal BlendAspect(Context context)
            {
                Targets = new TargetCollection(context.Implementation.MaxDrawBuffers);
                BlendColor = new Color4(0f, 0f, 0f, 0f);
                SampleMask = uint.MaxValue;
                BlendEnable = false;
                AlphaToCoverageEnable = false;
                IndependentBlendEnable = false;
            }

            public void SetDefault(bool forceDefaultOnEachTarget)
            {
                BlendColor = new Color4(0f, 0f, 0f, 0f);
                SampleMask = uint.MaxValue;
                BlendEnable = false;
                AlphaToCoverageEnable = false;
                IndependentBlendEnable = false;

                if (forceDefaultOnEachTarget)
                    for (int i = 0; i < Targets.Count; i++)
                        Targets[i].SetDefault();
                else
                    Targets[0].SetDefault();
            }
        }
    }
}
