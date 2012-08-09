#region License
/*
Copyright (c) 2012 Daniil Rodin

This software is provided 'as-is', without any express or implied
warranty. In no event will the authors be held liable for any damages
arising from the use of this software.

Permission is granted to anyone to use this software for any purpose,
including commercial applications, and to alter it and redistribute it
freely, subject to the following restrictions:

   1. The origin of this software must not be misrepresented; you must not
   claim that you wrote the original software. If you use this software
   in a product, an acknowledgment in the product documentation would be
   appreciated but is not required.

   2. Altered source versions must be plainly marked as such, and must not be
   misrepresented as being the original software.

   3. This notice may not be removed or altered from any source
   distribution.
*/
#endregion

using System;

namespace ObjectGL
{
    abstract class RedundantBase<T>
    {
        readonly Action<T> action;
        T currentValue;
        bool invalid;

        public RedundantBase(Action<T> action)
        {
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
            action(value);
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
