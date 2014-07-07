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
using OpenTK.Graphics.OpenGL;

namespace ObjectGL.GL4
{
    public unsafe class GL42 : IGL
    {
        public void ActiveTexture(int texture)
        {
            GL.ActiveTexture((TextureUnit)texture);
        }

        public void AttachShader(uint program, uint shader)
        {
            GL.AttachShader(program, shader);
        }

        public void BeginTransformFeedback(int primitiveMode)
        {
            GL.BeginTransformFeedback((TransformFeedbackPrimitiveType)primitiveMode);
        }

        public void BindAttribLocation(uint program, uint index, string name)
        {
            GL.BindAttribLocation(program, index, name);
        }

        public void BindBuffer(int target, uint buffer)
        {
            GL.BindBuffer((BufferTarget)target, buffer);
        }

        public void BindBufferBase(int target, uint index, uint buffer)
        {
            GL.BindBufferBase((BufferRangeTarget)target, index, buffer);
        }

        public void BindFramebuffer(int target, uint framebuffer)
        {
            GL.BindFramebuffer((FramebufferTarget)target, framebuffer);
        }

        public void BindRenderbuffer(int target, uint renderbuffer)
        {
            GL.BindRenderbuffer((RenderbufferTarget)target, renderbuffer);
        }

        public void BindSampler(uint unit, uint sampler)
        {
            GL.BindSampler(unit, sampler);
        }

        public void BindTexture(int target, uint texture)
        {
            GL.BindTexture((TextureTarget)target, texture);
        }

        public void BindTransformFeedback(int target, uint id)
        {
            GL.BindTransformFeedback((TransformFeedbackTarget)target, id);
        }

        public void BindVertexArray(uint array)
        {
            GL.BindVertexArray(array);
        }

        public void BlendColor(float red, float green, float blue, float alpha)
        {
            GL.BlendColor(red, green, blue, alpha);
        }

        public void BlendEquation(int mode)
        {
            GL.BlendEquation((BlendEquationMode)mode);
        }

        public void BlendEquation(uint buf, int mode)
        {
            GL.BlendEquation(buf, (BlendEquationMode)mode);
        }

        public void BlendEquationSeparate(int modeRGB, int modeAlpha)
        {
            GL.BlendEquationSeparate((BlendEquationMode)modeRGB, (BlendEquationMode)modeAlpha);
        }

        public void BlendEquationSeparate(uint buf, int modeRGB, int modeAlpha)
        {
            GL.BlendEquationSeparate(buf, (BlendEquationMode)modeRGB, (BlendEquationMode)modeAlpha);
        }

        public void BlendFunc(int sfactor, int dfactor)
        {
            GL.BlendFunc((BlendingFactorSrc)sfactor, (BlendingFactorDest)dfactor);
        }

        public void BlendFunc(uint buf, int sfactor, int dfactor)
        {
            GL.BlendFunc(buf, (BlendingFactorSrc)sfactor, (BlendingFactorDest)dfactor);
        }

        public void BlendFuncSeparate(int srcRGB, int dstRGB, int srcAlpha, int dstAlpha)
        {
            GL.BlendFuncSeparate((BlendingFactorSrc)srcRGB, (BlendingFactorDest)dstRGB, (BlendingFactorSrc)srcAlpha, (BlendingFactorDest)dstAlpha);
        }

        public void BlendFuncSeparate(uint buf, int srcRGB, int dstRGB, int srcAlpha, int dstAlpha)
        {
            GL.BlendFuncSeparate(buf, (BlendingFactorSrc)srcRGB, (BlendingFactorDest)dstRGB, (BlendingFactorSrc)srcAlpha, (BlendingFactorDest)dstAlpha);
        }

        public void BlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, int filter)
        {
            GL.BlitFramebuffer(srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, (ClearBufferMask)mask, (BlitFramebufferFilter)filter);
        }

        public void BufferData(int target, IntPtr size, IntPtr data, int usage)
        {
            GL.BufferData((BufferTarget)target, size, data, (BufferUsageHint)usage);
        }

        public void ClearBuffer(int buffer, int drawBuffer, int* value)
        {
            GL.ClearBuffer((ClearBuffer)buffer, drawBuffer, value);
        }

        public void ClearBuffer(int buffer, int drawBuffer, uint* value)
        {
            GL.ClearBuffer((ClearBuffer)buffer, drawBuffer, value);
        }

