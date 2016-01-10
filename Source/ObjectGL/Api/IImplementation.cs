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

namespace ObjectGL.Api
{
    public interface IImplementation
    {
        string Vendor { get; }
        string Renderer { get; }
        string Version { get; }
        string ShadingLanguageVersion { get; }
        int MajorVersion { get; }
        int MinorVersion { get; }

        int MaxPatchVertices { get; }
        int MaxVertexAttributes { get; }
        int MaxCombinedTextureImageUnits { get; }
        int MaxUniformBufferBindings { get; }
        int MaxTransformFeedbackBuffers { get; }
        int MaxDrawBuffers { get; }
        int MaxColorAttachments { get; }

        int MaxTransformFeedbackInterleavedComponents { get; }
        int MaxTransformFeedbackSeparateComponents { get; }
        int MaxTransformFeedbackSeparateAttribs { get; }

        int MaxViewports { get; }
        float ViewportBoundsRange { get; }
        float MaxViewportDims { get; }

        int MaxSampleMaskWords { get; }

        int MaxTextureBufferSize { get; }
    }
}