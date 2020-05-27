using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using GTLib.Elements;
using GTLib.Interfaces;
using GTLib.Primitives;
using GTLib.Scenes;

namespace GTLib.Drawers
{
    public unsafe class GTDrawerSlow : GTDrawer
    {
        public enum AlgsForLine
        {
            Bresenham,
            Luke
        }

        private readonly Dictionary<AlgsForLine, AlgorithmDrawingLine> _algsDrawingLines =
            new Dictionary<AlgsForLine, AlgorithmDrawingLine>
            {
                {
                    AlgsForLine.Luke, (self, line) =>
                    {
                        var Xd = (int) line.start.X;
                        var Yd = (int) line.start.Y;
                        var Xf = (int) line.finish.X;
                        var Yf = (int) line.finish.Y;

                        int Dx, Dy, Cumul;
                        int Xinc, Yinc, X, Y;
                        int i;
                        X = Xd;
                        Y = Yd;
                        //self.Bitmap.SetPixel(X, Y, EnvVar.STD_COLOR);
                        self._setPixelInBytes(X, Y, EnvVar.STD_COLOR);
                        if (Xd < Xf)
                            Xinc = 1;
                        else
                            Xinc = -1;
                        if (Yd < Yf)
                            Yinc = 1;
                        else
                            Yinc = -1;
                        Dx = Math.Abs(Xd - Xf);
                        Dy = Math.Abs(Yd - Yf);
                        if (Dx > Dy)
                        {
                            Cumul = Dx / 2;
                            for (i = 0; i < Dx; i++)
                            {
                                X = X + Xinc;
                                Cumul = Cumul + Dy;
                                if (Cumul >= Dx)
                                {
                                    Cumul = Cumul - Dx;
                                    Y = Y + Yinc;
                                }

                                //Sleep(10);
                                //self.Bitmap.SetPixel(X, Y, EnvVar.STD_COLOR);
                                self._setPixelInBytes(X, Y, EnvVar.STD_COLOR);
                            }
                        }
                        else
                        {
                            Cumul = Dy / 2;
                            for (i = 0; i < Dy; i++)
                            {
                                Y += Yinc;
                                Cumul = Cumul + Dx;
                                if (Cumul >= Dy)
                                {
                                    Cumul = Cumul - Dy;
                                    X = X + Xinc;
                                }

                                //Sleep(10);
                                //self.Bitmap.SetPixel(X, Y, EnvVar.STD_COLOR);
                                self._setPixelInBytes(X, Y, EnvVar.STD_COLOR);
                            }
                        }
                    }
                },
                {
                    AlgsForLine.Bresenham, (self, line) =>
                    {
                        var Xd = (int) line.start.X;
                        var Yd = (int) line.start.Y;
                        var Xf = (int) line.finish.X;
                        var Yf = (int) line.finish.Y;

                        int Dx, Dy, Dx2, Dy2, Dxy, S, Xinc, Yinc, X, Y, i;

                        if (Xd < Xf)
                            Xinc = 1;
                        else
                            Xinc = -1;

                        if (Yd < Yf)
                            Yinc = 1;
                        else
                            Yinc = -1;
                        Dx = Math.Abs(Xd - Xf);
                        Dy = Math.Abs(Yd - Yf);
                        Dx2 = Dx + Dx;
                        Dy2 = Dy + Dy;
                        X = Xd;
                        Y = Yd;
                        //self.Bitmap.SetPixel(X, Y, EnvVar.STD_COLOR);
                        self._setPixelInBytes(X, Y, EnvVar.STD_COLOR);

                        if (Dx > Dy)
                        {
                            S = Dy2 - Dx;
                            Dxy = Dy2 - Dx2;
                            for (i = 0; i < Dx; i++)
                            {
                                if (S >= 0)
                                {
                                    Y = Y + Yinc;
                                    S = S + Dxy;
                                }
                                else
                                {
                                    S = S + Dy2;
                                }

                                X = X + Xinc;
                                //self.Bitmap.SetPixel(X, Y, EnvVar.STD_COLOR);
                                self._setPixelInBytes(X, Y, EnvVar.STD_COLOR);
                            }
                        }
                        else
                        {
                            S = Dx2 - Dy;
                            Dxy = Dx2 - Dy2;
                            for (i = 0; i < Dy; i++)
                            {
                                if (S >= 0)
                                {
                                    X = X + Xinc;
                                    S = S + Dxy;
                                }
                                else
                                {
                                    S = S + Dx2;
                                }

                                Y = Y + Yinc;
                                //self.Bitmap.SetPixel(X, Y, EnvVar.STD_COLOR);
                                self._setPixelInBytes(X, Y, EnvVar.STD_COLOR);
                            }
                        }
                    }
                }
            };


