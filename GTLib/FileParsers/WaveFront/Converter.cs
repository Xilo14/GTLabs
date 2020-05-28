using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTLib.Elements;
using GTLib.Primitives;

namespace GTLib.FileParsers.WaveFront
{
    public static class Converter
    {
        public static WireModel ObjModelToWireModel(ObjModel objModel)
        {
            var wireModel = new WireModel();

            foreach (var face in objModel.faces)
            {
                var PreviousVertInd = face[0].X;

                for (int i = 1; i < face.Count; i++)
                {
                    wireModel.AddWire(new Line3D(
                        new Dot3D(
                            objModel.vertices[(int)(PreviousVertInd - 1)].X,
                            objModel.vertices[(int)(PreviousVertInd - 1)].Y,
                            objModel.vertices[(int)(PreviousVertInd - 1)].Z),
                        new Dot3D(
                            objModel.vertices[(int)(face[i].X - 1)].X,
                            objModel.vertices[(int)(face[i].X - 1)].Y,
                            objModel.vertices[(int)(face[i].X - 1)].Z)));

                    PreviousVertInd = face[i].X;
                }
                wireModel.AddWire(new Line3D(
                    new Dot3D(
                        objModel.vertices[(int)(face.Last().X - 1)].X,
                        objModel.vertices[(int)(face.Last().X - 1)].Y,
                        objModel.vertices[(int)(face.Last().X - 1)].Z),
                    new Dot3D(
                        objModel.vertices[(int)(face[0].X - 1)].X,
                        objModel.vertices[(int)(face[0].X - 1)].Y,
                        objModel.vertices[(int)(face[0].X - 1)].Z)));

            }

            return wireModel;
        }
    }
}
