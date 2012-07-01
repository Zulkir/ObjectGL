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
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ObjectGL.Tester
{
    class TextureLoader : IDisposable
    {
        readonly byte[] data;
        readonly IntPtr[] mipPointers;
        readonly int width;
        readonly int height;
        GCHandle gcHandle;

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public unsafe TextureLoader (string fileName)
        {
            var bitmap = new Bitmap(fileName);

            width = bitmap.Width;
            height = bitmap.Height;

            data = new byte[bitmap.Width * bitmap.Height * 4 * 2];
            mipPointers = new IntPtr[GetMipCount(bitmap.Width, bitmap.Height)];

            gcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            var startPointer = gcHandle.AddrOfPinnedObject();
            mipPointers[0] = startPointer;

            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            {
                var dst = (byte*)startPointer;
                var src = (byte*)bitmapData.Scan0;

                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        //*(int*) dst = *(int*) &src;

                        dst[0] = src[2];
                        dst[1] = src[1];
                        dst[2] = src[0];
                        dst[3] = 255;

                        src += 3;
                        dst += 4;
                    }
                }

                int mipWidth = bitmap.Width;
                int mipHeight = bitmap.Height;

                for (int level = 1; level < mipPointers.Length; level++)
                {
                    mipPointers[level] = (IntPtr)dst;
                    src = (byte*)mipPointers[level - 1];

                    int srcRowSpan = mipWidth * 4;

                    mipWidth = Math.Max(mipWidth / 2, 1);
                    mipHeight = Math.Max(mipHeight / 2, 1);

                    for (int y = 0; y < mipHeight; y++)
                    {
                        for (int x = 0; x < mipWidth; x++)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                float tl = SrgbToLinear(src[i] / 255f);
                                float tr = SrgbToLinear(src[i + 4] / 255f);
                                float bl = SrgbToLinear(src[i + srcRowSpan] / 255f);
                                float br = SrgbToLinear(src[i + 4 + srcRowSpan] / 255f);

                                dst[i] = (byte) (LinearToSrgb((tl + tr + bl + br)/4f)*255.9999f);
                            }

                            dst[3] = 255;

                            src += 8;
                            dst += 4;
                        }

                        src += srcRowSpan;
                    }
                }
            }
            bitmap.UnlockBits(bitmapData);
        }

        public IntPtr GetMipData(int level)
        {
            return mipPointers[level];
        }

        public void Dispose()
        {
            gcHandle.Free();
        }

        static int GetMipCount(int width, int height)
        {
            int maxDimension = Math.Max(width, height);

            int mipCount = 1;
            while (maxDimension > 1)
            {
                mipCount++;
                maxDimension /= 2;
            }

            return mipCount;
        }

        static float SrgbToLinear(float c)
        {
            if (c <= 0.04045f)
                return c / 12.92f;

            return (float)Math.Pow((c + 0.055f) / (1f + 0.055f), 2.4f);
        }

        static float LinearToSrgb(float c)
        {
            if (c <= 0.0031308f)
                return c * 12.92f;

            return (1f + 0.055f) * (float)Math.Pow(c, 1f / 2.4f) - 0.055f;
        }
    }
}
