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
using ObjectGL.Api;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;
using ObjectGL.CachingImpl.Objects;
using ObjectGL.CachingImpl.Objects.Resources;
using Buffer = ObjectGL.CachingImpl.Objects.Resources.Buffer;

namespace ObjectGL.CachingImpl
{
    internal class ContextObjectFactory : IContextObjectFactory
    {
        private readonly Context context;

        private IGL GL { get { return context.GL; } }

        public ContextObjectFactory(Context context)
        {
            this.context = context;
        }

        public IFramebuffer Framebuffer()
        {
            return new Framebuffer(context);
        }

        public ISampler Sampler()
        {
            return new Sampler(context);
        }

        public IVertexArray VertexArray()
        {
            return new VertexArray(context);
        }

        public IVertexShader VertexShader(string source)
        {
            VertexShader result;
            string errors;
            if (!Objects.VertexShader.TryCompile(GL, source, out result, out errors))
                throw new ShaderException("Failed to compile a vertex shader:\r\n\r\n" + errors);
            return result;
        }

        public ITesselationControlShader TesselationControlShader(string source)
        {
            TesselationControlShader result;
            string errors;
            if (!Objects.TesselationControlShader.TryCompile(GL, source, out result, out errors))
                throw new ShaderException("Failed to compile a tesselation control shader:\r\n\r\n" + errors);
            return result;
        }

        public ITesselationEvaluationShader TesselationEvaluationShader(string source)
        {
            TesselationEvaluationShader result;
            string errors;
            if (!Objects.TesselationEvaluationShader.TryCompile(GL, source, out result, out errors))
                throw new ShaderException("Failed to compile a tesselation evaluation shader:\r\n\r\n" + errors);
            return result;
        }

        public IGeometryShader GeometryShader(string source)
        {
            GeometryShader result;
            string errors;
            if (!Objects.GeometryShader.TryCompile(GL, source, out result, out errors))
                throw new ShaderException("Failed to compile a geometry shader:\r\n\r\n" + errors);
            return result;
        }

        public IFragmentShader FragmentShader(string source)
        {
            FragmentShader result;
            string errors;
            if (!Objects.FragmentShader.TryCompile(GL, source, out result, out errors))
                throw new ShaderException("Failed to compile a fragment shader:\r\n\r\n" + errors);
            return result;
        }

        public IShaderProgram Program(ShaderProgramDescription description)
        {
            ShaderProgram result;
            string errors;
            if (!ShaderProgram.TryLink(context, description, out result, out errors))
                throw new ShaderException("Failed to link a shader prgram:\r\n\r\n" + errors);
            return result;
        }

        public ITransformFeedback TransformFeedback()
        {
            return new TransformFeedback(context);
        }

        public IBuffer Buffer(BufferTarget creationTarget, int sizeInBytes, BufferUsageHint usage, IntPtr initialData = default(IntPtr))
        {
            return new Buffer(context, creationTarget, sizeInBytes, usage, initialData);
        }

        public IRenderbuffer Renderbuffer(int width, int height, Format internalFormat, int samples)
        {
            return new Renderbuffer(context, width, height, internalFormat, samples);
        }

        public ITexture1D Texture1D(int width, int mipCount, Format internalFormat)
        {
            return new Texture1D(context, width, mipCount, internalFormat);
        }

        public ITexture1DArray Texture1DArray(int width, int sliceCount, int mipCount, Format internalFormat)
        {
            return new Texture1DArray(context, width, sliceCount, mipCount, internalFormat);
        }

        public ITexture2D Texture2D(int width, int height, int mipCount, Format internalFormat)
        {
            return new Texture2D(context, width, height, mipCount, internalFormat);
        }

        public ITexture2DArray Texture2DArray(int width, int height, int sliceCount, int mipCount, Format internalFormat)
        {
            return new Texture2DArray(context, width, height, sliceCount, mipCount, internalFormat);
        }

        public ITexture2DMultisample Texture2DMultisample(int width, int height, int samples, Format internalFormat, bool fixedSampleLocations = false)
        {
            return new Texture2DMultisample(context, width, height, samples, internalFormat, fixedSampleLocations);
        }

        public ITexture2DMultisampleArray Texture2DMultisampleArray(int width, int height, int sliceCount, int samples, Format internalFormat, bool fixedSampleLocations = false)
        {
            return new Texture2DMultisampleArray(context, width, height, sliceCount, samples, internalFormat, fixedSampleLocations);
        }

        public ITexture3D Texture3D(int width, int height, int depth, int mipCount, Format internalFormat)
        {
            return new Texture3D(context, width, height, depth, mipCount, internalFormat);
        }

        public ITextureCubemap TextureCubemap(int width, int mipCount, Format internalFormat)
        {
            return new TextureCubemap(context, width, mipCount, internalFormat);
        }

        public ITextureCubemapArray TextureCubemapArray(int width, int cubeCount, int mipCount, Format internalFormat)
        {
            return new TextureCubemapArray(context, width, cubeCount, mipCount, internalFormat);
        }
    }
}