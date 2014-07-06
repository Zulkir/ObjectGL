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

using System;

namespace ObjectGL.CachingImpl.Utilitties
{
    internal abstract class RedundantBase<T>
    {
        private readonly IGL gl;
        private readonly Action<IGL, T> action;
        private T currentValue;
        private bool invalid;

        protected RedundantBase(IGL gl, Action<IGL, T> action)
        {
            this.gl = gl;
            if (action == null) 
                throw new ArgumentNullException("action");
            this.action = action;
            invalid = true;
        }

        protected abstract bool ValuesEqual(T v1, T v2);

        public T Get()
        {
            if (invalid) 
                throw new InvalidOperationException("Trying to get a binding's value, while it is yet unknown.");
            return currentValue;
        }

        public bool HasValueSet()
        {
            return !invalid;
        }

        public bool HasValueSet(T value)
        {
            return !invalid && ValuesEqual(value, currentValue);
        }

        public void Set(T value)
        {
            if (!invalid && ValuesEqual(value, currentValue)) return;
            Force(value);
        }

        public void Force(T value)
        {
            action(gl, value);
            currentValue = value;
            invalid = false;
        }

        public void OnOutsideChange(T value)
        {
            currentValue = value;
            invalid = false;
        }

        public void Invalidate()
        {
            invalid = true;
        }
    }
}
