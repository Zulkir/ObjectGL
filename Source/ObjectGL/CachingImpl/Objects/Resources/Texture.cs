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

using System;
using ObjectGL.Api;
using ObjectGL.Api.Context;
using ObjectGL.Api.Objects.Resources;

namespace ObjectGL.CachingImpl.Objects.Resources
{
    internal abstract class Texture : ITexture
    {
        private readonly IContext context;
        readonly uint handle;
        readonly ResourceType resourceType;

        readonly int width;
        readonly int height;
        readonly int depth;

        readonly TextureTarget target;
        readonly Format internalFormat;
        readonly int sliceCount;
        readonly int mipCount;

        int baseLevel;
        int maxLevel;
        float lodBias;

        protected IContext Context { get { return context; } }
        protected IGL GL { get { return context.GL; }}

        public uint Handle { get { return handle; } }
        public GLObjectType GLObjectType { get { return GLObjectType.Resource; } }
        public ResourceType ResourceType { get { return resourceType; } }

        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public int Depth { get { return depth; } }

        public TextureTarget Target { get { return target; } }
        public Format InternalFormat { get { return internalFormat; } }
        public int SliceCount { get { return sliceCount; } }
        public int MipCount { get { return mipCount; } }

        /*
        TextureMinFilter minFilter;
        TextureMagFilter magFilter;
        TextureWrapMode wrapS = TextureWrapMode.Repeat;
        TextureWrapMode wrapT = TextureWrapMode.Repeat;
        TextureWrapMode wrapR = TextureWrapMode.Repeat;
        float lodBias;
        uint maxAnisotropy;
        TextureCompareMode compareMode;
        CompareFunc compareFunc;
        Color4 borderColor;
        float minLod = -1000f;
        float maxLod = 1000f;
        float maxAnisotropy = 16f;
        */

        protected unsafe Texture(IContext context, TextureTarget target,
            int width, int height, int depth,
            Format internalFormat, int sliceCount, int mipCount)
        {
            this.context = context;
            resourceType = TextureTargetToResourceType(target);

            this.width = width;
            this.height = height;
            this.depth = depth;

            this.target = target;
            this.internalFormat = internalFormat;
            this.sliceCount = sliceCount;
            this.mipCount = mipCount;

            uint handleProxy;
            GL.GenTextures(1, &handleProxy);
            handle = handleProxy;
        }

        static ResourceType TextureTargetToResourceType(TextureTarget target)
        {
            switch (target)
            {
                case TextureTarget.Texture1D: return ResourceType.Texture1D;
                case TextureTarget.Texture1DArray: return ResourceType.Texture1DArray;
                case TextureTarget.Texture2D: return ResourceType.Texture2D;
                case TextureTarget.Texture2DArray: return ResourceType.Texture2DArray;
                case TextureTarget.Texture2DMultisample: return ResourceType.Texture2DMultisample;
                case TextureTarget.Texture2DMultisampleArray: return ResourceType.Texture2DMultisampleArray;
                case TextureTarget.Texture3D: return ResourceType.Texture3D;
                case TextureTarget.TextureBuffer: return ResourceType.TextureBuffer;
                case TextureTarget.TextureCubeMap: return ResourceType.TextureCubemap;
                case TextureTarget.TextureCubeMapArray: return ResourceType.TextureCubemapArray;
                case TextureTarget.TextureRectangle: return ResourceType.TextureRectangle;
                default: throw new ArgumentOutOfRangeException("target");
            }
        }

        public void SetBaseLevel(int value)
        {
            if (baseLevel == value) 
                return;

            context.Bindings.Textures.Units[context.Bindings.Textures.EditingIndex].Set(this);
            GL.TexParameter((int)target, (int)All.TextureBaseLevel, value);
            baseLevel = value;
        }

        public void SetMaxLevel(int value)
        {
            if (maxLevel == value) 
                return;
            context.Bindings.Textures.Units[context.Bindings.Textures.EditingIndex].Set(this);
            GL.TexParameter((int)target, (int)All.TextureMaxLevel, value);
            maxLevel = value;
        }

        public void SetLodBias(float value)
        {
            if (lodBias == value) 
                return;
            context.Bindings.Textures.Units[context.Bindings.Textures.EditingIndex].Set(this);
            GL.TexParameter((int)target, (int)All.TextureLodBias, value);
            lodBias = value;
        }

        public void GenerateMipmap()
        {
            context.Bindings.Textures.Units[context.Bindings.Textures.EditingIndex].Set(this);
            GL.GenerateMipmap((int)target);
        }

        public unsafe void Dispose()
        {
            uint handleProxy = handle;
            GL.DeleteTextures(1, &handleProxy);
        }

        public static int CalculateMipCount(int width, int height = 0, int depth = 0)
        {
            int largestDimension = Math.Max(Math.Max(width, height), depth);

            int result = 0;
            while (largestDimension > 0)
            {
                largestDimension >>= 1;
                result++;
            }

            return result;
        }

        public static int CalculateMipSize(int level, int baseSliceSize)
        {
            return Math.Max(1, baseSliceSize >> level);
        }

