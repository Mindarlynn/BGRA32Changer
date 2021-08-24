using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WpfApp1.ViewModel.Page1ViewModel;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;

namespace WpfApp1.Helper
{
    [StructLayout(LayoutKind.Explicit)]
    public struct PixelColor
    {
        // 32 bit BGRA 
        [FieldOffset(0)] public uint ColorBGRA;
        // 8 bit components
        [FieldOffset(0)] public byte Blue;
        [FieldOffset(1)] public byte Green;
        [FieldOffset(2)] public byte Red;
        [FieldOffset(3)] public byte Alpha;

        public override string ToString()
        {
            return Blue.ToString() + ", " + Green.ToString() + ", " + Red.ToString() + ", " + Alpha.ToString();
        }
        public bool Equals(byte b, byte g, byte r, byte a)
        {
            return Equals(b, g, r) && Alpha == a;
        }
        public bool Equals(byte b, byte g, byte r)
        {
            return Blue == b && Green == g && Red == r;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType() || GetHashCode() != obj.GetHashCode()) return false;

            var other = (PixelColor)obj;
            return Blue == other.Blue && Green == other.Green && Red == other.Red && Alpha == other.Alpha;
        }
        public override int GetHashCode()
        {
            return (int)ColorBGRA;
        }
    }

    public struct IntPixelColor
    {
        public int Blue;
        public int Green;
        public int Red;
        public int Alpha;
    }

    public class BitmapSourceHelper
    {
#if UNSAFE
  public unsafe static void CopyPixels(this BitmapSource source, PixelColor[,] pixels, int stride, int offset)
  {
    fixed(PixelColor* buffer = &pixels[0, 0])
      source.CopyPixels(
        new Int32Rect(0, 0, source.PixelWidth, source.PixelHeight),
        (IntPtr)(buffer + offset),
        pixels.GetLength(0) * pixels.GetLength(1) * sizeof(PixelColor),
        stride);
  }
#else
        public static void CopyPixels(BitmapSource source, PixelColor[,] pixels, int stride, int offset)
        {
            var height = source.PixelHeight;
            var width = source.PixelWidth;
            var pixelBytes = new byte[height * width * 4];
            source.CopyPixels(pixelBytes, stride, 0);
            int y0 = offset / width;
            int x0 = offset - width * y0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    pixels[y + y0, x + x0] = new PixelColor
                    {
                        Blue = pixelBytes[(y * width + x) * 4 + 0],
                        Green = pixelBytes[(y * width + x) * 4 + 1],
                        Red = pixelBytes[(y * width + x) * 4 + 2],
                        Alpha = pixelBytes[(y * width + x) * 4 + 3],
                    };
                }
            }
        }
#endif
        public static void WritePixels(WriteableBitmap source, PixelColor[,] pixels, int stride, int offset)
        {
            var height = pixels.GetLength(0);
            var width = pixels.GetLength(1);

            source.WritePixels(new System.Windows.Int32Rect(0, 0, width, height), pixels, stride, offset);
        }
    }
}
