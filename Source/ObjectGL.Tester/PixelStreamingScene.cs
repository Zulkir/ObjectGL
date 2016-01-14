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
using System.Linq;
using System.Runtime.InteropServices;
using ObjectGL.Api;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Actions;
using ObjectGL.Api.Objects.Framebuffers;
using ObjectGL.Api.Objects.Resources.Buffers;
using ObjectGL.Api.Objects.Resources.Images;
using ObjectGL.Api.Objects.Samplers;
using ObjectGL.Api.Objects.Shaders;
using ObjectGL.Api.Objects.VertexArrays;
using OpenTK;

namespace ObjectGL.Tester
{
    public class PixelStreamingScene : Scene
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Vertex
        {
            public Vector4 Position;
            public Vector4 TexCoord;

            public Vertex(float px, float py, float tx, float ty)
            {
                Position.X = px;
                Position.Y = py;
                Position.Z = 0f;
                Position.W = 0f;

                TexCoord.X = tx;
                TexCoord.Y = ty;
                TexCoord.Z = 0f;
                TexCoord.W = 0f;
            }
        }

        private const string VertexShaderText =
@"#version 150

in vec4 in_position;
in vec4 in_tex_coord;

out vec2 v_tex_coord;

void main()
{
    gl_Position = vec4(in_position.x, in_position.y, 0.0f, 1.0f);
    v_tex_coord = in_tex_coord.xy;
}
";

        private const string FragmentShaderText =
@"#version 150

uniform sampler2D DiffuseMap;

in vec2 v_tex_coord;

out vec4 out_color;

void main()
{
    out_color = texture(DiffuseMap, v_tex_coord);
}
";

        private IShaderProgram program;
        private IVertexArray vertexArray;
        private IBuffer pixelUnpackBuffer;
        private ITexture2D diffuseMap;
        private ISampler sampler;

        private byte[] data;
        private int offset;

        public PixelStreamingScene(IContext context, GameWindow gameWindow) : base(context, gameWindow)
        {
        }

        public override void Initialize()
        {
            var vertexBuffer = Context.Create.Buffer(BufferTarget.Array, 4 * 8 * sizeof(float), BufferUsageHint.StaticDraw, new[]
            {
                new Vertex(-1f, -1f, 0f, 1f),
                new Vertex(-1f, 1f, 0f, 0f),
                new Vertex(1f, 1f, 1f, 0f),
                new Vertex(1f, -1f, 1f, 1f),
            });

            var indexBuffer = Context.Create.Buffer(BufferTarget.ElementArray, 6 * sizeof(ushort), BufferUsageHint.StaticDraw, new ushort[]
            { 
                0, 1, 2, 0, 2, 3
            });

            vertexArray = Context.Create.VertexArray();
            vertexArray.SetElementArrayBuffer(indexBuffer);
            vertexArray.SetVertexAttributeF(0, vertexBuffer, VertexAttributeDimension.Four, VertexAttribPointerType.Float, false, 32, 0);
            vertexArray.SetVertexAttributeF(1, vertexBuffer, VertexAttributeDimension.Four, VertexAttribPointerType.Float, false, 32, 16);

            pixelUnpackBuffer = Context.Create.Buffer(BufferTarget.PixelUnpack, 1024 * 1024 * 4, BufferUsageHint.StreamDraw);

            data = Enumerable.Range(0, 1024 * 1024 * 4 * 2).Select(x => (byte)(128.0 + 128.0 * Math.Sin((2.0 / 3.0) * Math.PI * (x % 4) + (double)x / 4 / 11111))).ToArray();

            diffuseMap = Context.Create.Texture2D(640, 360, 1, Format.Rgba8);

            sampler = Context.Create.Sampler();
            sampler.SetMagFilter(TextureMagFilter.Nearest);
            sampler.SetMinFilter(TextureMinFilter.Nearest);
            sampler.SetMaxAnisotropy(16f);

            var vsh = Context.Create.VertexShader(VertexShaderText);
            var fsh = Context.Create.FragmentShader(FragmentShaderText);
            program = Context.Create.Program(new ShaderProgramDescription
            {
                VertexShaders = new[] {vsh},
                FragmentShaders = new[] {fsh},
                VertexAttributeNames = new[] {"in_position", "in_tex_coord"},
                SamplerNames = new[] {"DiffuseMap"}
            });
        }

        public unsafe override void OnNewFrame(float totalSeconds, float elapsedSeconds)
        {
            fixed (byte* pData = data)
                pixelUnpackBuffer.SetDataByMapping((IntPtr)pData + offset);
            offset = (offset + 1024) % (data.Length / 2);
            diffuseMap.SetData(0, IntPtr.Zero, FormatColor.Rgba, FormatType.UnsignedByte, pixelUnpackBuffer);

            Context.Actions.ClearWindowColor(new Color4(0, 0, 0, 0));
            Context.Actions.ClearWindowDepthStencil(DepthStencil.Both, 1f, 0);

            Context.States.ScreenClipping.United.Viewport.Set(GameWindow.Width, GameWindow.Height);
            
            Context.Bindings.Program.Set(program);
            Context.Bindings.VertexArray.Set(vertexArray);

            Context.Bindings.Textures.Units[0].Set(diffuseMap);
            Context.Bindings.Samplers[0].Set(sampler);

            Context.Actions.Draw.Elements(BeginMode.Triangles, 6, DrawElementsType.UnsignedShort, 0);
        }
    }
}
