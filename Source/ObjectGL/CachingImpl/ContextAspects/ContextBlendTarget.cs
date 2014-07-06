#region License
/*
Copyright (c) 2010-2014 ObjectGL Project - Daniil Rodin

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

using ObjectGL.Api.PipelineAspects;

namespace ObjectGL.CachingImpl.ContextAspects
{
    internal class ContextBlendTarget
    {
        private readonly IGL gl;
        private readonly uint index;

        private ContextBlendTargetPart color;
        private ContextBlendTargetPart alpha;

        public ContextBlendTarget(IGL gl, uint index)
        {
            this.gl = gl;
            this.index = index;
            color.Initialize();
            alpha.Initialize();
        }

        public void CopyFrom(ContextBlendTarget other)
        {
            color = other.color;
            alpha = other.alpha;
        }

        /// <param name="pipelineTarget"></param>
        /// <param name="independent">Should only be False when calling first (0) target.</param>
        public void ConsumePipelineTarget(IPipelineBlendTarget pipelineTarget, bool independent = true)
        {
            if (pipelineTarget.SeparateAlphaBlendEnable)
            {
                if (color.EquationMode != pipelineTarget.Color.EquationMode || alpha.EquationMode != pipelineTarget.Alpha.EquationMode)
                {
                    if (independent)
                        gl.BlendEquationSeparate(index, (int)pipelineTarget.Color.EquationMode, (int)pipelineTarget.Alpha.EquationMode);
                    else
                        gl.BlendEquationSeparate((int)pipelineTarget.Color.EquationMode, (int)pipelineTarget.Alpha.EquationMode);
                    color.EquationMode = pipelineTarget.Color.EquationMode;
                    alpha.EquationMode = pipelineTarget.Alpha.EquationMode;
                }

                if (color.SrcFactor != pipelineTarget.Color.SrcFactor || color.DestFactor != pipelineTarget.Color.DestFactor ||
                    alpha.SrcFactor != pipelineTarget.Alpha.SrcFactor || alpha.DestFactor != pipelineTarget.Alpha.DestFactor)
                {
                    if (independent)
                    {
                        gl.BlendFuncSeparate(index,
                                             (int)pipelineTarget.Color.SrcFactor,
                                             (int)pipelineTarget.Color.DestFactor,
                                             (int)pipelineTarget.Alpha.SrcFactor,
                                             (int)pipelineTarget.Alpha.DestFactor);
                    }
                    else
                    {
                        gl.BlendFuncSeparate(
                            (int)pipelineTarget.Color.SrcFactor, (int)pipelineTarget.Color.DestFactor,
                            (int)pipelineTarget.Alpha.SrcFactor, (int)pipelineTarget.Alpha.DestFactor);
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
                        gl.BlendEquation(index, (int)pipelineTarget.Color.EquationMode);
                    else
                        gl.BlendEquation((int)pipelineTarget.Color.EquationMode);
                    color.EquationMode = alpha.EquationMode = pipelineTarget.Color.EquationMode;
                }

                if (color.SrcFactor != alpha.SrcFactor || color.DestFactor != alpha.DestFactor ||
                    color.SrcFactor != pipelineTarget.Color.SrcFactor || color.DestFactor != pipelineTarget.Color.DestFactor)
                {
                    if (independent)
                        gl.BlendFunc(index, (int)pipelineTarget.Color.SrcFactor, (int)pipelineTarget.Color.DestFactor);
                    else
                        gl.BlendFunc((int)pipelineTarget.Color.SrcFactor, (int)pipelineTarget.Color.DestFactor);
                    color.SrcFactor = alpha.SrcFactor = pipelineTarget.Color.SrcFactor;
                    color.DestFactor = alpha.DestFactor = pipelineTarget.Color.DestFactor;
                }
            }
        }
    }
}