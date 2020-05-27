namespace GTLib.Primitives
{
    public class Circle2D : Primitive2D
    {
        public Circle2D(Dot2D center, int radius)
        {
            Center = center;
            Radius = radius;
        }

        public Dot2D Center { get; set; }
        public double Radius { get; set; }
    }
}