using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Threading.Tasks;
using GTLib.Elements;
using GTLib.Interfaces;
using GTLib.Primitives;
using GTLib.Scenes;

namespace GTLib.Drawers
{
    public unsafe class GTDrawerSlow : GTDrawer
    {
        private int Height = 0;
        private int Width = 0;

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

                        //check is line outside
                        if ((line2d.start.X > 0 && line2d.start.X < self.Width &&
                            line2d.start.Y > 0 && line2d.start.Y < self.Height) &&
                            (line2d.finish.X > 0 && line2d.finish.X < self.Width &&
                            line2d.finish.Y > 0 && line2d.finish.Y < self.Height) )
                        {
                            self.CurrentAlgorithmDrawingLine(self, line2d);

                        } else if (!((line2d.start.X > 0 && line2d.start.X < self.Width &&
                                      line2d.start.Y > 0 && line2d.start.Y < self.Height)) &&
                                   !((line2d.finish.X > 0 && line2d.finish.X < self.Width &&
                                      line2d.finish.Y > 0 && line2d.finish.Y < self.Height)))
                        {

                        }else {
                            Vector2 topleft = new Vector2(0,0);
                            Vector2 topright = new Vector2(self.Width,0);
                            Vector2 botleft = new Vector2(0,self.Height);
                            Vector2 botright = new Vector2(self.Width,self.Height);

                            Vector2 start = new Vector2((float)line2d.start.X,(float)line2d.start.Y);
                            Vector2 finish = new Vector2((float) line2d.finish.X, (float) line2d.finish.Y);

                            if (((line2d.start.X > 0 && line2d.start.X < self.Width &&
                                  line2d.start.Y > 0 && line2d.start.Y < self.Height) &&
                                 !(line2d.finish.X > 0 && line2d.finish.X < self.Width &&
                                   line2d.finish.Y > 0 && line2d.finish.Y < self.Height)))
                            {
                                var result = self.AreCross(start, finish, topleft, topright);
                                if (result != null)
                                {
                                    line2d.finish.X = (double)result?.X;
                                    line2d.finish.Y = (double)result?.Y;
                                }else if ((result = self.AreCross(start, finish, topleft, botleft)) != null)
                                {
                                    line2d.finish.X = (double)result?.X;
                                    line2d.finish.Y = (double)result?.Y;
                                }else if ((result = self.AreCross(start, finish, botright, topright)) != null)
                                {
                                    line2d.finish.X = (double)result?.X;
                                    line2d.finish.Y = (double)result?.Y;
                                }else if ((result = self.AreCross(start, finish, botright, botleft)) != null)
                                {
                                    line2d.finish.X = (double)result?.X;
                                    line2d.finish.Y = (double)result?.Y;
                                }
                                self.CurrentAlgorithmDrawingLine(self, line2d);
                            }
                            else if ((!(line2d.start.X > 0 && line2d.start.X < self.Width &&
                                        line2d.start.Y > 0 && line2d.start.Y < self.Height) &&
                                      (line2d.finish.X > 0 && line2d.finish.X < self.Width &&
                                       line2d.finish.Y > 0 && line2d.finish.Y < self.Height)))
                            {
                                var result = self.AreCross(start, finish, topleft, topright);
                                if (result != null)
                                {
                                    line2d.start.X = (double)result?.X;
                                    line2d.start.Y = (double)result?.Y;
                                }else if ((result = self.AreCross(start, finish, topleft, botleft)) != null)
                                {
                                    line2d.start.X = (double)result?.X;
                                    line2d.start.Y = (double)result?.Y;
                                }else if ((result = self.AreCross(start, finish, botright, topright)) != null)
                                {
                                    line2d.start.X = (double)result?.X;
                                    line2d.start.Y = (double)result?.Y;
                                }else if ((result = self.AreCross(start, finish, botright, botleft)) != null)
                                {
                                    line2d.finish.X = (double)result?.X;
                                    line2d.finish.Y = (double)result?.Y;
                                }
                                self.CurrentAlgorithmDrawingLine(self, line2d);
                            }



                            

                        }

