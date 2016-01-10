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
using System.Runtime.InteropServices;
using ObjectGL.Api;
using ObjectGL.Api.Context;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;
using OpenTK;

namespace ObjectGL.Tester
{
    public class PatrticleFountainScene : Scene
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Vertex
        {
            public const int SizeInBytes = 10 * sizeof (float);

            public Vector2 Position;
            public Vector2 Velocity;
            public float SpawnCooldown;
            public Vector2 InitialVelocity;
            public Vector3 Color;

            public Vertex(float spawnCooldown)
            {
                Position = new Vector2(0, -1);
                Velocity = new Vector2(0, 0);

                SpawnCooldown = spawnCooldown;

                double angle = Math.PI/8*(Rand.NextDouble() - 0.5);
                float initialVelocityAbs = 0.012f * (float)Rand.NextDouble() + 0.032f;
                InitialVelocity = initialVelocityAbs * (new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle)));

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

            private static readonly Random Rand = new Random();
        }

        private const string VertexShaderText =
@"#version 150

layout(std140) uniform Time
{
    float CurrentTime;
};

const float RespawnTicks = 180.0;
const float Gravity = -0.0005;

in vec2 in_position;
in vec2 in_velocity;
in float in_spawn_cooldown;
in vec2 in_initial_velocity;
in vec3 in_color;

out vec2 v_position;
out vec2 v_velocity;
out float v_spawn_cooldown;
out vec2 v_initial_velocity;
out vec3 v_color;

void main()
{
    v_spawn_cooldown = in_spawn_cooldown - 1.0;
    if (v_spawn_cooldown >= 0.0)
    {
        v_velocity = vec2(in_velocity.x, in_velocity.y + Gravity);
        v_position = in_position + v_velocity;
    }
    else
    {
        v_spawn_cooldown += RespawnTicks;
        v_position = vec2(0.0, -1.0);
        v_velocity = in_initial_velocity;
    }
    v_initial_velocity = in_initial_velocity;
    v_color = in_color;

    gl_Position = vec4(v_position, 0.0, 1.0);
}
";

        private const string FragmentShaderText =
@"#version 150

in vec3 v_color;

out vec4 out_color;

void main()
{
    out_color = vec4(v_color, 1.0);
}
";

        private const int ParticleCount = 16384;
        private const float ParticleTicksDelta = 0.125f;

        private IShaderProgram program;
        private IVertexArray vertexArraySource;
        private IVertexArray vertexArrayTarget;
        private ITransformFeedback transformFeedbackSource;
        private ITransformFeedback transformFeedbackTarget;

        public PatrticleFountainScene(IContext context, GameWindow gameWindow) : base(context, gameWindow)
        {
        }

        public override void Initialize()
        {
            var vertexData = new Vertex[ParticleCount];
            for (int i = 0; i < vertexData.Length; i++)
                vertexData[i] = new Vertex(i * ParticleTicksDelta);

            var vertexBufferSource = Context.Create.Buffer(BufferTarget.TransformFeedback, ParticleCount * Vertex.SizeInBytes, BufferUsageHint.StaticDraw, vertexData);
            var vertexBufferTarget = Context.Create.Buffer(BufferTarget.TransformFeedback, ParticleCount*Vertex.SizeInBytes, BufferUsageHint.StaticDraw, IntPtr.Zero);

            vertexArraySource = Context.Create.VertexArray();
            vertexArraySource.SetVertexAttributeF(0, vertexBufferSource, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);
            vertexArraySource.SetVertexAttributeF(1, vertexBufferSource, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 2 * sizeof(float));
            vertexArraySource.SetVertexAttributeF(2, vertexBufferSource, VertexAttributeDimension.One, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 4 * sizeof(float));
            vertexArraySource.SetVertexAttributeF(3, vertexBufferSource, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 5 * sizeof(float));
            vertexArraySource.SetVertexAttributeF(4, vertexBufferSource, VertexAttributeDimension.Three, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 7 * sizeof(float));

            vertexArrayTarget = Context.Create.VertexArray();
            vertexArrayTarget.SetVertexAttributeF(0, vertexBufferTarget, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);
            vertexArrayTarget.SetVertexAttributeF(1, vertexBufferTarget, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 2 * sizeof(float));
            vertexArrayTarget.SetVertexAttributeF(2, vertexBufferTarget, VertexAttributeDimension.One, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 4 * sizeof(float));
            vertexArrayTarget.SetVertexAttributeF(3, vertexBufferTarget, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 5 * sizeof(float));
            vertexArrayTarget.SetVertexAttributeF(4, vertexBufferTarget, VertexAttributeDimension.Three, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 7 * sizeof(float));

            transformFeedbackSource = Context.Create.TransformFeedback();
            transformFeedbackSource.SetBuffer(0, vertexBufferSource);

            transformFeedbackTarget = Context.Create.TransformFeedback();
            transformFeedbackTarget.SetBuffer(0, vertexBufferTarget);

            var vsh = Context.Create.VertexShader(VertexShaderText);
            var fsh = Context.Create.FragmentShader(FragmentShaderText);
            program = Context.Create.Program(new ShaderProgramDescription
            {
                VertexShaders = new[] {vsh},
                FragmentShaders = new[] {fsh},
                VertexAttributeNames = new[] {"in_position", "in_velocity", "in_spawn_cooldown", "in_initial_velocity", "in_color"},
                TransformFeedbackAttributeNames = new[] {"v_position", "v_velocity", "v_spawn_cooldown", "v_initial_velocity", "v_color"},
                TransformFeedbackMode = TransformFeedbackMode.InterleavedAttribs
            });
        }

        public override void OnNewFrame(float totalSeconds, float elapsedSeconds)
        {
            Context.Actions.ClearWindowColor(new Color4(0, 0, 0, 1));
            Context.Actions.ClearWindowDepthStencil(DepthStencil.Both, 1f, 0);

            Context.States.ScreenClipping.United.Viewport.Set(GameWindow.ClientSize.Width, GameWindow.ClientSize.Height);

            Context.Bindings.Program.Set(program);
            Context.Bindings.VertexArray.Set(vertexArraySource);

            Context.Actions.BeginTransformFeedback(transformFeedbackTarget, BeginFeedbackMode.Points);
            Context.Actions.Draw.Arrays(BeginMode.Points, 0, ParticleCount);
            Context.Actions.EndTransformFeedback();
            
            Swap(ref vertexArraySource, ref vertexArrayTarget);
            Swap(ref transformFeedbackSource, ref transformFeedbackTarget);
        }

        private static void Swap<T>(ref T o1, ref T o2)
        {
            var temp = o2;
            o2 = o1;
            o1 = temp;
        }
    }
}
