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
    class ColorfulSpaceScene : Scene
    {
        struct Vertex
        {
            public const int SizeInBytes = 6 * sizeof(float);

            public Vector3 Position;
            public Vector3 Color;

            public Vertex(float offset)
            {
                Position = new Vector3((float)Rand.NextDouble() - 0.5f, (float)Rand.NextDouble() - 0.5f, offset);

                // random saturated color
                switch (Rand.Next(1, 6))
                {
                    case 1: Color = new Vector3(1, 0, (float)Rand.NextDouble()); break;
                    case 2: Color = new Vector3(0, 1, (float)Rand.NextDouble()); break;
                    case 3: Color = new Vector3(1, (float)Rand.NextDouble(), 0); break;
                    case 4: Color = new Vector3(0, (float)Rand.NextDouble(), 1); break;
                    case 5: Color = new Vector3((float)Rand.NextDouble(), 1, 0); break;
                    case 6: Color = new Vector3((float)Rand.NextDouble(), 0, 1); break;
                    default: Color = new Vector3(1, 1, 1); break;
                }
            }

            static readonly Random Rand = new Random();
        }

        const string VertexShaderText =
@"#version 150

layout(std140) uniform Time
{
    float CurrentTime;
};

in vec3 in_position;
in vec3 in_color;

out vec3 v_color;

void main()
{
    float normalizedOffset = mod((in_position.z + CurrentTime), 1.0);
    gl_Position = vec4(in_position.x, in_position.y, 1.0 - normalizedOffset, 1.0);
    v_color = in_color;
}
";

        const string GeometryShaderText =
@"#version 150

layout ( points ) in;
layout ( triangle_strip, max_vertices = 4 ) out;

layout(std140) uniform Camera
{
    float AspectRatio;
};

const float MaxHalfSize = 0.01;

in vec3 v_color[];

out vec3 g_color;
out vec2 g_tex_coord;

void main()
{
    float z = gl_in[0].gl_Position.z;
    vec4 position = vec4(gl_in[0].gl_Position.xy / z, z, 1.0);
    float halfWidth = MaxHalfSize * (1.0 - z);
    float halfHeight = halfWidth * AspectRatio;

    gl_Position = position + vec4(-halfWidth, -halfHeight, 0.0, 0.0);
    g_color = v_color[0];
    g_tex_coord = vec2(-1.0, -1.0);
    EmitVertex();

    gl_Position = position + vec4(halfWidth, -halfHeight, 0.0, 0.0);
    g_color = v_color[0];
    g_tex_coord = vec2(1.0, -1.0);
    EmitVertex();

    gl_Position = position + vec4(-halfWidth, halfHeight, 0.0, 0.0);
    g_color = v_color[0];
    g_tex_coord = vec2(-1.0, 1.0);
    EmitVertex();

    gl_Position = position + vec4(halfWidth, halfHeight, 0.0, 0.0);
    g_color = v_color[0];
    g_tex_coord = vec2(1.0, 1.0);
    EmitVertex();

    EndPrimitive();
}
";

        const string FragmentShaderText =
@"#version 150

in vec3 g_color;
in vec2 g_tex_coord;

out vec4 out_color;

void main()
{
    float fancyLength = abs(g_tex_coord.x * g_tex_coord.y);
    out_color = vec4(g_color, 1.0) * max(0.0, 1.0 - 10.0 * fancyLength);
}
";

        const int ParticleCount = 512;
        const float ParticleSpeed = 0.75f;

        ShaderProgram program;
        VertexArray vertexArray;

        Buffer timeBuffer;
        Buffer cameraBuffer;

        public ColorfulSpaceScene(Context context, GameWindow gameWindow) : base(context, gameWindow)
        {
        }

        public override void Initialize()
        {
            var vertexData = new Vertex[ParticleCount];
            for (int i = 0; i < vertexData.Length; i++)
                vertexData[i] = new Vertex((float) i/ParticleCount);

            var vertexBuffer = new Buffer(Context, BufferTarget.ArrayBuffer, ParticleCount * Vertex.SizeInBytes, BufferUsageHint.StaticDraw, new Data(vertexData));
            vertexArray = new VertexArray(Context);
            vertexArray.SetVertexAttributeF(Context, 0, vertexBuffer, VertexAttributeDimension.Three, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);
            vertexArray.SetVertexAttributeF(Context, 1, vertexBuffer, VertexAttributeDimension.Three, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 3 * sizeof(float));

            timeBuffer = new Buffer(Context, BufferTarget.UniformBuffer, sizeof(float), BufferUsageHint.DynamicDraw);
            cameraBuffer = new Buffer(Context, BufferTarget.UniformBuffer, sizeof(float), BufferUsageHint.DynamicDraw);

            string shaderErrors;

            VertexShader vsh;
            GeometryShader gsh;
            FragmentShader fsh;

            if (!VertexShader.TryCompile(VertexShaderText, out vsh, out shaderErrors) ||
                !GeometryShader.TryCompile(GeometryShaderText, out gsh, out shaderErrors) ||
                !FragmentShader.TryCompile(FragmentShaderText, out fsh, out shaderErrors) ||
                !ShaderProgram.TryLink(Context, new ShaderProgramDescription
                {
                    VertexShaders = vsh,
                    GeometryShaders = gsh,
                    FragmentShaders = fsh,
                    VertexAttributeNames = new[] { "in_position", "in_color" },
                    UniformBufferNames = new[] { "Time", "Camera" }
                }, 
                out program, out shaderErrors))
                throw new ArgumentException("Program errors:\n\n" + shaderErrors);
        }

        public override unsafe void OnNewFrame(float totalSeconds, float elapsedSeconds)
        {
            float time = totalSeconds*ParticleSpeed;
            float aspectRatio = (float)GameWindow.ClientSize.Width / GameWindow.ClientSize.Height;
            timeBuffer.SetData(Context, BufferTarget.UniformBuffer, new Data((IntPtr)(&time)));
            cameraBuffer.SetData(Context, BufferTarget.UniformBuffer, new Data((IntPtr)(&aspectRatio)));

            Context.ClearWindowColor(Color4.Black);
            Context.ClearWindowDepthStencil(DepthStencil.Both, 1f, 0);

            Context.Pipeline.DepthStencil.DepthMask = false;
            Context.Pipeline.DepthStencil.DepthTestEnable = false;

            Context.Pipeline.Blend.BlendEnable = true;
            Context.Pipeline.Blend.Targets[0].Color.SrcFactor = BlendFactor.One;
            Context.Pipeline.Blend.Targets[0].Color.DestFactor = BlendFactor.One;

            Context.Pipeline.Program = program;
            Context.Pipeline.VertexArray = vertexArray;
            Context.Pipeline.UniformBuffers[0] = timeBuffer;
            Context.Pipeline.UniformBuffers[1] = cameraBuffer;

            Context.DrawArrays(BeginMode.Points, 0, ParticleCount);
        }
    }
}
