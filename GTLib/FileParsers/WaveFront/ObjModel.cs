using System.Collections.Generic;
using System.Numerics;
using GTLib.Primitives;

namespace GTLib
{
    public class ObjModel
    {
        public List<Dot3D> vertices = new List<Dot3D>();
        public List<Vector2> verticesTexture = new List<Vector2>();
        public List<Vector3> verticesNormal = new List<Vector3>();
        public List<List<Vector3>> faces = new List<List<Vector3>>();
    }
}