        public void ClearBuffer(int buffer, int drawBuffer, float* value)
        {
            GL.ClearBuffer((ClearBuffer)buffer, drawBuffer, value);
        }

        public void ClearBuffer(int buffer, int drawBuffer, float depth, int stencil)
        {
            GL.ClearBuffer((ClearBufferCombined)buffer, drawBuffer, depth, stencil);
        }

        public void CompileShader(uint shader)
        {
            GL.CompileShader(shader);
        }

        public void CompressedTexImage1D(int target, int level, int internalformat, int width, int border, int imageSize, IntPtr data)
        {
            GL.CompressedTexImage1D((TextureTarget)target, level, (PixelInternalFormat)internalformat, width, border, imageSize, data);
        }

        public void CompressedTexImage2D(int target, int level, int internalformat, int width, int height, int border, int imageSize, IntPtr data)
        {
            GL.CompressedTexImage2D((TextureTarget)target, level, (PixelInternalFormat)internalformat, width, height, border, imageSize, data);
        }

        public void CompressedTexImage3D(int target, int level, int internalFormat, int width, int height, int depth, int border, int imageSize, IntPtr data)
        {
            GL.CompressedTexImage3D((TextureTarget)target, level, (PixelInternalFormat)internalFormat, width, height, depth, border, imageSize, data);
        }

        public void CompressedTexSubImage2D(int target, int level, int xoffset, int yoffset, int width, int height, int format, int imageSize, IntPtr data)
        {
            GL.CompressedTexSubImage2D((TextureTarget)target, level, xoffset, yoffset, width, height, (PixelFormat)format, imageSize, data);
        }

        public void CompressedTexSubImage3D(int target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int imageSize, IntPtr data)
        {
            GL.CompressedTexSubImage3D((TextureTarget)target, level, xoffset, yoffset, zoffset, width, height, depth, (PixelFormat)format, imageSize, data);
        }

        public uint CreateProgram()
        {
            return (uint)GL.CreateProgram();
        }

        public uint CreateShader(int shaderType)
        {
            return (uint)GL.CreateShader((ShaderType)shaderType);
        }

        public void CullFace(int mode)
        {
            GL.CullFace((CullFaceMode)mode);
        }

        public void DeleteBuffers(int n, uint* buffers)
        {
            GL.DeleteBuffers(n, buffers);
        }

        public void DeleteFramebuffers(int n, uint* framebuffers)
        {
            GL.DeleteFramebuffers(n, framebuffers);
        }

        public void DeleteProgram(uint program)
        {
            GL.DeleteProgram(program);
        }

        public void DeleteRenderbuffers(int n, uint* renderbuffers)
        {
            GL.DeleteRenderbuffers(n, renderbuffers);
        }

        public void DeleteSamplers(int n, uint* samplers)
        {
            GL.DeleteSamplers(n, samplers);
        }

        public void DeleteShader(uint shader)
        {
            GL.DeleteShader(shader);
        }

        public void DeleteTextures(int n, uint* textures)
        {
            GL.DeleteTextures(n, textures);
        }

        public void DeleteTransformFeedbacks(int n, uint* ids)
        {
            GL.DeleteTransformFeedbacks(n, ids);
        }

        public void DeleteVertexArrays(int n, uint* arrays)
        {
            GL.DeleteVertexArrays(n, arrays);
        }

        public void DepthFunc(int func)
        {
            GL.DepthFunc((DepthFunction)func);
        }

        public void DepthMask(bool flag)
        {
            GL.DepthMask(flag);
        }

        public void DepthRange(float nearVal, float farVal)
        {
            GL.DepthRange(nearVal, farVal);
        }

        public void DepthRangeIndexed(uint index, double nearVal, double farVal)
        {
            GL.DepthRangeIndexed(index, nearVal, farVal);
        }

        public void Disable(int cap)
        {
            GL.Disable((EnableCap)cap);
        }

        public void DisableVertexAttribArray(uint index)
        {
            GL.DisableVertexAttribArray(index);
        }

        public void DrawArrays(int mode, int first, int count)
        {
            GL.DrawArrays((PrimitiveType)mode, first, count);
        }

        public void DrawArraysIndirect(int mode, IntPtr indirect)
        {
            GL.DrawArraysIndirect((PrimitiveType)mode, indirect);
        }

