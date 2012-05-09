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
using ObjectGL.v42;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Buffer = ObjectGL.v42.Buffer;

namespace ObjectGL.Tester
{
    class TexturedQuadScene : Scene
    {
        struct Vertex
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

        const string VertexShaderText =
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

        const string FragmentShaderText =
@"#version 150

uniform sampler2D DiffuseMap;

in vec2 v_tex_coord;

out vec4 out_color;

void main()
{
    out_color = texture(DiffuseMap, v_tex_coord);
}
";

        Buffer vertices;
        Buffer indices;

        Texture2D diffuseMap;
        Sampler sampler;

        ShaderProgram program;

        VertexArray vertexArray;

        public TexturedQuadScene(Context context, GameWindow gameWindow) : base(context, gameWindow)
        {
        }

        public override void Initialize()
        {
            vertices = new Buffer(Context, BufferTarget.ArrayBuffer, 24 * 8 * sizeof(float), BufferUsageHint.StaticDraw, new Data(new[]
            {
                new Vertex(-1f, -1f, 0f, 1f),
                new Vertex(-1f, 1f, 0f, 0f),
                new Vertex(1f, 1f, 1f, 0f),
                new Vertex(1f, -1f, 1f, 1f),
            }));

            indices = new Buffer(Context, BufferTarget.ElementArrayBuffer, 36 * sizeof(ushort), BufferUsageHint.StaticDraw, new Data(new ushort[] 
            { 
                0, 1, 2, 0, 2, 3
            }));

            using (var textureLoader = new TextureLoader("../Textures/DiffuseTest.bmp"))
            {
                diffuseMap = new Texture2D(Context, textureLoader.Width, textureLoader.Height, 0, Format.Rgba8,
                                           FormatColor.Rgba, FormatType.UnsignedByte, i => textureLoader.GetMipData(i));
            }

            sampler = new Sampler();
            sampler.SetMagFilter(TextureMagFilter.Nearest);
            sampler.SetMinFilter(TextureMinFilter.NearestMipmapLinear);
            sampler.SetMaxAnisotropy(16f);

            string shaderErrors;

            VertexShader vsh;
            FragmentShader fsh;

            if (!VertexShader.TryCompile(VertexShaderText, out vsh, out shaderErrors) ||
                !FragmentShader.TryCompile(FragmentShaderText, out fsh, out shaderErrors) ||
                !ShaderProgram.TryLink(Context, vsh, fsh, null, 
                new[] { "in_position", "in_tex_coord" },
                null,
                null, 
                new[] { "DiffuseMap" },  
                0, out program, out shaderErrors))
                throw new ArgumentException("Program errors:\n\n" + shaderErrors);

            vertexArray = new VertexArray(Context);
            vertexArray.SetElementArrayBuffer(Context, indices);
            vertexArray.SetVertexAttributeF(Context, 0, vertices, 4, VertexAttribPointerType.Float, false, 32, 0);
            vertexArray.SetVertexAttributeF(Context, 1, vertices, 4, VertexAttribPointerType.Float, false, 32, 16);
        }

        public unsafe override void OnNewFrame(float totalSeconds, float elapsedSeconds)
        {
            Context.ClearWindowColor(Color4.Black);
            Context.ClearWindowDepthStencil(DepthStencil.Both, 1f, 0);

            Context.Pipeline.Program = program;
            Context.Pipeline.VertexArray = vertexArray;
            Context.Pipeline.Textures[0] = diffuseMap;
            Context.Pipeline.Samplers[0] = sampler;

            Context.Pipeline.DepthStencil.DepthTestEnable = true;
            Context.Pipeline.DepthStencil.DepthMask = true;

            Context.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedShort, 0);
        }
    }
}
