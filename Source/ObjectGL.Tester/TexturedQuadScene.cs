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

using System.Runtime.InteropServices;
using ObjectGL.Api;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;
using OpenTK;

namespace ObjectGL.Tester
{
    public class TexturedQuadScene : Scene
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
    out_color = pow(texture(DiffuseMap, v_tex_coord), vec4(1/2.2));
}
";

        private IShaderProgram program;
        private IVertexArray vertexArray;
        private ITexture2D diffuseMap;
        private ISampler sampler;

        public TexturedQuadScene(IContext context, GameWindow gameWindow) : base(context, gameWindow)
        {
        }

        public override void Initialize()
        {
            var vertexBuffer = Context.Create.Buffer(BufferTarget.ArrayBuffer, 4 * 8 * sizeof(float), BufferUsageHint.StaticDraw, new[]
            {
                new Vertex(-1f, -1f, 0f, 1f),
                new Vertex(-1f, 1f, 0f, 0f),
                new Vertex(1f, 1f, 1f, 0f),
                new Vertex(1f, -1f, 1f, 1f),
            });

            var indexBuffer = Context.Create.Buffer(BufferTarget.ElementArrayBuffer, 6 * sizeof(ushort), BufferUsageHint.StaticDraw, new ushort[]
            { 
                0, 1, 2, 0, 2, 3
            });

            vertexArray = Context.Create.VertexArray();
            vertexArray.SetElementArrayBuffer(indexBuffer);
            vertexArray.SetVertexAttributeF(0, vertexBuffer, VertexAttributeDimension.Four, VertexAttribPointerType.Float, false, 32, 0);
            vertexArray.SetVertexAttributeF(1, vertexBuffer, VertexAttributeDimension.Four, VertexAttribPointerType.Float, false, 32, 16);

            using (var textureLoader = new TextureLoader("../Textures/Chess256.png"))
            {
                diffuseMap = Context.Create.Texture2D(textureLoader.Width, textureLoader.Height, TextureHelper.CalculateMipCount(textureLoader.Width, textureLoader.Height, 1), Format.Srgb8Alpha8);
                for (int i = 0; i < diffuseMap.MipCount; i++)
                    diffuseMap.SetData(i, textureLoader.GetMipData(i), FormatColor.Rgba, FormatType.UnsignedByte);
            }

            sampler = Context.Create.Sampler();
            sampler.SetMagFilter(TextureMagFilter.Linear);
            sampler.SetMinFilter(TextureMinFilter.LinearMipmapLinear);
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

        public override void OnNewFrame(float totalSeconds, float elapsedSeconds)
        {
            Context.ClearWindowColor(new Color4(0, 0, 0, 0));
            Context.ClearWindowDepthStencil(DepthStencil.Both, 1f, 0);

            Context.Pipeline.Program = program;
            Context.Pipeline.VertexArray = vertexArray;
            Context.Pipeline.Textures[0] = diffuseMap;
            Context.Pipeline.Samplers[0] = sampler;

            Context.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedShort, 0);
        }
    }
}
