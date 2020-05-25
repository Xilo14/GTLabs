using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GTLib.Primitives;

namespace GTLib.Elements
{
    public class SimpleText2D : Element2D
    {
        private double _scale = 1.0;
        public double Scale
        {
            get => _scale;
            set
            {
                UpdateCord(this.X, this.Y, value);
                _scale = value;
            }
        }

        private double _x = 0;

        public double X
        {
            get => _x;
            set
            {
                UpdateCord(value, this.Y, this.Scale);
                _x = value;
            }
        }

        private double _y = 0;
        public double Y
        {
            get => _y;
            set
            {
                UpdateCord(this.X, value, this.Scale);
                _y = value;
            }
        }
        private string _text = "";
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
            this.Primitives.Clear();
            double OffsetWidth = 0.0;
            foreach (var ch in Text)
            {

                var templateChar = _templateChars[ch];
                foreach (var line in templateChar)
                {
                    line.start.X = (line.start.X + OffsetWidth) * Scale + this.X;
                    line.start.Y = (line.start.Y) * Scale + this.Y;
                    line.finish.X = (line.finish.X + OffsetWidth) * Scale + this.X;
                    line.finish.Y = (line.finish.Y) * Scale + this.Y;


                }
                OffsetWidth += 7.0;
                this.Primitives.AddRange(templateChar);
            }
        }

        private void UpdateCord(double x, double y, double scale)
        {
            foreach (Line2D primitive in Primitives)
            {
                primitive.start.X = (primitive.start.X - this.X)
                                    * (scale / this.Scale)
                                    + x;
                primitive.start.Y = (primitive.start.Y - this.Y)
                                    * (scale / this.Scale)
                                    + y;
                primitive.finish.X = (primitive.finish.X - this.X)
                                    * (scale / this.Scale)
                                    + x;
                primitive.finish.Y = (primitive.finish.Y - this.Y)
                                     * (scale / this.Scale)
                                     + y;
            }
        }

        public Dictionary<char, List<Line2D>> _templateChars =
            new Dictionary<char, List<Line2D>>()
            {
                {
                    'Ю', new List<Line2D>()
                    {
                        new Line2D(new Dot2D(0,0), new Dot2D(0,6)),
                        new Line2D(new Dot2D(0,3), new Dot2D(3,3)),
                        new Line2D(new Dot2D(3,0), new Dot2D(3,6)),
                        new Line2D(new Dot2D(3,0), new Dot2D(5,0)),
                        new Line2D(new Dot2D(3,6), new Dot2D(5,6)),
                        new Line2D(new Dot2D(5,0), new Dot2D(5,6)),
                    }
                },
                {
                    'Д', new List<Line2D>()
                    {
                        new Line2D(new Dot2D(0,6), new Dot2D(0,5)),
                        new Line2D(new Dot2D(5,6), new Dot2D(5,5)),
                        new Line2D(new Dot2D(0,5), new Dot2D(5,5)),
                        new Line2D(new Dot2D(1,0), new Dot2D(4,0)),
                        new Line2D(new Dot2D(1,5), new Dot2D(1,0)),
                        new Line2D(new Dot2D(4,5), new Dot2D(4,0)),
                    }
                },
                {
                    'А', new List<Line2D>()
                    {
                        new Line2D(new Dot2D(2,0), new Dot2D(3,0)),
                        new Line2D(new Dot2D(0,6), new Dot2D(2,0)),
                        new Line2D(new Dot2D(3,0), new Dot2D(5,6)),
                        new Line2D(new Dot2D(0,4), new Dot2D(5,4)),
                    }
                }
            };

        public SimpleText2D()
        {
            this.Primitives = new List<Primitive2D>();
        }
        public SimpleText2D(string text):this()
        {
            this.Text = text;
        }
    }
}