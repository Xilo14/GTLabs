using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using GTLib.Primitives;

namespace GTLib.Cameras
{
    public class Camera
    {
        public Vector3 Eye { get; set; }
        public Vector3 Focus { get; set; }
        public Vector3 Up { get; set; }

        public Camera(Vector3 eye, Vector3 focus, Vector3 up)
        {
            Eye = eye;
            Focus = focus;
            Up = up;
        }

        public void MoveUpDown(float lenght)
        {
            this.MoveCamera(Vector3.Multiply(Vector3.Normalize(Up - Eye), lenght));
        }

        public void MoveLeftRight(float lenght)
        {
            var eyeUpV = Vector3.Normalize(Up - Eye);
            var eyeFocusV = Vector3.Normalize(Focus - Eye);
            var result = Vector3.Cross(eyeUpV, eyeFocusV);
            this.MoveCamera(Vector3.Multiply(result, lenght));
        }

        public void MoveAheadBack(float lenght)
        {
            this.MoveCamera(Vector3.Multiply(Vector3.Normalize(Focus - Eye), lenght));
        }

        public void MoveCamera(Vector3 vector3)
        {
            Eye += vector3;
            Focus += vector3;
            Up += vector3;
        }

        public void RotateCamera(double degx, double degy)
        {
            var rady = (degy / 10) * (Math.PI / 180);
            var radx = (degx / 10) * (Math.PI / 180);



            var eyeFocusV = Focus - Eye;
            var eyeUpV = Up - Eye;

            Matrix4x4 matrixY = new Matrix4x4(
                (float)Math.Cos(radx), 0, (float)Math.Sin(radx), 0,
                0, 1, 0, 0,
                -(float)Math.Sin(radx), 0, (float)Math.Cos(radx), 0,
                0, 0, 0, 0);

            //Matrix4x4 matrixX = new Matrix4x4(
            //    1, 0, 0, 0,
            //    0, (float)Math.Cos(rady), -(float)Math.Sin(rady), 0,
            //    0, (float)Math.Sin(rady), (float)Math.Cos(rady), 0,
            //    0, 0, 0, 0);

            eyeFocusV = Vector3.Transform(eyeFocusV, matrixY);
            eyeUpV = Vector3.Transform(eyeUpV, matrixY);

            Focus = Eye + eyeFocusV;
            Up = Eye + eyeUpV;

            eyeFocusV = Focus - Eye;
            eyeUpV = Up - Eye;

            Matrix4x4 matrixZ = new Matrix4x4(
                (float)Math.Cos(rady), -(float)Math.Sin(rady), 0, 0,
                (float)Math.Sin(rady), (float)Math.Cos(rady), 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 0);


            var vectorV = Vector3.Normalize(Vector3.Cross(eyeUpV, eyeFocusV));
            Matrix4x4 matrixP = new Matrix4x4(
                (float)(Math.Cos(rady) + (1 - Math.Cos(rady)) * (Math.Pow(vectorV.X, 2))),
                    (float)((1 - Math.Cos(rady)) * vectorV.X * vectorV.Y - (Math.Sin(rady)) * vectorV.Z),
                        (float)((1 - Math.Cos(rady))*vectorV.X*vectorV.Z + Math.Sin(rady)*vectorV.Y), 0,
                (float)((1 - Math.Cos(rady)) * vectorV.X * vectorV.Y + (Math.Sin(rady)) * vectorV.Z),
                    (float)(Math.Cos(rady) + (1 - Math.Cos(rady)) * (Math.Pow(vectorV.Y, 2))),
                        (float)((1 - Math.Cos(rady)) * vectorV.Y * vectorV.Z - Math.Sin(rady) * vectorV.X), 0,
                (float)((1 - Math.Cos(rady)) * vectorV.X * vectorV.Z - (Math.Sin(rady)) * vectorV.Y),
                    (float)((1 - Math.Cos(rady)) * vectorV.Y * vectorV.Z + (Math.Sin(rady)) * vectorV.X),
                        (float)(Math.Cos(rady) + (1 - Math.Cos(rady)) * (Math.Pow(vectorV.Z, 2))), 0,
                0, 0, 0, 0);

            eyeFocusV = Vector3.Transform(eyeFocusV, matrixP);
            eyeUpV = Vector3.Transform(eyeUpV, matrixP);


            Focus = Eye + eyeFocusV;
            Up = Eye + eyeUpV;
            //vector3 = Vector3.Transform(vector3, matrixX);
            return;
        }
    }
}
