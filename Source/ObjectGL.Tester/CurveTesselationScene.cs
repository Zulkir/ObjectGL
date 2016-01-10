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
    public class CurveTesselationScene : Scene
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Vertex
        {
            public const int SizeInBytes = 2 * sizeof(float);

            public Vector2 Position;

            public Vertex(float x, float y)
            {
                Position = new Vector2(x, y);
            }
        }

        private const string VertexShaderText =
@"#version 400

in vec2 in_position;

void main()
{
    gl_Position = vec4(in_position, 0.0, 1.0);
}
";

        private const string TessControlShaderText =
@"#version 400

layout (vertices = 4) out;

layout(std140) uniform TessFactor
{
    int NumSegments;
};

void main()
{
    gl_out[gl_InvocationID].gl_Position = gl_in[gl_InvocationID].gl_Position;
    gl_TessLevelOuter[0] = float(NumSegments);
    gl_TessLevelOuter[1] = 1.0;
}
";

        private const string TessEvaluationShaderText =
@"#version 400

layout (isolines) in;

void main()
{
    float u = gl_TessCoord.x;
    
    vec3 p0 = gl_in[0].gl_Position.xyz;
    vec3 p1 = gl_in[1].gl_Position.xyz;
    vec3 p2 = gl_in[2].gl_Position.xyz;
    vec3 p3 = gl_in[3].gl_Position.xyz;
    
    float u1 = 1.0 - u;
    float u2 = u * u;

    float b3 = u2 * u;
    float b2 = 3.0 * u2 * u1;
    float b1 = 3.0 * u * u1 * u1;
    float b0 = u1 * u1 * u1;

    gl_Position = vec4(p0 * b0 + p1 * b1 + p2 * b2 + p3 * b3, 1.0);
}
";

        private const string FragmentShaderText =
@"#version 400

out vec4 out_color;

void main()
{
    out_color = vec4(1.0, 1.0, 0.0, 1.0);
}
";

        public CurveTesselationScene(IContext context, GameWindow gameWindow) : base(context, gameWindow) {}

        private IShaderProgram program;
        private IVertexArray vertexArray;
        private IBuffer tessFactorBuffer;

        public override void Initialize()
        {
            var vertexBuffer = Context.Create.Buffer(BufferTarget.Array, 12 * Vertex.SizeInBytes, BufferUsageHint.StaticDraw, new Vertex[]
            {
                new Vertex(-1f, 1/3f), new Vertex(4f, 1f), new Vertex(-4f, 1f), new Vertex(1f, 1/3f),    
                new Vertex(-1f, -1/3f), new Vertex(4f, 1/3f), new Vertex(-4f, 1/3f), new Vertex(1f, -1/3f),    
                new Vertex(-1f, -1f), new Vertex(4f, -1/3f), new Vertex(-4f, -1/3f), new Vertex(1f, -1f)    
            });
            
            vertexArray = Context.Create.VertexArray();
            vertexArray.SetVertexAttributeF(0, vertexBuffer, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);

            tessFactorBuffer = Context.Create.Buffer(BufferTarget.Uniform, sizeof(int), BufferUsageHint.DynamicDraw);

            var vsh = Context.Create.VertexShader(VertexShaderText);
            var tcsh = Context.Create.TesselationControlShader(TessControlShaderText);
            var tesh = Context.Create.TesselationEvaluationShader(TessEvaluationShaderText);
            var fsh = Context.Create.FragmentShader(FragmentShaderText);
            program = Context.Create.Program(new ShaderProgramDescription
            {
                VertexShaders = new[] {vsh},
                TesselationControlShaders = new[] {tcsh},
                TesselationEvaluationShaders = new[] {tesh},
                FragmentShaders = new[] {fsh},
                VertexAttributeNames = new[] {"in_position"},
                UniformBufferNames = new[] {"TessFactor"}
            });
        }

        public unsafe override void OnNewFrame(float totalSeconds, float elapsedSeconds)
        {
            Context.Actions.ClearWindowColor(new Color4(0.5f, 0.0f, 0.75f, 1.0f));

            Context.States.ScreenClipping.United.Viewport.Set(GameWindow.ClientSize.Width, GameWindow.ClientSize.Height);
            Context.States.PatchVertexCount.Set(4);

            Context.Bindings.Program.Set(program);
            Context.Bindings.VertexArray.Set(vertexArray);
            Context.Bindings.Buffers.UniformIndexed[0].Set(tessFactorBuffer);

            int tessFactor = 4;
            tessFactorBuffer.SetDataByMapping((IntPtr)(&tessFactor));
            Context.Actions.Draw.Arrays(BeginMode.Patches, 0, 4);

            tessFactor = 64;
            tessFactorBuffer.SetDataByMapping((IntPtr)(&tessFactor));
            Context.Actions.Draw.Arrays(BeginMode.Patches, 4, 4);
            
            tessFactor = (int)(32.0 * (Math.Sin((totalSeconds - Math.PI) / 2.0) + 1.0)) + 1;
            tessFactorBuffer.SetDataByMapping((IntPtr)(&tessFactor));
            Context.Actions.Draw.Arrays(BeginMode.Patches, 8, 4);
        }
    }
}
