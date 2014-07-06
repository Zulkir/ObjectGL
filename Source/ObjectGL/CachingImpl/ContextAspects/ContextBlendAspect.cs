#region License
/*
Copyright (c) 2012-2014 ObjectGL Project - Daniil Rodin

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

using ObjectGL.CachingImpl.PipelineAspects;
using ObjectGL.CachingImpl.Utilitties;

namespace ObjectGL.CachingImpl.ContextAspects
{
    internal class ContextBlendAspect
    {
        private readonly IGL gl;
        private readonly ContextBlendTarget[] targets;
        private readonly RedundantEnable alphaToCoverageEnable;
        private readonly RedundantEnable blendEnable;
        private readonly RedundantEnable sampleMaskEnable;
        private readonly RedundantUInt sampleMask;
        private Color4 blendColor;
        private bool independentBlendEnable;

        public ContextBlendAspect(IGL gl, Implementation implementation)
        {
            this.gl = gl;
            alphaToCoverageEnable = new RedundantEnable(gl, (EnableCap)All.SampleAlphaToCoverage);
            blendEnable = new RedundantEnable(gl, EnableCap.Blend);
            sampleMaskEnable = new RedundantEnable(gl, EnableCap.SampleMask);
            sampleMask = new RedundantUInt(gl, (g, x) => g.SampleMask(0, x));
            targets = new ContextBlendTarget[implementation.MaxDrawBuffers];
            for (uint i = 0; i < targets.Length; i++)
                targets[i] = new ContextBlendTarget(gl, i);
        }

        public void ConsumePipelineBlend(PipelineBlendAspect pipelineBlend)
        {
            alphaToCoverageEnable.Set(pipelineBlend.AlphaToCoverageEnable);
            sampleMaskEnable.Set(pipelineBlend.SampleMask != uint.MaxValue);
            sampleMask.Set(pipelineBlend.SampleMask);

            blendEnable.Set(pipelineBlend.BlendEnable);
            if (!pipelineBlend.BlendEnable) return;

            var pipelineBlendColor = pipelineBlend.BlendColor;
            if (!Color4.AreEqual(ref blendColor, ref pipelineBlendColor))
            {
                gl.BlendColor(pipelineBlendColor.Red, pipelineBlendColor.Green, pipelineBlendColor.Blue, pipelineBlendColor.Alpha);
                blendColor = pipelineBlend.BlendColor;
            }

            if (pipelineBlend.IndependentBlendEnable)
            {
                if (!independentBlendEnable)
                    for (int i = 1; i < targets.Length; i++)
                        targets[i].CopyFrom(targets[0]);
                for (int i = 0; i < targets.Length; i++)
                    targets[i].ConsumePipelineTarget(pipelineBlend.Targets[i]);
            }
            else
            {
                targets[0].ConsumePipelineTarget(pipelineBlend.Targets[0], false);
            }

            independentBlendEnable = pipelineBlend.IndependentBlendEnable;
        }
    }
}
