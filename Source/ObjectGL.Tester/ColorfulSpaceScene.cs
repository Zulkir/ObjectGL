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
using System.Runtime.InteropServices;
using ObjectGL.Api;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Actions;
using ObjectGL.Api.Context.States.Blend;
using ObjectGL.Api.Objects.Framebuffers;
using ObjectGL.Api.Objects.Resources.Buffers;
using ObjectGL.Api.Objects.Shaders;
using ObjectGL.Api.Objects.VertexArrays;
using OpenTK;

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

namespace ObjectGL.Tester
{
    public class ColorfulSpaceScene : Scene
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Vertex
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

        private const string VertexShaderText =
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

        private const string GeometryShaderText =
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

        private const string FragmentShaderText =
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

        private const int ParticleCount = 512;
        private const float ParticleSpeed = 0.75f;

        private IShaderProgram program;
        private IVertexArray vertexArray;

        private IBuffer timeBuffer;
        private IBuffer cameraBuffer;

        public ColorfulSpaceScene(IContext context, GameWindow gameWindow) : base(context, gameWindow)
        {
        }

        public override void Initialize()
        {
            var vertexData = new Vertex[ParticleCount];
            for (int i = 0; i < vertexData.Length; i++)
                vertexData[i] = new Vertex((float) i/ParticleCount);

            var vertexBuffer = Context.Create.Buffer(BufferTarget.Array, ParticleCount * Vertex.SizeInBytes, BufferUsageHint.StaticDraw, vertexData);
            vertexArray = Context.Create.VertexArray();
            vertexArray.SetVertexAttributeF(0, vertexBuffer, VertexAttributeDimension.Three, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);
            vertexArray.SetVertexAttributeF(1, vertexBuffer, VertexAttributeDimension.Three, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 3 * sizeof(float));

            timeBuffer = Context.Create.Buffer(BufferTarget.Uniform, sizeof(float), BufferUsageHint.DynamicDraw);
            cameraBuffer = Context.Create.Buffer(BufferTarget.Uniform, sizeof(float), BufferUsageHint.DynamicDraw);

            var vsh = Context.Create.VertexShader(VertexShaderText);
            var gsh = Context.Create.GeometryShader(GeometryShaderText);
            var fsh = Context.Create.FragmentShader(FragmentShaderText);
            program = Context.Create.Program(new ShaderProgramDescription
            {
                VertexShaders = new[] {vsh},
                GeometryShaders = new[] {gsh},
                FragmentShaders = new[] {fsh},
                VertexAttributeNames = new[] {"in_position", "in_color"},
                UniformBufferNames = new[] {"Time", "Camera"}
            });
        }

        public override unsafe void OnNewFrame(float totalSeconds, float elapsedSeconds)
        {
            float time = totalSeconds*ParticleSpeed;
            float aspectRatio = (float)GameWindow.ClientSize.Width / GameWindow.ClientSize.Height;
            timeBuffer.SetDataByMapping((IntPtr)(&time));
            cameraBuffer.SetDataByMapping((IntPtr)(&aspectRatio));

            Context.Actions.ClearWindowColor(new Color4(0, 0, 0, 1));
            Context.Actions.ClearWindowDepthStencil(DepthStencil.Both, 1f, 0);

            Context.States.ScreenClipping.United.Viewport.Set(GameWindow.ClientSize.Width, GameWindow.ClientSize.Height);

            Context.States.DepthStencil.DepthMask.Set(false);
            Context.States.DepthStencil.DepthTestEnable.Set(false);

            Context.States.Blend.BlendEnable.Set(true);
            Context.States.Blend.United.Function.Set(BlendFactor.One, BlendFactor.One);

            Context.Bindings.Program.Set(program);
            Context.Bindings.VertexArray.Set(vertexArray);
            Context.Bindings.Buffers.UniformIndexed[0].Set(timeBuffer);
            Context.Bindings.Buffers.UniformIndexed[1].Set(cameraBuffer);

            Context.Actions.Draw.Arrays(BeginMode.Points, 0, ParticleCount);
        }
    }
}
