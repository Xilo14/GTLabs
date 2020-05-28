using System;
using System.Collections.Generic;
using GTLib.Elements;
using GTLib.Interfaces;
using GTLib.Primitives;
using GTLib.Scenes;

namespace GTLib.Tranformers
{
    public class TransformerMove : IGTTransformer2D
    {
        private readonly Dictionary<Type, TransformMethod> _stdDictPrimitives =
            new Dictionary<Type, TransformMethod>
            {
                {
                    typeof(Dot2D), (self, primitive) =>
                    {
                        var dot2d = (Dot2D) primitive;
                        dot2d.X += self.MoveX;
                        dot2d.Y += self.MoveY;
                    }
                },
                {
                    typeof(Line2D), (self, primitive) =>
                    {
                        var line2d = (Line2D) primitive;
                        line2d.start.X += self.MoveX;
                        line2d.start.Y += self.MoveY;
                        line2d.finish.X += self.MoveX;
                        line2d.finish.Y += self.MoveY;
                    }
                },
                {
                    typeof(Circle2D), (self, primitive) =>
                    {
                        var circle2d = (Circle2D) primitive;
                        circle2d.Center.X += self.MoveX;
                        circle2d.Center.Y += self.MoveY;
                    }
                }
            };

        public int MoveX { get; set; } = 0;
        public int MoveY { get; set; } = 0;

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

        private delegate void TransformMethod(TransformerMove self, Primitive2D primitive);
    }
}