        public void DrawArraysInstanced(int mode, int first, int count, int primcount)
        {
            GL.DrawArraysInstanced((PrimitiveType)mode, first, count, primcount);
        }

        public void DrawArraysInstancedBaseInstance(int mode, int first, int count, int primcount, uint baseinstance)
        {
            GL.DrawArraysInstancedBaseInstance((PrimitiveType)mode, first, count, primcount, baseinstance);
        }

        public void DrawElements(int mode, int count, int type, IntPtr indices)
        {
            GL.DrawElements((PrimitiveType)mode, count, (DrawElementsType)type, indices);
        }

        public void DrawElementsBaseVertex(int mode, int count, int type, IntPtr indices, int basevertex)
        {
            GL.DrawElementsBaseVertex((PrimitiveType)mode, count, (DrawElementsType)type, indices, basevertex);
        }

        public void DrawElementsIndirect(int mode, int type, IntPtr indirect)
        {
            GL.DrawElementsIndirect((PrimitiveType)mode, (All)type, indirect);
        }

        public void DrawElementsInstanced(int mode, int count, int type, IntPtr indices, int primcount)
        {
            GL.DrawElementsInstanced((PrimitiveType)mode, count, (DrawElementsType)type, indices, primcount);
        }

        public void DrawElementsInstancedBaseInstance(int mode, int count, int type, IntPtr indices, int primcount, uint baseinstance)
        {
            GL.DrawElementsInstancedBaseInstance((PrimitiveType)mode, count, (DrawElementsType)type, indices, primcount, baseinstance);
        }

        public void DrawElementsInstancedBaseVertex(int mode, int count, int type, IntPtr indices, int primcount, int basevertex)
        {
            GL.DrawElementsInstancedBaseVertex((PrimitiveType)mode, count, (DrawElementsType)type, indices, primcount, basevertex);
        }

        public void DrawElementsInstancedBaseVertexBaseInstance(int mode, int count, int type, IntPtr indices, int primcount, int basevertex, uint baseinstance)
        {
            GL.DrawElementsInstancedBaseVertexBaseInstance((PrimitiveType)mode, count, (DrawElementsType)type, indices, primcount, basevertex, baseinstance);
        }

        public void DrawRangeElements(int mode, uint start, uint end, int count, int type, IntPtr indices)
        {
            GL.DrawRangeElements((PrimitiveType)mode, start, end, count, (DrawElementsType)type, indices);
        }

        public void DrawRangeElementsBaseVertex(int mode, uint start, uint end, int count, int type, IntPtr indices, int basevertex)
        {
            GL.DrawRangeElementsBaseVertex((PrimitiveType)mode, start, end, count, (DrawElementsType)type, indices, basevertex);
        }

        public void DrawTransformFeedback(int mode, uint id)
        {
            GL.DrawTransformFeedback((PrimitiveType)mode, id);
        }

        public void DrawTransformFeedbackInstanced(int mode, uint id, int primcount)
        {
            GL.DrawTransformFeedbackInstanced((PrimitiveType)mode, id, primcount);
        }

        public void DrawTransformFeedbackStream(int mode, uint id, uint stream)
        {
            GL.DrawTransformFeedbackStream((PrimitiveType)mode, id, stream);
        }

        public void DrawTransformFeedbackStreamInstanced(int mode, uint id, uint stream, int primcount)
        {
            GL.DrawTransformFeedbackStreamInstanced((PrimitiveType)mode, id, stream, primcount);
        }

        public void Enable(int cap)
        {
            GL.Enable((EnableCap)cap);
        }

        public void EnableVertexAttribArray(uint index)
        {
            GL.EnableVertexAttribArray(index);
        }

        public void EndTransformFeedback()
        {
            GL.EndTransformFeedback();
        }

        public void FramebufferRenderbuffer(int target, int attachment, int renderbuffertarget, uint renderbuffer)
        {
            GL.FramebufferRenderbuffer((FramebufferTarget)target, (FramebufferAttachment)attachment, (RenderbufferTarget)renderbuffertarget, renderbuffer);
        }

        public void FramebufferTexture2D(int target, int attachment, int textarget, uint texture, int level)
        {
            GL.FramebufferTexture2D((FramebufferTarget)target, (FramebufferAttachment)attachment, (TextureTarget)textarget, texture, level);
        }

