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

using OpenTK.Graphics;

namespace ObjectGL.v42
{
    static class Helpers
    {
        public static bool ColorsEqual(ref Color4 c1, ref Color4 c2)
        {
            return
                c1.R == c2.R &&
                c1.G == c2.G &&
                c1.B == c2.B &&
                c1.A == c2.A;
        }

        public static int ObjectHandle(IContextObject contextObject)
        {
            return contextObject != null ? contextObject.Handle : 0;
        }
    }
}
