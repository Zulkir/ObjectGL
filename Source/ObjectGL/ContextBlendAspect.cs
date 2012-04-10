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
    public partial class Context
    {
        class BlendAspect
        {
            #region Nested classes
            private struct Part
            {
                public BlendEquationMode EquationMode;
                public BlendingFactorSrc SrcFactor;
                public BlendingFactorDest DestFactor;

                public void Initialize()
                {
                    EquationMode = BlendEquationMode.FuncAdd;
                    SrcFactor = BlendingFactorSrc.One;
                    DestFactor = BlendingFactorDest.Zero;
                }
            }

            private class Target
            {
                readonly int index;
                
                Part color;
                Part alpha;

                public Target(int index)
                {
                    this.index = index;
                    color.Initialize();
                    alpha.Initialize();
                }

                public void CopyFrom(Target other)
                {
                    color = other.color;
                    alpha = other.alpha;
                }


                /// <param name="pipelineTarget"></param>
                /// <param name="independent">Should only be False when calling first (0) target.</param>
                public void ConsumePipelineTarget(Pipeline.BlendAspect.Target pipelineTarget, bool independent = true)
                {
                    if (pipelineTarget.SeparateAlphaBlendEnable)
                    {
                        if (color.EquationMode != pipelineTarget.Color.EquationMode || alpha.EquationMode != pipelineTarget.Alpha.EquationMode)
                        {
                            if (independent)
                                GL.BlendEquationSeparate(index, pipelineTarget.Color.EquationMode, pipelineTarget.Alpha.EquationMode);
                            else
                                GL.BlendEquationSeparate(pipelineTarget.Color.EquationMode, pipelineTarget.Alpha.EquationMode);
                            color.EquationMode = pipelineTarget.Color.EquationMode;
                            alpha.EquationMode = pipelineTarget.Alpha.EquationMode;
                        }

                        if (color.SrcFactor != pipelineTarget.Color.SrcFactor || color.DestFactor != pipelineTarget.Color.DestFactor ||
                            alpha.SrcFactor != pipelineTarget.Alpha.SrcFactor || alpha.DestFactor != pipelineTarget.Alpha.DestFactor)
                        {
                            if (independent)
                            {
                                GL.BlendFuncSeparate(index,
                                    (Version40)pipelineTarget.Color.SrcFactor,
                                    (Version40)pipelineTarget.Color.DestFactor,
                                    (Version40)pipelineTarget.Alpha.SrcFactor,
                                    (Version40)pipelineTarget.Alpha.DestFactor);
                            }
                            else
                            {
                                GL.BlendFuncSeparate(
                                    pipelineTarget.Color.SrcFactor, pipelineTarget.Color.DestFactor,
                                    pipelineTarget.Alpha.SrcFactor, pipelineTarget.Alpha.DestFactor);
                            }
                            color.SrcFactor = pipelineTarget.Color.SrcFactor;
                            color.DestFactor = pipelineTarget.Color.DestFactor;
                            alpha.SrcFactor = pipelineTarget.Alpha.SrcFactor;
                            alpha.DestFactor = pipelineTarget.Alpha.DestFactor;
                        }
                    }
                    else
                    {
                        if (color.EquationMode != alpha.EquationMode || color.EquationMode != pipelineTarget.Color.EquationMode)
                        {
                            if (independent)
                                GL.BlendEquation(index, (Version40)pipelineTarget.Color.EquationMode);
                            else
                                GL.BlendEquation(pipelineTarget.Color.EquationMode);
                            color.EquationMode = alpha.EquationMode = pipelineTarget.Color.EquationMode;
                        }

                        if (color.SrcFactor != alpha.SrcFactor || color.DestFactor != alpha.DestFactor ||
                            color.SrcFactor != pipelineTarget.Color.SrcFactor || color.DestFactor != pipelineTarget.Color.DestFactor)
                        {
                            if (independent)
                                GL.BlendFunc(index, (Version40)pipelineTarget.Color.SrcFactor, (Version40)pipelineTarget.Color.DestFactor);
                            else
                                GL.BlendFunc(pipelineTarget.Color.SrcFactor, pipelineTarget.Color.DestFactor);
                            color.SrcFactor = alpha.SrcFactor = pipelineTarget.Color.SrcFactor;
                            color.DestFactor = alpha.DestFactor = pipelineTarget.Color.DestFactor;
                        }
                    }
                }
            }
            #endregion

            readonly Target[] targets;
            readonly RedundantEnable blendEnable = new RedundantEnable(EnableCap.Blend);
            readonly RedundantEnable alphaToCoverageEnable = new RedundantEnable((EnableCap)All.SampleAlphaToCoverage);
            Color4 blendColor = new Color4();

            bool independentBlendEnable = false;

            public BlendAspect(Capabilities capabilities)
            {
                targets = new Target[capabilities.MaxDrawBuffers];
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i] = new Target(i);
                }
            }

            public void ConsumePipelineBlend(Pipeline.BlendAspect pipelineBlend)
            {
                blendEnable.Set(pipelineBlend.BlendEnable);
                if (!pipelineBlend.BlendEnable) return;

                alphaToCoverageEnable.Set(pipelineBlend.AlphaToCoverageEnable);

                var pipelineBlendColor = pipelineBlend.BlendColor;
                if (!Helpers.ColorsEqual(ref blendColor, ref pipelineBlendColor))
                {
                    GL.BlendColor(pipelineBlendColor);
                    blendColor = pipelineBlend.BlendColor;
                }

                if (pipelineBlend.IndependentBlendEnable)
                {
                    if (!independentBlendEnable)
                    {
                        for (int i = 1; i < targets.Length; i++)
                        {
                            targets[i].CopyFrom(targets[0]);
                        }
                    }

                    for (int i = 0; i < targets.Length; i++)
                    {
                        targets[i].ConsumePipelineTarget(pipelineBlend.Targets[i]);
                    }
                }
                else
                {
                    targets[0].ConsumePipelineTarget(pipelineBlend.Targets[0], false);
                }

                independentBlendEnable = pipelineBlend.IndependentBlendEnable;
            }
        }
    }
}
