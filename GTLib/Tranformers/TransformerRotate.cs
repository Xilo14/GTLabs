using System;
using System.Collections.Generic;
using GTLib.Elements;
using GTLib.Interfaces;
using GTLib.Primitives;

namespace GTLib.Tranformers
{
    public class TransformerRotate : IGTTransformer2D
    {
        private void RotateDot2D(Dot2D dot2d)
        {
            //Cord
            var X = dot2d.X - Center.X;
            var Y = dot2d.Y - Center.Y;

            //Rotate
            X = ( X * Math.Cos(this.Degree* Math.PI / 180)
                                - Y * Math.Sin(this.Degree* Math.PI / 180));
            Y = (X * Math.Sin(this.Degree* Math.PI / 180)
                                + Y * Math.Cos(this.Degree* Math.PI / 180));

            //Cord
            dot2d.X = X + Center.X;
            dot2d.Y = Y + Center.Y;
        }

        private readonly Dictionary<Type, TransformMethod> _stdDictPrimitives =
            new Dictionary<Type, TransformMethod>
            {
                {
                    typeof(Dot2D), (self,primitive) =>
                    {
                        var dot2d = (Dot2D) primitive;
                        self.RotateDot2D(dot2d);
                    }
                },
                {
                    typeof(Line2D), (self,primitive )=>
                    {
                        var line2d = (Line2D) primitive;
                        self.RotateDot2D(line2d.start);
                        self.RotateDot2D(line2d.finish);
                    }
                },
                {
                    typeof(Circle2D),(self,primitive) =>
                    {
                        var circle2d = (Circle2D) primitive;
                        self.RotateDot2D(circle2d.Center);
                    }
                }
            };

        public double Degree { get; set; } = 0;
        public Dot2D Center { get; set; }

        public TransformerRotate(Dot2D center)
        {
            this.Center = center;
        }
        public TransformerRotate() 
            : this(new Dot2D(0, 0)) { }

        public void Transform(Primitive2D primitive)
        {
            if (primitive is Element2D element2D)
                foreach (var el in element2D.Primitives)
                    this.Transform(el);
            else
                _stdDictPrimitives[primitive.GetType()](this, primitive);
        }

        private delegate void TransformMethod(TransformerRotate self, Primitive2D primitive);
    }
}