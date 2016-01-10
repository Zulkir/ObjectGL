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

using ObjectGL.Api.Objects.Resources;

namespace ObjectGL.Api.Context
{
    public static class BindingExtensions
    {
        public static void Set(this IBinding<BlendEquation> binding, BlendMode rgba)
        {
            binding.Set(new BlendEquation { Rgb = rgba, Alpha = rgba });
        }

        public static void Set(this IBinding<BlendEquation> binding, BlendMode rgb, BlendMode alpha)
        {
            binding.Set(new BlendEquation { Rgb = rgb, Alpha = alpha });
        }

        public static void Set(this IBinding<BlendFunction> binding, BlendFactor sourceRgba, BlendFactor destinationRgba)
        {
            binding.Set(new BlendFunction
            {
                SourceRgb = sourceRgba,
                SourceAlpha = sourceRgba,
                DestinationRgb = destinationRgba,
                DestinationAlpha = destinationRgba
            });
        }

        public static void Set(this IBinding<BlendFunction> binding, BlendFactor sourceRgb, BlendFactor destinationRgb, BlendFactor sourceAlpha, BlendFactor destinationAlpha)
        {
            binding.Set(new BlendFunction
            {
                SourceRgb = sourceRgb,
                SourceAlpha = sourceAlpha,
                DestinationRgb = destinationRgb,
                DestinationAlpha = destinationAlpha
            });
        }

        public static void Set(this IBinding<BufferRange> binding, IBuffer buffer)
        {
            binding.Set(new BufferRange
            {
                Buffer = buffer,
                Offset = 0,
                Size = buffer.SizeInBytes
            });
        }

        public static void Force(this IBinding<BufferRange> binding, IBuffer buffer)
        {
            binding.Force(new BufferRange
            {
                Buffer = buffer,
                Offset = 0,
                Size = buffer.SizeInBytes
            });
        }

        public static void Set(this IBinding<ViewportInt> binding, int width, int height)
        {
            binding.Set(new ViewportInt
            {
                X = 0,
                Y = 0,
                Width = width,
                Height = height
            });
        }

        public static void Set(this IBinding<ViewportInt> binding, int x, int y, int width, int height)
        {
            binding.Set(new ViewportInt
            {
                X = x, 
                Y = y, 
                Width = width, 
                Height = height
            });
        }
    }
}