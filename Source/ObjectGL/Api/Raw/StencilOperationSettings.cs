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

using System;
using ObjectGL.Api.PipelineAspects;

namespace ObjectGL.Api.Raw
{
    public struct StencilOperationSettings : IEquatable<StencilOperationSettings>
    {
        public StencilOp StencilFail;
        public StencilOp DepthFail;
        public StencilOp DepthPass;

        public bool Equals(StencilOperationSettings other)
        {
            return StencilFail == other.StencilFail && DepthFail == other.DepthFail && DepthPass == other.DepthPass;
        }

        public override bool Equals(object obj)
        {
            return obj is StencilOperationSettings && Equals((StencilOperationSettings)obj);
        }

        public override int GetHashCode()
        {
            return (int)StencilFail ^ ((int)DepthFail << 8) ^ ((int)DepthPass << 16);
        }

        public override string ToString()
        {
            return string.Format("[SFail: {0}; DFail: {1}; DPass: {2}]", StencilFail, DepthFail, DepthPass);
        }
    }
}