#region License
/*
Copyright (c) 2012-2016 ObjectGL Project - Daniil Rodin

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

using System.Collections.Generic;
using System.Linq;
using ObjectGL.Api;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Subsets;

namespace ObjectGL.CachingImpl.ContextImpl.Subsets
{
    public class ContextBlendBindings : IContextBlendBindings
    {
        public IBinding<bool> BlendEnable { get; set; }
        public IBinding<Color4> BlendColor { get; set; }
        public IBinding<bool> SampleMaskEnable { get; set; }
        public IReadOnlyList<IBinding<uint>> SampleMasks { get; set; }
        public IBinding<bool> AlphaToCoverageEnable { get; set; }

        public IContextBlendTargetBindings United { get; private set; }
        public IReadOnlyList<IContextBlendTargetBindings> Separate { get; private set; }
        private SeparationMode separationModeCache;

        public ContextBlendBindings(IContext context, IImplementation implementation)
        {
            BlendEnable = new EnableCapBinding(context, EnableCap.Blend);
            BlendColor = new Binding<Color4>(context, (c, x) => c.GL.BlendColor(x.Red, x.Green, x.Blue, x.Alpha));
            SampleMaskEnable = new EnableCapBinding(context, EnableCap.SampleMask);
            SampleMasks = Enumerable.Range(0, implementation.MaxSampleMaskWords)
                .Select(i => new Binding<uint>(context, (c, x) => c.GL.SampleMask((uint)i, x)))
                .ToArray();
            AlphaToCoverageEnable = new EnableCapBinding(context, EnableCap.SampleAlphaToCoverage);

            United = new ContextBlendTargetBinding(context, null);
            Separate = Enumerable.Range(0, implementation.MaxDrawBuffers)
                .Select(i => new ContextBlendTargetBinding(context, i))
                .ToArray();
        }

        public SeparationMode SeparationModeCache
        {
            get { return separationModeCache; }
            set
            {
                if (separationModeCache == value)
                    return;
                separationModeCache = value;
                if (separationModeCache == SeparationMode.United)
                    foreach (var targetBindings in Separate)
                        targetBindings.OnExternalChange();
                else
                    United.OnExternalChange();
            }
        }
    }
}