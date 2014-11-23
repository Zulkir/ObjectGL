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

namespace ObjectGL
{
    public unsafe interface IGL
    {
        void ActiveTexture(int texture);
        void AttachShader(uint program, uint shader);
        void BeginTransformFeedback(int primitiveMode);
        void BindAttribLocation(uint program, uint index, string name);
        void BindBuffer(int target, uint buffer);
        void BindBufferBase(int target, uint index, uint buffer);
        void BindBufferRange(int target, uint index, uint buffer, IntPtr offset, IntPtr size);
        void BindFramebuffer(int target, uint framebuffer);
        void BindRenderbuffer(int target, uint renderbuffer);
        void BindSampler(uint unit, uint sampler);
        void BindTexture(int target, uint texture);
        void BindTransformFeedback(int target, uint id);
        void BindVertexArray(uint array);
        void BlendColor(float red, float green, float blue, float alpha);
        void BlendEquation(int mode);
        void BlendEquation(uint buf, int mode);
        void BlendEquationSeparate(int modeRGB, int modeAlpha);
        void BlendEquationSeparate(uint buf, int modeRGB, int modeAlpha);
        void BlendFunc(int sfactor, int dfactor);
        void BlendFunc(uint buf, int sfactor, int dfactor);
        void BlendFuncSeparate(int srcRGB, int dstRGB, int srcAlpha, int dstAlpha);
        void BlendFuncSeparate(uint buf, int srcRGB, int dstRGB, int srcAlpha, int dstAlpha);
        void BlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, int filter);
        void BufferData(int target, IntPtr size, IntPtr data, int usage);
        void BufferSubData(int target, IntPtr offset, IntPtr size, IntPtr data);
        void ClearBuffer(int buffer, int drawBuffer, int* value);
        void ClearBuffer(int buffer, int drawBuffer, uint* value);
        void ClearBuffer(int buffer, int drawBuffer, float* value);
        void ClearBuffer(int buffer, int drawBuffer, float depth, int stencil);
        void CompileShader(uint shader);
        void CompressedTexImage1D(int target, int level, int internalformat, int width, int border, int imageSize, IntPtr data);
        void CompressedTexImage2D(int target, int level, int internalformat, int width, int height, int border, int imageSize, IntPtr data);
        void CompressedTexImage3D(int target, int level, int internalFormat, int width, int height, int depth, int border, int imageSize, IntPtr data);
        void CompressedTexSubImage1D(int target, int level, int xoffset, int width, int format, int imageSize, IntPtr data);
        void CompressedTexSubImage2D(int target, int level, int xoffset, int yoffset, int width, int height, int format, int imageSize, IntPtr data);
        void CompressedTexSubImage3D(int target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int imageSize, IntPtr data);
        uint CreateProgram();
        uint CreateShader(int shaderType);
        void CullFace(int mode);
        void DeleteBuffers(int n, uint* buffers);
        void DeleteFramebuffers(int n, uint* framebuffers);
        void DeleteProgram(uint program);
        void DeleteRenderbuffers(int n, uint* renderbuffers);
        void DeleteSamplers(int n, uint* samplers);
        void DeleteShader(uint shader);
        void DeleteTextures(int n, uint* textures);
        void DeleteTransformFeedbacks(int n, uint* ids);
        void DeleteVertexArrays(int n, uint* arrays);
        void DepthFunc(int func);
        void DepthMask(bool flag);
        //void DepthRange(double nearVal, double farVal);
        void DepthRange(float nearVal, float farVal);
        void DepthRangeIndexed(uint index, double nearVal, double farVal);
        void Disable(int cap);
        //void Disable(int cap, uint index);
        void DisableVertexAttribArray(uint index);
        void DrawArrays(int mode, int first, int count);
        void DrawArraysIndirect(int mode, IntPtr indirect);
        void DrawArraysInstanced(int mode, int first, int count, int primcount);
        void DrawArraysInstancedBaseInstance(int mode, int first, int count, int primcount, uint baseinstance);
        void DrawElements(int mode, int count, int type, IntPtr indices);
        void DrawElementsBaseVertex(int mode, int count, int type, IntPtr indices, int basevertex);
        void DrawElementsIndirect(int mode, int type, IntPtr indirect);
        void DrawElementsInstanced(int mode, int count, int type, IntPtr indices, int primcount);
        void DrawElementsInstancedBaseInstance(int mode, int count, int type, IntPtr indices, int primcount, uint baseinstance);
        void DrawElementsInstancedBaseVertex(int mode, int count, int type, IntPtr indices, int primcount, int basevertex);
        void DrawElementsInstancedBaseVertexBaseInstance(int mode, int count, int type, IntPtr indices, int primcount, int basevertex, uint baseinstance);
        void DrawRangeElements(int mode, uint start, uint end, int count, int type, IntPtr indices);
        void DrawRangeElementsBaseVertex(int mode, uint start, uint end, int count, int type, IntPtr indices, int basevertex);
        void DrawTransformFeedback(int mode, uint id);
        void DrawTransformFeedbackInstanced(int mode, uint id, int primcount);
        void DrawTransformFeedbackStream(int mode, uint id, uint stream);
        void DrawTransformFeedbackStreamInstanced(int mode, uint id, uint stream, int primcount);
        void Enable(int cap);
        //void Enable(int cap, uint index);
        void EnableVertexAttribArray(uint index);
        void EndTransformFeedback();
        void FramebufferRenderbuffer(int target, int attachment, int renderbuffertarget, uint renderbuffer);
        //void FramebufferTexture(int target, int attachment, uint texture, int level);
        //void FramebufferTexture1D(int target, int attachment, int textarget, uint texture, int level);
        void FramebufferTexture2D(int target, int attachment, int textarget, uint texture, int level);
        //void FramebufferTexture3D(int target, int attachment, int textarget, uint texture, int level, int layer);
        void FramebufferTextureLayer(int target, int attachment, uint texture, int level, int layer);
        void FrontFace(int mode);
        void GenBuffers(int n, uint* buffers);
        void GenerateMipmap(int target);
        void GenFramebuffers(int n, uint* framebuffers);
        void GenRenderbuffers(int n, uint* renderbuffers);
        void GenSamplers(int n, uint* samplers);
        void GenTextures(int n, uint* textures);
        void GenTransformFeedbacks(int n, uint* ids);
        void GenVertexArrays(int n, uint* arrays);
        int GetAttribLocation(uint program, string name);
        //void GetBoolean(int pname, bool* data);
        //void GetBoolean(int pname, uint index, bool* data);
        //void GetDouble(int pname, double* data);
        //void GetDouble(int pname, uint index, double* data);
        void GetFloat(int pname, float* data);
        //void GetFloat(int pname, uint index, float* data);
        void GetInteger(int pname, int* data);
        //void GetInteger(int pname, uint index, int* data);
        //void GetInteger64(int pname, long* data);
        //void GetInteger64(int pname, uint index, long* data);
        void GetProgram(uint program, int pname, int* parameters);
        string GetProgramInfoLog(uint program);
        void GetShader(uint shader, int pname, int* parameters);
        string GetShaderInfoLog(uint shader);
        string GetString(int name);
        //string GetString(int name, uint index);
        uint GetUniformBlockIndex(uint program, string uniformBlockName);
        int GetUniformLocation(uint program, string name);
        void LinkProgram(uint program);
        IntPtr MapBufferRange(int target, IntPtr offset, IntPtr length, int access);
        void PatchParameter(int pname, int value);
        //void PatchParameter(int pname, float* values);
        //void PixelStore(int pname, float param);
        void PixelStore(int pname, int param);
        void PolygonMode(int face, int mode);
        void RenderbufferStorage(int target, int internalformat, int width, int height);
        void RenderbufferStorageMultisample(int target, int samples, int internalformat, int width, int height);
        void SampleMask(uint maskNumber, uint mask);
        void SamplerParameter(uint sampler, int pname, float param);
        void SamplerParameter(uint sampler, int pname, int param);
        void SamplerParameter(uint sampler, int pname, float* parameters);
        //void SamplerParameter(uint sampler, int pname, int* parameters);
        //void SamplerParameterI(uint sampler, int pname, int* parameters);
        //void SamplerParameterI(uint sampler, int pname, uint* parameters);
        void Scissor(int x, int y, int width, int height);
        void ScissorIndexed(uint index, int left, int bottom, int width, int height);
        //void ScissorIndexed(uint index, int* v);
        void ShaderSource(uint shader, string strings);
        //void ShaderSource(uint shader, int count, string[] strings);
        void StencilFuncSeparate(int face, int func, int reference, uint mask);
        void StencilMaskSeparate(int face, uint mask);
        void StencilOpSeparate(int face, int sfail, int dpfail, int dppass);
        void TexImage1D(int target, int level, int internalFormat, int width, int border, int format, int type, IntPtr data);
        void TexImage2D(int target, int level, int internalFormat, int width, int height, int border, int format, int type, IntPtr data);
        void TexImage3D(int target, int level, int internalFormat, int width, int height, int depth, int border, int format, int type, IntPtr data);
        void TexParameter(int target, int pname, float param);
        void TexParameter(int target, int pname, int param);
        //void TexParameter(int target, int pname, float* parameters);
        //void TexParameter(int target, int pname, int* parameters);
        //void TexParameterI(int target, int pname, int* parameters);
        //void TexParameterI(int target, int pname, uint* parameters);
        void TexStorage1D(int target, int levels, int internalformat, int width);
        void TexStorage2D(int target, int levels, int internalformat, int width, int height);
        void TexStorage2DMultisample(int target, int samples, int internalformat, int width, int height, bool fixedsamplelocations);
        void TexStorage3D(int target, int levels, int internalformat, int width, int height, int depth);
        void TexStorage3DMultisample(int target, int samples, int internalformat, int width, int height, int depth, bool fixedsamplelocations);
        void TexSubImage1D(int target, int level, int xoffset, int width, int format, int type, IntPtr data);
        void TexSubImage2D(int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, IntPtr data);
        void TexSubImage3D(int target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, IntPtr data);
        void TransformFeedbackVaryings(uint program, int count, string[] varyings, int bufferMode);
        void Uniform(int location, int v0);
        void UniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding);
        bool UnmapBuffer(int target);
        void UseProgram(uint program);
        void VertexAttribPointer(uint index, int size, int type, bool normalized, int stride, IntPtr pointer);
        void VertexAttribIPointer(uint index, int size, int type, int stride, IntPtr pointer);
        //void VertexAttribLPointer(uint index, int size, int type, int stride, IntPtr pointer);
        void VertexAttribDivisor(uint index, uint divisor);
        void Viewport(int x, int y, int width, int height);
        void ViewportIndexed(uint index, float x, float y, float w, float h);
        //void ViewportIndexed(uint index, float* v);
    }
}
