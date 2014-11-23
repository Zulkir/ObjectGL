﻿#region License
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

namespace ObjectGL.Api.Objects.Resources
{
    public struct BufferRange : IEquatable<BufferRange>
    {
        public IBuffer Buffer;
        public int Offset;
        public int Size;

        public BufferRange(IBuffer buffer, int offset, int size) : this()
        {
            Buffer = buffer;
            Offset = offset;
            Size = size;
        }

        public bool Equals(BufferRange other)
        {
            return ReferenceEquals(Buffer, other.Buffer) && Offset == other.Offset && Size == other.Offset;
        }

        public override bool Equals(object obj)
        {
            return obj is BufferRange && Equals((BufferRange)obj);
        }

        public override int GetHashCode()
        {
            return (int)Buffer.SafeGetHandle() ^ (Offset << 8) ^ (Size << 16);
        }

        public override string ToString()
        {
            return string.Format("[Buffer: {0}; Offset: {1}; Size: {2}]", Buffer.SafeGetHandle(), Offset, Size);
        }
    }
}