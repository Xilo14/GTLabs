using System;
using System.Collections.Generic;
using GTLib.Elements;
using GTLib.Interfaces;
using GTLib.Primitives;
using GTLib.Scenes;

namespace GTLib.Tranformers
{
    public class TransformerRotate : IGTTransformer2D
    {
        private readonly Dictionary<Type, TransformMethod> _stdDictPrimitives =
            new Dictionary<Type, TransformMethod>
            {
                {
                    typeof(Dot2D), (self, primitive) =>
                    {
                        var dot2d = (Dot2D) primitive;
                        self.RotateDot2D(dot2d);
                    }
                },
                {
                    typeof(Line2D), (self, primitive) =>
                    {
                        var line2d = (Line2D) primitive;
                        self.RotateDot2D(line2d.start);
                        self.RotateDot2D(line2d.finish);
                    }
                },
                {
                    typeof(Circle2D), (self, primitive) =>
                    {
                        var circle2d = (Circle2D) primitive;
                        self.RotateDot2D(circle2d.Center);
                    }

                },
                {
                    typeof(FilledTriangle2D), (self, primitive) =>
                    {
                        var filledTriangle2D = (FilledTriangle2D) primitive;
                        Dot2D A = filledTriangle2D.A;
                        Dot2D B = filledTriangle2D.B;
                        Dot2D C = filledTriangle2D.C;
                        self.RotateDot2D(A);
                        self.RotateDot2D(B);
                        self.RotateDot2D(C);
                    }
                }
            };

        public TransformerRotate(Dot2D center)
        {
            Center = center;
        }

        public TransformerRotate()
            : this(new Dot2D(0, 0))
        {
        }

        public double Degree { get; set; } = 0;
        public Dot2D Center { get; set; }

        public void Transform(Primitive2D primitive)
        {
            if (primitive is Element2D element2D)
                foreach (var el in element2D.DeclarativePrimitives)
                    Transform(el);
            else
                _stdDictPrimitives[primitive.GetType()](this, primitive);
        }
        public void Transform(Scene2D scene)
        {
            foreach (var el in scene.Get2DElements())
                Transform(el);
        }

        private void RotateDot2D(Dot2D dot2d)
        {
            //Translate cord to local system
            var X = dot2d.X - Center.X;
            var Y = dot2d.Y - Center.Y;

            //Rad
            var rad = Degree * (Math.PI / 180);

            //Rotate
            var newX = X * Math.Cos(rad)
                       - Y * Math.Sin(rad);
            var newY = X * Math.Sin(rad)
                       + Y * Math.Cos(rad);

            //Translate cord back to global system
            dot2d.X = newX + Center.X;
            dot2d.Y = newY + Center.Y;
        }

        private delegate void TransformMethod(TransformerRotate self, Primitive2D primitive);
    }
}