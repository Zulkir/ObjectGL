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

using System;

namespace ObjectGL.Api.Context
{
    public struct BlendEquation : IEquatable<BlendEquation>
    {
        public BlendMode Rgb;
        public BlendMode Alpha;

        public bool Equals(BlendEquation other)
        {
            return Rgb == other.Rgb && Alpha == other.Alpha;
        }

        public override bool Equals(object obj)
        {
            return obj is BlendEquation && Equals((BlendEquation)obj);
        }

        public override int GetHashCode()
        {
            return (int)Rgb ^ (int)Alpha;
        }

        public override string ToString()
        {
            return string.Format("[RGB: {0}; Alpha: {1}]", Rgb, Alpha);
        }
    }
}