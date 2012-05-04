﻿#region License
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
    class RedundantBool
    {
        readonly Action<bool> action;
        bool currentValue;
        bool invalid;

        public RedundantBool(Action<bool> action)
        {
            if (action == null) throw new ArgumentNullException("action");

            this.action = action;
            invalid = true;
        }

        public bool Get()
        {
            if (invalid) throw new InvalidOperationException("Trying to get a binding's value, while it is yet unknown.");

            return currentValue;
        }

        public bool HasValueSet()
        {
            return !invalid;
        }

        public bool HasValueSet(bool value)
        {
            return !invalid && currentValue == value;
        }

        public void Set(bool value)
        {
            if (!invalid && value == currentValue) return;
            Force(value);
        }

        public void Force(bool value)
        {
            action(value);
            currentValue = value;
            invalid = false;
        }

        public void Invalidate()
        {
            invalid = true;
        }
    }
}
