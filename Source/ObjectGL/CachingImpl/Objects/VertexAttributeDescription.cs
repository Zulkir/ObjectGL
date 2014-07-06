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

using System.Runtime.InteropServices;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;

namespace ObjectGL.CachingImpl.Objects
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct VertexAttributeDescription
    {
        public bool IsEnabled;
        public All SetFunction;
        public VertexAttributeDimension Dimension;
        public bool IsNormalized;
        public int Type;
        public int Stride;
        public int Offset;
        public uint Divisor;
        public IBuffer Buffer;

        public static bool Equals(ref VertexAttributeDescription d1, ref VertexAttributeDescription d2)
        {
            if (d1.IsEnabled == false && d2.IsEnabled == false) return true;

            return
                d1.IsEnabled == d2.IsEnabled &&
                d1.SetFunction == d2.SetFunction &&
                d1.Dimension == d2.Dimension &&
                d1.IsNormalized == d2.IsNormalized &&
                d1.Type == d2.Type &&
                d1.Offset == d2.Offset &&
                d1.Stride == d2.Stride &&
                d1.Divisor == d2.Divisor &&
                d1.Buffer == d2.Buffer;
        }
    }
}