        public static FormatColor GetAppropriateFormatColor(Format pixelInternalFormat)
        {
            switch (pixelInternalFormat)
            {
                case Format.Rgba32f:
                case Format.Rgba32ui:
                case Format.Rgba32i:
                case Format.Rgba16f:
                case Format.Rgba16:
                case Format.Rgba16ui:
                case Format.Rgba16sn:
                case Format.Rgba16i:
                case Format.Rgb10A2:
                case Format.Rgb10A2ui:
                case Format.Rgba8:
                case Format.Srgb8Alpha8:
                case Format.Rgba8sn:
                case Format.Rgba8i:
                    return FormatColor.Rgba;
                case Format.Rgb32f:
                case Format.Rgb32ui:
                case Format.Rgb32i:
                case Format.R11fG11fB10f:
                case Format.Rgb9E5:
                    return FormatColor.Rgb;
                case Format.Rg32f:
                case Format.Rg32ui:
                case Format.Rg32i:
                case Format.Rg16f:
                case Format.Rg16:
                case Format.Rg16ui:
                case Format.Rg16sn:
                case Format.Rg16i:
                case Format.Rg8:
                case Format.Rg8ui:
                case Format.Rg8sn:
                case Format.Rg8i:
                    return FormatColor.Rg;
                case Format.Depth32fStencil8:
                case Format.Depth24Stencil8:
                    return FormatColor.DepthStencil;
                case Format.DepthComponent32f:
                case Format.DepthComponent16:
                    return FormatColor.DepthComponent;
                case Format.R32f:
                case Format.R32ui:
                case Format.R32i:
                case Format.R16f:
                case Format.R16:
                case Format.R16ui:
                case Format.R16sn:
                case Format.R16i:
                case Format.R8:
                case Format.R8ui:
                case Format.R8sn:
                case Format.R8i:
                    return FormatColor.Red;
                case Format.CompressedRgbaS3tcDxt1Ext:
                case Format.CompressedSrgbAlphaS3tcDxt1Ext:
                case Format.CompressedRgbaS3tcDxt3Ext:
                case Format.CompressedSrgbAlphaS3tcDxt3Ext:
                case Format.CompressedRgbaS3tcDxt5Ext:
                case Format.CompressedSrgbAlphaS3tcDxt5Ext:
                case Format.CompressedRgbaBptcUf:
                case Format.CompressedRgbaBptcSf:
                case Format.CompressedRgbaBptc:
                    return FormatColor.Rgba;
                default:
                    throw new ArgumentOutOfRangeException("pixelInternalFormat");
            }
        }

        public static FormatType GetAppropriateFormatType(Format pixelInternalFormat)
        {
            switch (pixelInternalFormat)
            {
                case Format.Rgba32f:
                case Format.Rgb32f:
                case Format.Rg32f:
                case Format.Rgba16f:
                case Format.R11fG11fB10f:
                case Format.Rg16f:
                case Format.DepthComponent32f:
                case Format.R32f:
                case Format.R16f:
                    return FormatType.Float;
                case Format.Rgba32ui:
                case Format.Rgb32ui:
                case Format.Rg32ui:
                case Format.R32ui:
                    return FormatType.UnsignedInt;
                case Format.Rgba32i:
                case Format.Rgb32i:
                case Format.Rg32i:
                case Format.R32i:
                    return FormatType.Int;
                case Format.DepthComponent16:
                case Format.Rgba16:
                case Format.Rgba16ui:
                case Format.Rg16:
                case Format.Rg16ui:
                case Format.R16:
                case Format.R16ui:
                    return FormatType.UnsignedShort;
                case Format.Rgba16sn:
                case Format.Rgba16i:
                case Format.Rg16sn:
                case Format.Rg16i:
                case Format.R16sn:
                case Format.R16i:
                    return FormatType.Short;
                case Format.Depth32fStencil8:
                    return FormatType.Float32UnsignedInt248Rev;
                case Format.Rgb10A2:
                case Format.Rgb10A2ui:
                    return FormatType.UnsignedInt1010102;
                case Format.Rgba8:
                case Format.Srgb8Alpha8:
                case Format.Rg8:
                case Format.Rg8ui:
                case Format.R8:
                case Format.R8ui:
                    return FormatType.UnsignedByte;
                case Format.Rgba8sn:
                case Format.Rgba8i:
                case Format.Rg8sn:
                case Format.Rg8i:
                case Format.R8sn:
                case Format.R8i:
                    return FormatType.Byte;
                case Format.Depth24Stencil8:
                    return FormatType.UnsignedInt248;
                case Format.Rgb9E5:
                    return FormatType.UnsignedInt5999Rev;
                case Format.CompressedRgbaS3tcDxt1Ext:
                case Format.CompressedSrgbAlphaS3tcDxt1Ext:
                case Format.CompressedRgbaS3tcDxt3Ext:
                case Format.CompressedSrgbAlphaS3tcDxt3Ext:
                case Format.CompressedRgbaS3tcDxt5Ext:
                case Format.CompressedSrgbAlphaS3tcDxt5Ext:
                case Format.CompressedRgbaBptcUf:
                case Format.CompressedRgbaBptcSf:
                case Format.CompressedRgbaBptc:
                    return FormatType.Byte;
                default:
                    throw new ArgumentOutOfRangeException("pixelInternalFormat");
            }
        }
    }
}
