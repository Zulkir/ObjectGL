﻿#region License
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

using OpenTK.Graphics.OpenGL;

namespace ObjectGL.GL42
{
    public partial class Context
    {
        private class TexturesAspect
        {
            readonly int textureUnitForEditing;
            readonly RedundantInt activeTextureUnitBinding = new RedundantInt(i => GL.ActiveTexture(TextureUnit.Texture0 + i));

            readonly Texture[] actualTextures;

            int enabledTextureUnitRange = 0;

            public TexturesAspect(Implementation implementation)
            {
                actualTextures = new Texture[implementation.MaxCombinedTextureImageUnits];
                textureUnitForEditing = implementation.MaxCombinedTextureImageUnits - 1;
            }

            public void BindTexture(TextureTarget target, Texture texture)
            {
                for (int i = 0; i < actualTextures.Length; i++)
                {
                    if (actualTextures[i] == texture)
                    {
                        activeTextureUnitBinding.Set(i);
                        return;
                    }
                }

                activeTextureUnitBinding.Set(textureUnitForEditing);
                GL.BindTexture(target, Helpers.ObjectHandle(texture));
                actualTextures[textureUnitForEditing] = texture;
            }

            public void ConsumePipelineTextures(Pipeline.TexturesAspect pipelineTextures)
            {
                for (int i = 0; i < pipelineTextures.EnabledTextureRange; i++)
                {
                    activeTextureUnitBinding.Set(i);
                    var texture = pipelineTextures[i];
                    GL.BindTexture(texture.Target, texture.Handle);
                    actualTextures[i] = texture;
                }

                for (int i = pipelineTextures.EnabledTextureRange; i < enabledTextureUnitRange; i++)
                {
                    if (actualTextures[i] != null)
                    {
                        activeTextureUnitBinding.Set(i);
                        GL.BindTexture(TextureTarget.Texture2D, 0);
                        actualTextures[i] = null;
                    }
                }

                enabledTextureUnitRange = pipelineTextures.EnabledTextureRange;
            }
        }
    }
}