using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using GTLib.Primitives;

namespace GTLib.Elements
{
    public class Model: Element3D
    {
        public List<Dot3D> v = new List<Dot3D>();
        public List<Vector2> vt = new List<Vector2>();
        public List<Vector3> vn = new List<Vector3>();
        public List<List<Vector3>> f = new List<List<Vector3>>();
    }
}
