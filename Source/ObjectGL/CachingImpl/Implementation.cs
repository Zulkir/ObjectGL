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

using ObjectGL.Api;

namespace ObjectGL.CachingImpl
{
    internal class Implementation : IImplementation
    {
        public string Vendor { get; private set; }
        public string Renderer { get; private set; }
        public string Version { get; private set; }
        public string ShadingLanguageVersion { get; private set; }
        public int MajorVersion { get; private set; }
        public int MinorVersion { get; private set; }

        public int MaxPatchVertices { get; private set; }
        public int MaxVertexAttributes { get; private set; }
        public int MaxCombinedTextureImageUnits { get; private set; }
        public int MaxUniformBufferBindings { get; private set; }
        public int MaxTransformFeedbackBuffers { get; private set; }
        public int MaxDrawBuffers { get; private set; }
        public int MaxColorAttachments { get; private set; }

        public int MaxTransformFeedbackInterleavedComponents { get; private set; }
        public int MaxTransformFeedbackSeparateComponents { get; private set; }
        public int MaxTransformFeedbackSeparateAttribs { get; private set; }

        public int MaxViewports { get; private set; }
        public float ViewportBoundsRange { get; private set; }
        public float MaxViewportDims { get; private set; }

        public int MaxTextureBufferSize { get; private set; }

        internal unsafe Implementation(IGL gl)
        {
            int localInt;
            float localFloat;

            Vendor = gl.GetString((int)All.Vendor);
            Renderer = gl.GetString((int)All.Renderer);
            Version = gl.GetString((int)All.Version);
            ShadingLanguageVersion = gl.GetString((int)All.ShadingLanguageVersion);

            gl.GetInteger((int)All.MajorVersion, &localInt);
            MajorVersion = localInt;

            gl.GetInteger((int)All.MinorVersion, &localInt);
            MinorVersion = localInt;

            gl.GetInteger((int)All.MaxPatchVertices, &localInt);
            MaxPatchVertices = localInt;

            gl.GetInteger((int)All.MaxVertexAttribs, &localInt);
            MaxVertexAttributes = localInt;

            gl.GetInteger((int)All.MaxCombinedTextureImageUnits, &localInt);
            MaxCombinedTextureImageUnits = localInt;

            gl.GetInteger((int)All.MaxUniformBufferBindings, &localInt);
            MaxUniformBufferBindings = localInt;

            gl.GetInteger((int)All.MaxTransformFeedbackBuffers, &localInt);
            MaxTransformFeedbackBuffers = localInt;

            gl.GetInteger((int)All.MaxDrawBuffers, &localInt);
            MaxDrawBuffers = localInt;

            gl.GetInteger((int)All.MaxColorAttachments, &localInt);
            MaxColorAttachments = localInt;

            gl.GetInteger((int)All.MaxTransformFeedbackInterleavedComponents, &localInt);
            MaxTransformFeedbackInterleavedComponents = localInt;

            gl.GetInteger((int)All.MaxTransformFeedbackSeparateComponents, &localInt);
            MaxTransformFeedbackSeparateComponents = localInt;

            gl.GetInteger((int)All.MaxTransformFeedbackSeparateAttribs, &localInt);
            MaxTransformFeedbackSeparateAttribs = localInt;

            gl.GetInteger((int)All.MaxViewports, &localInt);
            MaxViewports = localInt;

            gl.GetFloat((int)All.ViewportBoundsRange, &localFloat);
            ViewportBoundsRange = localFloat;

            gl.GetFloat((int)All.MaxViewportDims, &localFloat);
            MaxViewportDims = localFloat;

            gl.GetInteger((int)All.MaxTextureBufferSize, &localInt);
            MaxTextureBufferSize = localInt;
        }
    }
}
