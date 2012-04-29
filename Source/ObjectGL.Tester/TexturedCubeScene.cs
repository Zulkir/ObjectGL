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
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ObjectGL.Tester
{
    class TexturedCubeScene : Scene
    {
        struct Vertex
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

        const string VertexShaderText =
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
in vec3 in_tex_coord;

out vec3 v_world_position;
out vec3 v_world_normal;
out vec2 v_tex_coord;

void main()
{
    vec4 worldPosition = vec4(in_position, 1.0f) * World;

    gl_Position = worldPosition * ViewProjection;

    v_world_position = worldPosition;
    v_world_normal = (vec4(in_normal, 0.0f) * World).xyz;;
    v_tex_coord = in_tex_coord;
}
";

        const string FragmentShaderText =
@"#version 150

layout(std140) uniform Light
{
    vec3 LightPosition;
};

uniform sampler2D DiffuseMap;

in vec3 v_world_position;
in vec3 v_world_normal;
in vec2 v_tex_coord;

out vec4 out_color;

void main()
{
    vec3 toLight = normalize(LightPosition - v_world_position);
    float diffuseFactor = clamp(dot(toLight, normalize(v_world_normal)), 0.0f, 1.0f);
    out_color = vec4(texture(DiffuseMap, v_tex_coord).xyz * clamp(diffuseFactor + 0.2f, 0.0f, 1.0f), 1.0f);
    //out_color = vec4(v_world_normal, 1.0f);
    //out_color = vec4(toLight, 1.0f);
    //out_color = texture(DiffuseMap, v_tex_coord);
}
";

        Buffer vertices;
        Buffer indices;

        Buffer transform;
        Buffer camera;
        Buffer light;

        Texture2D diffuseMap;
        Sampler diffuseSampler;

        ShaderProgram program;

        VertexArray vertexArray;

        public TexturedCubeScene(Context context, GameWindow gameWindow) : base(context, gameWindow)
        {
        }

        public unsafe override void Initialize()
        {
            vertices = new Buffer(Context, BufferTarget.ArrayBuffer, 24 * 8 * sizeof(float), BufferUsageHint.StaticDraw, new Data(new[]
            {
                new Vertex(-1f, 1f, 1f, 0.0f, 0.0f, 1f, 0f, 0f),
                new Vertex(1f, 1f, 1f, 0.0f, 0.0f, 1f, 1f, 0f),
                new Vertex(1f, -1f, 1f, 0.0f, 0.0f, 1f, 1f, 1f),
                new Vertex(-1f, -1f, 1f, 0.0f, 0.0f, 1f, 0f, 1f),

                new Vertex(-1f, 1f, -1f, 0.0f, 1f, 0.0f, 0f, 0f),
                new Vertex(1f, 1f, -1f, 0.0f, 1f, 0.0f, 1f, 0f),
                new Vertex(1f, 1f, 1f, 0.0f, 1f, 0.0f, 1f, 1f),
                new Vertex(-1f, 1f, 1f, 0.0f, 1f, 0.0f, 0f, 1f),

                new Vertex(1f, -1f, -1f, 0.0f, -1f, 0.0f, 0f, 0f),
                new Vertex(-1f, -1f, -1f, 0.0f, -1f, 0.0f, 1f, 0f),
                new Vertex(-1f, -1f, 1f, 0.0f, -1f, 0.0f, 1f, 1f),
                new Vertex(1f, -1f, 1f, 0.0f, -1f, 0.0f, 0f, 1f),

                new Vertex(1f, 1f, -1f, 0.0f, 0.0f, -1f, 0f, 0f),
                new Vertex(-1f, 1f, -1f, 0.0f, 0.0f, -1f, 1f, 0f),
                new Vertex(-1f, -1f, -1f, 0.0f, 0.0f, -1f, 1f, 1f),
                new Vertex(1f, -1f, -1f, 0.0f, 0.0f, -1f, 0f, 1f),

                new Vertex(1f, 1f, 1f, 1f, 0.0f, 0.0f, 0f, 0f),
                new Vertex(1f, 1f, -1f, 1f, 0.0f, 0.0f, 1f, 0f),
                new Vertex(1f, -1f, -1f, 1f, 0.0f, 0.0f, 1f, 1f),
                new Vertex(1f, -1f, 1f, 1f, 0.0f, 0.0f, 0f, 1f),

                new Vertex(-1f, 1f, -1f, -1f, 0.0f, 0.0f, 0f, 0f),
                new Vertex(-1f, 1f, 1f, -1f, 0.0f, 0.0f, 1f, 0f),
                new Vertex(-1f, -1f, 1f, -1f, 0.0f, 0.0f, 1f, 1f),
                new Vertex(-1f, -1f, -1f, -1f, 0.0f, 0.0f, 0f, 1f),
            }));

            indices = new Buffer(Context, BufferTarget.ElementArrayBuffer, 36 * sizeof(ushort), BufferUsageHint.StaticDraw, new Data(new ushort[] 
            { 
                0, 1, 2, 0, 2, 3,
                4, 5, 6, 4, 6, 7,
                8, 9, 10, 8, 10, 11,
                12, 13, 14, 12, 14, 15,
                16, 17, 18, 16, 18, 19,
                20, 21, 22, 20, 22, 23,
            }));

            Matrix4 world = Matrix4.Identity; 
            transform = new Buffer(Context, BufferTarget.UniformBuffer, 64, BufferUsageHint.DynamicDraw, (IntPtr)(&world));
            camera = new Buffer(Context, BufferTarget.UniformBuffer, 64, BufferUsageHint.DynamicDraw);
            light = new Buffer(Context, BufferTarget.UniformBuffer, 12, BufferUsageHint.DynamicDraw);

            using (var textureLoader = new TextureLoader("../Textures/DiffuseTest.bmp"))
            {
                diffuseMap = new Texture2D(Context, textureLoader.Width, textureLoader.Height, PixelInternalFormat.Rgba8,
                                           PixelFormat.Rgba, PixelType.UnsignedByte, i => textureLoader.GetMipData(i));
            }

            diffuseSampler = new Sampler();
            diffuseSampler.SetMagFilter(TextureMagFilter.Nearest);
            diffuseSampler.SetMinFilter(TextureMinFilter.NearestMipmapNearest);

            string shaderErrors;

            VertexShader vsh;
            FragmentShader fsh;

            if (!VertexShader.TryCompile(VertexShaderText, out vsh, out shaderErrors) ||
                !FragmentShader.TryCompile(FragmentShaderText, out fsh, out shaderErrors) ||
                !ShaderProgram.TryLink(Context, vsh, fsh, null, 
                new[] { "in_position", "in_normal", "in_tex_coord" },
                new[] { "Transform", "Camera", "Light" }, 
                null, 
                new[] { "DiffuseMap" },  
                0, out program, out shaderErrors))
                throw new ArgumentException("Program errors:\n\n" + shaderErrors);

            vertexArray = new VertexArray(Context);
            vertexArray.SetElementArrayBuffer(Context, indices);
            vertexArray.SetVertexAttributeF(Context, 0, vertices, 3, VertexAttribPointerType.Float, false, 32, 0);
            vertexArray.SetVertexAttributeF(Context, 1, vertices, 3, VertexAttribPointerType.Float, false, 32, 12);
            vertexArray.SetVertexAttributeF(Context, 2, vertices, 2, VertexAttribPointerType.Float, false, 32, 24);
        }

        public unsafe override void OnNewFrame(float totalSeconds, float elapsedSeconds)
        {
            float angle = totalSeconds * 0.5f;
            Matrix4 world = Matrix4.CreateRotationX(angle) * Matrix4.CreateRotationY(2 * angle) * Matrix4.CreateRotationZ(3 * angle);
            world.Transpose();

            Matrix4 view = Matrix4.LookAt(new Vector3(5, 3, 0), Vector3.Zero, Vector3.UnitZ);
            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView((float) Math.PI/4f, (float) GameWindow.Width/GameWindow.Height, 0.1f, 1000f);
            Matrix4 viewProjection = view * proj;
            viewProjection.Transpose();

            Vector3 lightPosition = new Vector3(10, -7, 2);

            transform.SetData(Context, BufferTarget.UniformBuffer, (IntPtr)(&world));
            camera.SetData(Context, BufferTarget.UniformBuffer, (IntPtr)(&viewProjection));
            light.SetData(Context, BufferTarget.UniformBuffer, (IntPtr)(&lightPosition));

            Context.ClearWindowColor(Color4.Black);
            Context.ClearWindowDepthStencil(DepthStencil.Both, 1f, 0);

            Context.Pipeline.Program = program;
            Context.Pipeline.UniformBuffers[0] = transform;
            Context.Pipeline.UniformBuffers[1] = camera;
            Context.Pipeline.UniformBuffers[2] = light;
            Context.Pipeline.VertexArray = vertexArray;
            Context.Pipeline.Textures[0] = diffuseMap;
            Context.Pipeline.Samplers[0] = diffuseSampler;

            Context.Pipeline.DepthStencil.DepthTestEnable = true;
            Context.Pipeline.DepthStencil.DepthMask = true;

            Context.DrawElements(BeginMode.Triangles, 36, DrawElementsType.UnsignedShort, 0);
        }
    }
}