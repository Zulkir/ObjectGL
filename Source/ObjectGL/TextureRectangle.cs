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

namespace ObjectGL
{
    public class TextureRectangle : Texture
    {
        readonly int width;
        readonly int height;

        public int Width { get { return width; } }
        public int Height { get { return height; } } 

        TextureRectangle(Context currentContext,
                  int width, int height,
                  Format internalFormat, Data initialData,
                  Action<TextureTarget, int, PixelInternalFormat, int, int, IntPtr> glTexImage)
            : base(TextureTarget.TextureRectangle, internalFormat, 1, 1)
        {
            if ((width == 0) || ((width & (~width + 1)) != width)) throw new ArgumentException("'width' must be a power of two");
            if ((height == 0) || ((height & (~height + 1)) != height)) throw new ArgumentException("'width' must be a power of two");

            this.width = width;
            this.height = height;

            currentContext.BindTexture(Target, Handle);

            Data data = initialData;
            glTexImage(Target, 0, (PixelInternalFormat)internalFormat, width, height, data.Pointer);
            data.UnpinPointer();
        }

        public TextureRectangle(Context currentContext,
                         int width, int height,
                         Format internalFormat)
            : this(currentContext, width, height, internalFormat, new Data(IntPtr.Zero), 
                   (tt, l, f, w, h, p) => GL.TexImage2D(tt, l, f, w, h, 0,
                       (PixelFormat)GetAppropriateFormatColor(internalFormat), (PixelType)GetAppropriateFormatType(internalFormat), p))
        {
        }

        public TextureRectangle(Context currentContext,
                         int width, int height,
                         Format internalFormat, FormatColor format, FormatType type,
                         Data initialData)
            : this(currentContext, width, height, internalFormat, initialData,
                   (tt, l, f, w, h, p) => GL.TexImage2D(tt, l, f, w, h, 0, (PixelFormat)format, (PixelType)type, p))
        {
        }

        public TextureRectangle(Context currentContext,
                         int width, int height,
                         Format internalFormat, Func<int, int> getComressedImageSizeForMip,
                         Data compressedInitialData)
            : this(currentContext, width, height, internalFormat, compressedInitialData,
                   (tt, l, f, w, h, p) => GL.CompressedTexImage2D(tt, l, f, w, h, 0, getComressedImageSizeForMip(l), p))
        {
        }
    }
}