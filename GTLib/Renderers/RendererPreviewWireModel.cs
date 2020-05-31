using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO.Compression;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GTLib.Cameras;
using GTLib.Primitives;
using GTLib.Scenes;
using OpenTK;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;


namespace GTLib.Renderers
{
    /// <summary>
    /// Render Scene3DPreviewWireModel in Scene2D
    /// </summary>
    public class RendererPreviewWireModel : Renderer
    {
        public new Scene3DPreviewWireModel Scene3D { get; set; }
        public Scene2D Scene2D { get; set; }

        public RendererPreviewWireModel(Scene3DPreviewWireModel scene3d)
        {
            Scene3D = scene3d;
            Scene2D = new Scene2D();
        }
        public RendererPreviewWireModel(Scene3DPreviewWireModel scene3d, Scene2D scene2d)
        {
            Scene3D = scene3d;
            Scene2D = scene2d;
        }
        public override void Render()
        {
            _stdMethodRender[Scene3D.Camera.GetType()](this);
        }

        private Dictionary<Type, RenderType> _stdMethodRender = new Dictionary<Type, RenderType>()
        {
            {typeof(OrthogonalCamera), (self) =>
            {
                var camera = (OrthogonalCamera)self.Scene3D.Camera;
                Vector3 Position = new Vector3(
                    (float)camera.Eye.X,
                    (float)camera.Eye.Y,
                    (float)camera.Eye.Z);
                Vector3 Top = new Vector3(
                    (float)camera.Up.X,
                    (float)camera.Up.Y,
                    (float)camera.Up.Z);
                Vector3 CursNormal = new Vector3(
                    (float)camera.Focus.X,
                    (float)camera.Focus.Y,
                    (float)camera.Focus.Z);



                Vector3 BaseY = -(Top - Position);
                Vector3 BaseX = Vector3.Cross(BaseY, CursNormal - Position);

                //Parallel.ForEach(self.Scene3D.Model.Primitives, (line3D) =>
                foreach (var line3D in self.Scene3D.Model.Primitives)
                {
                    Vector3 startDot3d = new Vector3(
                        (float) line3D.start.X,
                        (float) line3D.start.Y,
                        (float) line3D.start.Z);
                    Vector3 finishDot3d = new Vector3(
                        (float) line3D.finish.X,
                        (float) line3D.finish.Y,
                        (float) line3D.finish.Z);

                    Dot2D startDot2d;
                    Dot2D finishDot2d;

                    startDot2d = new Dot2D(
                        Vector3.Dot(startDot3d, BaseX) / BaseX.Length,
                        Vector3.Dot(startDot3d, BaseY) / BaseY.Length);

                    finishDot2d = new Dot2D(
                        Vector3.Dot(finishDot3d, BaseX) / BaseX.Length,
                        Vector3.Dot(finishDot3d, BaseY) / BaseY.Length);

                    self.Scene2D.AddElement(new Line2D(startDot2d, finishDot2d));

                }
            }},
            {typeof(PerspectiveCamera), (self) =>
            {
                var camera = (PerspectiveCamera)self.Scene3D.Camera;


                Vector3 Position = new Vector3(
                    (float)camera.Eye.X,
                    (float)camera.Eye.Y,
                    (float)camera.Eye.Z);
                Vector3 Top = new Vector3(
                    (float)camera.Up.X,
                    (float)camera.Up.Y,
                    (float)camera.Up.Z);
                Vector3 CursNormal = new Vector3(
                    (float)camera.Focus.X,
                    (float)camera.Focus.Y,
                    (float)camera.Focus.Z);

                var C = (CursNormal - Position).Length;

                Vector3 z = Vector3.Normalize(Position - CursNormal);
                Vector3 x = Vector3.Normalize(Vector3.Cross(Top-Position,z));
                Vector3 y = Vector3.Normalize(Vector3.Cross(z,x));

                Matrix4 Minv = new Matrix4();
                Matrix4 Tr = new Matrix4();
                Minv.Diagonal = new OpenTK.Vector4(1,1,1,1);
                Tr.Diagonal = new OpenTK.Vector4(1,1,1,1);
                for (int i=0; i<3; i++)
                {
                    Minv[0, i] = x[i];
                    Minv[1,i] = y[i];
                    Minv[2,i] = z[i];
                    Tr[i,3] = -CursNormal[i];
                }
                Matrix4 ModelView = Minv*Tr;

                foreach (var line3D in self.Scene3D.Model.Primitives)
                {

                    var newStartDot3d = ModelView * new Vector4(
                        (float)line3D.start.X,
                        (float)line3D.start.Y,
                        (float)line3D.start.Z,
                        1);
                    var newFinishDot3d = ModelView * new Vector4(
                        (float)line3D.finish.X,
                        (float)line3D.finish.Y,
                        (float)line3D.finish.Z,
                        1);



                    if (newStartDot3d.Z != C && newFinishDot3d.Z != C)
                    {
                        //var newStartDot2dZbuff = newStartDot3d.Z / (1 - newStartDot3d.Z / C);
                        //var newFinishDot2dZbuff = newFinishDot3d.Z / (1 - newFinishDot3d.Z / C);
                        if (newStartDot3d.Z > C && newFinishDot3d.Z < C)
                        {
                            newStartDot3d.Z = C * 2 - newStartDot3d.Z;
                        }
                        if (newFinishDot3d.Z > C && newStartDot3d.Z < C)
                        {
                            newFinishDot3d.Z = C * 2 - newFinishDot3d.Z;
                        }

                        if (newStartDot3d.Z < C && newFinishDot3d.Z < C)
                        {
                            newStartDot3d = newStartDot3d / (1 - newStartDot3d.Z / C);
                            newFinishDot3d = newFinishDot3d / (1 - newFinishDot3d.Z / C);




                            //Dot2D startDot2d = new Dot2D(
                            //    newStartDot3d.X / (1 - newStartDot3d.Z / C),
                            //    -newStartDot3d.Y / (1 - newStartDot3d.Z / C));
                            //Dot2D finishDot2d = new Dot2D(
                            //    newFinishDot3d.X / (1 - newFinishDot3d.Z / C),
                            //    -newFinishDot3d.Y / (1 - newFinishDot3d.Z / C));

                            //Matrix4 ViewPort = new Matrix4(
                            //    800/2,0,650+800/2,0,
                            //    0,400/2,250+400/2,0,
                            //    0,0,255/2,0,
                            //    0,0,255/2,0);

                            Matrix4 ViewPort = new Matrix4(
                                800,0,0,0,
                                0,400,0,0,
                                0,0,1/2,1,
                                0,0,-1/2,0);

                            newStartDot3d = Vector4.Transform(newStartDot3d, ViewPort);
                            newFinishDot3d = Vector4.Transform(newFinishDot3d, ViewPort);

                            Dot2D startDot2d = new Dot2D(
                                newStartDot3d.X,
                                -newStartDot3d.Y);
                            Dot2D finishDot2d = new Dot2D(
                                newFinishDot3d.X ,
                                -newFinishDot3d.Y );

                            self.Scene2D.AddElement(new Line2D(startDot2d, finishDot2d));
                        }
                    }


                }

            }}
        };

        private delegate void RenderType(RendererPreviewWireModel self);
    }
}