        private readonly Dictionary<Type, DrawMethod> _stdDictPrimitives =
            new Dictionary<Type, DrawMethod>
            {
                {
                    typeof(Dot2D), (self, primitive) =>
                    {
                        var dot2d = (Dot2D) primitive;
                        //self.Bitmap.SetPixel((int)dot2d.X,(int)dot2d.Y,EnvVar.STD_COLOR);
                        self._setPixelInBytes((int) dot2d.X, (int) dot2d.Y, EnvVar.STD_COLOR);
                    }
                },
                {
                    typeof(Line2D), (self, primitive) =>
                    {
                        var line2d = (Line2D) primitive;
                        self._algsDrawingLines[self.CurrentAlgForLine](self, line2d);
                    }
                },
                {
                    typeof(Circle2D), (self, primitive) =>
                    {
                        var circle2d = (Circle2D) primitive;
                        int Xc = (int) circle2d.Center.X,
                            Yc = (int) circle2d.Center.Y;
                        var x = 0;
                        var y = (int) circle2d.Radius;
                        var delta = 1 - 2 * (int) circle2d.Radius;
                        var error = 0;

                        while (y >= 0)
                        {
                            //self.Bitmap.SetPixel(Xc+x, Yc+y, EnvVar.STD_COLOR);
                            //self.Bitmap.SetPixel(Xc + x, Yc - y, EnvVar.STD_COLOR);
                            //self.Bitmap.SetPixel(Xc - x, Yc + y, EnvVar.STD_COLOR);
                            //self.Bitmap.SetPixel(Xc - x, Yc - y, EnvVar.STD_COLOR);
                            self._setPixelInBytes(Xc + x, Yc + y, EnvVar.STD_COLOR);
                            self._setPixelInBytes(Xc + x, Yc - y, EnvVar.STD_COLOR);
                            self._setPixelInBytes(Xc - x, Yc + y, EnvVar.STD_COLOR);
                            self._setPixelInBytes(Xc - x, Yc - y, EnvVar.STD_COLOR);
                            error = 2 * (delta + y) - 1;
                            if (delta < 0 && error <= 0)
                            {
                                delta += 2 * x++ + 1;
                                continue;
                            }

                            if (delta > 0 && error > 0)
                            {
                                delta -= 2 * y-- + 1;
                                continue;
                            }

                            delta += 2 * (x++ - y--);
                        }
                    }
                }
            };

        private Bitmap _bitmap;
        private bool _bitmapReady;
        private byte[,,] _bytes;

        public GTDrawerSlow(IGTHavingPrimitives2D scene, Bitmap bitmap)
        {
            this.scene = scene;
            Bitmap = bitmap;
        }

        public GTDrawerSlow() : this(new Scene2D(),
            new Bitmap(EnvVar.STANDART_BMP_WIDTH,
                EnvVar.STANDART_BMP_HEIGHT))
        {
        }

        public GTDrawerSlow(IGTHavingPrimitives2D scene) : this(scene,
            new Bitmap(EnvVar.STANDART_BMP_WIDTH, EnvVar.STANDART_BMP_HEIGHT))
        {
        }

        public GTDrawerSlow(Bitmap bitmap) : this(new Scene2D(), bitmap)
        {
        }

        public Bitmap Bitmap
        {
            get
            {
                if (!_bitmapReady)
                    RgbToBitmapQ();
                return _bitmap;
            }
            set
            {
                _bitmap = value;
                BitmapToByteRgbQ();
            }
        }

        public AlgsForLine CurrentAlgForLine { get; set; } = AlgsForLine.Bresenham;

        public override void Draw()
        {
            //Array.Clear(_bytes, 0, _bytes.Length);
            _bytes = new byte[3, _bitmap.Height, _bitmap.Width];
            foreach (var element in scene.GetElements())
                DrawElement2D(element);
            _bitmapReady = false;
        }

        private void DrawPrimitive2D(Primitive2D primitive)
        {
            _stdDictPrimitives[primitive.GetType()](this, primitive);
        }

        private void DrawElement2D(Primitive2D element)
        {
            if (element is Element2D element2D)
                foreach (var el in element2D.Primitives)
                    DrawElement2D(el);
            else
                DrawPrimitive2D(element);
        }

        private void BitmapToByteRgbQ()
        {
            var bmp = _bitmap;
            int width = bmp.Width,
                height = bmp.Height;
            var res = new byte[3, height, width];
            var bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            try
            {
                fixed (byte* _res = res)
                {
                    byte* _r = _res, _g = _res + width * height, _b = _res + 2 * width * height;
                    for (var h = 0; h < height; h++)
                    {
                        var curPos = (byte*) bd.Scan0 + h * bd.Stride;
                        for (var w = 0; w < width; w++)
                        {
                            *_b = *curPos++;
                            ++_b;
                            *_g = *curPos++;
                            ++_g;
                            *_r = *curPos++;
                            ++_r;
                        }
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
                _bitmapReady = false;
            }

            _bytes = res;
        }

        private void RgbToBitmapQ()
        {
            var rgb = _bytes;
            if (rgb.GetLength(0) != 3)
                throw new ArrayTypeMismatchException(
                    "Size of first dimension for passed array must be 3 (RGB components)");

            int width = rgb.GetLength(2),
                height = rgb.GetLength(1);

            var result = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            var bd = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            try
            {
                byte* curpos;
                fixed (byte* _rgb = rgb)
                {
                    byte* _r = _rgb, _g = _rgb + width * height, _b = _rgb + 2 * width * height;
                    for (var h = 0; h < height; h++)
                    {
                        curpos = (byte*) bd.Scan0 + h * bd.Stride;
                        for (var w = 0; w < width; w++)
                        {
                            *curpos++ = *_b;
                            ++_b;
                            *curpos++ = *_g;
                            ++_g;
                            *curpos++ = *_r;
                            ++_r;
                        }
                    }
                }
            }
            finally
            {
                result.UnlockBits(bd);
                _bitmapReady = true;
            }

            _bitmap = result;
        }

        private void _setPixelInBytes(int x, int y, Color color)
        {
            if (y >= _bitmap.Height || y < 0 ||
                x >= _bitmap.Width || x < 0)
                return;

            _bytes[0, y, x] = color.R;
            _bytes[1, y, x] = color.G;
            _bytes[2, y, x] = color.B;
        }

        private Color _getPixelInBytes(int x, int y)
        {
            return Color.FromArgb(
                _bytes[0, y, x],
                _bytes[1, y, x],
                _bytes[2, y, x]);
        }

        private delegate void DrawMethod(GTDrawerSlow self, Primitive2D primitive);

        private delegate void AlgorithmDrawingLine(GTDrawerSlow self, Line2D line);
    }
}