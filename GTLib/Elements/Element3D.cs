using GTLib.Primitives;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GTLib.Elements
{
    public abstract class Element3D : Primitive3D
    {
        public List<Primitive3D> Primitives { get; protected set; }
        public List<Primitive3D> DeclarativePrimitives { get; protected set; }

        public float Scale { get; set; } = 1;

        /// <summary>
        /// Cord in global system (Point)
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// The direction of the view (Vector)
        /// </summary>
        public Vector3 Forward { get; set; }
        /// <summary>
        /// Up from Position (Vector)
        /// </summary>
        public Vector3 Up { get; set; }
        /// <summary>
        /// Right from Position (Vector)
        /// </summary>
        public Vector3 Right { get; set; }

        public float RadZ { get; set; } = 0;
        public float RadX { get; set; } = 0;
        public float RadY { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">Position cord</param>
        /// <param name="forward">Vector</param>
        /// <param name="up">Vector</param>
        public Element3D(
            Vector3 position,
            Vector3 forward,
            Vector3 up)
        {
            Position = position;
            Forward = Vector3.Normalize(forward);
            Up = Vector3.Normalize(up);
            Right = Vector3.Normalize(Vector3.Cross(Forward,Up));
        }
        public Element3D(
            Vector3 position,
            Vector3 forward)
        {
            Position = position;
            Forward = forward;
            Up = new Vector3(0,1,0);
        }
        public Element3D(
            Vector3 position)
        {
            Position = position;
            Forward = new Vector3(0, 0, -1);
            Up = new Vector3(0, 1, 0);
        }
        public Element3D()
        {
            Position = new Vector3(0,0,0);
            Forward = new Vector3(0, 0, -1);
            Up = new Vector3(0, 1, 0);
        }
    }
}
