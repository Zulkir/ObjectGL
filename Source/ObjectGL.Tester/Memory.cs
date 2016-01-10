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
using System.Reflection;
using System.Reflection.Emit;

namespace ObjectGL.Tester
{
    public unsafe static class Memory
    {
        private delegate void CopyBulkDelegate(IntPtr destination, IntPtr source, int length);

        private static readonly CopyBulkDelegate CopyBulkMethod;

        static Memory()
        {
            var dynamicMethod = new DynamicMethod("CopyBulk", typeof(void), new[] { typeof(IntPtr), typeof(IntPtr), typeof(int) }, Assembly.GetExecutingAssembly().ManifestModule, true);
            var il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Cpblk);
            il.Emit(OpCodes.Ret);
            CopyBulkMethod = (CopyBulkDelegate)dynamicMethod.CreateDelegate(typeof(CopyBulkDelegate));
        }

        public static void CopyBulk(IntPtr destination, IntPtr source, int length)
        {
            CopyBulkMethod(destination, source, length);
        }

        public static void CopyBulk(byte* destination, byte* source, int length)
        {
            CopyBulkMethod((IntPtr)destination, (IntPtr)source, length);
        }
    }
}