        public void FramebufferTextureLayer(int target, int attachment, uint texture, int level, int layer)
        {
            GL.FramebufferTextureLayer((FramebufferTarget)target, (FramebufferAttachment)attachment, texture, level, layer);
        }

        public void FrontFace(int mode)
        {
            GL.FrontFace((FrontFaceDirection)mode);
        }

        public void GenBuffers(int n, uint* buffers)
        {
            GL.GenBuffers(n, buffers);
        }

        public void GenerateMipmap(int target)
        {
            GL.GenerateMipmap((GenerateMipmapTarget)target);
        }

        public void GenFramebuffers(int n, uint* framebuffers)
        {
            GL.GenFramebuffers(n, framebuffers);
        }

        public void GenRenderbuffers(int n, uint* renderbuffers)
        {
            GL.GenRenderbuffers(n, renderbuffers);
        }

        public void GenSamplers(int n, uint* samplers)
        {
            GL.GenSamplers(n, samplers);
        }

        public void GenTextures(int n, uint* textures)
        {
            GL.GenTextures(n, textures);
        }

        public void GenTransformFeedbacks(int n, uint* ids)
        {
            GL.GenTransformFeedbacks(n, ids);
        }

        public void GenVertexArrays(int n, uint* arrays)
        {
            GL.GenVertexArrays(n, arrays);
        }

        public int GetAttribLocation(uint program, string name)
        {
            return GL.GetAttribLocation(program, name);
        }

        public void GetFloat(int pname, float* data)
        {
            GL.GetFloat((GetPName)pname, data);
        }

        public void GetInteger(int pname, int* data)
        {
            GL.GetInteger((GetPName)pname, data);
        }

        public void GetProgram(uint program, int pname, int* parameters)
        {
            GL.GetProgram(program, (GetProgramParameterName)pname, parameters);
        }

        public string GetProgramInfoLog(uint program)
        {
            return GL.GetProgramInfoLog((int)program);
        }

        public void GetShader(uint shader, int pname, int* parameters)
        {
            GL.GetShader(shader, (ShaderParameter)pname, parameters);
        }

        public string GetShaderInfoLog(uint shader)
        {
            return GL.GetShaderInfoLog((int)shader);
        }

        public string GetString(int name)
        {
            return GL.GetString((StringName)name);
        }

        public uint GetUniformBlockIndex(uint program, string uniformBlockName)
        {
            return (uint)GL.GetUniformBlockIndex(program, uniformBlockName);
        }

        public int GetUniformLocation(uint program, string name)
        {
            return GL.GetUniformLocation(program, name);
        }

        public void LinkProgram(uint program)
        {
            GL.LinkProgram(program);
        }

        public void PatchParameter(int pname, int value)
        {
            GL.PatchParameter((PatchParameterInt)pname, value);
        }

        public void PixelStore(int pname, int param)
        {
            GL.PixelStore((PixelStoreParameter)pname, param);
        }

        public void PolygonMode(int face, int mode)
        {
            GL.PolygonMode((MaterialFace)face, (PolygonMode)mode);
        }

        public void RenderbufferStorage(int target, int internalformat, int width, int height)
        {
            GL.RenderbufferStorage((RenderbufferTarget)target, (RenderbufferStorage)internalformat, width, height);
        }

        public void RenderbufferStorageMultisample(int target, int samples, int internalformat, int width, int height)
        {
            GL.RenderbufferStorageMultisample((RenderbufferTarget)target, samples, (RenderbufferStorage)internalformat, width, height);
        }

        public void SampleMask(uint maskNumber, uint mask)
        {
            GL.SampleMask(maskNumber, mask);
        }

        public void SamplerParameter(uint sampler, int pname, float param)
        {
            GL.SamplerParameter(sampler, (SamplerParameterName)pname, param);
        }

        public void SamplerParameter(uint sampler, int pname, int param)
        {
            GL.SamplerParameter(sampler, (SamplerParameterName)pname, param);
        }

        public void SamplerParameter(uint sampler, int pname, float* parameters)
        {
            GL.SamplerParameter(sampler, (SamplerParameterName)pname, parameters);
        }

        public void Scissor(int x, int y, int width, int height)
        {
            GL.Scissor(x, y, width, height);
        }

        public void ScissorIndexed(uint index, int left, int bottom, int width, int height)
        {
            GL.ScissorIndexed(index, left, bottom, width, height);
        }

