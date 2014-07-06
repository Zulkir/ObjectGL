#region License
/*
Copyright (c) 2010-2014 ObjectGL Project - Daniil Rodin

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
using ObjectGL.Api.Objects.Resources;
using ObjectGL.CachingImpl.PipelineAspects;
using ObjectGL.CachingImpl.Utilitties;

namespace ObjectGL.CachingImpl.ContextAspects
{
    internal class ContextTexturesAspect
    {
        private readonly IGL gl;
        private readonly int textureUnitForEditing;
        private readonly RedundantInt activeTextureUnitBinding;
        private readonly ITexture[] actualTextures;
        private int enabledTextureUnitRange;

        public ContextTexturesAspect(IGL gl, Implementation implementation)
        {
            this.gl = gl;
            actualTextures = new ITexture[implementation.MaxCombinedTextureImageUnits];
            textureUnitForEditing = implementation.MaxCombinedTextureImageUnits - 1;
            activeTextureUnitBinding = new RedundantInt(gl, (g, x) => g.ActiveTexture((int)All.Texture0 + x));
        }

        public void BindTexture(TextureTarget target, ITexture texture)
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
            gl.BindTexture((int)target, texture.SafeGetHandle());
            actualTextures[textureUnitForEditing] = texture;
        }

        public void ConsumePipelineTextures(PipelineTexturesAspect pipelineTextures)
        {
            for (int i = 0; i < pipelineTextures.EnabledTextureRange; i++)
            {
                activeTextureUnitBinding.Set(i);
                var texture = pipelineTextures[i];
                gl.BindTexture((int)texture.Target, texture.Handle);
                actualTextures[i] = texture;
            }

            for (int i = pipelineTextures.EnabledTextureRange; i < enabledTextureUnitRange; i++)
            {
                if (actualTextures[i] == null) 
                    continue;
                activeTextureUnitBinding.Set(i);
                gl.BindTexture((int)TextureTarget.Texture2D, 0);
                actualTextures[i] = null;
            }

            enabledTextureUnitRange = pipelineTextures.EnabledTextureRange;
        }
    }
}
