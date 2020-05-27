using System.Collections.Generic;
using GTLib.Primitives;

namespace GTLib.Elements
{
    public class SimpleText2D : Element2D
    {
        private double _scaleIndex = 1.0;

        public Dictionary<char, List<Line2D>> _templateChars =
            new Dictionary<char, List<Line2D>>
            {
                {
                    'Ю', new List<Line2D>
                    {
                        new Line2D(new Dot2D(0, 0), new Dot2D(0, 6)),
                        new Line2D(new Dot2D(0, 3), new Dot2D(3, 3)),
                        new Line2D(new Dot2D(3, 0), new Dot2D(3, 6)),
                        new Line2D(new Dot2D(3, 0), new Dot2D(5, 0)),
                        new Line2D(new Dot2D(3, 6), new Dot2D(5, 6)),
                        new Line2D(new Dot2D(5, 0), new Dot2D(5, 6))
                    }
                },
                {
                    'Д', new List<Line2D>
                    {
                        new Line2D(new Dot2D(0, 6), new Dot2D(0, 5)),
                        new Line2D(new Dot2D(5, 6), new Dot2D(5, 5)),
                        new Line2D(new Dot2D(0, 5), new Dot2D(5, 5)),
                        new Line2D(new Dot2D(1, 0), new Dot2D(4, 0)),
                        new Line2D(new Dot2D(1, 5), new Dot2D(1, 0)),
                        new Line2D(new Dot2D(4, 5), new Dot2D(4, 0))
                    }
                },
                {
                    'А', new List<Line2D>
                    {
                        new Line2D(new Dot2D(2, 0), new Dot2D(3, 0)),
                        new Line2D(new Dot2D(0, 6), new Dot2D(2, 0)),
                        new Line2D(new Dot2D(3, 0), new Dot2D(5, 6)),
                        new Line2D(new Dot2D(0, 4), new Dot2D(5, 4))
                    }
                }
            };

        private string _text = "";

        private double _x;

        private double _y;

        public SimpleText2D()
        {
            Primitives = new List<Primitive2D>();
            DeclarativePrimitives = Primitives;
        }

        public SimpleText2D(string text) : this()
        {
            Text = text;
        }

        public double ScaleIndex
        {
            get => _scaleIndex;
            set
            {
                UpdateCord(X, Y, value);
                _scaleIndex = value;
            }
        }

        public double X
        {
            get => _x;
            set
            {
                UpdateCord(value, Y, ScaleIndex);
                _x = value;
            }
        }

        public double Y
        {
            get => _y;
            set
            {
                UpdateCord(X, value, ScaleIndex);
                _y = value;
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                ReParseText();
            }
        }

        private void ReParseText()
        {
            Primitives.Clear();
            var OffsetWidth = 0.0;
            foreach (var ch in Text)
            {
                var templateChar = _templateChars[ch];
                foreach (var line in templateChar)
                {
                    line.start.X = (line.start.X + OffsetWidth) * ScaleIndex + X;
                    line.start.Y = line.start.Y * ScaleIndex + Y;
                    line.finish.X = (line.finish.X + OffsetWidth) * ScaleIndex + X;
                    line.finish.Y = line.finish.Y * ScaleIndex + Y;
                }

                OffsetWidth += 7.0;
                Primitives.AddRange(templateChar);
            }
        }

        private void UpdateCord(double x, double y, double scale)
        {
            foreach (Line2D primitive in Primitives)
            {
                primitive.start.X = (primitive.start.X - X)
                                    * (scale / ScaleIndex)
                                    + x;
                primitive.start.Y = (primitive.start.Y - Y)
                                    * (scale / ScaleIndex)
                                    + y;
                primitive.finish.X = (primitive.finish.X - X)
                                     * (scale / ScaleIndex)
                                     + x;
                primitive.finish.Y = (primitive.finish.Y - Y)
                                     * (scale / ScaleIndex)
                                     + y;
            }
        }
    }
}