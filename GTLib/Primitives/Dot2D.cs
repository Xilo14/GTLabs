namespace GTLib.Primitives
{
    public class Dot2D : Primitive2D
    {
        public Dot2D(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public Dot2D() : this(0, 0) { }

        public double X { get; set; }
        public double Y { get; set; }
    }
}