        public void ShaderSource(uint shader, string strings)
        {
            GL.ShaderSource((int)shader, strings);
        }

        public void StencilFuncSeparate(int face, int func, int reference, uint mask)
        {
            GL.StencilFuncSeparate((StencilFace)face, (StencilFunction)func, reference, mask);
        }

        public void StencilMaskSeparate(int face, uint mask)
        {
            GL.StencilMaskSeparate((StencilFace)face, mask);
        }

        public void StencilOpSeparate(int face, int sfail, int dpfail, int dppass)
        {
            GL.StencilOpSeparate((StencilFace)face, (StencilOp)sfail, (StencilOp)dpfail, (StencilOp)dppass);
        }

        public void TexImage1D(int target, int level, int internalFormat, int width, int border, int format, int type, IntPtr data)
        {
            GL.TexImage1D((TextureTarget)target, level, (PixelInternalFormat)internalFormat, width, border, (PixelFormat)format, (PixelType)type, data);
        }

        public void TexImage2D(int target, int level, int internalFormat, int width, int height, int border, int format, int type, IntPtr data)
        {
            GL.TexImage2D((TextureTarget)target, level, (PixelInternalFormat)internalFormat, width, height, border, (PixelFormat)format, (PixelType)type, data);
        }

        public void TexImage3D(int target, int level, int internalFormat, int width, int height, int depth, int border, int format, int type, IntPtr data)
        {
            GL.TexImage3D((TextureTarget)target, level, (PixelInternalFormat)internalFormat, width, height, depth, border, (PixelFormat)format, (PixelType)type, data);
        }

        public void TexParameter(int target, int pname, float param)
        {
            GL.TexParameter((TextureTarget)target, (TextureParameterName)pname, param);
        }

        public void TexParameter(int target, int pname, int param)
        {
            GL.TexParameter((TextureTarget)target, (TextureParameterName)pname, param);
        }

        public void TexStorage2DMultisample(int target, int samples, int internalformat, int width, int height, bool fixedsamplelocations)
        {
            GL.TexStorage2DMultisample((TextureTargetMultisample2d)target, samples, (SizedInternalFormat)internalformat, width, height, fixedsamplelocations);
        }

        public void TexStorage3DMultisample(int target, int samples, int internalformat, int width, int height, int depth, bool fixedsamplelocations)
        {
            GL.TexStorage3DMultisample((TextureTargetMultisample3d)target, samples, (SizedInternalFormat)internalformat, width, height, depth, fixedsamplelocations);
        }

        public void TexSubImage2D(int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, IntPtr data)
        {
            GL.TexSubImage2D((TextureTarget)target, level, xoffset, yoffset, width, height, (PixelFormat)format, (PixelType)type, data);
        }

        public void TexSubImage3D(int target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, IntPtr data)
        {
            GL.TexSubImage3D((TextureTarget)target, level, xoffset, yoffset, zoffset, width, height, depth, (PixelFormat)format, (PixelType)type, data);
        }

        public void TransformFeedbackVaryings(uint program, int count, string[] varyings, int bufferMode)
        {
            GL.TransformFeedbackVaryings(program, count, varyings, (TransformFeedbackMode)bufferMode);
        }

        public void Uniform(int location, int v0)
        {
            GL.Uniform1(location, v0);
        }

        public void UniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding)
        {
            GL.UniformBlockBinding(program, uniformBlockIndex, uniformBlockBinding);
        }

        public void UseProgram(uint program)
        {
            GL.UseProgram(program);
        }

        public void VertexAttribPointer(uint index, int size, int type, bool normalized, int stride, IntPtr pointer)
        {
            GL.VertexAttribPointer(index, size, (VertexAttribPointerType)type, normalized, stride, pointer);
        }

        public void VertexAttribIPointer(uint index, int size, int type, int stride, IntPtr pointer)
        {
            GL.VertexAttribIPointer(index, size, (VertexAttribIntegerType)type, stride, pointer);
        }

        public void VertexAttribDivisor(uint index, uint divisor)
        {
            GL.VertexAttribDivisor(index, divisor);
        }

        public void Viewport(int x, int y, int width, int height)
        {
            GL.Viewport(x, y, width, height);
        }

        public void ViewportIndexed(uint index, float x, float y, float w, float h)
        {
            GL.ViewportIndexed(index, x, y, w, h);
        }
    }
}