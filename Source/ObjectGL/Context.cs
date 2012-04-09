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
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ObjectGL
{
    public partial class Context
    {
        readonly GraphicsContext nativeContext;

        public Capabilities Capabilities { get; private set; }
        public Pipeline Pipeline { get; private set; }

        public Context(GraphicsContext nativeContext)
        {
            this.nativeContext = nativeContext;

            Capabilities = new Capabilities();
            Pipeline = new Pipeline(this);

            transormFeedbackBufferIndexedBindings = new RedundantStruct<int>[Capabilities.MaxTransformFeedbackBuffers];
            for (int i = 0; i < Capabilities.MaxTransformFeedbackBuffers; i++)
            {
                int iLoc = i;
                transormFeedbackBufferIndexedBindings[i] = new RedundantStruct<int>(h =>
                    { 
                        GL.BindBufferBase(BufferTarget.TransformFeedbackBuffer, iLoc, h);
                        actualTransformFeedbackBuffer = h;
                    });
            }

            uniformBufferIndexedBindings = new RedundantStruct<int>[Capabilities.MaxUniformBufferBindings];
            for (int i = 0; i < Capabilities.MaxUniformBufferBindings; i++)
            {
                int iLoc = i;
                uniformBufferIndexedBindings[i] = new RedundantStruct<int>(h =>
                    {
                        GL.BindBufferBase(BufferTarget.UniformBuffer, iLoc, h);
                        actualUniformBuffer = h;
                    });
            }

            actualTextures = new int[Capabilities.MaxCombinedTextureImageUnits];
            textureUnitForEditing = Capabilities.MaxCombinedTextureImageUnits - 1;

            samplerBindings = new RedundantStruct<int>[Capabilities.MaxCombinedTextureImageUnits];
            for (int i = 0; i < Capabilities.MaxCombinedTextureImageUnits; i++)
            {
                int iLoc = i;
                samplerBindings[i] = new RedundantStruct<int>(h => GL.BindSampler(iLoc, h));
            }

            Rasterizer = new ContextRasterizerAspect();
        }

        #region Buffers
        readonly RedundantStruct<int> vertexArrayBinding = new RedundantStruct<int>(GL.BindVertexArray);

        readonly RedundantStruct<int> arrayBufferBinding = new RedundantStruct<int>(h => GL.BindBuffer(BufferTarget.ArrayBuffer, h));
        readonly RedundantStruct<int> copyReadBufferBinding = new RedundantStruct<int>(h => GL.BindBuffer(BufferTarget.CopyReadBuffer, h));
        readonly RedundantStruct<int> copyWriteBufferBinding = new RedundantStruct<int>(h => GL.BindBuffer(BufferTarget.CopyWriteBuffer, h));
        readonly RedundantStruct<int> elementArrayBufferBinding = new RedundantStruct<int>(h => GL.BindBuffer(BufferTarget.ElementArrayBuffer, h));
        readonly RedundantStruct<int> pixelPackBufferBinding = new RedundantStruct<int>(h => GL.BindBuffer(BufferTarget.PixelPackBuffer, h));
        readonly RedundantStruct<int> pixelUnpackBufferBinding = new RedundantStruct<int>(h => GL.BindBuffer(BufferTarget.PixelUnpackBuffer, h));
        readonly RedundantStruct<int> textureBufferBinding = new RedundantStruct<int>(h => GL.BindBuffer(BufferTarget.TextureBuffer, h));
        readonly RedundantStruct<int>[] transormFeedbackBufferIndexedBindings;
        readonly RedundantStruct<int>[] uniformBufferIndexedBindings;

        int actualTransformFeedbackBuffer;
        int actualUniformBuffer;

        internal void BindVertexArray(int vertexArrayHandle)
        {
            vertexArrayBinding.Set(vertexArrayHandle);
        }

        internal void BindBuffer(BufferTarget target, int bufferHandle)
        {
            if (target == BufferTarget.ElementArrayBuffer)
                vertexArrayBinding.Set(0);

            switch (target)
            {
                case BufferTarget.ArrayBuffer: arrayBufferBinding.Set(bufferHandle); return;
                case BufferTarget.CopyReadBuffer: copyReadBufferBinding.Set(bufferHandle); return;
                case BufferTarget.CopyWriteBuffer: copyWriteBufferBinding.Set(bufferHandle); return;
                case BufferTarget.ElementArrayBuffer: elementArrayBufferBinding.Set(bufferHandle); return;
                case BufferTarget.PixelPackBuffer: pixelPackBufferBinding.Set(bufferHandle); return;
                case BufferTarget.PixelUnpackBuffer: pixelUnpackBufferBinding.Set(bufferHandle); return;
                case BufferTarget.TextureBuffer: textureBufferBinding.Set(bufferHandle); return;
                case BufferTarget.TransformFeedbackBuffer:
                    {
                        if (actualTransformFeedbackBuffer == bufferHandle) return;
                        transormFeedbackBufferIndexedBindings[0].Force(bufferHandle);
                        actualTransformFeedbackBuffer = bufferHandle;
                        return;
                    }
                case BufferTarget.UniformBuffer:
                    {
                        if (actualUniformBuffer == bufferHandle) return;
                        uniformBufferIndexedBindings[0].Force(bufferHandle);
                        actualUniformBuffer = bufferHandle;
                        return;
                    }
                default: throw new ArgumentOutOfRangeException("target");
            }
        }

        internal void BindUniformBufferForDrawing(int binding, int bufferHandle)
        {
            uniformBufferIndexedBindings[binding].Set(bufferHandle);
        }
        #endregion

        #region Textures
        readonly int textureUnitForEditing;
        readonly RedundantStruct<int> activeTextureUnitBinding = new RedundantStruct<int>(i => GL.ActiveTexture(TextureUnit.Texture0 + i));

        readonly int[] actualTextures;

        internal void BindTexture(TextureTarget target, int textureHandle)
        {
            for (int i = 0; i < actualTextures.Length; i++)
            {
                if (actualTextures[i] == textureHandle)
                {
                    activeTextureUnitBinding.Set(i);
                    return;
                }
            }

            activeTextureUnitBinding.Set(textureUnitForEditing);
            GL.BindTexture(target, textureHandle);
            actualTextures[textureUnitForEditing] = textureHandle;
        }

        internal void BindTextureForDrawing(int unit, TextureTarget target, int textureHandle)
        {
            if (actualTextures[unit] == textureHandle) return;

            activeTextureUnitBinding.Set(unit);
            GL.BindTexture(target, textureHandle);
            actualTextures[unit] = textureHandle;
        }
        #endregion

        #region Samplers
        readonly RedundantStruct<int>[] samplerBindings;

        internal void BindSamplerForDrawing(int unit, int samplerHandle)
        {
            samplerBindings[unit].Set(samplerHandle);
        }
        #endregion

        #region Programs
        readonly RedundantStruct<int> programBinding = new RedundantStruct<int>(GL.UseProgram);

        internal void BindProgramForDrawing(int programHandle)
        {
            programBinding.Set(programHandle);
        }
        #endregion

        #region Render States
        internal ContextRasterizerAspect Rasterizer { get; private set; }
        
        #endregion
    }
}
