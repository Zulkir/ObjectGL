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
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ObjectGL.Tester
{
    class MyGameWindow : GameWindow
    {
        private Context context;
        private Scene scene;
        private double totalSeconds;

        public MyGameWindow() 
            : base(600, 400, new GraphicsMode(new ColorFormat(32), 24, 8, 4), "Object.GL Tester", 
            GameWindowFlags.Default, DisplayDevice.Default, 3, 3, GraphicsContextFlags.ForwardCompatible)
        {
            VSync = VSyncMode.On;
            Context.SwapInterval = 1;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            context = new Context(Context);

            //scene = new TriangleScene(context, this);
            //scene = new TexturedCubeScene(context, this);
            scene = new RenderToTextureScene(context, this);
            scene.Initialize();

            unsafe
            {
                int major, minor;
                GL.GetInteger(GetPName.MajorVersion, &major);
                GL.GetInteger(GetPName.MinorVersion, &minor);
                //MessageBox.Show(string.Format("OpenGL version {0}.{1}", major, minor));
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientSize);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            totalSeconds += e.Time;

            scene.OnNewFrame((float)totalSeconds, (float)e.Time);
            SwapBuffers();

            Title = string.Format("FPS: {0}", RenderFrequency);

            base.OnRenderFrame(e);
        }
    }
}