                        //self.CurrentAlgorithmDrawingLine(self, line2d);
                        //self._algsDrawingLines[self.CurrentAlgForLine](self, line2d);
                    }
                },
                {
                    typeof(Circle2D), (self, primitive) =>
                    {
                        var circle2d = (Circle2D) primitive;

                        //check is circle outside
                        if (circle2d.Center.X - circle2d.Radius >= self.Width ||
                            circle2d.Center.X + circle2d.Radius <= 0 ||
                            circle2d.Center.Y - circle2d.Radius >= self.Height ||
                            circle2d.Center.Y + circle2d.Radius <= 0)
                            return;

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
                },
                {
                    typeof(FilledTriangle2D), (self, primitive) =>
                    {
                    
                        var filledTriangle2D = (FilledTriangle2D) primitive;
                        self.ClipTriangle(filledTriangle2D);


                       //for (sy = A.Y; sy <= C.Y; sy++) {
                       //   x1 = A.X + (sy - A.Y) * (C.X - A.X) / (C.Y - A.Y);
                       //   if (sy < B.Y)
                       //     x2 = A.X + (sy - A.Y) * (B.X - A.X) / (B.Y - A.Y);
                       //   else {
                       //     if (C.Y == B.Y)
                       //       x2 = B.X;
                       //     else
                       //       x2 = B.X + (sy - B.Y) * (C.X - B.X) / (C.Y - B.Y);
                       //   }
                       //   if (x1 > x2) { tmp = x1; x1 = x2; x2 = tmp; }
                       //   //нарисовать линию
                       //   for(int i=(int)x1; i<(int)x2; i++)
                       //     self._setPixelInBytes((int) i, (int) sy, filledTriangle2D.Color);
                       //}
                    }
                }
            };
        public void DrawFilledTriangle(Dot2D A, Dot2D B, Dot2D C, Color color)
        {
            double sy;
            double x1, x2;
            double tmp;
            for (sy = A.Y; sy <= C.Y; sy++)
            {
                x1 = A.X + (sy - A.Y) * (C.X - A.X) / (C.Y - A.Y);
                if (sy < B.Y)
                    x2 = A.X + (sy - A.Y) * (B.X - A.X) / (B.Y - A.Y);
                else
                {
                    if (C.Y == B.Y)
                        x2 = B.X;
                    else
                        x2 = B.X + (sy - B.Y) * (C.X - B.X) / (C.Y - B.Y);
                }
                if (x1 > x2) { tmp = x1; x1 = x2; x2 = tmp; }
                //нарисовать линию
                for (int i = (int)x1; i < (int)x2; i++)
                    _setPixelInBytes((int)i, (int)sy, color);
            }
        }
        public void ClipTriangle(FilledTriangle2D filledTriangle2D)
        {
            Dot2D A = filledTriangle2D.A;
            Dot2D B = filledTriangle2D.B;
            Dot2D C = filledTriangle2D.C;
            Vector2 a = new Vector2((float)filledTriangle2D.A.X, (float)filledTriangle2D.A.Y);
            Vector2 b = new Vector2((float)filledTriangle2D.B.X, (float)filledTriangle2D.B.Y);
            Vector2 c = new Vector2((float)filledTriangle2D.C.X, (float)filledTriangle2D.C.Y);
            Vector2 topleft = new Vector2(0, 0);
            Vector2 topright = new Vector2(Width, 0);
            Vector2 botleft = new Vector2(0, Height);
            Vector2 botright = new Vector2(Width, Height);

            //if ((a.X < 0 && a.X > self.Width && a.Y < 0 && a.Y > self.Height) &&
            //    (b.X < 0 && b.X > self.Width && b.Y < 0 && b.Y > self.Height) &&
            //    (c.X < 0 && c.X > self.Width && c.Y < 0 && c.Y > self.Height))
            //{
            //    return;
            //}
            if ((a.X > 0 && a.X < Width && a.Y > 0 && a.Y < Height) &&
                (b.X > 0 && b.X < Width && b.Y > 0 && b.Y < Height) &&
                (c.X > 0 && c.X < Width && c.Y > 0 && c.Y < Height))
            {
                DrawFilledTriangle(A, B, C, filledTriangle2D.Color);
            }
            var result1 = AreCross(a, b, topleft, topright);
            var result2 = AreCross(a, b, topleft, topright);

            if (a.X < 0 || a.X > Width || a.Y < 0 || a.Y > Height)
            {
                if ((result1 = AreCross(a, b, topleft, botleft)) != null && (result2 = AreCross(a, c, topleft, botleft)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(O, B, C, filledTriangle2D.Color);
                    DrawFilledTriangle(O, E, C, filledTriangle2D.Color);
                }
                if ((result1 = AreCross(a, b, topleft, topright)) != null && (result2 = AreCross(a, c, topleft, topright)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(O, B, C, filledTriangle2D.Color);
                    DrawFilledTriangle(O, E, C, filledTriangle2D.Color);
                }
                if ((result1 = AreCross(a, b, topright, botright)) != null && (result2 = AreCross(a, c, topright, botright)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(O, B, C, filledTriangle2D.Color);
                    DrawFilledTriangle(O, E, C, filledTriangle2D.Color);
                }
                if ((result1 = AreCross(a, b, botright, botleft)) != null && (result2 = AreCross(a, c, botright, botleft)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(O, B, C, filledTriangle2D.Color);
                    DrawFilledTriangle(O, E, C, filledTriangle2D.Color);
                }
            }
            if (b.X < 0 || b.X > Width || b.Y < 0 || b.Y > Height)
            {
                if ((result1 = AreCross(b, a, topleft, botleft)) != null && (result2 = AreCross(b, c, topleft, botleft)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(A, O, C, filledTriangle2D.Color);
                    DrawFilledTriangle(O, E, C, filledTriangle2D.Color);
                }
                if ((result1 = AreCross(b, a, topleft, topright)) != null && (result2 = AreCross(b, c, topleft, topright)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(A, O, C, filledTriangle2D.Color);
                    DrawFilledTriangle(O, E, C, filledTriangle2D.Color);
                }
                if ((result1 = AreCross(b, a, topright, botright)) != null && (result2 = AreCross(b, c, topright, botright)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(A, O, C, filledTriangle2D.Color);
                    DrawFilledTriangle(O, E, C, filledTriangle2D.Color);
                }
                if ((result1 = AreCross(b, a, botright, botleft)) != null && (result2 = AreCross(b, c, botright, botleft)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(A, O, C, filledTriangle2D.Color);
                    DrawFilledTriangle(O, E, C, filledTriangle2D.Color);
                }
            }
            if (c.X < 0 || c.X > Width || c.Y < 0 || c.Y > Height)
            {
                if ((result1 = AreCross(c, b, topleft, botleft)) != null && (result2 = AreCross(a, c, topleft, botleft)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(A, B, O, filledTriangle2D.Color);
                    DrawFilledTriangle(A, O, E, filledTriangle2D.Color);
                }
                if ((result1 = AreCross(c, b, topleft, topright)) != null && (result2 = AreCross(a, c, topleft, topright)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(A, B, O, filledTriangle2D.Color);
                    DrawFilledTriangle(A, O, E, filledTriangle2D.Color);
                }
                if ((result1 = AreCross(c, b, topright, botright)) != null && (result2 = AreCross(a, c, topright, botright)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(A, B, O, filledTriangle2D.Color);
                    DrawFilledTriangle(A, O, E, filledTriangle2D.Color);
                }
                if ((result1 = AreCross(c, b, botright, botleft)) != null && (result2 = AreCross(a, c, botright, botleft)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(A, B, O, filledTriangle2D.Color);
                    DrawFilledTriangle(A, O, E, filledTriangle2D.Color);
                }
            }
            if ((a.X < 0 || a.X > Width || a.Y < 0 || a.Y > Height) && (b.X < 0 || b.X > Width || b.Y < 0 || b.Y > Height))
            {
                if ((result1 = AreCross(b, c, topleft, botleft)) != null && (result2 = AreCross(a, c, topleft, botleft)) != null)
                {
                    Dot2D E = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D O = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(O, E, C, filledTriangle2D.Color);
                }
                if ((result1 = AreCross(b, c, topleft, topright)) != null && (result2 = AreCross(a, c, topleft, topright)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(O, E, C, filledTriangle2D.Color);
                }
                if ((result1 = AreCross(b, c, topright, botright)) != null && (result2 = AreCross(a, c, topright, botright)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(O, E, C, filledTriangle2D.Color);
                }
                if ((result1 = AreCross(b, c, botright, botleft)) != null && (result2 = AreCross(a, c, botright, botleft)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(O, E, C, filledTriangle2D.Color);
                }
            }
            if ((a.X < 0 || a.X > Width || a.Y < 0 || a.Y > Height) && (c.X < 0 || c.X > Width || c.Y < 0 || c.Y > Height))
            {
                if ((result1 = AreCross(b, c, topleft, botleft)) != null && (result2 = AreCross(a, b, topleft, botleft)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(E, B, O, filledTriangle2D.Color);
                }
                if ((result1 = AreCross(b, c, topleft, topright)) != null && (result2 = AreCross(a, b, topleft, topright)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(E, B, O, filledTriangle2D.Color);
                }
                if ((result1 = AreCross(b, c, topright, botright)) != null && (result2 = AreCross(a, b, topright, botright)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(E, B, O, filledTriangle2D.Color);
                }
                if ((result1 = AreCross(b, c, botleft, botright)) != null && (result2 = AreCross(a, b, botleft, botright)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(E, B, O, filledTriangle2D.Color);
                }
            }
            if ((c.X < 0 || c.X > Width || c.Y < 0 || c.Y > Height) && (b.X < 0 || b.X > Width || b.Y < 0 || b.Y > Height))
            {
                if ((result1 = AreCross(b, a, topleft, botleft)) != null && (result2 = AreCross(a, c, topleft, botleft)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(A, O, E, filledTriangle2D.Color);
                }
                if ((result1 = AreCross(b, a, topleft, topright)) != null && (result2 = AreCross(a, c, topleft, topright)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(A, O, E, filledTriangle2D.Color);
                }
                if ((result1 = AreCross(b, a, topright, botright)) != null && (result2 = AreCross(a, c, topright, botright)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(A, O, E, filledTriangle2D.Color);
                }
                if ((result1 = AreCross(b, a, botright, botleft)) != null && (result2 = AreCross(a, c, botright, botleft)) != null)
                {
                    Dot2D O = new Dot2D((double)result1?.X, (double)result1?.Y);
                    Dot2D E = new Dot2D((double)result2?.X, (double)result2?.Y);
                    DrawFilledTriangle(A, O, E, filledTriangle2D.Color);
                }
            }
        }
        private Bitmap _bitmap;
        private bool _bitmapReady;
        private byte[,,] _bytes;

        public GTDrawerSlow(Scene2D scene2D, Bitmap bitmap)
        {
            CurrentAlgForLine = AlgsForLine.Luke;
            this.Scene2D = scene2D;
            Bitmap = bitmap;
        }

        public GTDrawerSlow() : this(new Scene2D(),
            new Bitmap(EnvVar.STANDART_BMP_WIDTH,
                EnvVar.STANDART_BMP_HEIGHT))
        { }

        public GTDrawerSlow(Scene2D scene2D) : this(scene2D,
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
                this.Height = _bitmap.Height;
                this.Width = _bitmap.Width;
                BitmapToByteRgbQ();
            }
        }

        private AlgsForLine _currentAlgForLine;
        public AlgsForLine CurrentAlgForLine
        {
            get => _currentAlgForLine;
            set
            {
                _currentAlgForLine = value;
                CurrentAlgorithmDrawingLine = _algsDrawingLines[_currentAlgForLine];
            }
        }
        private AlgorithmDrawingLine CurrentAlgorithmDrawingLine;

        public override void Draw()
        {
            Array.Clear(_bytes, 0, _bytes.Length);
            //_bytes = new byte[3, Height, Width];
            foreach (var element in Scene2D.Get2DElements())
                DrawElement2D(element);
            _bitmapReady = false;
        }
        public void ParallelDraw()
        {
            Array.Clear(_bytes, 0, _bytes.Length);
            Parallel.ForEach(Scene2D.Get2DElements(), DrawElement2D);

            _bitmapReady = false;
        }

        public virtual uint ParallelDrawWithMetric()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            ParallelDraw();

            stopWatch.Stop();
            //return (UInt32)stopWatch.ElapsedTicks;
            var seconds = stopWatch.ElapsedTicks / (double)Stopwatch.Frequency;
            var nanoseconds = seconds * 1000000000;
            return (uint)nanoseconds;
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
                        var curPos = (byte*)bd.Scan0 + h * bd.Stride;
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

            var result = _bitmap;//new Bitmap(width, height, PixelFormat.Format24bppRgb);

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
                        curpos = (byte*)bd.Scan0 + h * bd.Stride;
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
            if (y >= Height || y < 0 ||
                x >= Width || x < 0)
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

        public Vector2? AreCross(
            Vector2 v11,
            Vector2 v12,
            Vector2 v21,
            Vector2 v22)
        {
            Vector3 v11v3 = new Vector3(v11.X, v11.Y, 1);
            Vector3 v12v3 = new Vector3(v12.X, v12.Y, 1);
            Vector3 v21v3 = new Vector3(v21.X, v21.Y, 1);
            Vector3 v22v3 = new Vector3(v22.X, v22.Y, 1);

            Vector2 v2cut1 = v12 - v11;
            Vector2 v2cut2 = v22 - v21;
            Vector3 cut1 = new Vector3(v2cut1.X, v2cut1.Y, 1);
            Vector3 cut2 = new Vector3(v2cut2.X, v2cut2.Y, 1);
            Vector3 prod1, prod2;


            prod1 = Vector3.Cross(cut1, v21v3 - v11v3);
            //cross(cut1 * (v21 - v11));
            prod2 = Vector3.Cross(cut1, v22v3 - v11v3);
            //prod2 = cross(cut1 * (v22 - v11));

            if (prod1.Z < 0 && prod2.Z < 0 ||
                prod1.Z >= 0 && prod2.Z >= 0)
                return null;

            prod1 = Vector3.Cross(cut2, v11v3 - v21v3);
            prod2 = Vector3.Cross(cut2, v12v3 - v21v3);
            //prod1 = cross(cut2 * (v11 - v21));
            //prod2 = cross(cut2 * (v12 - v21));

            if (prod1.Z < 0 && prod2.Z < 0 ||
                prod1.Z >= 0 && prod2.Z >= 0)
                return null;

            Vector2 crossing = new Vector2();

            crossing.X = v11.X + cut1.X * Math.Abs(prod1.Z) / Math.Abs(prod2.Z - prod1.Z);
            crossing.Y = v11.Y + cut1.Y * Math.Abs(prod1.Z) / Math.Abs(prod2.Z - prod1.Z);


            return crossing;
        }

        //public void ClipTriangle(Triangle2D triangle2D)
        //{
        //    Vector2 a = new Vector2((float)triangle2D.a.X, (float)triangle2D.a.Y);
        //    Vector2 b = new Vector2((float)triangle2D.b.X, (float)triangle2D.b.Y);
        //    Vector2 c = new Vector2((float)triangle2D.c.X, (float)triangle2D.c.Y);
        //    Vector2 topleft = new Vector2(0, 0);
        //    Vector2 topright = new Vector2(0, Width);
        //    Vector2 botleft = new Vector2(Height, 0);
        //    Vector2 botright = new Vector2(Height, Width);
            
        //    if ((a.X < 0 && a.X > Width && a.Y < 0 && a.Y > Height) &&
        //        (b.X < 0 && b.X > Width && b.Y < 0 && b.Y > Height) &&
        //        (c.X < 0 && c.X > Width && c.Y < 0 && c.Y > Height))
        //    {
        //        return;
        //    }
        //    if ((a.X > 0 && a.X < Width && a.Y > 0 && a.Y < Height) &&
        //        (b.X > 0 && b.X < Width && b.Y > 0 && b.Y < Height) &&
        //        (c.X > 0 && c.X < Width && c.Y > 0 && c.Y < Height))
        //    {
        //        Scene2D
        //    }


        //}

        private delegate void DrawMethod(GTDrawerSlow self, Primitive2D primitive);

        private delegate void AlgorithmDrawingLine(GTDrawerSlow self, Line2D line);
    }
}