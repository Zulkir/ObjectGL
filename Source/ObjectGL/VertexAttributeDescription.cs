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

namespace ObjectGL
{
    struct VertexAttributeDescription
    {
        public bool IsEnabled;
        public VertexArraySetFunction SetFunction;
        public byte Dimension;
        public bool IsNormalized;
        public int Type;
        public int Stride;
        public int Offset;
        public int Divisor;
        public Buffer Buffer;

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
