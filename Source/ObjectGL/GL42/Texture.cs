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
using OpenTK.Graphics.OpenGL;

namespace ObjectGL.GL42
{
    public abstract class Texture : IResource
    {
        readonly int handle;
        readonly ResourceType resourceType;

        readonly TextureTarget target;
        readonly Format internalFormat;
        readonly int sliceCount;
        readonly int mipCount;

        int baseLevel;
        int maxLevel;
        float lodBias;

        public int Handle { get { return handle; } }
        public ContextObjectType ContextObjectType { get { return ContextObjectType.Resource; } }
        public ResourceType ResourceType { get { return resourceType; } }

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

        protected unsafe Texture(TextureTarget target,
            Format internalFormat, int sliceCount, int mipCount)
        {
            resourceType = TextureTargetToResourceType(target);
            this.target = target;
            this.internalFormat = internalFormat;
            this.sliceCount = sliceCount;
            this.mipCount = mipCount;

            int handleProxy;
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

        public void SetBaseLevel(Context currentContext, int value)
        {
            if (baseLevel == value) return;
            currentContext.BindTexture(target, this);
            GL.TexParameter(target, TextureParameterName.TextureBaseLevel, value);
            baseLevel = value;
        }

        public void SetMaxLevel(Context currentContext, int value)
        {
            if (maxLevel == value) return;
            currentContext.BindTexture(target, this);
            GL.TexParameter(target, TextureParameterName.TextureMaxLevel, value);
            maxLevel = value;
        }

        public void SetLodBias(Context currentContext, float value)
        {
            if (lodBias == value) return;
            currentContext.BindTexture(target, this);
            GL.TexParameter(target, TextureParameterName.TextureLodBias, value);
            lodBias = value;
        }

        public void GenerateMipmap(Context currentContext)
        {
            currentContext.BindTexture(target, this);
            GL.GenerateMipmap((GenerateMipmapTarget)target);
        }

        public unsafe void Dispose()
        {
            int handleProxy = handle;
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
