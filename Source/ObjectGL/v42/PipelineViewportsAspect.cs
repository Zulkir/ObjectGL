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

namespace ObjectGL.v42
{
    public partial class Pipeline
    {
        public class ViewportsAspect
        {
            #region Nested class Viewport
            public class Viewport
            {
                public float X { get; set; }
                public float Y { get; set; }
                public float Width { get; set; }
                public float Height { get; set; }
                public float Near { get; set; }
                public float Far { get; set; }

                internal Viewport(float initialWidth, float initialHeight)
                {
                    X = 0f;
                    Y = 0f;
                    Width = initialWidth;
                    Height = initialHeight;
                    Near = 0f;
                    Far = 1f;
                }
            }
            #endregion

            readonly Viewport[] viewports;
            public int EnabledViewportCount { get; set; }

            public Viewport this[int viewportIndex]
            {
                get { return viewports[viewportIndex]; }
            }

            internal ViewportsAspect(Context context)
            {
                var viewportData = new int[4];
                GL.GetInteger(GetPName.Viewport, viewportData);

                viewports = new Viewport[context.Implementation.MaxViewports];
                for (int i = 0; i < viewports.Length; i++)
                {
                    viewports[i] = new Viewport(viewportData[2], viewportData[3]);
                }

                EnabledViewportCount = 1;
            }
        }
    }
}
