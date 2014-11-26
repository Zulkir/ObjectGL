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

using System.Collections.Generic;
using System.Linq;
using ObjectGL.Api;
using ObjectGL.Api.Raw;
using IContext = ObjectGL.Api.Raw.IContext;

namespace ObjectGL.CachingImpl.Raw
{
    public class RawContextBlendBindings : IContextBlendBindings
    {
        public IContextBlendTargetBindings United { get; private set; }
        public IReadOnlyList<IContextBlendTargetBindings> Separate { get; private set; }
        private SeparationMode separationModeCache;

        public RawContextBlendBindings(IContext context, IImplementation implementation)
        {
            United = new RawContextBlendTargetBinding(context, null);
            Separate = Enumerable.Range(0, implementation.MaxDrawBuffers)
                .Select(i => new RawContextBlendTargetBinding(context, i))
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