using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ObjectGL.Tester
{
    class MyGameWindow : GameWindow
    {
        private Context context;

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

            GL.Viewport(Size);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            context.Clear(Color4.CornflowerBlue, 1f, 0);

            SwapBuffers();

            Title = string.Format("FPS: {0}", RenderFrequency);

            base.OnRenderFrame(e);
        }
    }
}
