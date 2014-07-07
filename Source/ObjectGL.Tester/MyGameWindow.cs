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
using ObjectGL.CachingImpl;
using ObjectGL.GL4;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Context = ObjectGL.CachingImpl.Context;

namespace ObjectGL.Tester
{
    public class MyGameWindow : GameWindow
    {
        private readonly IGL gl;
        private readonly INativeGraphicsContext nativeGraphicsContext;
        private Context context;
        private Scene scene;
        private double totalSeconds;

        public MyGameWindow() 
            : base(256, 256, new GraphicsMode(new ColorFormat(32), 24, 8, 4), "Object.GL Tester", 
            GameWindowFlags.Default, DisplayDevice.Default)
        {
            gl = new GL42();
            nativeGraphicsContext = new NativeGraphicsContextWrapper(Context);
            VSync = VSyncMode.On;
            Context.SwapInterval = 1;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            context = new Context(gl, nativeGraphicsContext);

            //scene = new TriangleScene(context, this);
            //scene = new TexturedQuadScene(context, this);
            //scene = new TexturedCubeScene(context, this);
            scene = new RenderToTextureScene(context, this);
            //scene = new ColorfulSpaceScene(context, this);
            //scene = new PatrticleFountainScene(context, this);
            //scene = new CurveTesselationScene(context, this);
            //scene = new FireworksScene(context, this);
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

            //Title = string.Format("FPS: {0}", RenderFrequency);

            base.OnRenderFrame(e);
        }
    }
}
