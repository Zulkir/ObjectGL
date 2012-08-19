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
    class FireworksScene : Scene
    {
        struct Vertex
        {
            public const int SizeInBytes = 9 * sizeof(float);

            public Vector2 Position;
            public Vector2 Velocity;
            public float TimeToAction;
            public int Type;
            public Vector3 Color;

            public static Vertex Cannon
            {
                get
                {
                    return new Vertex
                    {
                        Position = new Vector2(0f, -1f),
                        Velocity = new Vector2(0f, 0f),
                        TimeToAction = 0f,
                        Type = 1,
                        Color = new Vector3(0f, 0f, 1f)
                    };
                }
            }
        }

        const string VertexShaderText =
@"#version 150

in vec2 in_position;
in vec2 in_velocity;
in float in_timeToAction;
in int in_type;
in vec3 in_color;

out vec2 v_position;
out vec2 v_velocity;
out float v_timeToAction;
out int v_type;
out vec3 v_color;

void main()
{
    v_position = in_position;
    v_velocity = in_velocity;
    v_timeToAction = in_timeToAction;
    v_type = in_type;
    v_color = in_color;

    gl_Position = vec4(in_position, 0.0, 1.0);
}
";

        const string GeometryShaderText =
@"#version 150

layout ( points ) in;
layout ( points, max_vertices = 16 ) out;

layout(std140) uniform Time
{
    float TotalTime;
    float ElapsedTime;
};

in vec2 v_position[];
in vec2 v_velocity[];
in float v_timeToAction[];
in int v_type[];
in vec3 v_color[];

out vec2 g_position;
out vec2 g_velocity;
out float g_timeToAction;
out int g_type;
out vec3 g_color;

const float Pi = 3.14159;
const float RedSpeed = 0.7;
const float YellowSpeed = 0.2;

const float TimeToShoot = 1.0 / Pi;
const float TimeToExplode = 2.0;
const float TimeToDie = 1.0;

void main()
{
    g_position = v_position[0] + v_velocity[0] * ElapsedTime;
    g_velocity = v_velocity[0];
    g_timeToAction = v_timeToAction[0] - ElapsedTime;
    g_type = v_type[0];
    g_color = v_color[0];    
    gl_Position = vec4(g_position, 0.0, 1.0);       
    
    if (g_timeToAction <= 0.0)
    {
        if (v_type[0] == 1) // Cannon
        {
            g_timeToAction += TimeToShoot;
            EmitVertex();
            EndPrimitive();
            
            float angle = Pi / 6.0 * sin(1234.0 * TotalTime);
            g_position = vec2(0.0, -1.0);
            g_velocity = RedSpeed * vec2(sin(angle), cos(angle));
            g_timeToAction = TimeToExplode;
            g_type = 2;
            g_color = vec3(1.0, 0.0, 0.0);
            gl_Position = vec4(g_position, 0.0, 1.0);   
            EmitVertex();
            EndPrimitive();
        }
        else if (v_type[0] == 2) // Red
        {
            for (int i = 1; i < 17; i++)
            {
                float angle = float(i) * (Pi / 8.0);
                g_velocity = YellowSpeed * vec2(sin(angle), cos(angle));
                g_timeToAction = TimeToDie;
                g_type = 3;
                g_color = vec3(1.0, 1.0, 0.0);
                EmitVertex();
                EndPrimitive();
            }
        } 
    }
    else
    {
        EmitVertex();
        EndPrimitive();
    }
}
";

        const string FragmentShaderText =
@"#version 150

in vec3 g_color;

out vec4 out_color;

void main()
{
    out_color = vec4(g_color, 1.0);
}
";

        const int MaxVertices = 128;

        ShaderProgram program;
        VertexArray vertexArraySource;
        VertexArray vertexArrayTarget;
        TransformFeedback transformFeedbackSource;
        TransformFeedback transformFeedbackTarget;
        Buffer timeBuffer;

        public FireworksScene(Context context, GameWindow gameWindow) : base(context, gameWindow) {}

        public override void Initialize()
        {
            var vertexData = new Vertex[MaxVertices];
            vertexData[0] = Vertex.Cannon;

            var vertexBufferSource = new Buffer(Context, BufferTarget.TransformFeedbackBuffer, Vertex.SizeInBytes * MaxVertices, BufferUsageHint.StaticDraw, new Data(vertexData));
            var vertexBufferTarget = new Buffer(Context, BufferTarget.TransformFeedbackBuffer, Vertex.SizeInBytes * MaxVertices, BufferUsageHint.StaticDraw);

            vertexArraySource = new VertexArray(Context);
            vertexArraySource.SetVertexAttributeF(Context, 0, vertexBufferSource, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);
            vertexArraySource.SetVertexAttributeF(Context, 1, vertexBufferSource, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 2 * sizeof(float));
            vertexArraySource.SetVertexAttributeF(Context, 2, vertexBufferSource, VertexAttributeDimension.One, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 4 * sizeof(float));
            vertexArraySource.SetVertexAttributeI(Context, 3, vertexBufferSource, VertexAttributeDimension.One, VertexAttribIPointerType.Int, Vertex.SizeInBytes, 5 * sizeof(float));
            vertexArraySource.SetVertexAttributeF(Context, 4, vertexBufferSource, VertexAttributeDimension.Three, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 6 * sizeof(float));

            vertexArrayTarget = new VertexArray(Context);
            vertexArrayTarget.SetVertexAttributeF(Context, 0, vertexBufferTarget, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);
            vertexArrayTarget.SetVertexAttributeF(Context, 1, vertexBufferTarget, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 2 * sizeof(float));
            vertexArrayTarget.SetVertexAttributeF(Context, 2, vertexBufferTarget, VertexAttributeDimension.One, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 4 * sizeof(float));
            vertexArrayTarget.SetVertexAttributeI(Context, 3, vertexBufferTarget, VertexAttributeDimension.One, VertexAttribIPointerType.Int, Vertex.SizeInBytes, 5 * sizeof(float));
            vertexArrayTarget.SetVertexAttributeF(Context, 4, vertexBufferTarget, VertexAttributeDimension.Three, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 6 * sizeof(float));

            transformFeedbackSource = new TransformFeedback(Context);
            transformFeedbackSource.SetBuffer(Context, 0, vertexBufferSource);

            transformFeedbackTarget = new TransformFeedback(Context);
            transformFeedbackTarget.SetBuffer(Context, 0, vertexBufferTarget);

            timeBuffer = new Buffer(Context, BufferTarget.UniformBuffer, 2 * sizeof(float), BufferUsageHint.DynamicDraw);

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
                    VertexAttributeNames = new[] { "in_position", "in_velocity", "in_timeToAction", "in_type", "in_color" },
                    TransformFeedbackAttributeNames = new[] { "g_position", "g_velocity", "g_timeToAction", "g_type", "g_color" },
                    TransformFeedbackMode = TransformFeedbackMode.InterleavedAttribs,
                    UniformBufferNames = new [] { "Time" }
                },
                out program, out shaderErrors))
                throw new ArgumentException("Program errors:\n\n" + shaderErrors);
        }

        bool firstTime = true;

        public unsafe override void OnNewFrame(float totalSeconds, float elapsedSeconds)
        {
            var time = new Vector2(totalSeconds, elapsedSeconds);
            timeBuffer.SetData(Context, BufferTarget.UniformBuffer, (IntPtr)(&time));

            Context.ClearWindowColor(Color4.Black);
            Context.ClearWindowDepthStencil(DepthStencil.Both, 1f, 0);

            Context.Pipeline.Program = program;
            Context.Pipeline.VertexArray = vertexArraySource;
            Context.Pipeline.UniformBuffers[0] = timeBuffer;

            Context.BeginTransformFeedback(transformFeedbackTarget, BeginFeedbackMode.Points);
            if (firstTime)
            {
                Context.DrawArrays(BeginMode.Points, 0, 1);
                firstTime = false;
            }
            else
            {
                Context.DrawTransformFeedback(BeginMode.Points, transformFeedbackSource);
            }
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
