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
using ObjectGL.Api;
using ObjectGL.Api.Context;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;
using OpenTK;
using IContext = ObjectGL.Api.IContext;

namespace ObjectGL.Tester
{
    public class TexturedCubeScene : Scene
    {
        private struct Vertex
        {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector2 TexCoord;

            public Vertex(float px, float py, float pz, float nx, float ny, float nz, float tx, float ty)
            {
                Position.X = px;
                Position.Y = py;
                Position.Z = pz;
                Normal.X = nx;
                Normal.Y = ny;
                Normal.Z = nz;
                TexCoord.X = tx;
                TexCoord.Y = ty;
            }
        }

        private const string VertexShaderText =
@"#version 150

layout(std140) uniform Transform
{
    mat4 World;
};

layout(std140) uniform Camera
{
    mat4 ViewProjection;
};


in vec3 in_position;
in vec3 in_normal;
in vec2 in_tex_coord;

out vec3 v_world_position;
out vec3 v_world_normal;
out vec2 v_tex_coord;

void main()
{
    vec4 worldPosition = vec4(in_position, 1.0f) * World;

    gl_Position = worldPosition * ViewProjection;

    v_world_position = worldPosition.xyz;
    v_world_normal = (vec4(in_normal, 0.0f) * World).xyz;;
    v_tex_coord = in_tex_coord;
}
";

        private const string FragmentShaderText =
@"#version 150

layout(std140) uniform CameraExtra
{
    vec3 CameraPosition;
};

layout(std140) uniform Light
{
    vec3 LightPosition;
};

uniform sampler2D DiffuseMap;
uniform sampler2D SpecularMap;

in vec3 v_world_position;
in vec3 v_world_normal;
in vec2 v_tex_coord;

out vec4 out_color;

void main()
{
    vec3 toEye = normalize(CameraPosition - v_world_position);
    vec3 toLight = normalize(LightPosition - v_world_position);
    vec3 normal = normalize(v_world_normal);

    float diffuseFactor = clamp(dot(toLight, normal), 0.0f, 1.0f);
    float specularFactor = pow(clamp(dot(toEye, reflect(-toLight, normal)), 0.0f, 1.0f), 24.0f);

    out_color = vec4(
                texture(DiffuseMap, v_tex_coord).xyz * clamp(diffuseFactor + 0.2f, 0.0f, 1.0f) + 
                texture(SpecularMap, v_tex_coord).xyz * specularFactor, 
                1.0f);
}
";
        private IShaderProgram program;
        private IVertexArray vertexArray;

        private IBuffer transformBuffer;
        private IBuffer cameraBuffer;
        private IBuffer cameraExtraBuffer;
        private IBuffer lightBuffer;

        private ITexture2D diffuseMap;
        private ITexture2D specularMap;
        private ISampler sampler;

        public TexturedCubeScene(IContext context, GameWindow gameWindow) : base(context, gameWindow)
        {
        }

