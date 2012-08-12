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
using ObjectGL.GL42;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Buffer = ObjectGL.GL42.Buffer;

namespace ObjectGL.Tester
{
    class TriangleScene : Scene
    {
        struct Vertex
        {
            public Vector4 Position;
            public Color4 Color;
        }

        const string VertexShaderText =
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

        const string FragmentShaderText =
@"#version 150

in vec4 v_color;

void main()
{
    gl_FragColor = v_color;   
}
";

        VertexArray vertexArray;
        ShaderProgram program;

        public TriangleScene(Context context, GameWindow gameWindow) : base(context, gameWindow)
        {
        }

        public override void Initialize()
        {
            var vertexBuffer = new Buffer(Context, BufferTarget.ArrayBuffer, 3 * 8 * sizeof(float), BufferUsageHint.StaticDraw, new Data(new[]
            {
                new Vertex { Position = new Vector4(-0.5f, -0.5f, 0f, 1f), Color = Color4.Red},
                new Vertex { Position = new Vector4(0.0f, 0.5f, 0f, 1f), Color = Color4.Green},
                new Vertex { Position = new Vector4(0.5f, -0.5f, 0f, 1f), Color = Color4.Yellow}
            }));

            var indexBuffer = new Buffer(Context, BufferTarget.ElementArrayBuffer, 3 * sizeof(ushort), BufferUsageHint.StaticDraw, new Data(new ushort[] { 0, 1, 2 }));

            vertexArray = new VertexArray(Context);
            vertexArray.SetElementArrayBuffer(Context, indexBuffer);
            vertexArray.SetVertexAttributeF(Context, 0, vertexBuffer, VertexAttributeDimension.Four, VertexAttribPointerType.Float, false, 32, 0);
            vertexArray.SetVertexAttributeF(Context, 1, vertexBuffer, VertexAttributeDimension.Four, VertexAttribPointerType.Float, false, 32, 16);

            string shaderErrors;

            VertexShader vsh;
            FragmentShader fsh;

            if (!VertexShader.TryCompile(VertexShaderText, out vsh, out shaderErrors) ||
                !FragmentShader.TryCompile(FragmentShaderText, out fsh, out shaderErrors) ||
                !ShaderProgram.TryLink(Context, new ShaderProgramDescription
                {
                    VertexShaders = vsh,
                    FragmentShaders = fsh,
                    VertexAttributeNames = new[] { "in_position", "in_color" }
                },  
                out program, out shaderErrors))
                throw new ArgumentException("Program errors:\n\n" + shaderErrors);

            
        }

        public override void OnNewFrame(float totalSeconds, float elapsedSeconds)
        {
            Context.ClearWindowColor(Color4.CornflowerBlue);
            Context.ClearWindowDepthStencil(DepthStencil.Both, 1f, 0);

            Context.Pipeline.Program = program;
            Context.Pipeline.VertexArray = vertexArray;

            Context.DrawElements(BeginMode.Triangles, 3, DrawElementsType.UnsignedShort, 0);
        }
    }
}
