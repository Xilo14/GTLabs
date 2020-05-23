﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RasterConversionProductivity
{
    public static class Rgb
    {
        /// <summary>
        /// Метод загружает растровые изображения без блокирования файла
        /// (как это делает конструктор Bitmap(fileName)).
        /// </summary>
        /// <param name="fileName">Имя файла для загрузки.</param>
        /// <returns>Экземпляр Bitmap.</returns>
        public static Bitmap LoadBitmap(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                return new Bitmap(fs);
        }

        /// <summary>
        /// Функция предназначена для извлечения из экземпляра класса Bitmap данных о
        /// яркости отдельных пикселов и преобразования их в формат byte[,,].
        /// При этом первый индекс соответствует одной из трех цветовых компонент (R, 
        /// G или B соответственно), второй - номер строки (координата Y), третий -
        /// номер столбца (координата X).
        /// </summary>
        /// <param name="bmp">Экземпляр Bitmap, из которого необходимо извлечь 
        /// яркостные данные.</param>
        /// <returns>Байтовый массив с данными о яркости каждой компоненты
        /// каждого пиксела.</returns>
        public unsafe static byte[, ,] BitmapToByteRgb(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            byte[, ,] res = new byte[3, height, width];
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            try
            {
                byte* curpos;
                for (int h = 0; h < height; h++)
                {
                    curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                    for (int w = 0; w < width; w++)
                    {
                        res[2, h, w] = *(curpos++);
                        res[1, h, w] = *(curpos++);
                        res[0, h, w] = *(curpos++);
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
            return res;
        }

        public static unsafe byte[,,] BitmapToByteRgbQ(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            byte[,,] res = new byte[3, height, width];
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            try
            {
                byte* curpos;
                fixed (byte* _res = res)
                {
                    byte* _r = _res, _g = _res + width*height, _b = _res + 2*width*height;
                    for (int h = 0; h < height; h++)
                    {
                        curpos = ((byte*) bd.Scan0) + h*bd.Stride;
                        for (int w = 0; w < width; w++)
                        {
                            *_b = *(curpos++);
                            ++_b;
                            *_g = *(curpos++);
                            ++_g;
                            *_r = *(curpos++);
                            ++_r;
                        }
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
            return res;
        }

        public unsafe static byte[] BitmapToByteRgbMarshal(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            byte[] res = new byte[3 * height * width];
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            try
            {
                int lineSize = width * 3;
                for (int h = 0; h < height; h++)
                {
                    int pos = h * lineSize;
                    IntPtr curpos = (IntPtr)((byte*)bd.Scan0) + h * bd.Stride;
                    Marshal.Copy(curpos, res, pos, lineSize);
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
            return res;
        }

        public unsafe static Bitmap ByteRgbToBitmapMarshal(byte[] rgb, int width, int height)
        {
            Bitmap res = new Bitmap(width, height);
            BitmapData bd = res.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            try
            {
                int lineSize = width * 3;
                for (int h = 0; h < height; h++)
                {
                    int pos = h * lineSize;
                    IntPtr curpos = (IntPtr)((byte*)bd.Scan0) + h * bd.Stride;
                    Marshal.Copy(rgb, pos, curpos, lineSize);
                }
            }
            finally
            {
                res.UnlockBits(bd);
            }
            return res;
        }


        /// <summary>
        /// Функция предназначена для извлечения из экземпляра класса Bitmap данных о
        /// яркости отдельных пикселов и преобразования их в формат double[,,].
        /// При этом первый индекс соответствует одной из трех цветовых компонент (R, 
        /// G или B соответственно), второй - номер строки (координата Y), третий -
        /// номер столбца (координата X).
        /// </summary>
        /// <param name="bmp">Экземпляр Bitmap, из которого необходимо извлечь 
        /// яркостные данные.</param>
        /// <returns>Mассив double с данными о яркости каждой компоненты
        /// каждого пиксела.</returns>
        public unsafe static double[, ,] BitmapToDoubleRgb(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            double[, ,] res = new double[3, height, width];
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            try
            {
                byte* curpos;
                fixed (double* _res = res)
                {
                    double* _r = _res, _g = _res + 1, _b = _res + 2;
                    for (int h = 0; h < height; h++)
                    {
                        curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                        for (int w = 0; w < width; w++)
                        {
                            *_b = *(curpos++); _b += 3;
                            *_g = *(curpos++); _g += 3;
                            *_r = *(curpos++); _r += 3;
                        }
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
            return res;
        }

        public static byte[, ,] BitmapToByteRgbNaive(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            byte[, ,] res = new byte[3, height, width];
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    Color color = bmp.GetPixel(x, y);
                    res[0, y, x] = color.R;
                    res[1, y, x] = color.G;
                    res[2, y, x] = color.B;
                }
            }
            return res;
        }

        /// <summary>
        /// Функция предназначена для создания нового экземпляра класса Bitmap на 
        /// базе имеющейся в виде byte[,,]-массива информацией о яркости каждого пиксела.
        /// При этом первый индекс соответствует одной из трех цветовых компонент (R, 
        /// G или B соответственно), второй - номер строки (координата Y), третий -
        /// номер столбца (координата X).
        /// </summary>
        /// <param name="rgb">Byte массив с данными о яркости каждой компоненты
        /// каждого пиксела</param>
        /// <returns>Новый экземпляр класса Bitmap</returns>
        public unsafe static Bitmap RgbToBitmap(byte[, ,] rgb)
        {
            if ((rgb.GetLength(0) != 3))
            {
                throw new ArrayTypeMismatchException("Size of first dimension for passed array must be 3 (RGB components)");
            }

            int width = rgb.GetLength(2),
                height = rgb.GetLength(1);

            Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bd = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            try
            {
                byte* curpos;
                for (int h = 0; h < height; h++)
                {
                    curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                    for (int w = 0; w < width; w++)
                    {
                        *(curpos++) = rgb[2, h, w];
                        *(curpos++) = rgb[1, h, w];
                        *(curpos++) = rgb[0, h, w];
                    }
                }
            }
            finally
            {
                result.UnlockBits(bd);
            }

            return result;
        }

        public unsafe static Bitmap RgbToBitmapQ(byte[, ,] rgb)
        {
            if ((rgb.GetLength(0) != 3))
            {
                throw new ArrayTypeMismatchException("Size of first dimension for passed array must be 3 (RGB components)");
            }

            int width = rgb.GetLength(2),
                height = rgb.GetLength(1);

            Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bd = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            try
            {
                byte* curpos;
                fixed (byte* _rgb = rgb)
                {
                    byte* _r = _rgb, _g = _rgb + width*height, _b = _rgb + 2*width*height;
                    for (int h = 0; h < height; h++)
                    {
                        curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                        for (int w = 0; w < width; w++)
                        {
                            *(curpos++) = *_b; ++_b;
                            *(curpos++) = *_g; ++_g;
                            *(curpos++) = *_r; ++_r;
                        }
                    }
                }
            }
            finally
            {
                result.UnlockBits(bd);
            }

            return result;
        }

        /// <summary>
        /// Функция предназначена для создания нового экземпляра класса Bitmap на 
        /// базе имеющейся в виде byte[,,]-массива информацией о яркости каждого пиксела.
        /// При этом первый индекс соответствует одной из трех цветовых компонент (R, 
        /// G или B соответственно), второй - номер строки (координата Y), третий -
        /// номер столбца (координата X).
        /// </summary>
        /// <param name="rgb">Double массив с данными о яркости каждой компоненты
        /// каждого пиксела</param>
        /// <returns>Новый экземпляр класса Bitmap</returns>
        public unsafe static Bitmap RgbToBitmap(double[, ,] rgb)
        {
            if ((rgb.GetLength(0) != 3))
            {
                throw new ArrayTypeMismatchException("Size of first dimension for passed array must be 3 (RGB components)");
            }

            int width = rgb.GetLength(2),
                height = rgb.GetLength(1);

            Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bd = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            try
            {
                byte* curpos;
                fixed (double* _rgb = rgb)
                {
                    double* _r = _rgb, _g = _rgb + 1, _b = _rgb + 2;
                    for (int h = 0; h < height; h++)
                    {
                        curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                        for (int w = 0; w < width; w++)
                        {
                            *(curpos++) = Limit(*_b); _b += 3;
                            *(curpos++) = Limit(*_g); _g += 3;
                            *(curpos++) = Limit(*_r); _r += 3;
                        }
                    }
                }
            }
            finally
            {
                result.UnlockBits(bd);
            }

            return result;
        }

        private static byte Limit(double x)
        {
            if (x < 0)
                return 0;
            if (x > 255)
                return 255;
            return (byte)x;
        }

        public static Bitmap RgbToBitmapNaive(byte[, ,] rgb)
        {
            if ((rgb.GetLength(0) != 3))
            {
                throw new ArrayTypeMismatchException("Size of first dimension for passed array must be 3 (RGB components)");
            }

            int width = rgb.GetLength(2),
                height = rgb.GetLength(1);

            Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    result.SetPixel(x, y, Color.FromArgb(rgb[0, y, x], rgb[1, y, x], rgb[2, y, x]));
                }
            }

            return result;
        }
    }
}
