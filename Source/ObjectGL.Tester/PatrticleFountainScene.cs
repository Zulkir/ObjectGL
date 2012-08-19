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
    class PatrticleFountainScene : Scene
    {
        struct Vertex
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

            static readonly Random Rand = new Random();
        }

        const string VertexShaderText =
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

        const string FragmentShaderText =
@"#version 150

in vec3 v_color;

out vec4 out_color;

void main()
{
    out_color = vec4(v_color, 1.0);
}
";

        const int ParticleCount = 16384;
        const float ParticleTicksDelta = 0.125f;

        ShaderProgram program;
        VertexArray vertexArraySource;
        VertexArray vertexArrayTarget;
        TransformFeedback transformFeedbackSource;
        TransformFeedback transformFeedbackTarget;

        public PatrticleFountainScene(Context context, GameWindow gameWindow) : base(context, gameWindow)
        {
        }

        public override void Initialize()
        {
            var vertexData = new Vertex[ParticleCount];
            for (int i = 0; i < vertexData.Length; i++)
                vertexData[i] = new Vertex(i * ParticleTicksDelta);

            var vertexBufferSource = new Buffer(Context, BufferTarget.TransformFeedbackBuffer, ParticleCount*Vertex.SizeInBytes, BufferUsageHint.StaticDraw, new Data(vertexData));
            var vertexBufferTarget = new Buffer(Context, BufferTarget.TransformFeedbackBuffer, ParticleCount*Vertex.SizeInBytes, BufferUsageHint.StaticDraw, IntPtr.Zero);

            vertexArraySource = new VertexArray(Context);
            vertexArraySource.SetVertexAttributeF(Context, 0, vertexBufferSource, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);
            vertexArraySource.SetVertexAttributeF(Context, 1, vertexBufferSource, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 2 * sizeof(float));
            vertexArraySource.SetVertexAttributeF(Context, 2, vertexBufferSource, VertexAttributeDimension.One, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 4 * sizeof(float));
            vertexArraySource.SetVertexAttributeF(Context, 3, vertexBufferSource, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 5 * sizeof(float));
            vertexArraySource.SetVertexAttributeF(Context, 4, vertexBufferSource, VertexAttributeDimension.Three, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 7 * sizeof(float));

            vertexArrayTarget = new VertexArray(Context);
            vertexArrayTarget.SetVertexAttributeF(Context, 0, vertexBufferTarget, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);
            vertexArrayTarget.SetVertexAttributeF(Context, 1, vertexBufferTarget, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 2 * sizeof(float));
            vertexArrayTarget.SetVertexAttributeF(Context, 2, vertexBufferTarget, VertexAttributeDimension.One, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 4 * sizeof(float));
            vertexArrayTarget.SetVertexAttributeF(Context, 3, vertexBufferTarget, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 5 * sizeof(float));
            vertexArrayTarget.SetVertexAttributeF(Context, 4, vertexBufferTarget, VertexAttributeDimension.Three, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 7 * sizeof(float));

            transformFeedbackSource = new TransformFeedback(Context);
            transformFeedbackSource.SetBuffer(Context, 0, vertexBufferSource);

            transformFeedbackTarget = new TransformFeedback(Context);
            transformFeedbackTarget.SetBuffer(Context, 0, vertexBufferTarget);

            string shaderErrors;

            VertexShader vsh;
            FragmentShader fsh;

            if (!VertexShader.TryCompile(VertexShaderText, out vsh, out shaderErrors) ||
                !FragmentShader.TryCompile(FragmentShaderText, out fsh, out shaderErrors) ||
                !ShaderProgram.TryLink(Context, new ShaderProgramDescription
                {
                    VertexShaders = vsh,
                    FragmentShaders = fsh,
                    VertexAttributeNames = new[] { "in_position", "in_velocity", "in_spawn_cooldown", "in_initial_velocity", "in_color" },
                    TransformFeedbackAttributeNames = new[] { "v_position", "v_velocity", "v_spawn_cooldown", "v_initial_velocity", "v_color" },
                    TransformFeedbackMode = TransformFeedbackMode.InterleavedAttribs
                }, 
                out program, out shaderErrors))
                throw new ArgumentException("Program errors:\n\n" + shaderErrors);
        }

        public override void OnNewFrame(float totalSeconds, float elapsedSeconds)
        {
            Context.ClearWindowColor(Color4.Black);
            Context.ClearWindowDepthStencil(DepthStencil.Both, 1f, 0);

            Context.Pipeline.Program = program;
            Context.Pipeline.VertexArray = vertexArraySource;

            Context.BeginTransformFeedback(transformFeedbackTarget, BeginFeedbackMode.Points);
            Context.DrawArrays(BeginMode.Points, 0, ParticleCount);
            Context.EndTransformFeedback();
            
            Swap(ref vertexArraySource, ref vertexArrayTarget);
            Swap(ref transformFeedbackSource, ref transformFeedbackTarget);
        }

        static void Swap<T>(ref T o1, ref T o2)
        {
            var temp = o2;
            o2 = o1;
            o1 = temp;
        }
    }
}
