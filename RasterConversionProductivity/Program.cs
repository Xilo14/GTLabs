using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasterConversionProductivity
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap original = Rgb.LoadBitmap("Lenna.png");
            Test();
            Console.WriteLine("Lenna has been loaded. Warming up");

            // Warm-up
            ConvertSimple(original, 1);
            ConvertQuickByte(original, 1);
            ConvertVeryQuickByte(original, 1);
            ConvertQuickDouble(original, 1);
            ConvertMarshal(original, 1);
            Console.WriteLine("Code is warmed up.");

            DateTime start = DateTime.Now;
            ConvertSimple(original, 100);
            DateTime finish = DateTime.Now;
            Console.WriteLine("Simple processing done in {0} msec", (finish - start).TotalMilliseconds);

            start = DateTime.Now;
            ConvertQuickByte(original, 100);
            finish = DateTime.Now;
            Console.WriteLine("Quick processing (byte) done in {0} msec", (finish - start).TotalMilliseconds);

            start = DateTime.Now;
            ConvertVeryQuickByte(original, 100);
            finish = DateTime.Now;
            Console.WriteLine("Quick processing (byte*) done in {0} msec", (finish - start).TotalMilliseconds);

            start = DateTime.Now;
            ConvertQuickDouble(original, 100);
            finish = DateTime.Now;
            Console.WriteLine("Quick processing (double*) done in {0} msec", (finish - start).TotalMilliseconds);

            start = DateTime.Now;
            ConvertMarshal(original, 100);
            finish = DateTime.Now;
            Console.WriteLine("Marshal.Copy() processing done in {0} msec", (finish - start).TotalMilliseconds);

            Console.WriteLine("Press enter to exit");
            Console.ReadKey();
        }

        static void ConvertSimple(Bitmap source, int counter)
        {
            for (int i = 0; i < counter; ++i)
            {
                Bitmap restored = Rgb.RgbToBitmapNaive(Rgb.BitmapToByteRgbNaive(source));
            }
        }

        static void ConvertQuickByte(Bitmap source, int counter)
        {
            for (int i = 0; i < counter; ++i)
            {
                Bitmap restored = Rgb.RgbToBitmap(Rgb.BitmapToByteRgb(source));
            }
        }

        static void ConvertVeryQuickByte(Bitmap source, int counter)
        {
            for (int i = 0; i < counter; ++i)
            {
                Bitmap restored = Rgb.RgbToBitmapQ(Rgb.BitmapToByteRgbQ(source));
            }
        }

        static void ConvertQuickDouble(Bitmap source, int counter)
        {
            for (int i = 0; i < counter; ++i)
            {
                Bitmap restored = Rgb.RgbToBitmap(Rgb.BitmapToDoubleRgb(source));
            }
        }

        static void ConvertMarshal(Bitmap source, int counter)
        {
            int width = source.Width, height = source.Height;
            for (int i = 0; i < counter; ++i)
            {
                Bitmap restored = Rgb.ByteRgbToBitmapMarshal(Rgb.BitmapToByteRgbMarshal(source), width, height);
            }
        }

        static void Test()
        {
            int width=64, height=32;
            byte[, ,] rgb = GenerateBytePicture(width, height);
            byte[, ,] res = Rgb.BitmapToByteRgbQ(Rgb.RgbToBitmapQ(rgb));
            Console.WriteLine("Converted bitmap is equal: {0}", ArraysEqual(rgb, res));
        }

        private static byte[, ,] GenerateBytePicture(int width, int height)
        {
            Random rnd = new Random(1974);
            byte[, ,] result = new byte[3, height, width];
            for (int c = 0; c < 3; ++c)
                for (int h = 0; h < height; ++h)
                    for (int w = 0; w < width; ++w)
                        result[c, h, w] = (byte)rnd.Next(256);
            return result;
        }

        static bool ArraysEqual<T>(T[,,] a1, T[,,] a2)
        {
            var equal =
                a1.Rank == a2.Rank &&
                Enumerable.Range(0, a1.Rank).All(dim => a1.GetLength(dim) == a2.GetLength(dim)) &&
                a1.Cast<T>().SequenceEqual(a2.Cast<T>());
            return equal;
        }
    }
}
