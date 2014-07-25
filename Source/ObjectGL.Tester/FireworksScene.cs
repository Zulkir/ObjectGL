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
using System.Runtime.InteropServices;
using ObjectGL.Api;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;
using OpenTK;

namespace ObjectGL.Tester
{
    public class FireworksScene : Scene
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Vertex
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

        private const string VertexShaderText =
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

        private const string GeometryShaderText =
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

        private const string FragmentShaderText =
@"#version 150

in vec3 g_color;

out vec4 out_color;

void main()
{
    out_color = vec4(g_color, 1.0);
}
";

        private const int MaxVertices = 128;

        private IShaderProgram program;
        private IVertexArray vertexArraySource;
        private IVertexArray vertexArrayTarget;
        private ITransformFeedback transformFeedbackSource;
        private ITransformFeedback transformFeedbackTarget;
        private IBuffer timeBuffer;

        public FireworksScene(IContext context, GameWindow gameWindow) : base(context, gameWindow) {}

        public override void Initialize()
        {
            var vertexData = new Vertex[MaxVertices];
            vertexData[0] = Vertex.Cannon;

            var vertexBufferSource = Context.Create.Buffer(BufferTarget.TransformFeedbackBuffer, Vertex.SizeInBytes * MaxVertices, BufferUsageHint.StaticDraw, vertexData);
            var vertexBufferTarget = Context.Create.Buffer(BufferTarget.TransformFeedbackBuffer, Vertex.SizeInBytes * MaxVertices, BufferUsageHint.StaticDraw);

            vertexArraySource = Context.Create.VertexArray();
            vertexArraySource.SetVertexAttributeF(0, vertexBufferSource, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);
            vertexArraySource.SetVertexAttributeF(1, vertexBufferSource, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 2 * sizeof(float));
            vertexArraySource.SetVertexAttributeF(2, vertexBufferSource, VertexAttributeDimension.One, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 4 * sizeof(float));
            vertexArraySource.SetVertexAttributeI(3, vertexBufferSource, VertexAttributeDimension.One, VertexAttribIPointerType.Int, Vertex.SizeInBytes, 5 * sizeof(float));
            vertexArraySource.SetVertexAttributeF(4, vertexBufferSource, VertexAttributeDimension.Three, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 6 * sizeof(float));

            vertexArrayTarget = Context.Create.VertexArray();
            vertexArrayTarget.SetVertexAttributeF(0, vertexBufferTarget, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);
            vertexArrayTarget.SetVertexAttributeF(1, vertexBufferTarget, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 2 * sizeof(float));
            vertexArrayTarget.SetVertexAttributeF(2, vertexBufferTarget, VertexAttributeDimension.One, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 4 * sizeof(float));
            vertexArrayTarget.SetVertexAttributeI(3, vertexBufferTarget, VertexAttributeDimension.One, VertexAttribIPointerType.Int, Vertex.SizeInBytes, 5 * sizeof(float));
            vertexArrayTarget.SetVertexAttributeF(4, vertexBufferTarget, VertexAttributeDimension.Three, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 6 * sizeof(float));

            transformFeedbackSource = Context.Create.TransformFeedback();
            transformFeedbackSource.SetBuffer(0, vertexBufferSource);

            transformFeedbackTarget = Context.Create.TransformFeedback();
            transformFeedbackTarget.SetBuffer(0, vertexBufferTarget);

            timeBuffer = Context.Create.Buffer(BufferTarget.UniformBuffer, 2 * sizeof(float), BufferUsageHint.DynamicDraw);

            IVertexShader vsh = Context.Create.VertexShader(VertexShaderText);
            IGeometryShader gsh = Context.Create.GeometryShader(GeometryShaderText);
            IFragmentShader fsh = Context.Create.FragmentShader(FragmentShaderText);
            program = Context.Create.Program(new ShaderProgramDescription
            {
                VertexShaders = new[] {vsh},
                GeometryShaders = new[] {gsh},
                FragmentShaders = new[] {fsh},
                VertexAttributeNames = new[] {"in_position", "in_velocity", "in_timeToAction", "in_type", "in_color"},
                TransformFeedbackAttributeNames = new[] {"g_position", "g_velocity", "g_timeToAction", "g_type", "g_color"},
                TransformFeedbackMode = TransformFeedbackMode.InterleavedAttribs,
                UniformBufferNames = new[] {"Time"}
            });
        }

        private bool firstTime = true;

        public unsafe override void OnNewFrame(float totalSeconds, float elapsedSeconds)
        {
            var time = new Vector2(totalSeconds, elapsedSeconds);
            timeBuffer.Recreate(BufferTarget.UniformBuffer, (IntPtr)(&time));

            Context.ClearWindowColor(new Color4(0, 0, 0, 1));
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

        private static void Swap<T>(ref T o1, ref T o2)
        {
            var temp = o2;
            o2 = o1;
            o1 = temp;
        }
    }
}
