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
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Subsets;
using ObjectGL.CachingImpl.ContextImpl;
using ObjectGL.GL4;
using OpenTK;
using OpenTK.Graphics;

namespace ObjectGL.Tester
{
    public class MyGameWindow : GameWindow
    {
        private readonly IGL gl;
        private readonly IContextInfra contextInfra;
        private IContext context;
        private Scene scene;
        private double totalSeconds;

        public MyGameWindow() 
            : base(256, 256, new GraphicsMode(new ColorFormat(32), 24, 8, 4), "Object.GL Tester", 
            GameWindowFlags.Default, DisplayDevice.Default)
        {
            gl = new GL4.GL4();
            contextInfra = new GL4ContextInfra(Context);
            VSync = VSyncMode.On;
            Context.SwapInterval = 1;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            context = new Context(gl, contextInfra);

            //scene = new TriangleScene(context, this);
            //scene = new TexturedQuadScene(context, this);
            //scene = new PixelStreamingScene(context, this);
            //scene = new TexturedCubeScene(context, this);
            //scene = new RenderToTextureScene(context, this);
            scene = new ColorfulSpaceScene(context, this);
            //scene = new PatrticleFountainScene(context, this);
            //scene = new CurveTesselationScene(context, this);
            //scene = new FireworksScene(context, this);
            scene.Initialize();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            totalSeconds += e.Time;

            scene.OnNewFrame((float)totalSeconds, (float)e.Time);
            context.Infra.SwapBuffers();

            base.OnRenderFrame(e);
        }
    }
}
