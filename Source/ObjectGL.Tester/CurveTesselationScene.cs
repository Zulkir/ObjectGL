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
    class CurveTesselationScene : Scene
    {
        struct Vertex
        {
            public const int SizeInBytes = 2 * sizeof(float);

            public Vector2 Position;

            public Vertex(float x, float y)
            {
                Position = new Vector2(x, y);
            }
        }

        const string VertexShaderText =
@"#version 400

in vec2 in_position;

void main()
{
    gl_Position = vec4(in_position, 0.0, 1.0);
}
";

        const string TessControlShaderText =
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

        const string TessEvaluationShaderText =
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

        const string FragmentShaderText =
@"#version 400

out vec4 out_color;

void main()
{
    out_color = vec4(1.0, 1.0, 0.0, 1.0);
}
";

        public CurveTesselationScene(Context context, GameWindow gameWindow) : base(context, gameWindow) {}

        ShaderProgram program;
        VertexArray vertexArray;
        Buffer tessFactorBuffer;

        public override void Initialize()
        {
            var vertexBuffer = new Buffer(Context, BufferTarget.ArrayBuffer, 12 * Vertex.SizeInBytes, BufferUsageHint.StaticDraw, new Data(new Vertex[]
            {
                new Vertex(-1f, 1/3f), new Vertex(4f, 1f), new Vertex(-4f, 1f), new Vertex(1f, 1/3f),    
                new Vertex(-1f, -1/3f), new Vertex(4f, 1/3f), new Vertex(-4f, 1/3f), new Vertex(1f, -1/3f),    
                new Vertex(-1f, -1f), new Vertex(4f, -1/3f), new Vertex(-4f, -1/3f), new Vertex(1f, -1f)    
            }));
            
            vertexArray = new VertexArray(Context);
            vertexArray.SetVertexAttributeF(Context, 0, vertexBuffer, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);

            tessFactorBuffer = new Buffer(Context, BufferTarget.UniformBuffer, sizeof(int), BufferUsageHint.DynamicDraw);

            string shaderErrors;

            VertexShader vsh;
            TesselationControlShader tcsh;
            TesselationEvaluationShader tesh;
            FragmentShader fsh;

            if (!VertexShader.TryCompile(VertexShaderText, out vsh, out shaderErrors) ||
                !TesselationControlShader.TryCompile(TessControlShaderText, out tcsh, out shaderErrors) ||
                !TesselationEvaluationShader.TryCompile(TessEvaluationShaderText, out tesh, out shaderErrors) ||
                !FragmentShader.TryCompile(FragmentShaderText, out fsh, out shaderErrors) ||
                !ShaderProgram.TryLink(Context, new ShaderProgramDescription
                {
                    VertexShaders = vsh,
                    TesselationControlShaders = tcsh,
                    TesselationEvaluationShaders = tesh,
                    FragmentShaders = fsh,
                    VertexAttributeNames = new[] { "in_position" },
                    UniformBufferNames = new[] { "TessFactor" }
                },
                out program, out shaderErrors))
                throw new ArgumentException("Program errors:\n\n" + shaderErrors);
        }

        public unsafe override void OnNewFrame(float totalSeconds, float elapsedSeconds)
        {
            Context.ClearWindowColor(Color4.DarkViolet);

            Context.Pipeline.Program = program;
            Context.Pipeline.VertexArray = vertexArray;
            Context.Pipeline.PatchVertexCount = 4;
            Context.Pipeline.UniformBuffers[0] = tessFactorBuffer;

            int tessFactor = 4;
            tessFactorBuffer.SetData(Context, BufferTarget.UniformBuffer, (IntPtr)(&tessFactor));
            Context.DrawArrays(BeginMode.Patches, 0, 4);

            tessFactor = 64;
            tessFactorBuffer.SetData(Context, BufferTarget.UniformBuffer, (IntPtr)(&tessFactor));
            Context.DrawArrays(BeginMode.Patches, 4, 4);
            
            tessFactor = (int)(32.0 * (Math.Sin((totalSeconds - Math.PI) / 2.0) + 1.0)) + 1;
            tessFactorBuffer.SetData(Context, BufferTarget.UniformBuffer, (IntPtr)(&tessFactor));
            Context.DrawArrays(BeginMode.Patches, 8, 4);
        }
    }
}
