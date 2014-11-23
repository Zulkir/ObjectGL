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
using System.Collections.Generic;
using ObjectGL.Api.Raw;

namespace ObjectGL.CachingImpl.Raw
{
    public class Binding<T> : IBinding<T>
    {
        private readonly IContext context;
        private readonly Action<IContext, T> prepareAndSet;
        private T currentValue;
        private bool currentValueUnkown;

        public IContext Context { get { return context; } }

        public Binding(IContext context, Action<IContext, T> prepareAndSet)
        {
            this.context = context;
            this.prepareAndSet = prepareAndSet;
            currentValueUnkown = true;
        }

        public T Get()
        {
            if (currentValueUnkown)
                throw new NotSupportedException("Trying to get a yet unkown value");
            return currentValue;
        }

        public bool IsSetTo(T value)
        {
            return !currentValueUnkown && EqualityComparer<T>.Default.Equals(value, currentValue);
        }

        public void Set(T value)
        {
            if (!currentValueUnkown && EqualityComparer<T>.Default.Equals(value, currentValue)) 
                return;
            Force(value);
        }

        public void Force(T value)
        {
            prepareAndSet(context, value);
            currentValue = value;
            currentValueUnkown = false;
        }

        public void OnOutsideChange()
        {
            currentValueUnkown = true;
        }

        public void OnOutsideChange(T value)
        {
            currentValue = value;
            currentValueUnkown = false;
        }
    }
}
