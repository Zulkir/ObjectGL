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
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;

namespace ObjectGL.Api
{
    public interface IContextObjectFactory
    {
        IFramebuffer Framebuffer();
        ISampler Sampler();
        IVertexArray VertexArray();
        IVertexShader VertexShader(string source);
        ITesselationControlShader TesselationControlShader(string source);
        ITesselationEvaluationShader TesselationEvaluationShader(string source);
        IGeometryShader GeometryShader(string source);
        IFragmentShader FragmentShader(string source);
        IShaderProgram Program(ShaderProgramDescription description);
        ITransformFeedback TransformFeedback();
        IBuffer Buffer(BufferTarget creationTarget, int sizeInBytes, BufferUsageHint usage, IntPtr initialData = default(IntPtr));
        IRenderbuffer Renderbuffer(int width, int height, Format internalFormat, int samples = 0);
        ITexture1D Texture1D(int width, int mipCount, Format internalFormat);
        ITexture1DArray Texture1DArray(int width, int sliceCount, int mipCount, Format internalFormat);
        ITexture2D Texture2D(int width, int height, int mipCount, Format internalFormat);
        ITexture2DArray Texture2DArray(int width, int height, int sliceCount, int mipCount, Format internalFormat);
        ITexture2DMultisample Texture2DMultisample(int width, int height, int samples, Format internalFormat, bool fixedSampleLocations = false);
        ITexture2DMultisampleArray Texture2DMultisampleArray(int width, int height, int sliceCount, int samples, Format internalFormat, bool fixedSampleLocations = false);
        ITexture3D Texture3D(int width, int height, int depth, int mipCount, Format internalFormat);
        ITextureCubemap TextureCubemap(int width, int mipCount, Format internalFormat);
        ITextureCubemapArray TextureCubemapArray(int width, int cubeCount, int mipCount, Format internalFormat);
    }
}