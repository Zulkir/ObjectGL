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
    public class Context
    {
        readonly GraphicsContext nativeContext;

        public Capabilities Capabilities { get; private set; }
        public Pipeline Pipeline { get; private set; }

        public Context(GraphicsContext nativeContext)
        {
            this.nativeContext = nativeContext;

            Capabilities = new Capabilities();
            Pipeline = new Pipeline(this);

            transormFeedbackBufferIndexedBindings = new RedundantEquatable<int>[Capabilities.MaxTransformFeedbackBuffers];
            for (int i = 0; i < Capabilities.MaxTransformFeedbackBuffers; i++)
            {
                int iLoc = i;
                transormFeedbackBufferIndexedBindings[i] = new RedundantEquatable<int>(h =>
                    { 
                        GL.BindBufferBase(BufferTarget.TransformFeedbackBuffer, iLoc, h);
                        actualTransformFeedbackBuffer = h;
                    });
            }

            uniformBufferIndexedBindings = new RedundantEquatable<int>[Capabilities.MaxUniformBufferBindings];
            for (int i = 0; i < Capabilities.MaxUniformBufferBindings; i++)
            {
                int iLoc = i;
                uniformBufferIndexedBindings[i] = new RedundantEquatable<int>(h =>
                    {
                        GL.BindBufferBase(BufferTarget.UniformBuffer, iLoc, h);
                        actualUniformBuffer = h;
                    });
            }

            actualTextures = new int[Capabilities.MaxCombinedTextureImageUnits];
            textureUnitForEditing = Capabilities.MaxCombinedTextureImageUnits - 1;

            samplerBindings = new RedundantEquatable<int>[Capabilities.MaxCombinedTextureImageUnits];
            for (int i = 0; i < Capabilities.MaxCombinedTextureImageUnits; i++)
            {
                int iLoc = i;
                samplerBindings[i] = new RedundantEquatable<int>(h => GL.BindSampler(iLoc, h));
            }
        }

        #region Buffers
        readonly RedundantEquatable<int> vertexArrayBinding = new RedundantEquatable<int>(GL.BindVertexArray);

        readonly RedundantEquatable<int> arrayBufferBinding = new RedundantEquatable<int>(h => GL.BindBuffer(BufferTarget.ArrayBuffer, h));
        readonly RedundantEquatable<int> copyReadBufferBinding = new RedundantEquatable<int>(h => GL.BindBuffer(BufferTarget.CopyReadBuffer, h));
        readonly RedundantEquatable<int> copyWriteBufferBinding = new RedundantEquatable<int>(h => GL.BindBuffer(BufferTarget.CopyWriteBuffer, h));
        readonly RedundantEquatable<int> elementArrayBufferBinding = new RedundantEquatable<int>(h => GL.BindBuffer(BufferTarget.ElementArrayBuffer, h));
        readonly RedundantEquatable<int> pixelPackBufferBinding = new RedundantEquatable<int>(h => GL.BindBuffer(BufferTarget.PixelPackBuffer, h));
        readonly RedundantEquatable<int> pixelUnpackBufferBinding = new RedundantEquatable<int>(h => GL.BindBuffer(BufferTarget.PixelUnpackBuffer, h));
        readonly RedundantEquatable<int> textureBufferBinding = new RedundantEquatable<int>(h => GL.BindBuffer(BufferTarget.TextureBuffer, h));
        readonly RedundantEquatable<int>[] transormFeedbackBufferIndexedBindings;
        readonly RedundantEquatable<int>[] uniformBufferIndexedBindings;

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
        #endregion

        #region Textures
        readonly int textureUnitForEditing;
        readonly RedundantEquatable<int> activeTextureUnitBinding = new RedundantEquatable<int>(i => GL.ActiveTexture(TextureUnit.Texture0 + i));

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
        #endregion

        #region Samplers
        readonly RedundantEquatable<int>[] samplerBindings;

        internal void BindSamplerForDrawing(int unit, int samplerHandle)
        {
            samplerBindings[unit].Set(samplerHandle);
        }
        #endregion

        #region Programs
        readonly RedundantEquatable<int> programBinding = new RedundantEquatable<int>(GL.UseProgram);

        internal void BindProgramForDrawing(int programHandle)
        {
            programBinding.Set(programHandle);
        }
        #endregion
    }
}
