using System;
using System.Collections.Generic;
using System.Text;
using GTLib.Elements;
using GTLib.Interfaces;
using GTLib.Primitives;
using GTLib.Scenes;

namespace GTLib.Drawers
{
    class GTDrawerSlow : GTDrawer
    {
        public enum AlgsForLine
        {
            Bresenham,
            Luke,
        }

        private AlgsForLine CurrentAlgForLine { get; set; } = AlgsForLine.Bresenham;

        public new void Draw()
        {
            foreach (var element in scene.GetElements())
                DrawElement2D(element);
        }
        private void DrawPrimitive2D(Primitive2D primitive)
        {
            StdDictPrimitives[primitive.GetType()](this, primitive);
        }
        private void DrawElement2D(Primitive2D element)
        {
            if (element is Element2D)
                foreach (var el in ((Element2D)element).Primitives)
                    DrawElement2D(el);
            else
                DrawPrimitive2D(element);
        }

       

        private Dictionary<Type, DrawMethod> StdDictPrimitives = 
            new Dictionary<Type, DrawMethod>
        {
            { typeof(Dot2D), (GTDrawerSlow self,Primitive2D primitive) => {
                Dot2D dot2d = (Dot2D)primitive;
                self.bitmap.SetPixel(dot2d.X,dot2d.Y,EnvVar.STD_COLOR);
            } },
            { typeof(Line2D), (GTDrawerSlow self,Primitive2D primitive) => {
                Line2D line2d = (Line2D)primitive;
                self.AlgsDrawingLines[self.CurrentAlgForLine](self,line2d);
            } },
            { typeof(Circle2D), (GTDrawerSlow self,Primitive2D primitive) => {

            } },
        };

        private Dictionary<AlgsForLine, AlgorithmDrawingLine> AlgsDrawingLines =
            new Dictionary<AlgsForLine, AlgorithmDrawingLine>
            {
                {
                    AlgsForLine.Luke, (GTDrawerSlow self, Line2D line) =>
                    {
                        int Xd = line.start.X;
                        int Yd = line.start.Y;
                        int Xf = line.finish.X;
                        int Yf = line.finish.Y;

                        int Dx, Dy, Cumul;
                        int Xinc, Yinc, X, Y;
                        int i;
                        X = Xd;
                        Y = Yd;
                        self.bitmap.SetPixel(X, Y, EnvVar.STD_COLOR);
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
                                self.bitmap.SetPixel(X, Y, EnvVar.STD_COLOR);
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
                                self.bitmap.SetPixel(X, Y, EnvVar.STD_COLOR);
                            }
                        }
                    }
                },
                {
                    AlgsForLine.Bresenham, (GTDrawerSlow self, Line2D line) =>
                    {
                        int Xd = line.start.X;
                        int Yd = line.start.Y;
                        int Xf = line.finish.X;
                        int Yf = line.finish.Y;

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
                        self.bitmap.SetPixel(X, Y, EnvVar.STD_COLOR);

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
                                    S = S + Dy2;

                                X = X + Xinc;
                                self.bitmap.SetPixel(X, Y, EnvVar.STD_COLOR);
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
                                    S = S + Dx2;

                                Y = Y + Yinc;
                                self.bitmap.SetPixel(X, Y, EnvVar.STD_COLOR);
                            }
                        }
                    }
                },
            };
    }
    delegate void DrawMethod(GTDrawerSlow self, Primitive2D primitive);
    delegate void AlgorithmDrawingLine(GTDrawerSlow self, Line2D line);
}
