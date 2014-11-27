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

using ObjectGL.Api.PipelineAspects;

namespace ObjectGL.CachingImpl.PipelineAspects
{
    internal class PipelineBlendAspect : IPipelineBlendAspect
    {
        public IPipelineBlendTargetCollection Targets { get; private set; }
        public Color4 BlendColor { get; set; }
        public uint SampleMask { get; set; }
        public bool BlendEnable { get; set; }
        public bool AlphaToCoverageEnable { get; set; }
        public bool IndependentBlendEnable { get; set; }

        internal PipelineBlendAspect(OldContext context)
        {
            Targets = new PipelineBlendTargetCollection(context.Implementation.MaxDrawBuffers);
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
