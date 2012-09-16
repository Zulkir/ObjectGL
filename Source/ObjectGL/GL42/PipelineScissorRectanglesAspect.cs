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

using OpenTK.Graphics.OpenGL;

namespace ObjectGL.GL42
{
    public partial class Pipeline
    {
        public class ScissorBoxesAspect
        {
            #region Nested class ScissorBox
            public class ScissorBox
            {
                public int X { get; set; }
                public int Y { get; set; }
                public int Width { get; set; }
                public int Height { get; set; }

                internal ScissorBox(int initialWidth, int initialHeight)
                {
                    Set(initialWidth, initialHeight);
                }

                public void Set(int width, int height)
                {
                    X = 0;
                    Y = 0;
                    Width = width;
                    Height = height;
                }

                public void Set(int x, int y, int width, int height)
                {
                    X = x;
                    Y = y;
                    Width = width;
                    Height = height;
                }
            }
            #endregion

            readonly ScissorBox[] scissorBoxes;

            public ScissorBox this[int viewportIndex]
            {
                get { return scissorBoxes[viewportIndex]; }
            }

            internal ScissorBoxesAspect(Context context)
            {
                var scissorData = new int[4];
                GL.GetInteger(GetPName.ScissorBox, scissorData);

                scissorBoxes = new ScissorBox[context.Implementation.MaxViewports];
                for (int i = 0; i < scissorBoxes.Length; i++)
                {
                    scissorBoxes[i] = new ScissorBox(scissorData[2], scissorData[3]);
                }
            }
        }
    }
}
