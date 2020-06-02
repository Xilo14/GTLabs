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
        public static FilledTriangleModel ObjFilledToTriangleModel(ObjModel objModel)
        {
            var Model = new FilledTriangleModel();
            //генерация списка точек
            List<Dot3D> DotPool = new List<Dot3D>();
            foreach (var dot in objModel.vertices)
            {
                DotPool.Add(new Dot3D(dot.X, dot.Y, dot.Z));
            }

            //(ПАРСИМ)преобразование точек в 3угольники
            foreach (var face in objModel.faces)
            {
                if (face.Count > 3)
                {
                    throw new Exception("Больше трёх граней");
                }
                var Triangle = new Triangle3D(
                    DotPool[(int)face[0].X - 1],
                    DotPool[(int)face[1].X - 1],
                    DotPool[(int)face[2].X - 1]);
                Model.AddTriangle(Triangle);
            }

            return Model;
        }
    }
}
