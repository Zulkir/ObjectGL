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

using System.Runtime.InteropServices;
using ObjectGL.Api;
using ObjectGL.Api.Context;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;
using OpenTK;

namespace ObjectGL.Tester
{
    public class TriangleScene : Scene
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Vertex
        {
            public Vector4 Position;
            public Color4 Color;
        }

        private const string VertexShaderText =
@"#version 150

in vec4 in_position;
in vec4 in_color;

out vec4 v_color;

void main()
{
    gl_Position = in_position;
    v_color = in_color;
}
";

        private const string FragmentShaderText =
@"#version 150

in vec4 v_color;

void main()
{
    gl_FragColor = v_color;   
}
";

        private IVertexArray vertexArray;
        private IShaderProgram program;

        public TriangleScene(IContext context, GameWindow gameWindow) : base(context, gameWindow)
        {
        }

        public override void Initialize()
        {
            var vertexBuffer = Context.Create.Buffer(BufferTarget.Array, 3 * 8 * sizeof(float), BufferUsageHint.StaticDraw, new[]
            {
                new Vertex { Position = new Vector4(-0.5f, -0.5f, 0f, 1f), Color = new Color4(1, 0, 0, 1)},
                new Vertex { Position = new Vector4(0.0f, 0.5f, 0f, 1f), Color = new Color4(0, 1, 0, 1)},
                new Vertex { Position = new Vector4(0.5f, -0.5f, 0f, 1f), Color = new Color4(1, 1, 0, 1)}
            });

            var indexBuffer = Context.Create.Buffer(BufferTarget.ElementArray, 3 * sizeof(ushort), BufferUsageHint.StaticDraw, new ushort[] { 0, 1, 2 });

            vertexArray = Context.Create.VertexArray();
            vertexArray.SetElementArrayBuffer(indexBuffer);
            vertexArray.SetVertexAttributeF(0, vertexBuffer, VertexAttributeDimension.Four, VertexAttribPointerType.Float, false, 32, 0);
            vertexArray.SetVertexAttributeF(1, vertexBuffer, VertexAttributeDimension.Four, VertexAttribPointerType.Float, false, 32, 16);

            IVertexShader vsh = Context.Create.VertexShader(VertexShaderText);
            IFragmentShader fsh = Context.Create.FragmentShader(FragmentShaderText);
            program = Context.Create.Program(new ShaderProgramDescription
            {
                VertexShaders = new[] {vsh},
                FragmentShaders = new[] {fsh},
                VertexAttributeNames = new[] {"in_position", "in_color"}
            });
        }

        public override void OnNewFrame(float totalSeconds, float elapsedSeconds)
        {
            Context.Actions.ClearWindowColor(new Color4(0.4f, 0.6f, 0.9f, 1.0f));
            Context.Actions.ClearWindowDepthStencil(DepthStencil.Both, 1f, 0);

            Context.States.ScreenClipping.United.Viewport.Set(GameWindow.ClientSize.Width, GameWindow.ClientSize.Height);

            Context.Bindings.Program.Set(program);
            Context.Bindings.VertexArray.Set(vertexArray);

            Context.Actions.Draw.Elements(BeginMode.Triangles, 3, DrawElementsType.UnsignedShort, 0);
        }
    }
}