        public override void Initialize()
        {
            var vertexBuffer = Context.Create.Buffer(BufferTarget.ArrayBuffer, 24 * 8 * sizeof(float), BufferUsageHint.StaticDraw, new[]
            {
                new Vertex(1f, -1f, 1f, 1f, 0.0f, 0.0f, 0f, 0f),
                new Vertex(1f, 1f, 1f, 1f, 0.0f, 0.0f, 1f, 0f),
                new Vertex(1f, 1f, -1f, 1f, 0.0f, 0.0f, 1f, 1f),
                new Vertex(1f, -1f, -1f, 1f, 0.0f, 0.0f, 0f, 1f),

                new Vertex(1f, 1f, 1f, 0.0f, 1f, 0.0f, 0f, 0f),
                new Vertex(-1f, 1f, 1f, 0.0f, 1f, 0.0f, 1f, 0f),
                new Vertex(-1f, 1f, -1f, 0.0f, 1f, 0.0f, 1f, 1f),
                new Vertex(1f, 1f, -1f, 0.0f, 1f, 0.0f, 0f, 1f),

                new Vertex(-1f, 1f, 1f, -1f, 0.0f, 0.0f, 0f, 0f),
                new Vertex(-1f, -1f, 1f, -1f, 0.0f, 0.0f, 1f, 0f),
                new Vertex(-1f, -1f, -1f, -1f, 0.0f, 0.0f, 1f, 1f),
                new Vertex(-1f, 1f, -1f, -1f, 0.0f, 0.0f, 0f, 1f),

                new Vertex(-1f, -1f, 1f, 0.0f, -1f, 0.0f, 0f, 0f),
                new Vertex(1f, -1f, 1f, 0.0f, -1f, 0.0f, 1f, 0f),
                new Vertex(1f, -1f, -1f, 0.0f, -1f, 0.0f, 1f, 1f),
                new Vertex(-1f, -1f, -1f, 0.0f, -1f, 0.0f, 0f, 1f),

                new Vertex(-1f, -1f, 1f, 0.0f, 0.0f, 1f, 0f, 0f),
                new Vertex(-1f, 1f, 1f, 0.0f, 0.0f, 1f, 1f, 0f),
                new Vertex(1f, 1f, 1f, 0.0f, 0.0f, 1f, 1f, 1f),
                new Vertex(1f, -1f, 1f, 0.0f, 0.0f, 1f, 0f, 1f),

                new Vertex(-1f, 1f, -1f, 0.0f, 0.0f, -1f, 0f, 0f),
                new Vertex(-1f, -1f, -1f, 0.0f, 0.0f, -1f, 1f, 0f),
                new Vertex(1f, -1f, -1f, 0.0f, 0.0f, -1f, 1f, 1f),
                new Vertex(1f, 1f, -1f, 0.0f, 0.0f, -1f, 0f, 1f)
            });

            var indexBuffer = Context.Create.Buffer(BufferTarget.ElementArrayBuffer, 36 * sizeof(ushort), BufferUsageHint.StaticDraw, new ushort[]
            {
                0, 1, 2, 0, 2, 3,
                4, 5, 6, 4, 6, 7,
                8, 9, 10, 8, 10, 11,
                12, 13, 14, 12, 14, 15,
                16, 17, 18, 16, 18, 19,
                20, 21, 22, 20, 22, 23
            });

            vertexArray = Context.Create.VertexArray();
            vertexArray.SetElementArrayBuffer(indexBuffer);
            vertexArray.SetVertexAttributeF(0, vertexBuffer, VertexAttributeDimension.Three, VertexAttribPointerType.Float, false, 32, 0);
            vertexArray.SetVertexAttributeF(1, vertexBuffer, VertexAttributeDimension.Three, VertexAttribPointerType.Float, false, 32, 12);
            vertexArray.SetVertexAttributeF(2, vertexBuffer, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, 32, 24);

            transformBuffer = Context.Create.Buffer(BufferTarget.UniformBuffer, 64, BufferUsageHint.DynamicDraw);
            cameraBuffer = Context.Create.Buffer(BufferTarget.UniformBuffer, 64, BufferUsageHint.DynamicDraw);
            cameraExtraBuffer = Context.Create.Buffer(BufferTarget.UniformBuffer, 12, BufferUsageHint.DynamicDraw);
            lightBuffer = Context.Create.Buffer(BufferTarget.UniformBuffer, 12, BufferUsageHint.DynamicDraw);

            using (var textureLoader = new TextureLoader("../Textures/DiffuseTest.png"))
            {
                diffuseMap = Context.Create.Texture2D(textureLoader.Width, textureLoader.Height, TextureHelper.CalculateMipCount(textureLoader.Width, textureLoader.Height, 1), Format.Rgba8);
                for (int i = 0; i < diffuseMap.MipCount; i++)
                    diffuseMap.SetData(i, textureLoader.GetMipData(i), FormatColor.Rgba, FormatType.UnsignedByte);
            }

            using (var textureLoader = new TextureLoader("../Textures/SpecularTest.png"))
            {
                specularMap = Context.Create.Texture2D(textureLoader.Width, textureLoader.Height, TextureHelper.CalculateMipCount(textureLoader.Width, textureLoader.Height, 1), Format.Rgba8);
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
                VertexShaders = new[] { vsh },
                FragmentShaders = new[] { fsh },
                VertexAttributeNames = new[] {"in_position", "in_normal", "in_tex_coord"},
                UniformBufferNames = new[] {"Transform", "Camera", "CameraExtra", "Light"},
                SamplerNames = new[] {"DiffuseMap", "SpecularMap"}
            });
        }

        public unsafe override void OnNewFrame(float totalSeconds, float elapsedSeconds)
        {
            float angle = totalSeconds * 0.25f;
            Matrix4 world = Matrix4.CreateRotationX(angle) * Matrix4.CreateRotationY(2 * angle) * Matrix4.CreateRotationZ(3 * angle);
            world.Transpose();

            Vector3 cameraPosition = new Vector3(5, 3, 0);

            Matrix4 view = Matrix4.LookAt(cameraPosition, Vector3.Zero, Vector3.UnitZ);
            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView((float) Math.PI/4f, (float) GameWindow.Width/GameWindow.Height, 0.1f, 1000f);
            Matrix4 viewProjection = view * proj;
            viewProjection.Transpose();

            Vector3 lightPosition = new Vector3(10, -7, 2);

            transformBuffer.SetDataByMapping((IntPtr)(&world));
            cameraBuffer.SetDataByMapping((IntPtr)(&viewProjection));
            cameraExtraBuffer.SetDataByMapping((IntPtr)(&cameraPosition));
            lightBuffer.SetDataByMapping((IntPtr)(&lightPosition));

            Context.ClearWindowColor(new Color4(0, 0, 0, 1));
            Context.ClearWindowDepthStencil(DepthStencil.Both, 1f, 0);

            Context.Pipeline.DepthStencil.DepthTestEnable = true;
            Context.Pipeline.DepthStencil.DepthMask = true;

            Context.Pipeline.Program = program;
            Context.Pipeline.VertexArray = vertexArray;
            Context.Pipeline.UniformBuffers[0] = transformBuffer;
            Context.Pipeline.UniformBuffers[1] = cameraBuffer;
            Context.Pipeline.UniformBuffers[2] = cameraExtraBuffer;
            Context.Pipeline.UniformBuffers[3] = lightBuffer;
            Context.Pipeline.Textures[0] = diffuseMap;
            Context.Pipeline.Samplers[0] = sampler;
            Context.Pipeline.Textures[1] = specularMap;
            Context.Pipeline.Samplers[1] = sampler;

            Context.DrawElements(BeginMode.Triangles, 36, DrawElementsType.UnsignedShort, 0);
        }
    }
}